using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cancellation_Token_ReadFile
{
    class Cancellation_Token
    {
        static void Main( string[] args )
        {
            Cancel_Token();
            //PLinQ_AsParallel();
            //PLinQ_Cancel();

            Console.ReadKey();
        }
 
        public static void PLinQ_Cancel()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Console.WriteLine( "끝내려면 아무키나 누르세요\n" );
            Thread.Sleep( 1500 );

            int[] nums = Enumerable.Range( 1, 10000 ).ToArray();


            Task task = Task.Factory.StartNew( () =>
            {
                nums = SimpleParallelTask( nums, cts.Token );
            }, cts.Token );

            Console.Read();
            cts.Cancel();
            Console.WriteLine( "-------------------------------------" );
            try
            {
                task.Wait();
            }
            catch( AggregateException ae )
            {
                if( ae.InnerException != null )
                    Console.WriteLine( $"예외 처리 : { ae.InnerException}." );
            }
        }

        public static int[] SimpleParallelTask( int[] source, CancellationToken token )
        {

            Func<int, int> square = ( num ) =>
            {
                Console.Write( "\t" + Task.CurrentId );
                Thread.Sleep( 10 );
                return num * num;
            };
            try
            {
                return source.AsParallel().WithCancellation( token )
                        .WithDegreeOfParallelism( 3 ).Select( square ).ToArray();
            }
            catch( OperationCanceledException e )
            {
                Console.WriteLine( e.Message );
            }

            return source;
        }

        private static void PLinQ_AsParallel()
        {
            int[] nums = Enumerable.Range( 1, 1000 ).ToArray();
            Func<int, int> square = ( num ) =>
             {
                 Console.Write( "\t" + Task.CurrentId );
                 Thread.Sleep( 10 );
                 return num * num;
             };
            nums = nums.AsParallel().Select( square ).ToArray();
            Console.ReadLine();
        }

        private static void Cancel_Token()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            ParallelOptions parallelOptions = new ParallelOptions
            {
                CancellationToken = cts.Token
            };

            // 취소시 메세지 등록
            cts.Token.Register( () => Console.WriteLine( "Cancelling..." ) );
            Console.WriteLine( "엔터키를 누르면 취소 합니다." );

            try
            {
                IEnumerable<string> files = Directory.GetFiles( @"C:\Music", "*", SearchOption.AllDirectories );
                List<string> fileList = new List<string>();
                Console.WriteLine( $"파일 개수 : {files.Count()}" );

                Task task = Task.Factory.StartNew( () =>
                {
                    try
                    {
                        Parallel.ForEach( files, parallelOptions,
                         ( file ) =>
                         {
                             FileInfo fileinfo = new FileInfo( file );
                             if( fileinfo.Exists )
                             {
                                 if( fileinfo.Length >= 10000000 )
                                 {
                                     fileList.Add( fileinfo.Name );
                                 }
                             }
                         } );
                    }
                    catch( OperationCanceledException ) { }
                } );
                // 취소 대기
                Console.Read();
                // 취소 실행
                cts.Cancel();
                task.Wait();

                foreach( var file in fileList )
                {
                    Console.WriteLine( file );
                }
                Console.WriteLine( $"총 파일 개수 : {fileList.Count()}" );
            }
            catch( UnauthorizedAccessException )
            {
            }
        }
    }
    public static class WithCancellation_Class
    {
        public static Task<T> WithCancellation<T>( this Task<T> task, CancellationToken cancellationToken )
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            var cancellationRegistration = cancellationToken.Register(
                () => taskCompletionSource.TrySetCanceled() );
            task.ContinueWith(
                t =>
                {
                    cancellationRegistration.Dispose();
                    if( t.IsCanceled )
                    {
                        taskCompletionSource.TrySetCanceled();
                    }
                    else if( t.IsFaulted )
                    {
                        taskCompletionSource.TrySetException( t.Exception.InnerException );
                    }
                    else
                    {
                        taskCompletionSource.TrySetResult( t.Result );
                    }
                } );

            return taskCompletionSource.Task;
        }
    }
}
