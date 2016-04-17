using System;                           // Required for the IDisposable interface
using System.Collections.Generic;       // Required for the List class
using System.Text;                      // Required for the Encoding class
using System.IO;                        // Required for the MemoryStream class
using System.Diagnostics;               // Required for the Debug.Assert calls               
using System.Security.Cryptography;     // Required for the CNG APIs
using System.Threading;

namespace Cryptography_Next_Generation
{
    public partial class CNG_SecureCommunicationExample
    {

        internal sealed class Communicator : IDisposable
        {
            //private CngKey m_DSKey;
            private ECDiffieHellmanCng m_ECDH_Cng;
            private string m_ECDH_local_publicKey_XML;
            private ECDiffieHellmanPublicKey m_ECDH_remote_publicKey;

            public ChannelManager ChMgr;

            // 추가코드 ECDsa 코드 에러로 인해서
            private CngKey m_ECDsa_Key;

            public Communicator( string mode, string ChannelName )
            {
                // 지정된 키 크기와 난수 쌍을 사용해 인스턴스 초기화
                m_ECDH_Cng = new ECDiffieHellmanCng( 521 );

                // ECKeyXmlFormat.Rfc4050 형식으로 XML 변환
                m_ECDH_local_publicKey_XML = m_ECDH_Cng.ToXmlString( ECKeyXmlFormat.Rfc4050 );

                // 공개 채널 인스턴스 초기화
                ChMgr = new ChannelManager( mode, ChannelName );
            }

            public void Dispose()
            {
                if( m_ECDH_Cng != null )
                    ( m_ECDH_Cng as IDisposable ).Dispose();

                if( m_ECDH_remote_publicKey != null )
                    ( m_ECDH_remote_publicKey as IDisposable ).Dispose();

                if( ChMgr != null )
                    ( ChMgr as IDisposable ).Dispose();
            }

            public void StoreDSKey( byte[] DSKeyBlob )
            {

                Debug.Assert( DSKeyBlob != null, "DSKeyBlob != null" );
                //m_DSKey = CngKey.Import( DSKeyBlob, CngKeyBlobFormat.Pkcs8PrivateBlob );

                // 공개 키와 개인 키가 포함될 수 있으며, 공개 키가 포함되지 않을수도 있음. 
                // BLOB에 들어 있는 키의 형식을 확인하려면 BLOB을 검사해야 합니다.
                //m_ECDsa_Key = CngKey.Import( DSKeyBlob, CngKeyBlobFormat.Pkcs8PrivateBlob );

                // 모든 형식의 개인 키를 가지고 있을수도 없을수도 있는 형식
                m_ECDsa_Key = CngKey.Import( DSKeyBlob, CngKeyBlobFormat.GenericPrivateBlob );
            }

