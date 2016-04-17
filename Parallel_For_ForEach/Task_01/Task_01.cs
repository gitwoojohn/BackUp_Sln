using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task_01
{
    class Task_01
    {
        static void Main( string[] args )
        {
            //Exam_01();
            Exam_02();
        }

        private static void Exam_02()
        {
            Task<string> task = Task.Factory.StartNew(
                () =>
                {
                    long sum = 0;
                    for( long i = 0; i < 100000001; i++ )
                    {
                        sum += i;
                    }
                    return sum.ToString();
                } );

            foreach( char busySymbol in BusySymbols() )
            {
                if( task.IsCompleted )
                {
                    Console.WriteLine( '\b' );
                    break;
                }
                Console.WriteLine( busySymbol );
            }

            Console.WriteLine();
            //여기서 추가 스레드가 끝날 때 까지 기다린다.
            Console.WriteLine( task.Result );
            System.Diagnostics.Trace.Assert( task.IsCompleted );
        }
        private static IEnumerable<char> BusySymbols()
        {
            string busySymbols = @"-\|/-\|/";
            int next = 0;
            while( true )
            {
                yield return busySymbols[ next ];
                next = ( ++next ) % busySymbols.Length;
                yield return '\b';
            }
        }

        private static void Exam_01()
        {
            const int max = 10000;

            //현재 작업중인 스레드외에 추가로 스레드를 생성
            Task task = new Task( () =>
            {
                for( int count = 0; count < max; count++ )
                {
                    Console.Write( "|" );
                }
                Console.WriteLine( "추가 쓰레드 끝" );
            } );

            //추가 스레드 시작
            task.Start();

            //현재 작업중인 스레드에서도 반복문 시작
            for( int count = 0; count < max; count++ )
            {
                Console.Write( "-" );
            }
            Console.WriteLine( "메인 쓰레드 끝" );
            //혹시 현재 스레드가 빨리 끝나더라도,
            //추가 스레드가 끝날 때 까지 기다리기.            
            task.Wait();
        }
    }
}
