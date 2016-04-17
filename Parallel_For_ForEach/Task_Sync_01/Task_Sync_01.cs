using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_Sync_01
{
    class Task_Sync_01
    {
        // sync 잠금객체 - static해야 하며 ref 해야함.(값 형식 불가 Only One이 아닌 복사본 때문)
        readonly static object sync = new object();

        static readonly int count = 10000000;
        static int sum = 0;
        static void IncreaseByOne()
        {
            for( int i = 0; i < count; i++ )
            {
                sum += 1;
            }
        }
        
        static void Random_Number()
        {
            Task t = Task.Run( () => {
                Random rnd = new Random();
                long sum = 0;
                int n = 1000000;
                for( int ctr = 1; ctr <= n; ctr++ )
                {
                    int number = rnd.Next( 0, 101 );
                    sum += number;
                }
                Console.WriteLine( "Total:   {0:N0}", sum );
                Console.WriteLine( "Mean:    {0:N2}", sum / n );
                Console.WriteLine( "N:       {0:N0}", n );
            } );
            t.Wait();
        }
        static void Main( string[] args )
        {
            // Manual_Monitor_Lock, Auto_Lock와 개별 실행
            //Task task1 = Task.Factory.StartNew( IncreaseByOne );
            //for( int i = 0; i < count; i++ )
            //{
            //    sum -= 1;
            //}

            //task1.Wait();
            //Console.WriteLine( $"Result = {sum}\n" );

            // 랜던 난수
            //Random_Number();

            // 수동 Lock
            //Manual_Monitor_Lock();

            // 자동 Lock
            //Auto_Lock();

            // 더 편하고 더 빠른 방법 Interlocked.Increment
            InterLock_Increment();
            Console.ReadLine();
        }
        // ------------ InterLock 시작 ---------------
        static void IncreaseByOne_InterLock()
        {
            for( int i = 0; i < count; i++ )
            {
                Interlocked.Increment( ref sum );
            }
        }
        static void InterLock_Increment()
        {
            Task task = Task.Factory.StartNew( IncreaseByOne_InterLock );

            for( int i = 0; i < count; i++ )
            {
                Interlocked.Decrement( ref sum );
            }

            task.Wait();
            Console.WriteLine( $"Result = {sum}" );
        }
        // ------------ InterLock 끝 ------------------

        // ----------- Auto Lock ----------------------
        static void IncreaseByOne_AutoLock()
        {
            for( int i = 0; i < count; i++ )
            {
                lock ( sync )
                {
                    sum += 1;
                }
            }
        }
        private static void Auto_Lock()
        {
            Task task = Task.Factory.StartNew( IncreaseByOne_AutoLock );

            for( int i = 0; i < count; i++ )
            {
                lock ( sync )
                {
                    sum -= 1;
                }
            }

            task.Wait();
            Console.WriteLine( "Result = {0}", sum );
        }
        // ----------------- Auto Lock 끝 ---------------------

        // ----------- Manual Lock 시작 -----------------------    
        static void IncreaseByOne_Sync()
        {
            for( int i = 0; i < count; i++ )
            {
                bool locked = false;
                Monitor.Enter( sync, ref locked );
                try
                {
                    sum += 1;
                }
                finally
                {
                    if( locked )
                    {
                        Monitor.Exit( sync );
                    }
                }
            }
        }
        static void Manual_Monitor_Lock()
        {
            // 원하는 결과 값 0 출력
            Task task2 = Task.Factory.StartNew( IncreaseByOne_Sync );

            for( int i = 0; i < count; i++ )
            {
                /*
                락을 걸고 해제하는 대상이 되는 sync라는 객체를 보면, 
                readonly이므로 객체의 값을 변경시킬 수 없는 immutable한 객체이고, 
                static이므로 유일하게 존재하게 됩니다. 
                복사본이 없고, 값을 변경시킬 수 없는 객체에 대해서 락을 거는 것이 안전한 방법
                */
                bool locked = false;
                Monitor.Enter( sync, ref locked );
                try
                {
                    sum -= 1;
                }
                finally
                {
                    if( locked )
                    {
                        Monitor.Exit( sync );
                    }
                }
            }
            task2.Wait();
            Console.WriteLine( $"Result = {sum}" );
        }
        // ----------------- Manual Lock 끝 -------------------------
    }
}