            public bool Send_or_Receive_PublicCryptoKey( string mode, int color )
            {
                string xmlECDH;
                byte[] signature = null;  // 공개 서명 키로 암호문(ciphertext)를 서명(Sign the ciphertext with the public signature key )

                if( "send" == mode )
                {
                    // 
                    xmlECDH = m_ECDH_local_publicKey_XML;

                    // m_ECDH_local_publicKey_XML를 ASCII로 인코딩
                    Byte[] message = Encoding.ASCII.GetBytes( m_ECDH_local_publicKey_XML );

                    if( 3 <= Version )
                    {
                        using( ECDsaCng ecdsa = new ECDsaCng( m_ECDsa_Key ) )   // EDCsaCng(CngKey)
                        {
                            ecdsa.HashAlgorithm = CngAlgorithm.Sha512;
                            signature = ecdsa.SignData( message );              // 암호화된 메세지를 위한 디지털 서명 생성
                        }

                        //using( ECDsaCng ecdsa = new ECDsaCng( m_DSKey ) )    // EDCsaCng(CngKey)
                        //{
                        //    ecdsa.HashAlgorithm = CngAlgorithm.Sha512;
                        //    signature = ecdsa.SignData( message );            // Create a digital signature for the encrypted message
                        //}

                        string messageLength = message.Length.ToString();

                        byte[] bLengths = new byte[ 1 ];                      // 3 바이트 길이를 가지는 배열 생성
                        bLengths[ 0 ] = ( byte )messageLength.Length;

                        List<byte> list1 = new List<byte>( bLengths );         // 리스트 개체에 4개의 배열들을 인스턴스 초기화
                        List<byte> list2 = new List<byte>( Encoding.ASCII.GetBytes( messageLength ) );
                        List<byte> list3 = new List<byte>( message );
                        List<byte> list4 = new List<byte>( signature );

                        list1.AddRange( list2 );                                // 네개의 배열을 하나로 모으고
                        list1.AddRange( list3 );
                        list1.AddRange( list4 );

                        message = list1.ToArray();                              // byte[] message에 list1을 배열로 변환후 입력 
                    }
                    ChMgr.SendMessage( message );
                }
                else                                                            // Mode = 수신 ( Mode = receive )
                {
                    byte[] input = ChMgr.ReadMessage();

                    // Alice가 변조된 서명키를 발견 할 때  Mallory를 응용프로그램 Version 5에서 접속 유지 시키기 위해서                    
                    if( 0 == input.Length )                        // Application control for Version 5 to keep Mallory from
                        return false;                              // hanging when Alice discovers a bad signature

                    if( 3 <= Version )
                    {
                        List<byte> list = new List<byte>( input );
                        int iLength = ( int )input[ 0 ];                       // 첫번째 바이트는 메세지 길이를 유지하는데 
                                                                               // 필요한 바이트수를 구한다.
                                                                               // 이 메시지 길이바이트는 첫번째 바이트를 따릅니다.
                        Byte[] message_Length = new Byte[ iLength ];           // 메시지에 충분한 버퍼 할당
                        list.CopyTo( 1, message_Length, 0, iLength );          // 버퍼에 메시지 복사
                        string s_message_Length =
                                  Encoding.ASCII.GetString( message_Length );

                        int count = Convert.ToInt32( s_message_Length );

                        Byte[] TheMessage = new Byte[ count ];

                        // 바이트 4에서 시작하는 메세지 복사 ( 0 - based )
                        list.CopyTo( 4, TheMessage, 0, count );

                        Byte[] TheSignature = new Byte[ input.Length - 4 - count ];

                        // 입력으로부터 받은 서명 검색
                        list.CopyTo( 4 + count, TheSignature, 0, input.Length - 4 - count );

                        // 암호문 해싱 및 서명된 결과를 비교하여 암호문 확인
                        using( ECDsaCng ecdsa = new ECDsaCng( m_ECDsa_Key ) )
                        {
                            ecdsa.HashAlgorithm = CngAlgorithm.Sha512;
                            if( !ecdsa.VerifyData( TheMessage, TheSignature ) )
                            {
                                if( "Alice Green" == MyName || "Bob White" == MyName )
                                    if( 4 <= Version )
                                        //Display( " ======== SECURITY ERROR!! ===========\n" +
                                        //"Cryptographic Key: Failure verifying digital signature.\n" +
                                        //( 5 == Version ? "Contact your security administrator.\n" +
                                        //               "TERMINATING SESSION\n\n" : "" ) + "\n\n", 0 );
                                        Display( " ======== 보안 에러 !! ( SECURITY ERROR ) ===========\n" +
                                        "암호 키(Cryptographic Key): 디지털 서명된 키 확인 실패.\n" +
                                        ( 5 == Version ? "보안 관리자에게 문의 하세요.\n" +
                                                       "세션을 종료 합니다.( TERMINATING SESSION )\n\n" : "" ) + "\n\n", 0 );
                                if( 5 == Version )
                                {
                                    Thread.Sleep( 2000 );
                                    return false;
                                }
                            }
                        }
                        xmlECDH = Encoding.ASCII.GetString( TheMessage );
                    }

                    else xmlECDH = Encoding.ASCII.GetString( input );    // 암호키가 서명 되지 않음.( ASCII Code )

                    m_ECDH_remote_publicKey = ECDiffieHellmanCngPublicKey.FromXmlString( xmlECDH );
                }

                if( fVerbose )
                {
                    //Display( "Here it is: an ECDH public KeyBlob\n" +
                    //        "encoded within an XML string:\n\n" );

                    Display( "Here it is: ECDH 공개 KeyBlob\n" +
                            "XML 문자열로 인코딩:\n\n" );
                    Display( xmlECDH + "\n\n", color );
                }
                return true;
            }

