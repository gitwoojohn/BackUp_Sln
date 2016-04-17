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

            // �߰��ڵ� ECDsa �ڵ� ������ ���ؼ�
            private CngKey m_ECDsa_Key;

            public Communicator( string mode, string ChannelName )
            {
                // ������ Ű ũ��� ���� ���� ����� �ν��Ͻ� �ʱ�ȭ
                m_ECDH_Cng = new ECDiffieHellmanCng( 521 );

                // ECKeyXmlFormat.Rfc4050 �������� XML ��ȯ
                m_ECDH_local_publicKey_XML = m_ECDH_Cng.ToXmlString( ECKeyXmlFormat.Rfc4050 );

                // ���� ä�� �ν��Ͻ� �ʱ�ȭ
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

                // ���� Ű�� ���� Ű�� ���Ե� �� ������, ���� Ű�� ���Ե��� �������� ����. 
                // BLOB�� ��� �ִ� Ű�� ������ Ȯ���Ϸ��� BLOB�� �˻��ؾ� �մϴ�.
                //m_ECDsa_Key = CngKey.Import( DSKeyBlob, CngKeyBlobFormat.Pkcs8PrivateBlob );

                // ��� ������ ���� Ű�� ������ �������� �������� �ִ� ����
                m_ECDsa_Key = CngKey.Import( DSKeyBlob, CngKeyBlobFormat.GenericPrivateBlob );
            }

            public bool Send_or_Receive_PublicCryptoKey( string mode, int color )
            {
                string xmlECDH;
                byte[] signature = null;  // ���� ���� Ű�� ��ȣ��(ciphertext)�� ����(Sign the ciphertext with the public signature key )

                if( "send" == mode )
                {
                    // 
                    xmlECDH = m_ECDH_local_publicKey_XML;

                    // m_ECDH_local_publicKey_XML�� ASCII�� ���ڵ�
                    Byte[] message = Encoding.ASCII.GetBytes( m_ECDH_local_publicKey_XML );

                    if( 3 <= Version )
                    {
                        using( ECDsaCng ecdsa = new ECDsaCng( m_ECDsa_Key ) )   // EDCsaCng(CngKey)
                        {
                            ecdsa.HashAlgorithm = CngAlgorithm.Sha512;
                            signature = ecdsa.SignData( message );              // ��ȣȭ�� �޼����� ���� ������ ���� ����
                        }

                        //using( ECDsaCng ecdsa = new ECDsaCng( m_DSKey ) )    // EDCsaCng(CngKey)
                        //{
                        //    ecdsa.HashAlgorithm = CngAlgorithm.Sha512;
                        //    signature = ecdsa.SignData( message );            // Create a digital signature for the encrypted message
                        //}

                        string messageLength = message.Length.ToString();

                        byte[] bLengths = new byte[ 1 ];                      // 3 ����Ʈ ���̸� ������ �迭 ����
                        bLengths[ 0 ] = ( byte )messageLength.Length;

                        List<byte> list1 = new List<byte>( bLengths );         // ����Ʈ ��ü�� 4���� �迭���� �ν��Ͻ� �ʱ�ȭ
                        List<byte> list2 = new List<byte>( Encoding.ASCII.GetBytes( messageLength ) );
                        List<byte> list3 = new List<byte>( message );
                        List<byte> list4 = new List<byte>( signature );

                        list1.AddRange( list2 );                                // �װ��� �迭�� �ϳ��� ������
                        list1.AddRange( list3 );
                        list1.AddRange( list4 );

                        message = list1.ToArray();                              // byte[] message�� list1�� �迭�� ��ȯ�� �Է� 
                    }
                    ChMgr.SendMessage( message );
                }
                else                                                            // Mode = ���� ( Mode = receive )
                {
                    byte[] input = ChMgr.ReadMessage();

                    // Alice�� ������ ����Ű�� �߰� �� ��  Mallory�� �������α׷� Version 5���� ���� ���� ��Ű�� ���ؼ�                    
                    if( 0 == input.Length )                        // Application control for Version 5 to keep Mallory from
                        return false;                              // hanging when Alice discovers a bad signature

                    if( 3 <= Version )
                    {
                        List<byte> list = new List<byte>( input );
                        int iLength = ( int )input[ 0 ];                       // ù��° ����Ʈ�� �޼��� ���̸� �����ϴµ� 
                                                                               // �ʿ��� ����Ʈ���� ���Ѵ�.
                                                                               // �� �޽��� ���̹���Ʈ�� ù��° ����Ʈ�� �����ϴ�.
                        Byte[] message_Length = new Byte[ iLength ];           // �޽����� ����� ���� �Ҵ�
                        list.CopyTo( 1, message_Length, 0, iLength );          // ���ۿ� �޽��� ����
                        string s_message_Length =
                                  Encoding.ASCII.GetString( message_Length );

                        int count = Convert.ToInt32( s_message_Length );

                        Byte[] TheMessage = new Byte[ count ];

                        // ����Ʈ 4���� �����ϴ� �޼��� ���� ( 0 - based )
                        list.CopyTo( 4, TheMessage, 0, count );

                        Byte[] TheSignature = new Byte[ input.Length - 4 - count ];

                        // �Է����κ��� ���� ���� �˻�
                        list.CopyTo( 4 + count, TheSignature, 0, input.Length - 4 - count );

                        // ��ȣ�� �ؽ� �� ����� ����� ���Ͽ� ��ȣ�� Ȯ��
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
                                        Display( " ======== ���� ���� !! ( SECURITY ERROR ) ===========\n" +
                                        "��ȣ Ű(Cryptographic Key): ������ ����� Ű Ȯ�� ����.\n" +
                                        ( 5 == Version ? "���� �����ڿ��� ���� �ϼ���.\n" +
                                                       "������ ���� �մϴ�.( TERMINATING SESSION )\n\n" : "" ) + "\n\n", 0 );
                                if( 5 == Version )
                                {
                                    Thread.Sleep( 2000 );
                                    return false;
                                }
                            }
                        }
                        xmlECDH = Encoding.ASCII.GetString( TheMessage );
                    }

                    else xmlECDH = Encoding.ASCII.GetString( input );    // ��ȣŰ�� ���� ���� ����.( ASCII Code )

                    m_ECDH_remote_publicKey = ECDiffieHellmanCngPublicKey.FromXmlString( xmlECDH );
                }

                if( fVerbose )
                {
                    //Display( "Here it is: an ECDH public KeyBlob\n" +
                    //        "encoded within an XML string:\n\n" );

                    Display( "Here it is: ECDH ���� KeyBlob\n" +
                            "XML ���ڿ��� ���ڵ�:\n\n" );
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

                // ���� �ڵ� ����Ʈ �迭�� �޽��� ���ڿ��� ��ȯ�մϴ�.
                byte[] UnicodeText = Encoding.Unicode.GetBytes( plainTextMessage );

                if( 1 == Version )
                    // ��( ��ȣȭ���� ���� ������� ���� ) �޽����� �����ϴ�.
                    return ChMgr.SendMessage( UnicodeText );

                // �޴� ����� ���� AES���� Ű�� ���� �մϴ�.
                // �� Ű�� �޼����� ��ȣȭ�ϴµ� ��� �˴ϴ�.

                // ù ��° ����Ʈ�� �ߺ����� �ʰ� �ϱ� ���ؼ�
                // IV(Initialization Vector)�� AES �������� �ʱ�ȭ
                byte[] iv = null;

                // ECDiffieHellmanCng.DeriveKeyMaterial Method.
                // �� ����ڰ��� ���� �������κ��� �Ļ��Ǵ� Ű�� ����
                byte[] aesKey = m_ECDH_Cng.DeriveKeyMaterial( m_ECDH_remote_publicKey );

                // ��� ���ۿ� ��ȣȭ�� �޽����� ����
                byte[] ciphertext = null;

                // ���� ��ȣȭ ����
                using( Aes aes = new AesCryptoServiceProvider() )
                {
                    aes.Key = aesKey;

                    // ��ȣȭ�� ���ڿ��� ���� �����Ǵ� �ӽ� �޸� ����
                    using( MemoryStream ms = new MemoryStream() )
                    using( CryptoStream cs = new CryptoStream( ms,
                             aes.CreateEncryptor(), CryptoStreamMode.Write ) )
                    {
                        // ��ȣȭ�� �����ϰ�, �޸𸮿� ��� ����
                        cs.Write( UnicodeText, 0, UnicodeText.Length );

                        // �ӽ� �޸𸮸� �����ϰ� �ϰ� cs ���۸� ���ϴ�.         
                        cs.FlushFinalBlock();

                        // aes ��ȣ���� ���� �մϴ�.
                        iv = aes.IV; // Save the IV and ciphertext.
                        ciphertext = ms.ToArray();
                    }
                }

                // ���� ����Ű�� ��ȣ���� ����
                byte[] signature = null;

                if( 3 <= Version )
                    using( ECDsaCng ecdsa = new ECDsaCng( m_ECDsa_Key ) )//m_DSKey ) )             // EDCsaCng(CngKey)
                    {
                        ecdsa.HashAlgorithm = CngAlgorithm.Sha512;

                        // ��ȣȭ�� �޽����� ������ ���� �����
                        signature = ecdsa.SignData( ciphertext );
                    }

                // 3byte ���� �迭 ����
                byte[] bLengths = new byte[ 3 ];
                bLengths[ 0 ] = ( byte )iv.Length;
                bLengths[ 1 ] = ( byte )ciphertext.Length;

                // 3 �̻� �����ǰ�� ���� ���� �߰�
                bLengths[ 2 ] = ( byte )( 3 <= Version ? signature.Length : 0 );

                // 4���� List ��ü �迭 �����
                List<byte> list1 = new List<byte>( bLengths );
                List<byte> list2 = new List<byte>( iv );
                List<byte> list3 = new List<byte>( ciphertext );
                List<byte> list4 = 3 <= Version ? new List<byte>( signature ) : null;

                // �� �迭 �ϳ��� �迭�� ����
                list1.AddRange( list2 );
                list1.AddRange( list3 );
                if( 3 <= Version )
                    list1.AddRange( list4 );

                // ���յ� ù��° List��ü�� �迭�� ��ȯ
                byte[] message = list1.ToArray();

                // �޽��� ������
                return ChMgr.SendMessage( message );
            }   // End SendMessage



            public string ReceiveMessage()
            {
                // Utility byte buffer
                Byte[] byteBuffer;

                // �޼��� �б�
                byteBuffer = ChMgr.ReadMessage();                   // Read the message.
                // CTRL-C �Ǵ� SYS-EXIT
                if( 0 == byteBuffer.Length )                        // CTRL-C �Ǵ� SYS-EXIT�� �ٸ� ����� ���� â�� ����.
                {
                    Display( "Connection has been closed\n\n" );    // (�޼����� �����ϴ� ����)
                    return "";
                }
                // ���� 1�� ��ȣȭ ���� ����.(�� plaintext)
                if( 1 == Version )
                {
                    string AsciiMessage = Encoding.Unicode.GetString( byteBuffer );
                    Display( "   " + AsciiMessage + "\n", OtherColor );
                    return AsciiMessage;
                }


                List<byte> list = new List<byte>( byteBuffer );

                try
                {
                    // �޽��� ������� ���� 3byte �迭 �����
                    // �̷��� �� ����Ʈ �迭�� ũ��� �޽����� ó�� 3����Ʈ�� ���Եȴ�.
                    iv = new Byte[ byteBuffer[ 0 ] ];         // initialization vector�� ���� �����
                    ciphertext = new Byte[ byteBuffer[ 1 ] ]; // ��ȣ��(ciphertext)�� ���� �����
                    signature = new Byte[ byteBuffer[ 2 ] ];  // ����(signature)�� ���� �����
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
                list.CopyTo( ndx, ciphertext, 0, byteBuffer[ 1 ] );         // ��ȣ�� ���ϱ�
                ndx += byteBuffer[ 1 ];
                list.CopyTo( ndx, signature, 0, byteBuffer[ 2 ] );          // ������ ���� �޽��� ���ϱ�


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

                // �޽����� ��ȣȭ�ϴµ� ���Ǵ� Ű�� �Ļ�
                byte[] aesKey = m_ECDH_Cng.DeriveKeyMaterial( m_ECDH_remote_publicKey );


                // �޽����� �ص��� �� �ִ� ����(key material)
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
                // ���۵� �޽����� �ٽ� ���� ������ ��ȯ 
                string message = Encoding.Unicode.GetString( plaintext );
                Display( "   " + message + "\n", OtherColor );

                if( 3 <= Version )
                    // ��ȣ�� �ؽ� �� ���� ����� ���Ͽ� ���� ��ȣ������ Ȯ��
                    using( ECDsaCng ecdsa = new ECDsaCng( m_ECDsa_Key ) )
                    {
                        ecdsa.HashAlgorithm = CngAlgorithm.Sha512;
                        if( !ecdsa.VerifyData( ciphertext, signature ) )
                            if( "Alice Green" == MyName || "Bob White" == MyName )
                                Display( "���� ���!! ( SECURITY WARNING ) "
                                       + "Ȯ�ε��� ���� ������ �޾ҽ��ϴ�.(Received signature did not verify)\n\n", 0 );
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
Ŭ����, �޼��� �Ǵ� ���� ���� �̸�                �뵵
----------------------------------------------------------------------------------------------------------------------------
CNG_SecureCommunicationExample          ������Ʈ ���� partial Ŭ�����Դϴ�.
----------------------------------------------------------------------------------------------------------------------------
Communicator                            ��� ��ȣȭ ����� ĸ��ȭ�ϴ� Ŭ�����Դϴ�. 
                                        �� Ŭ������ Alice, Bob �� Mallory ���� ��� �޽����� ó���մϴ�. 
                                        ChannelManager AppControl �޼��忡�� �����ϴ� �޽����� ó������ �ʽ��ϴ�.
----------------------------------------------------------------------------------------------------------------------------
m_DSKey                                 Ŭ���� �����Դϴ�.
m_ECDH_Cng
m_ECDH_local_publicKey_XML
m_ECDH_remote_publicKey
ChMgr
----------------------------------------------------------------------------------------------------------------------------
Communicator                            Communicator ��ü�� �����ϴ� �޼����Դϴ�.
----------------------------------------------------------------------------------------------------------------------------
Dispose                                 �������� ������ ���ҽ��� �����ϴ� �޼����Դϴ�.
----------------------------------------------------------------------------------------------------------------------------
StoreDSKey                              ������ ���� Ű�� �����ϴ� �޼����Դϴ�.
----------------------------------------------------------------------------------------------------------------------------
Send_or_Receive_PublicCryptoKey         Ű ��ȯ�� �����ϴ� �޼����Դϴ�.
----------------------------------------------------------------------------------------------------------------------------
iv                                      �Ϲ� �ؽ�Ʈ �޽����� ��ȣȭ�ϴ� �� ���Ǵ� ���� Ŭ���� �����Դϴ�. 
ciphertext                              �̷��� ������ ReceiveMessage() �޼��� ��ó�� ����Ǿ� �ֽ��ϴ�.
signature                                        
----------------------------------------------------------------------------------------------------------------------------
ReceiveMessage  ���� ������ ���� �Ϲ� �ؽ�Ʈ �Ǵ� ��ȣȭ�� �޽����� �޴� �޼����Դϴ�.
----------------------------------------------------------------------------------------------------------------------------
SendMessage �Ϲ� �ؽ�Ʈ�� �޾Ƶ��̰� ���� ������ ���� �Ϲ� �ؽ�Ʈ �Ǵ� ��ȣȭ�� �������� �����ϴ� �޼����Դϴ�.
----------------------------------------------------------------------------------------------------------------------------
*/
