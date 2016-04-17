using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Parallel_Get_Directory
{
    class Traverse_Directory
    {
        static void Main( string[] args )
        {
            TraverseTreeParallelForEach( @"C:\Windows", ( f ) =>
             {
                 // 디렉터리를 읽어 들이는 코드입니다.
                 // 예외도 읽어 들임
                 try
                 {
                     byte[] data = File.ReadAllBytes( f );
                 }
                 catch( UnauthorizedAccessException e )
                 {
                     Console.WriteLine( e.Message );
                 }
                 // 파일 이름 출력
                 Console.WriteLine( f );
             } );
            
            Console.ReadKey();
        }

        private static void TraverseTreeParallelForEach( string root, Action<string> action )
        {
            // 파일개수와 걸린 시간 출력
            int fileCount = 0;
            var sw = Stopwatch.StartNew();

            // 시스템의 물리 CPU 개수 파악
            int procCount = Environment.ProcessorCount;

            // 서브폴더와 파일을 스택에 저장
            Stack<string> dirs = new Stack<string>();

            if(!Directory.Exists(root))
            {
                throw new ArgumentException();
            }
            dirs.Push( root );

            while( dirs.Count > 0 )
            {
                string currentDir = dirs.Pop();
                string[] subDirs = null;
                string[] files = null;

                // 하위 디렉터리 알아내기
                try
                {
                    subDirs = Directory.GetDirectories( currentDir );
                }
                catch( UnauthorizedAccessException e )
                {
                    Console.WriteLine( e.Message );
                }
                catch( DirectoryNotFoundException e )
                {
                    Console.WriteLine( e.Message );
                    continue;
                }

                // 인수로 넘겨받은 폴더내의 모든 파일 개수 알아내기
                try
                {
                    files = Directory.GetFiles( currentDir );
                }
                catch( UnauthorizedAccessException e )
                {
                    Console.WriteLine( e.Message );

                }
                catch( DirectoryNotFoundException e )
                {
                    Console.WriteLine( e.Message );
                }

                // 파일 개수가 시스템 물리 CPU 카운터 보다 작다면 일반적인 출력
                try
                {
                    if(files.Length < procCount)
                    {
                        foreach( var file in files )
                        {
                            action( file );
                            fileCount++;
                        }                       
                    }
                    // 위와 반대
                    else
                    {
                        Parallel.ForEach( files, () => 0, ( file, loopState, localCount ) =>
                         {
                             action( file );
                             return ( int )++localCount;
                         },
                        ( localCount ) =>
                        {
                            Interlocked.Exchange( ref fileCount, localCount + fileCount ); //fileCount + c ); //.Add( ref fileCount, c );
                        } );
                    }
                }
                catch( AggregateException ae )
                {
                    ae.Handle( ( ex ) =>
                     {
                         if( ex is UnauthorizedAccessException )
                         {
                             Console.WriteLine( ex.Message );
                             return true;
                         }

                        // 다른 예외를 여기에서 더 추가 처리( innerException )
                        return false;
                     } );

                    foreach( string str in subDirs )
                    {
                        dirs.Push( str );
                    }
                }
                Console.WriteLine( "\n === Processed {0} files in {1} sec ===", fileCount, (sw.ElapsedMilliseconds / 1000f));
            }
        }
    }
}