            /*
                public void Store_bArray_ECDH_remotePublicKey(Byte[] bArray)
                {
             //   m_ECDH_remote_publicKey = (ECDiffieHellmanPublicKey)enc.GetBytes(s);
             //   ECDiffieHellmanPublicKey Alice_ECDH_publicKey = Alice.m_ECDH_local_publicKey;

             //   byte[] bArray = enc.GetBytes(s);
             //   m_ECDH_remote_publicKey = new ECDiffieHellmanPublicKey(bArray);           // Abstract Base Class: can't instantiate
             //   m_ECDH_remote_publicKey = ECDiffieHellmanCngPublicKey.FromByteArray(bArray,CngKeyBlobFormat.GenericPublicBlob);
                 m_ECDH_remote_publicKey = ECDiffieHellmanCngPublicKey.FromByteArray(bArray,CngKeyBlobFormat.EccPublicBlob);
                 }
            */

            //-------------------------------------------------------------------------------------------------

            private byte[] iv;
            private byte[] ciphertext;
            private byte[] signature;

            public bool SendMessage( string plainTextMessage, bool fShowMsg )
            {
                Debug.Assert( plainTextMessage != null, "plainTextMessage != null" );
                if( fShowMsg )
                    Display( ":> " + plainTextMessage + "\n", MyColor );

                if( 1 != Version )
                    if( fVerbose )
                        Display( sep1, 1 );

                // 유니 코드 바이트 배열에 메시지 문자열을 변환합니다.
                byte[] UnicodeText = Encoding.Unicode.GetBytes( plainTextMessage );

                if( 1 == Version )
                    // 평문( 암호화되지 않은 서명되지 않은 ) 메시지를 보냅니다.
                    return ChMgr.SendMessage( UnicodeText );

                // 받는 사람과 공유 AES세션 키를 생성 합니다.
                // 이 키는 메세지를 암호화하는데 사용 됩니다.

                // 첫 번째 바이트는 중복되지 않게 하기 위해서
                // IV(Initialization Vector)를 AES 형식으로 초기화
                byte[] iv = null;

                // ECDiffieHellmanCng.DeriveKeyMaterial Method.
                // 두 당사자간의 보안 협정으로부터 파생되는 키를 생성
                byte[] aesKey = m_ECDH_Cng.DeriveKeyMaterial( m_ECDH_remote_publicKey );

                // 출력 버퍼에 암호화된 메시지를 넣음
                byte[] ciphertext = null;

                // 실제 암호화 시작
                using( Aes aes = new AesCryptoServiceProvider() )
                {
                    aes.Key = aesKey;

                    // 암호화된 문자열을 위한 관리되는 임시 메모리 생성
                    using( MemoryStream ms = new MemoryStream() )
                    using( CryptoStream cs = new CryptoStream( ms,
                             aes.CreateEncryptor(), CryptoStreamMode.Write ) )
                    {
                        // 암호화를 수행하고, 메모리에 결과 쓰기
                        cs.Write( UnicodeText, 0, UnicodeText.Length );

                        // 임시 메모리를 갱신하고 하고 cs 버퍼를 비웁니다.         
                        cs.FlushFinalBlock();

                        // aes 암호문을 저장 합니다.
                        iv = aes.IV; // Save the IV and ciphertext.
                        ciphertext = ms.ToArray();
                    }
                }

                // 공개 서명키와 암호문에 서명
                byte[] signature = null;

                if( 3 <= Version )
                    using( ECDsaCng ecdsa = new ECDsaCng( m_ECDsa_Key ) )//m_DSKey ) )             // EDCsaCng(CngKey)
                    {
                        ecdsa.HashAlgorithm = CngAlgorithm.Sha512;

                        // 암호화할 메시지의 디지털 서명 만들기
                        signature = ecdsa.SignData( ciphertext );
                    }

                // 3byte 길이 배열 생성
                byte[] bLengths = new byte[ 3 ];
                bLengths[ 0 ] = ( byte )iv.Length;
                bLengths[ 1 ] = ( byte )ciphertext.Length;

                // 3 이상 버전의경우 서명 길이 추가
                bLengths[ 2 ] = ( byte )( 3 <= Version ? signature.Length : 0 );

                // 4개의 List 개체 배열 만들기
                List<byte> list1 = new List<byte>( bLengths );
                List<byte> list2 = new List<byte>( iv );
                List<byte> list3 = new List<byte>( ciphertext );
                List<byte> list4 = 3 <= Version ? new List<byte>( signature ) : null;

                // 네 배열 하나의 배열로 결합
                list1.AddRange( list2 );
                list1.AddRange( list3 );
                if( 3 <= Version )
                    list1.AddRange( list4 );

                // 결합된 첫번째 List개체를 배열로 변환
                byte[] message = list1.ToArray();

                // 메시지 보내기
                return ChMgr.SendMessage( message );
            }   // End SendMessage



