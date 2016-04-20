using System;                           // Required for the IDisposable interface
using System.Text;                      // Required for the Encoding class
using System.Security.Cryptography;     // Required for the CNG APIs

namespace Cryptography_Next_Generation
{
    public partial class CNG_SecureCommunicationExample
    {
        // Global variables
        static int MyColor = 4;                // Alice는 녹색으로 표시
        static int OtherColor = 2;             // Bob은 흰색으로 표시

        static void Main( string[] args )
        {
            if( !Autoloader() )                // Bob과 Mallory 자동 실행
                return;                        // 에러 발생시 다시 로드

            string s = "";
            InitConsole( "Alice Green", 5, 5 );  //5, 5
            while( true )
            {
                SplashScreen();                // 실행 창 이름 표시
                s = InitializeOptions();       // 세션 옵션 구하기
                AppControl( "send", s );       // Bob과 Mallory 동기화
                if( "exit" == s )              // "x" 입력 종료
                    break;
                Run();                         // 세션 실행
            }
        } // End Main
          //---------------------------------------------------------------------------------------

        static void Run()
        {
            string NewChannelName = "AliceAndBobChannel";
            SendChannelName( NewChannelName );    // 회사 보안 정책에 문제가 있음.

            // 테스트 문자를 Bob에게 전송
            Display( "Hi, I'm Alice Green. My sales associate is Bob White.\n" +
                    "I need to send him a customer order right now!\n\n" );

            // Alice와 Bob이 통신을 하기 위한 파이프 라인 연결 및 옵션과 암호키 생성
            using( Communicator Alice = new Communicator( "server", NewChannelName ) )
            {
                string s;

                CngKeyCreationParameters keyCreateParms = new CngKeyCreationParameters();
                keyCreateParms.ExportPolicy = CngExportPolicies.AllowPlaintextExport;

                if( 3 <= Version )              // 공개 디지털 서명키 전송
                {
                    using( CngKey DSKey = CngKey.Create( CngAlgorithm.ECDsaP521, null, keyCreateParms ) )
                    {
                        // 제네릭 개인 키 내보내기
                        byte[] dsKeyBlob = DSKey.Export( CngKeyBlobFormat.GenericPrivateBlob ); //Pkcs8PrivateBlob );

                        // 내보내진 개인키를 저장
                        Alice.StoreDSKey( dsKeyBlob );

                        s = Encoding.ASCII.GetString( dsKeyBlob );
                        //Display( "\nFirst, I will send Bob a digital signature key "
                        //        + "over a public channel.\n" +
                        //        ( fVerbose ? "Here it is:\n\n" + s + "\n\n" : "" ) );

                        Display( "\n제일 먼저, 공개 채널로 Bob에게 디지털 서명된 키를 전송 "
                                +
                                ( fVerbose ? "디지털 서명된 공개 키:\n\n" + s + "\n\n" : "" ) );

                        // 디지털 서명된 키를 Bob에게 전송
                        Alice.ChMgr.SendMessage( dsKeyBlob );
                    }
                }

                if( 4 <= Version )  // 서명된 개인 디지털 키 전송
                {
                    using( CngKey DSKey = CngKey.Create( CngAlgorithm.ECDsaP521, null, keyCreateParms ) )
                    using( ChannelManager ChMgr2 = new ChannelManager( "server", "PrivateChannel" ) )
                    {
                        byte[] dsKeyBlob = DSKey.Export( CngKeyBlobFormat.GenericPrivateBlob ); //.Pkcs8PrivateBlob );
                        Alice.StoreDSKey( dsKeyBlob );
                        s = Encoding.ASCII.GetString( dsKeyBlob );
                        //Display( "\nNow I will send Bob a secret digital signature key " +
                        //        "over a private channel.\n" +
                        //        ( fVerbose ? "Here it is:\n\n" + s + "\n\n" : "" ) );
                        Display( "\n밥(Bob)에게 비공개 채널을 통해 비밀 디지털 서명 키를 전송합니다.\n" +
                                ( fVerbose ? "비밀 디지털 서명 키:\n\n" + s + "\n\n" : "" ) );
                        ChMgr2.SendMessage( dsKeyBlob );
                    }
                }

                if( 2 <= Version )   // 공개 암호 키를 송신과 수신
                {
                    Display( sep2, 1 );
                    //Display( "Now we will exchange our public cryptographic\n" +
                    //        "keys through a public channel\n" +
                    //        "First, I'll send Bob my key.\n\nSending...\n" );
                    Display( "제일 먼저 밥(Bob)에게 내(Alice) 공개 키를 보냅니다.\n" +
                            "공개 채널을 통해 밥과 앨리스의 공개 키를 교환 합니다.\n" +
                            "\n\n전송중...\n" );
                    Alice.Send_or_Receive_PublicCryptoKey( "send", MyColor );


                    Display( sep2, 1 );
                    Display( "밥(Bob)은 공개 채널로 나(Alice)에게 밥의 공개 키를 전송합니다.:\n\nListening...\n" );
                    if( !Alice.Send_or_Receive_PublicCryptoKey( "receive", OtherColor ) )
                    {
                        Display( "", 6 );
                        return;
                    }
                }

                //-----------------------------------------------------------------------------------
                // Now that all the keys have been transmitted, have a conversation.

                if( 1 < Version )
                {
                    Display( sep2, 1 );
                    Display( "공개키가 교환되었고 밥과 앨리스는\n" +
                           "이제 암호화된 대화를 할 수 있습니다.:\n\n", 4 );
                    Display( sep1, 1 );
                }

                Alice.SendMessage( "Hi Bob. I have a new customer contact.", true );
                Alice.ReceiveMessage();
                Alice.SendMessage( "Coho Winery, 111 AnyStreet, Chicago", true );
                Alice.ReceiveMessage();

                if( Version <= 2 )
                    Display( sep1, 1 );

                if( !fVerbose && Version >= 3 )
                    Display( sep1, 1 );

                //-----------------------------------------------------------------------------------
                Display( "밥과 대화 하시겠습니까?\n" +
                        "메시지를 입력하고 Enter를 누르세요.\n" +
                       "그렇지 않다면 엔터키를 누르세요.\n\n", 1 );
                Display( sep + sep1, 1 );
                s = " ";


                while( true )
                {
                    s = ReadALine( true );
                    if( !Alice.SendMessage( s, false ) )    // If Bob entered CTRL-C, or SYS_CLOSE
                        break;
                    if( "" == s )                        // If user entered ""
                        break;
                    s = Alice.ReceiveMessage();         // Read a message
                    if( "" == s )
                        break;
                }

                //-----------------------------------------------------------------------------------
            }   // End using ChannelManager, End using Communicator
        }       // End Run
    }           // End Alice.cs: public partial class CNG_SecureCommunicationExample
}
/*
*** 클래스 ***
 - 각 프로젝트에는 세 개의 클래스가 들어 있습니다.

    public partial class CNG_SecureCommunicationExample
    이 클래스는 Alice, Bob 및 Mallory 응용 프로그램에 포함된 네 개의 프로젝트 파일 모두에 걸쳐 있습니다.
    컴파일 후 CNG_SecureCommunicationExample 클래스에는 프로젝트 파일 네 개의 클래스, 변수 및 메서드가 모두 들어 있습니다. 
    partial 클래스에 대한 자세한 내용은 Partial 클래스 및 메서드(C# 프로그래밍 가이드)를 참조하십시오.
    
    internal sealed class ChannelManager
    이 클래스는 명명된 파이프를 지원합니다.각 프로젝트는 프로그램 실행 도중 서로 다른 시점에 몇 개의 ChannelManager 인스턴스를 만듭니다.
    이 클래스에 대한 자세한 내용은 ChannelManager 클래스의 코드 분석( CNG 예제 )을 참조하십시오.

    internal sealed class Communicator
    이 클래스는 암호화 및 암호 해독을 지원합니다.
    각 프로젝트는 프로그램 실행 도중 서로 다른 시점에 몇 개의 Communicator 인스턴스를 만듭니다.
    이 클래스에 대한 자세한 내용은 Communicator 클래스의 코드 분석( CNG 예제 )을 참조하십시오
*/
