using System;                           // Required for the IDisposable interface
using System.Text;                      // Required for the Encoding class

namespace Cryptography_Next_Generation
{
    public partial class CNG_SecureCommunicationExample
    {
        static int MyColor = 2;                        // Bob White displays white text
        static int OtherColor = 4;                     // Alice Green displays green text
        static void Main( string[] args )
        {
            InitConsole( "Bob White", 610, 5 );
            string s = "";
            while( true )
            {
                SplashScreen();
                s = AppControl( "receive", "BobControlChannel" );
                if( "exit" == s )
                    break;
                Run();
            }
        }  // End Main
        static void Run()
        {
            ASCIIEncoding enc = new ASCIIEncoding();

            InitializeOptions();

            System.Threading.Thread.Sleep( 200 );
            string NewChannelName = ReceiveChannelName();

            string s;

            using( Communicator Bob = new Communicator( "client", NewChannelName ) )
            {
                Display( "Hi, I'm Bob White: My sales associate is Alice Green.\n" +
                        "I think she has a new customer contact for me!\n\n" );

                if( 3 <= Version )
                {
                    Display( "\nFirst, Alice will publicaly send me a digital signature key.\n" );
                    Byte[] DSKey = Bob.ChMgr.ReadMessage();                      // Read the message
                    Bob.StoreDSKey( DSKey );

                    s = enc.GetString( DSKey );
                    if( fVerbose )
                    {
                        Display( "Here it is:\n\n" );
                        Display( s + "\n\n", 4 );
                    }
                }


                if( 4 <= Version )
                {
                    byte[] DSKey;
                    using( ChannelManager ChMgr2 = new ChannelManager( "client", "PrivateChannel" ) )
                        DSKey = ChMgr2.ReadMessage();
                    Bob.StoreDSKey( DSKey );

                    Display( "\nNow Alice privately sent me a digital signature key. I will use it instead.\n" );
                    s = enc.GetString( DSKey );
                    if( fVerbose )
                    {
                        Display( "Here it is:\n\n" );
                        Display( s + "\n\n", 4 );
                    }
                }

                //-------------------------------------------------------------------------------------------------

                if( 2 <= Version )
                {
                    Display( sep2, 1 );
                    Display( "Now we will exchange our public cryptographic\n" +              // Send and Receive ECDH public keys
                            "keys through a public channel.\n" +
                            "First, Alice will send me her key.\n\nListening...\n" );

                    if( !Bob.Send_or_Receive_PublicCryptoKey( "receive", OtherColor ) )
                        return;

                    Display( sep2, 1 );
                    Display( "Next, I will send my public cryptographic key to Alice:\n\nSending...\n" );
                    Bob.Send_or_Receive_PublicCryptoKey( "send", MyColor );
                }

                //-------------------------------------------------------------------------------------------------
                // Now that all the keys have been transmitted, have a conversation.
                if( 1 < Version )
                {
                    Display( sep2, 1 );
                    Display( "Now that our keys have been exchanged,\n" +
                           "we can have an encrypted conversation:\n\n", 2 );
                    Display( sep1, 1 );
                }

                Bob.ReceiveMessage();       // Read a message
                Bob.SendMessage( "Hi Alice. That is good news. Please send it to me.", true );
                Bob.ReceiveMessage();       // Read a message
                Bob.SendMessage( "Thanks, I'll arrange to meet with them.", true );
                //-------------------------------------------------------------------------------------------------
                if( Version <= 2 )
                    Display( sep1, 1 );

                if( !fVerbose && Version >= 3 )
                    Display( sep1, 1 );

                Display( "Would you like to talk to me?\n" +
                        "If yes, go to Alice's window\n" +
                        "and follow the instructions.\n\n", 1 );
                Display( sep + sep1, 1 );
                s = "";

                while( true )
                {
                    s = Bob.ReceiveMessage();       // Read a message
                    if( "" == s )
                        break;
                    s = ReadALine( true );
                    if( !Bob.SendMessage( s, false ) )                // If Alice entered CTRL-C or SYS_CLOSE
                        break;
                    if( "" == s )
                        break;
                }
            } // End using (Communicator Bob)
                     
            Display( sep1, 1 );
        } // End Run method
                        
    } // End public partial class CNG_SecureCommunicationExample

}