            public string ReceiveMessage()
            {
                // Utility byte buffer
                Byte[] byteBuffer;

                // 메세지 읽기
                byteBuffer = ChMgr.ReadMessage();                   // Read the message.
                // CTRL-C 또는 SYS-EXIT
                if( 0 == byteBuffer.Length )                        // CTRL-C 또는 SYS-EXIT시 다른 사람의 실행 창을 닫음.
                {
                    Display( "Connection has been closed\n\n" );    // (메세지를 수신하는 동안)
                    return "";
                }
                // 버전 1은 암호화 되지 않음.(평문 plaintext)
                if( 1 == Version )
                {
                    string AsciiMessage = Encoding.Unicode.GetString( byteBuffer );
                    Display( "   " + AsciiMessage + "\n", OtherColor );
                    return AsciiMessage;
                }


                List<byte> list = new List<byte>( byteBuffer );

                try
                {
                    // 메시지 구성요소 위한 3byte 배열 만들기
                    // 이러한 세 바이트 배열의 크기는 메시지의 처음 3바이트에 포함된다.
                    iv = new Byte[ byteBuffer[ 0 ] ];         // initialization vector를 위한 저장소
                    ciphertext = new Byte[ byteBuffer[ 1 ] ]; // 암호문(ciphertext)을 위한 저장소
                    signature = new Byte[ byteBuffer[ 2 ] ];  // 서명(signature)을 위한 저장소
                }
                catch( IndexOutOfRangeException e )
                {
                    Display( "Cryptographic error encountered\n\n" );
                    Display( e + "Cryptographic error encountered\n\n" );
                    return "";
                }

                int ndx = 3;
                // CopyTo: 0-based index in source, destination,
                //         0-based ndx in destination, elements to copy.
                list.CopyTo( ndx, iv, 0, byteBuffer[ 0 ] );                 // initialization vector (IV).
                ndx += byteBuffer[ 0 ];
                list.CopyTo( ndx, ciphertext, 0, byteBuffer[ 1 ] );         // 암호문 구하기
                ndx += byteBuffer[ 1 ];
                list.CopyTo( ndx, signature, 0, byteBuffer[ 2 ] );          // 디지털 서명 메시지 구하기


                Debug.Assert( iv != null, "iv != null" );
                Debug.Assert( ciphertext != null, "ciphertext != null" );
                Debug.Assert( signature != null, "signature != null" );

                if( fVerbose )
                {
                    ASCIIEncoding enc = new ASCIIEncoding();

                    string s = enc.GetString( iv );
                    Display( "   Incoming Message:\n\n", 7 );
                    Display( "   Initialization vector:  ", 7 );
                    Display( s + "\n\n", 7 );

                    s = enc.GetString( ciphertext );
                    Display( "   Ciphertext:\n", 7 );
                    Display( s + "\n\n", 7 );

                    s = enc.GetString( signature );
                    if( "" != s )
                    {
                        Display( "   Signature:\n", 7 );
                        Display( s + "\n\n", 7 );
                    }
                    Display( "   Incoming Decoded message:\n\n", 7 );
                }

                // 메시지를 복호화하는데 사용되는 키를 파생
                byte[] aesKey = m_ECDH_Cng.DeriveKeyMaterial( m_ECDH_remote_publicKey );


                // 메시지를 해독할 수 있는 열쇠(key material)
                byte[] plaintext = null;

                using( Aes aes = new AesCryptoServiceProvider() )
                {
                    aes.Key = aesKey;
                    aes.IV = iv;

                    using( MemoryStream ms = new MemoryStream() )
                    using( CryptoStream cs = new CryptoStream( ms,
                           aes.CreateDecryptor(),
                           CryptoStreamMode.Write ) )
                    {
                        cs.Write( ciphertext, 0, ciphertext.Length );
                        cs.FlushFinalBlock();
                        plaintext = ms.ToArray();
                    }
                }
                // Convert the raw plaintext back to the transmitted message
                // 전송된 메시지를 다시 원본 평문으로 변환 
                string message = Encoding.Unicode.GetString( plaintext );
                Display( "   " + message + "\n", OtherColor );

                if( 3 <= Version )
                    // 암호문 해싱 및 서명 결과를 비교하여 윈본 암호문인지 확인
                    using( ECDsaCng ecdsa = new ECDsaCng( m_ECDsa_Key ) )
                    {
                        ecdsa.HashAlgorithm = CngAlgorithm.Sha512;
                        if( !ecdsa.VerifyData( ciphertext, signature ) )
                            if( "Alice Green" == MyName || "Bob White" == MyName )
                                Display( "보안 경고!! ( SECURITY WARNING ) "
                                       + "확인되지 않은 서명을 받았습니다.(Received signature did not verify)\n\n", 0 );
                    }
                if( fVerbose )
                    Display( sep1, 1 );

                return message;
            }

