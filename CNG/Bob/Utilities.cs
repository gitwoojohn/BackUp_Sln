using System;                           // IDisposable interface
using System.Diagnostics;               // Process control               
using System.Runtime.InteropServices;   // MoveWindow, GetConsoleWindow Win32 APIs

namespace Cryptography_Next_Generation
{
    public partial class CNG_SecureCommunicationExample
    {
        static string sep = "------------------------------------" +
                            "------------------------------------",
                      sep1 = sep + "\n", sep2 = "\n" + sep1;

        static string MyName;

        // Application control flags
        static int Version = 0;      // 1 - Plaintext, 2 - Encryption, 3 - Signing
                                     // 4 - Private digital signature, 5 - Termination
        static bool fVerbose;         // Display encrypted output           
        static bool fMallory;         // Disable Mallory


        [DllImport( "user32.dll" )]
        public static extern bool MoveWindow( IntPtr hwnd, int x, int y,
                                      int nWidth, int nHeight, bool bRepaint );
        [DllImport( "kernel32.dll" )]
        public static extern IntPtr GetConsoleWindow();


        // Run the Bob and Mallory executable.
        // If Alice.exe is a standalone process, it is not being run from
        // the Visual Studio debugger; therefore, it must programatically 
        // run Bob and Mallory.
        static bool Autoloader()
        {
            Process[] AliceProcess = Process.GetProcessesByName( "Alice" );
            if( AliceProcess.Length > 0 )
            {
                try
                {
                    Process.Start( "bob.exe" );
                    Process.Start( "mallory.exe" );
                }

                catch( Exception e )
                {
                    Display( "\nFailure loading Bob.exe or Mallory.exe:\n"
                           + e.Message + "\n", 1 );
                    return false;
                }
            }
            return true;
        }

        // Set console size, position, title
        static void InitConsole( string name, int left, int top )
        {
            Console.Clear();
            IntPtr hwnd = GetConsoleWindow();
            MoveWindow( hwnd, left, top, 600, 500, true );
            Console.SetBufferSize( 72, 500 );
            MyName = name;
            SplashScreen();
        }

        // Provide titling services
        static void SplashScreen()
        {
            Console.Clear();
            Display( "       Cryptography Next Generation " +
                    "Secure Communication Example\n\n" + sep );
            Console.Title = "Hi, I'm " + MyName + "!";
        }

        // Prompt user for input.
        // fBlankOkay = true if blank lines are acceptable
        static string ReadALine( bool fBlankOkay )
        {
            while( true )
            {
                Display( ":> ", 1 );
                Display( "", 3 );
                string s = Console.ReadLine();
                if( fBlankOkay )
                    return s;
                if( "" != s )
                    return s;
            }
        }

        // Read until a character from list is entered.
        // Do not accept a blank.
        static string ReadAChar( string options )
        {
            while( true )
            {
                string s = ReadALine( false );        // No blanks allowed.
                if( options.Contains( s ) )
                    return s;
            }
        }

        // Display session options, prompt for input
        static string InitializeOptions()
        {
            string s;
            if( "Alice Green" == MyName )
            {

                Display( "Please select a security model:\n\n" +
                        "1 = Plaintext only\n" +
                        "2 = Encrypt messages\n" +
                        "3 = Encrypt messages, use public key to digitally sign messages.\n" +
                        "4 = Encrypt messages, use private key to digitally sign messages.\n" +
                        "5 = Encrypt messages, use private key to digitally sign messages\n" +
                        "    and cryptographic keys. Terminate on all security failures.\n" +
                        "x = Exit\n\n", 1 );

                s = ReadAChar( "12345x" );
                if( "x" == s ) return "exit";

                Version = Int32.Parse( s );

                Display( "Include Mallory? Y/N\n", 1 );
                fMallory = "y" == ReadAChar( "ynYN" ) ? true : false;

                if( Version != 1 )
                {
                    Display( "Verbose output mode? Y/N\n", 1 );
                    fVerbose = "y" == ReadAChar( "ynYN" ) ? true : false;
                }
            }
            Console.Clear();
            Console.Title = "Hi, I'm " + MyName + "!";

            Console.Title += "   Security Version " + Version;

            Display( "       Cryptography Next Generation Secure Communication Example\n\n" +
                    "Security Version: " + Version +
                    "   Mode: " + ( fVerbose ? "Verbose" : "Regular" ) +
                    "   Mallory: " + ( fMallory ? "Yes" : "No" ) +
                    "   Signatures: " + ( Version >= 3 ? "Yes" : "No" ) );

            Display( sep2, 1 );

            return ( fVerbose ? "y" : "n" ) + ( fMallory ? "y" : "n" ) + Version;
        }

        // Calls 
        static void Display( string s )
        {
            Display( s, MyColor );
        }


        /////////////////////////////////////////////////////////////////////////////////////////
        // NAME         Display:     Ouput function that wraps Console.Write calls and
        //                           colors the output message.
        //
        // PARAMETERS:  string DisplayString:   Output message.
        //              int color:              Output message color.  If no color, use yellow.
        //
        //              COLOR       USE                   DESCRIPTION
        //              ------------------------------------------------------------------------
        //              0 Red       Security failures     Error message displayed to the user
        //              1 Yellow    Default color         Options menu and application messages
        //              2 White     Bob's color           Messages from Bob White
        //              3 Cyan      User's color          User input
        //              4 Green     Alice's color         Messages from Alice Green
        //              5 Majenta   Mallory's color       Messages from Mallory Purple
        //              6 Yellow    Default color         Application Restart prompt
        //              7 Gray      Encrypted data        
        //
        // RETURN:      string:     "r":    rerun the application
        //                          "exit": close the application
        //
        // OPERATION:   1.  Set ConsoleColor to color. If color is not specified,
        //                  the default yellow color is set.  
        //              2.  Write the DisplayString in the requested color.
        //              3.  Process Continue functionality if color = 6.
        //                  Display("", 6) will generate a prompt, and query the user
        //                  to continue the application.
        //
        /////////////////////////////////////////////////////////////////////////////////////////

        static void Display( string DisplayString, int color )
        {
            ConsoleColor[] cc = { ConsoleColor.Red, ConsoleColor.Yellow,
                              ConsoleColor.White, ConsoleColor.Cyan,
                              ConsoleColor.Green, ConsoleColor.Magenta,
                              ConsoleColor.Yellow,ConsoleColor.Gray
                            };
            Console.ForegroundColor = cc[ color ];
            Console.Write( DisplayString );

            if( 6 == color )
            {
                Console.WriteLine( sep + sep +
                      "Press the enter key to continue\n\n" );
                Console.ReadLine();
            }
        }

    }       // End Utilities.cs: public partial class CNG_SecureCommunicationExample
}