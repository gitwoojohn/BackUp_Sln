using System;
using System.Collections.Generic;
using System.Text;

namespace GenericMethodExam
{
    public class Employee
    {
        private string name;
        private int id;

        public Employee() { }

        public Employee( string s, int i )
        {
            name = s;
            id = i;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
    }

    // 제약 조건 클래스 사용 
    public class GenericList<T> where T : Employee
    {
        private class Node
        {
            private Node next;
            //private Node prev;
            private T data;

            public Node( T t )
            {
                next = null;
                //prev = null;
                data = t;
            }

            public Node Next
            {
                get { return next; }
                set { next = value; }
            }
            //public Node Prev
            //{
            //    get { return prev; }
            //    set { prev = value; }
            //}

            public T Data
            {
                get { return data; }
                set { data = value; }
            }
        }

        private Node head;
        //private Node tail;

        public GenericList() //constructor
        {
            head = null;
            //tail = null;
        }

        public void AddHead( T t )
        {
            Node n = new Node( t );
            n.Next = head;
            head = n;
        }
        //public void AddTail(T t)
        //{
        //    Node p = new Node( t );
        //    p.Prev = tail;
        //    tail = p;
        //}

        public IEnumerator<T> GetEnumerator()
        {
            Node current = null;

            if( head != null )
            {
                current = head;
            }
            else
            {
                //current = tail;
            }

            while( current != null )
            {
                yield return current.Data;
                if( head != null )
                {
                    current = current.Next;
                }
                else
                {
                    //current = current.Prev;
                }
            }
        }

        // 제약 조건 사용시
        public T FindFirstOccurrence( string s )
        {
            Node current = head;
            T t = null;

            while( current != null )
            {
                //The constraint enables access to the Name property.
                if( current.Data.Name == s )
                {
                    t = current.Data;
                    break;
                }
                else
                {
                    current = current.Next;
                }
            }
            return t;
        }
    }
}
