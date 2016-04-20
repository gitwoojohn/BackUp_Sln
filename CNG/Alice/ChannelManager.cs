using System;                           // Required for the IDisposable interface
using System.Text;                      // Required for the Encoding class
using System.IO;                        // Required for the MemoryStream class
using System.IO.Pipes;                  // Required for the NamedPipeClientStream class


namespace Cryptography_Next_Generation
{
    public partial class CNG_SecureCommunicationExample
    {

        // 밥과 말로리의 응용프로그램의 세션 옵션을 동기화 합니다.
        static string AppControl( string mode, string data )
        {
            if( "send" == mode )
            {
                using( ChannelManager ChMgr = new ChannelManager( "server",
                                                     "BobControlChannel" ) )
                    ChMgr.SendMessage( Encoding.Unicode.GetBytes( data ) );

                if( fMallory || "exit" == data )
                    using( ChannelManager ChMgr = new ChannelManager( "server",
                                                         "MalloryControlChannel" ) )
                        ChMgr.SendMessage( Encoding.Unicode.GetBytes( data ) );
            }

            else
            {
                using( ChannelManager ChMgr = new ChannelManager( "client", data ) )
                {
                    byte[] byteBuffer = ChMgr.ReadMessage();
                    string options = Encoding.Unicode.GetString( byteBuffer );
                    if( "exit" == options ) return "exit";
                    fVerbose = options.Substring( 0, 1 ) == "y" ? true : false;
                    fMallory = options.Substring( 1, 1 ) == "y" ? true : false;
                    Version = int.Parse( options.Substring( 2, 1 ) );
                }
            }
            return "";
        }

        // 유니코드 바이트 배열에 메시지 문자열을 변환합니다.
        // 임시 ChannelManager 개체를 만듭니다.
        // 새로운 채널 이름을 전송합니다.
        // 이 예제는 보안정책이 좋지 않은 기업을 보여주고 
        // Mallory는 자신의 새로운 채널 이름을 허용 할 수 있습니다.
        static void SendChannelName( string name )
        {
            byte[] AB_Channel = Encoding.Unicode.GetBytes( name );
            using( ChannelManager ChMgr = new ChannelManager( "server", "PublicChannel" ) )
                ChMgr.SendMessage( AB_Channel );
        }

        static string ReceiveChannelName()
        {
            using( ChannelManager ChMgr = new ChannelManager( "client", "PublicChannel" ) )
                return Encoding.Unicode.GetString( ChMgr.ReadMessage() );
        }


        // Pipe server or client class 
        internal sealed class ChannelManager : IDisposable
        {
            NamedPipeServerStream m_ServerPipe;
            NamedPipeClientStream m_ClientPipe;
            Stream m_Stream;

            string m_ChannelType;                   // "client" or "server"
            string m_ChannelName;
            byte[] byteBuffer = new byte[ 1000 ];   // Only big enough for small messages

            public ChannelManager( string ChannelType, string ChannelName )
            {
                m_ChannelType = ChannelType;
                m_ChannelName = ChannelName;

                if( "server" == ChannelType )
                {
                    m_ServerPipe = new NamedPipeServerStream( ChannelName,
                            PipeDirection.InOut, 1,
                            PipeTransmissionMode.Message );
                    m_Stream = m_ServerPipe;
                    m_ServerPipe.WaitForConnection();
                }
                else
                {
                    m_ClientPipe = new NamedPipeClientStream( ChannelName );
                    m_Stream = m_ClientPipe;
                    m_ClientPipe.Connect();
                    m_ClientPipe.ReadMode = PipeTransmissionMode.Message;
                }
            }

            public void Dispose()
            {
                if( m_Stream != null )
                    ( m_Stream as IDisposable ).Dispose();
            }

            public byte[] ReadMessage()
            {
                MemoryStream ms = new MemoryStream();
                int count;

                do
                {
                    count = m_Stream.Read( byteBuffer, 0, byteBuffer.Length );
                    if( count > 0 )
                        ms.Write( byteBuffer, 0, count );
                } while( count == byteBuffer.Length );
                byte[] bA = ms.ToArray();
                return bA;
            }

            public bool SendMessage( byte[] msg )
            {
                try
                {
                    m_Stream.Write( msg, 0, msg.Length );
                }
                catch( IOException e )
                {
                    Display( "Connection has been closed.\n" + e + "\n", MyColor );
                    return false;
                }
                return true;
            }
        }   // End class ChannelManager

    }       // End ChannelManager.cs: public partial class CNG_SecureCommunicationExample
}

/*
----------------------------------------------------------------------------------------------------------------------------
클래스, 메서드 또는 전역 변수 이름                 용  도
----------------------------------------------------------------------------------------------------------------------------
CNG_SecureCommunicationExample          프로젝트 수준 partial 클래스입니다.
----------------------------------------------------------------------------------------------------------------------------
AppControl                              내부 응용 프로그램을 제어하고 세 콘솔 응용 프로그램을 동기화하는 메서드입니다.
                                        Alice에서는 이 메서드를 사용하여 프로그램 옵션(Version 및 fVerbose 플래그)을 
                                        Bob과 Mallory에 전송합니다. AppControl은 메시징 메서드가 아닙니다. 
                                        해당 내용은 암호화되거나 서명되지 않으며 Communicator 클래스는 호출되지 않습니다.
----------------------------------------------------------------------------------------------------------------------------    
SendChannelName                         PublicChannel 명명된 파이프에서 AliceAndBobChannel 및 AliceAndBobChannel1 
ReceiveChannelName                      명명된 파이프로 전환하는 데 사용되는 메서드입니다. 이러한 메서드를 통해 
                                        CNG 예제 개요에서 설명하는 보안 문제를 의도적으로 발생시킬 수 있습니다.
----------------------------------------------------------------------------------------------------------------------------
ChannelManager  응용 프로그램의 프로세스 간 통신 프레임워크를 제공하는 클래스입니다.
----------------------------------------------------------------------------------------------------------------------------
*/
