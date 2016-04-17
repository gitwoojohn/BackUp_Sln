using System;                           // Required for the IDisposable interface
using System.Text;                      // Required for the Encoding class
using System.IO;                        // Required for the MemoryStream class
using System.IO.Pipes;                  // Required for the NamedPipeClientStream class


namespace Cryptography_Next_Generation
{
    public partial class CNG_SecureCommunicationExample
    {

        // Synchronize session options with the Bob and Mallory applications.
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
                    Version = Int32.Parse( options.Substring( 2, 1 ) );
                }
            }
            return "";
        }

        // Convert the message string into a Unicode byte array.
        // Create temporary ChannelManager object.
        // Send the new channel name.
        // This method demonstrates a bad corporate security policy
        //  and allows Mallory to substitute his own new channel name.

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
            byte[] byteBuffer = new byte[ 1000 ];     // Only big enough for small messages

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