using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_For_StopBreak_02
{
    class Program
    {
        static readonly object _sync = new object();

        // 매번 프라임 카운트 틀리게 나오는 버그
        static IEnumerable<long> GetPrimeNumber_1( long num )
        {
            List<long> primeList = new List<long>();

            Parallel.For( 0, num + 1, ( i ) =>
            {
                bool isPrime = true;
                Parallel.For( 2, i, ( j, loopState ) =>
                {
                    if( i % j == 0 )
                    {
                        isPrime = false;
                        loopState.Break();
                    }
                } );

                if( isPrime )
                {
                    primeList.Add( i );
                }
            } );

            return primeList;
        }
        // ConcurrentBag를 이용해서 수정
        static IEnumerable<long> GetPrimeNumber_2( long num )
        {
            ConcurrentBag<long> primeList = new ConcurrentBag<long>();

            Parallel.For( 0, num + 1, ( i ) =>
            {
                bool isPrime = true;
                Parallel.For( 2, i, ( j, loopState ) =>
                {
                    if( i % j == 0 )
                    {
                        isPrime = false;
                        loopState.Break(); // Stop 변경 가능
                    }
                } );

                if( isPrime )
                {
                    primeList.Add( i );
                }
            } );

            return primeList;
        }

        static IEnumerable<long> GetPrimeNumber( long num )
        {
            List<long> primeList = new List<long>();

            Parallel.For<List<long>>( 0, num + 1,
                () => new List<long>(), //스레드 로컬 변수 초기화
                ( i, outerLoopState, subList ) =>
                {
                    bool isPrime = true;
                    Parallel.For( 2, i, ( j, loopState ) =>
                    {
                        if( i % j == 0 )
                        {
                            isPrime = false;
                            loopState.Break();
                        }
                    } );

                    if( isPrime )
                    {
                        subList.Add( i ); //스레드 로컬 리스트에 추가
                    }

                    return subList;
                },
            ( subList ) => //각 스레드 종료 후에 취합
            {
                lock ( _sync )
                {
                    primeList.AddRange( subList );
                }
            } );

            return primeList;
        }
        static void Main( string[] args )
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IEnumerable<long> primeList = GetPrimeNumber( 99999 );
            sw.Stop();

            Console.WriteLine( $"Elapsed : {sw.ElapsedMilliseconds / 1000d}초, Found prime counts : {primeList.Count()}" ); //,     sw.Elapsed.ToString(), primeList.Count() );

            FileIteration_1(@"C:\Temp");
            Console.ReadLine();
            FileIteration_2(@"C:\Temp");

            Console.ReadLine();
            //뭔가 한다.
        }
        struct FileResult
        {
            public string Text;
            public string FileName;
        }
        // Use Directory.GetFiles to get the source sequence of file names.
        /*
            첫 번째 쿼리에서는 GetFiles 메서드를 사용하여 디렉터리 및 
            모든 하위 디렉터리에 있는 파일 이름 배열을 채웁니다.
        
            이 메서드는 전체 배열이 채워질 때까지 반환하지 않으므로 
            작업의 시작 부분에서 지연이 발생할 수 있습니다.
            
            그러나 배열이 채워진 후 PLINQ는 아주 빨리 작업을 병렬로 처리할 수 있습니다.
        */
        public static void FileIteration_1( string path )
        {
            var sw = Stopwatch.StartNew();
            int count = 0;
            string[] files = null;
            try
            {
                files = Directory.GetFiles( path, "*.*", SearchOption.AllDirectories );
            }
            catch( UnauthorizedAccessException )
            {
                //Console.WriteLine( "You do not have permission to access one or more folders in this directory tree." );
                //return;
            }

            catch( FileNotFoundException )
            {
                Console.WriteLine( "The specified directory {0} was not found.", path );
            }

            var fileContents = from file in files.AsParallel()
                               let extension = Path.GetExtension( file )
                               where extension == ".txt" || extension == ".htm"
                               let text = File.ReadAllText( file )
                               select new FileResult { Text = text, FileName = file }; //Or ReadAllBytes, ReadAllLines, etc.              

            try
            {
                foreach( var item in fileContents )
                {
                    Console.WriteLine( Path.GetFileName( item.FileName ) + ":" + item.Text.Length );
                    count++;
                }
            }
            catch( AggregateException ae )
            {
                ae.Handle( ( ex ) =>
                {
                    if( ex is UnauthorizedAccessException )
                    {
                        //Console.WriteLine( ex.Message );
                        return true;
                    }
                    return false;
                } );
            }

            Console.WriteLine( "FileIteration_1 processed {0} files in {1} milliseconds", count, sw.ElapsedMilliseconds );
        }
        // Use Directory.EnumerateDirectories and EnumerateFiles to get the source sequence of file names.
        /*
            결과를 반환하기 시작하는 정적 EnumerateDirectories 및 EnumerateFiles 메서드를 사용합니다.

            이 방법은 처리 시간을 첫 번째 예제와 비교해 볼 때 여러 요인에 따라 달라질 수 있지만 
            큰 디렉터리 트리에 대해 반복하는 경우 첫 번째 예제보다 더 빠를 수 있습니다.
        */
        public static void FileIteration_2( string path ) //225512 ms
        {
            var count = 0;
            var sw = Stopwatch.StartNew();
            var fileNames = from dir in Directory.EnumerateFiles( path, "*.*", SearchOption.AllDirectories )
                            select dir;


            var fileContents = from file in fileNames.AsParallel() // Use AsOrdered to preserve source ordering
                               let extension = Path.GetExtension( file )
                               where extension == ".txt" || extension == ".htm"
                               let Text = File.ReadAllText( file )
                               select new { Text, FileName = file }; //Or ReadAllBytes, ReadAllLines, etc.
            try
            {
                foreach( var item in fileContents )
                {
                    Console.WriteLine( Path.GetFileName( item.FileName ) + ":" + item.Text.Length );
                    count++;
                }
            }
            catch( AggregateException ae )
            {
                ae.Handle( ( ex ) =>
                {
                    if( ex is UnauthorizedAccessException )
                    {
                        //Console.WriteLine( ex.Message );
                        return true;
                    }
                    return false;
                } );
            }

            Console.WriteLine( "FileIteration_2 processed {0} files in {1} milliseconds", count, sw.ElapsedMilliseconds );
        }
    }
}
/*
리플렉터로 코드 본 결과  this._size++

// List<T> 코드 리플렉트
public void Add(T item)
{
    if (this._size == this._items.Length)
    {
        this.EnsureCapacity(this._size + 1);
    }
    this._items[this._size++] = item;
    this._version++;
}

// ConcurrentBag 코드 리플렉트
private void AddInternal(ThreadLocalList<T> list, T item)
{
    bool lockTaken = false;
    try
    {
        Interlocked.Exchange(ref list.m_currentOp, 1);
        if ((list.Count < 2) || this.m_needSync)
        {
            list.m_currentOp = 0;
            Monitor.Enter(list, ref lockTaken);
        }
        list.Add(item, lockTaken);
    }
    finally
    {
        list.m_currentOp = 0;
        if (lockTaken)
        {
            Monitor.Exit(list);
        }
    }
}
*/