            //-------------------------------------------------------------------------------------------------

        }   // End Communicator class

    }  // End Communicator.cs: public partial class CNG_SecureCommunicationExample
}

/*
----------------------------------------------------------------------------------------------------------------------------
클래스, 메서드 또는 전역 변수 이름                용도
----------------------------------------------------------------------------------------------------------------------------
CNG_SecureCommunicationExample          프로젝트 수준 partial 클래스입니다.
----------------------------------------------------------------------------------------------------------------------------
Communicator                            모든 암호화 기능을 캡슐화하는 클래스입니다. 
                                        이 클래스는 Alice, Bob 및 Mallory 간의 모든 메시지를 처리합니다. 
                                        ChannelManager AppControl 메서드에서 전송하는 메시지는 처리하지 않습니다.
----------------------------------------------------------------------------------------------------------------------------
m_DSKey                                 클래스 변수입니다.
m_ECDH_Cng
m_ECDH_local_publicKey_XML
m_ECDH_remote_publicKey
ChMgr
----------------------------------------------------------------------------------------------------------------------------
Communicator                            Communicator 개체를 생성하는 메서드입니다.
----------------------------------------------------------------------------------------------------------------------------
Dispose                                 전용으로 보유된 리소스를 해제하는 메서드입니다.
----------------------------------------------------------------------------------------------------------------------------
StoreDSKey                              디지털 서명 키를 저장하는 메서드입니다.
----------------------------------------------------------------------------------------------------------------------------
Send_or_Receive_PublicCryptoKey         키 교환을 지원하는 메서드입니다.
----------------------------------------------------------------------------------------------------------------------------
iv                                      일반 텍스트 메시지를 암호화하는 데 사용되는 전용 클래스 변수입니다. 
ciphertext                              이러한 변수는 ReceiveMessage() 메서드 근처에 선언되어 있습니다.
signature                                        
----------------------------------------------------------------------------------------------------------------------------
ReceiveMessage  보안 버전에 따라 일반 텍스트 또는 암호화된 메시지를 받는 메서드입니다.
----------------------------------------------------------------------------------------------------------------------------
SendMessage 일반 텍스트를 받아들이고 보안 버전에 따라 일반 텍스트 또는 암호화된 형식으로 전송하는 메서드입니다.
----------------------------------------------------------------------------------------------------------------------------
*/
