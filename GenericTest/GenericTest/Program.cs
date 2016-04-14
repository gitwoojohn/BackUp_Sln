using System.Collections.Generic;
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        // 보수 구하기
        int[] p = { 0, 0x111, 0xff00 };
        foreach( var v in p )
        {
            Console.WriteLine( "~0x{0:x8} = 0x{1:x8}", v, ~v );
        }

        // T 연산자
        string[] arr = { "0", "1", "2", "3", "4" };
        List<int> list = new List<int>();

        for( int x = 5; x < 10; x++ )
        {
            list.Add( x );
        }


        ProcessItems( arr );
        ProcessItems( list );

        string[] strArr = { "D", "a", "B", "C" };
        List<string> oItem = new List<string>( strArr );
        oItem.Sort();
        foreach( var inItem in oItem )
        {
            Console.WriteLine( inItem.ToString() );
        }

        // Conver List To Array,  Array to List
        ListToArray();


        // 타입별 배열 반환 받기
        object[] inputs = { 1f, 2f, 3f };
        float[] array = GetValue<float[]>( inputs );
        float singleValue = GetValue<float>( inputs );

        //
        Console.ReadLine();
    }
    static void ListToArray()
    {
        // String array
        string[] strArray1 = new string[] { "one", "two", "three", "four", "five" };
        int[] intArray1 = new int[] { 1, 2, 3, 4, 5 };

        // Convert with new List constructor.
        List<string> strList1 = new List<string>( strArray1 );
        string[] strArray2 = strArray1.ToArray();

        // Convert Toggle Array
        List<int> intList1 = intArray1.ToList();
        int[] intArray2 = intList1.ToArray();
    }

    static void ProcessItems<T>( IList<T> coll )
    {
        // IsReadOnly returns True for the array and False for the List.
        Console.WriteLine( "IsReadOnly returns {0} for this collection.", coll.IsReadOnly );

        // List는 에러가 생기지 않고 배열은 에러가 생김.
        if( !coll.IsReadOnly )
        {
            coll.RemoveAt( 4 );
        }

        foreach( T item in coll )
        {
            Console.Write( item.ToString() + " " );
        }
        Console.WriteLine();
    }

    // 
    static T GetValue<T>( object[] inputs )
    {
        if( typeof( T ).IsArray )
        {
            Type elementType = typeof( T ).GetElementType();
            Array array = Array.CreateInstance( elementType, inputs.Length );
            inputs.CopyTo( array, 0 );
            T obj = ( T )( object )array;
            return obj;
        }
        else
        {
            return ( T )inputs[ 0 ];
            // will throw on 0-length array, check for length == 0 and return default(T)
            // if do not want exception
        }
    }
}

