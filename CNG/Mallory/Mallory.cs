using System;                           // Required for the IDisposable interface
using System.Text;                      // Required for the Encoding class

namespace Cryptography_Next_Generation
{
    public partial class CNG_SecureCommunicationExample
    {
        static int MyColor = 5;                        // Bob White displays white text
        static int OtherColor = 4;                     // Alice Green displays green text
        static bool fDisplaySendMessage = true;        // Boolean flag controls output display visibility

        static void Main( string[] args )
        {
            InitConsole( "Mallory Purple", 310, 490 );
            string s = "";
            while( true )
            {
                SplashScreen();
                s = AppControl( "receive", "MalloryControlChannel" );
                if( "exit" == s ) break;
                Run();
            }
        }   // End Main
        static void Run()
        {
            ASCIIEncoding enc = new ASCIIEncoding();

            InitializeOptions();

            Display( "Hi, I'm Mallory, the man in the middle.\n" +
                    "I wonder what Alice and Bob are talking about.\n" +
                    "I think I'll listen in.\n\n" );

            string AliceChannelName = ReceiveChannelName();
            string BobChannelName = AliceChannelName + "1";
            SendChannelName( BobChannelName );

            //-------------------------------------------------------------------------------------------------


            using( Communicator MalloryAlice = new Communicator( "client", AliceChannelName ) )
            using( Communicator MalloryBob = new Communicator( "server", BobChannelName ) )
            {
                string s;
                fDisplaySendMessage = true;

                if( 3 <= Version )
                {
                    Display( "\nI know Alice will publicaly send Bob a digital signature key.\n" );
                    byte[] DSKey = MalloryAlice.ChMgr.ReadMessage();                      // Read the message
                    MalloryAlice.StoreDSKey( DSKey );
                    MalloryBob.StoreDSKey( DSKey );

                    s = enc.GetString( DSKey );
                    if( fVerbose )
                    {
                        Display( "Here it is:\n\n" );
                        Display( s + "\n\n", 4 );
                    }
                    MalloryBob.ChMgr.SendMessage( DSKey );
                }

                if( 2 <= Version )
                {
                    Display( sep, 1 );
                    Display( "Alice and Bob are going to exchange their\n" +
                            "public cryptographic keys through a public channel.\n" +  // Send and Receive ECDH public keys
                            "First, Alice will send Bob her key.\n\n" );

                    MalloryAlice.Send_or_Receive_PublicCryptoKey( "receive", 4 );
                    Display( "Good.  I just intercepted Alice's public key: \n" );
                    Display( "Next, I will send my MalloryAlice public cryptographic key to Alice:\n\nSending...\n" );
                    MalloryAlice.Send_or_Receive_PublicCryptoKey( "send", 5 );

                    Display( "Next, I will send my MalloryBob public cryptographic key to Bob:\n\nSending...\n" );
                    MalloryBob.Send_or_Receive_PublicCryptoKey( "send", 5 );
                    Display( "Now I will receive Bob's public key: \n" );
                    if( !MalloryBob.Send_or_Receive_PublicCryptoKey( "receive", 2 ) )
                    {
                        Display( "I've been discovered!\n\n", 0 );
                        System.Threading.Thread.Sleep( 3000 );
                        return;
                    }
                    Display( "Good.  I just intercepted Bob's public key: \n" );
                }

                //-------------------------------------------------------------------------------------------------
                // Now that all the keys have been transmitted, have a conversation.

                if( 1 < Version )
                {
                    Display( sep, 1 );
                    Display( "Now that they have exchanged their keys,\n" +
                           "they can have a secure conversation:\n\n", 5 );
                    Display( sep, 1 );
                }

                Display( "From Alice:\n", 7 );
                OtherColor = 4;
                s = MalloryAlice.ReceiveMessage();

                Display( "To Bob:\n", 7 );
                MalloryBob.SendMessage( s, true );

                Display( "From Bob:\n", 7 );
                OtherColor = 2;
                s = MalloryBob.ReceiveMessage();

                Display( "To Alice:\n", 7 );
                MalloryAlice.SendMessage( s, true );

                Display( "From Alice:\n", 7 );
                OtherColor = 4;
                string NewSalesContact = MalloryAlice.ReceiveMessage();

                Display( "To Bob:\n", 7 );
                string FakeMessage = "Coho Winery, OneEleven EveryStreet, Chicago";
                MalloryBob.SendMessage( FakeMessage, true );

                Display( "From Bob:\n", 7 );
                OtherColor = 2;
                MalloryBob.ReceiveMessage();

                Display( "To Alice:\n", 7 );
                MalloryAlice.SendMessage( "I think the address is wrong, but I'll keep trying", true );

                // -------------   Gloat   ------------
                if( !fVerbose && Version >= 3 )
                    Display( sep1, 1 );

                Display( "\nI am so clever!\n\n" +
                       "Here is what I received: " + NewSalesContact + "\n" +
                       "and here is what I sent: " + FakeMessage + "\n\n" +
                       "They will never catch me!\n\n\n" );

                //-------------------------------------------------------------------------------------------------
                if( Version <= 2 )
                    Display( sep1, 1 );

                Display( "Perhaps they will talk some more?\n" +
                       "I'll listen in on what they say\n" +
                       "and add a few extra characters to their messages\n\n" );
                Display( sep + sep1, 1 );
                s = "";

                while( true )
                {
                    Display( "From Alice:\n", 7 );
                    OtherColor = 4;
                    s = MalloryAlice.ReceiveMessage();
                    if( "" == s )
                    {
                        MalloryBob.SendMessage( s, true );
                        break;
                    }

                    Display( "Send to Bob:\n", 7 );
                    if( !MalloryBob.SendMessage( s + " qwerty", true ) )        // If Alice entered CTRL-C, or SYS_CLOSE
                        break;

                    Display( "From Bob:\n", 7 );
                    OtherColor = 2;
                    s = MalloryBob.ReceiveMessage();

                    Display( "Send to Alice:\n", 7 );
                    if( "" != s )
                        s += " ytrewq";

                    if( !MalloryAlice.SendMessage( s, true ) )                  // If Alice entered CTRL-C, or SYS_CLOSE
                        break;

                    if( "" == s )
                        break;
                }
            }        // End using (Communicator MalloryAlice, Communicator MalloryBob)
                     //-------------------------------------------------------------------------------------------------
        }       // End Run

    }    // public partial class CNG_SecureCommunicationExample
}
