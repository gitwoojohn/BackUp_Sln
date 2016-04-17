using System;                           // IDisposable interface
using System.Diagnostics;               // Process control               
using System.Runtime.InteropServices;   // MoveWindow, GetConsoleWindow Win32 APIs

namespace Cryptography_Next_Generation
{
    public partial class CNG_SecureCommunicationExample
    {
        static string sep = "----------------------------------------" +
                            "----------------------------------------",
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
            //MoveWindow( hwnd, left, top, 600, 500, true );
            //Console.SetBufferSize( 72, 500 );
            MoveWindow( hwnd, left, top, 760, 500, true );
            Console.SetBufferSize( 90, 600 );
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

                Display( "\n보안 모델을 선택 하세요( Please select a security model ):\n\n" +
                        "1 = 평문 텍스트( Plaintext only )\n\n" +
                        "2 = 메시지 암호화( Encrypt messages )\n\n" +
                        "3 = 메시지 암호화, 공개키를 사용해서 메시지들을 디지털 서명 합니다.\n" +
                        "    Encrypt messages, use public key to digitally sign messages.\n\n" +

                        "4 = 메시지 암호화, 개인키를 사용해서 메시지들을 디지털 서명 합니다.\n" +
                        "    Encrypt messages, use private key to digitally sign messages.\n\n" +

                        "5 = 메시지 암호화, 개인 키와 암호화 키를 사용해서 메시지들을 디지털 서명 합니다.\n" +
                        "    개인키를 사용해서 메시지들을 디지털 서명 합니다.\n" +
                        "    Encrypt messages, use private key to digitally sign messages.\n" +
                        "    and cryptographic keys. Terminate on all security failures.\n\n" +
                        "x = 종료( Exit )\n\n", 1 );

                s = ReadAChar( "12345x" );
                if( "x" == s ) return "exit";

                Version = int.Parse( s );

                Display( "Mallory를 포함 시키겠습니까?(Include Mallory?) Y/N\n", 1 );
                fMallory = "y" == ReadAChar( "ynYN" ) ? true : false;

                if( Version != 1 )
                {
                    Display( "상세한 출력 모드 사용(Verbose output mode) Y/N\n", 1 );
                    fVerbose = "y" == ReadAChar( "ynYN" ) ? true : false;
                }
            }
            Console.Clear();
            Console.Title = "Hi, I'm " + MyName + "!";

            Console.Title += "   보안 버전( Security Version ) " + Version;

            Display( "      Cryptography Next Generation 통신 보안 예제" +
                    "       Cryptography Next Generation Secure Communication Example\n\n" +
                    "보안 버전( Security Version ): " + Version +
                    "   Mode: " + ( fVerbose ? "Verbose" : "Regular" ) +
                    "   Mallory: " + ( fMallory ? "Yes" : "No" ) +
                    "   서명(Signatures): " + ( Version >= 3 ? "Yes" : "No" ) );

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


/*

----------------------------------------------------------------------------------------------------------------------------
클래스, 메서드 또는 전역 변수 이름                                    용   도 
----------------------------------------------------------------------------------------------------------------------------
CNG_SecureCommunicationExample          프로젝트 수준 partial 클래스입니다.
----------------------------------------------------------------------------------------------------------------------------
Version                                 전역 변수입니다.
fVerbose
fMallory
----------------------------------------------------------------------------------------------------------------------------
Autoloader                              Alice에서 Bob.exe 및 Mallory.exe 응용 프로그램을 
                                        로드하기 위해 호출하는 메서드입니다.
----------------------------------------------------------------------------------------------------------------------------
InitConsole                             사용자 인터페이스 메뉴 및 응용 프로그램 수준 메시지를 처리하는 메서드입니다.
----------------------------------------------------------------------------------------------------------------------------
SplashScreen                            콘솔 창 제목을 제공하는 메서드입니다.
----------------------------------------------------------------------------------------------------------------------------
ReadALine                               콘솔에서 사용자가 입력한 줄을 읽는 유틸리티 메서드입니다.
----------------------------------------------------------------------------------------------------------------------------
ReadAChar                               질문을 표시하고 사용자가 예 또는 아니요로 대답하게 하는 유틸리티 메서드입니다.
----------------------------------------------------------------------------------------------------------------------------
InitializeOptions                       여러 옵션을 표시하고 사용자가 선택하게 하는 메서드입니다. 
                                        또한 이 메서드는 Version, fVerbose 및 fMallory 전역 플래그를 설정합니다.
----------------------------------------------------------------------------------------------------------------------------
Display(string s)                       두 Display 메서드 중 첫 번째입니다. 
                                        이 메서드는 문자열과 MyColor 변수를 두 번째 Display 메서드에 전달합니다.
----------------------------------------------------------------------------------------------------------------------------
Display                                 두 Display 메서드 중 두 번째입니다. 
(string DisplayString, int color)       이 메서드는 Console.WriteLine 문을 래핑하고 출력 줄에 색을 지정할 수 있게 합니다.
----------------------------------------------------------------------------------------------------------------------------

*/
