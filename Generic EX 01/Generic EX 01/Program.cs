using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class TestGenericList
    {
        static void Main()
        {
            // int is the type argument
            GenericList<int> list = new GenericList<int>();

            for( int x = 0; x < 10; x++ )
            {
                list.AddHead( x );
            }

            foreach( int i in list )
            {
                System.Console.Write( i + " " );
            }
            System.Console.WriteLine( "\nDone" );
        }
    }

    // type parameter T in angle brackets
    public class GenericList<T>
    {
        // The nested class is also generic on T.
        private class Node
        {
            // T used in non-generic constructor.
            public Node( T t )
            {
                next = null;
                data = t;
            }

            private Node next;
            public Node Next
            {
                get { return next; }
                set { next = value; }
            }

            // T as private member data type.
            private T data;

            // T as return type of property.
            public T Data
            {
                get { return data; }
                set { data = value; }
            }
        }

        private Node head;

        // constructor
        public GenericList()
        {
            head = null;
        }

        // T as method parameter type:
        public void AddHead( T t )
        {
            Node n = new Node( t );
            n.Next = head;
            head = n;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;

            while( current != null )
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }

}
