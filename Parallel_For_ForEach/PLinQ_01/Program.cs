using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main( string[] args )
    {
        while( true )
        {
            var cd = new ConcurrentDictionary<int, int>();
            var sw = Stopwatch.StartNew();
            cd.TryAdd( 42, 0 );
            for( int i = 1; i < 10000000; i++ )
            {
                cd.TryUpdate( 42, i, i - 1 );
            }
            Console.WriteLine( sw.Elapsed );
        }

    //    var resources = new ThreadLocal<BigResource>(
    //() => new BigResource(), trackAllValues: true );
    //    var tasks = inputData
    //        .Select( i => Task.Run( () => Process( i, resources.Value ) ) )
    //        .ToArray();
    //    Task.WaitAll( tasks );
    //    foreach( var resource in resources.Values ) resource.Dispose();
    }
}

internal class BigResource
{
    public BigResource() { }
    internal void Dispose()
    {
        throw new NotImplementedException();
    }
}