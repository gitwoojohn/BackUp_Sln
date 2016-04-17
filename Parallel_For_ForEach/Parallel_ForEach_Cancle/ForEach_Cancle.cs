using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_ForEach_Cancle
{
    class ForEach_Cancle
    {
        static void Main( string[] args )
        {
            int[] nums = Enumerable.Range( 0, 10000000 ).ToArray();
            CancellationTokenSource cts = new CancellationTokenSource();

            // Use ParallelOptions instance to store the CancellationToken
            // ParallelOptions에 CancellationToken 인스턴스를 저장
            ParallelOptions po = new ParallelOptions();            
            po.CancellationToken = cts.Token;

            // 현재 시스템의 프로세서 개수 알아내기
            po.MaxDegreeOfParallelism = Environment.ProcessorCount;

            Console.WriteLine( "Press any key to start. Press 'c' to cancel." );
            Console.ReadKey();

            // Run a task so that we can cancel from another thread.
            Task.Factory.StartNew( () =>
            {
                if( Console.ReadKey().KeyChar == 'c' )
                    cts.Cancel();
            } );

            try
            {
                Parallel.ForEach( nums, po, ( num ) =>
                {
                    double d = Math.Sqrt( num );
                    Console.WriteLine( "Math.Sqrt : {0},  스레드 아이디 : {1}", d, Thread.CurrentThread.ManagedThreadId );
                    try
                    {
                        po.CancellationToken.ThrowIfCancellationRequested();
                    }
                    catch( OperationCanceledException e )
                    {

                        Console.WriteLine("작업 중지 요청 : {0}", e.Message);
                    }
                    catch(ObjectDisposedException e)
                    {
                        Console.WriteLine( "Disposed 에러 : {0}", e.Message );
                    }
                } );
            }
            catch( OperationCanceledException e )
            {
                Console.WriteLine( e.Message );
            }
            Console.WriteLine("\n===== Press any Key exit =====");
            Console.ReadKey();
        }
    }
}
