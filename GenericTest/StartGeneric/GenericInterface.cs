using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartGeneric
{
    // Type parameter T in angle brackets.
    public class GenericList<T> : IEnumerable<T>
    {
        protected Node head;
        protected Node current = null;

        // Nested class is also generic on T
        protected class Node
        {
            public Node next;
            private T data;  // T as private member 데이터형

            public Node( T t )  // T 비 제네릭 생성자 사용
            {
                next = null;
                data = t;
            }

            public Node Next
            {
                get { return next; }
                set { next = value; }
            }

            public T Data  // T as 속성 타입을 반환
            {
                get { return data; }
                set { data = value; }
            }
        }

        public GenericList()  // 생성자
        {
            head = null;
        }

        public void AddHead( T t )  // T as 메소드 파라메터
        {
            Node n = new Node( t );
            n.Next = head;
            head = n;
        }

        // Implementation of the iterator
        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            while( current != null )
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        // IEnumerable<T>는 IEnumerable로 부터 상속을 받는다. 이 클래스가 상속을 받으면
        // 제네릭과 비제네릭 둘다 GetEnumerator를 반드시 구현해야 한다.
        // 대부분의 경우 비제네릭 메소드를 간단하게 제네릭 메소드를 호출 할 수 있다.

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class SortedList<T> : GenericList<T> where T : IComparable<T>
    {
        // 목록의 요소를 낮은 것부터 높은것 까지 정렬 하는 간단하고, 최적화 되지않은 알고리즘.
        public void BubbleSort()
        {
            if( null == head || null == head.Next )
            {
                return;
            }
            bool swapped;

            do
            {
                Node previous = null;
                Node current = head;
                swapped = false;

                while( current.next != null )
                {
                    // 이 메소드를 호출할 필요가 있으므로 SortedList 클래스를 IEnumerable<T>에 제약 시킴
                    if( current.Data.CompareTo( current.next.Data ) > 0 )
                    {
                        Node tmp = current.next;
                        current.next = current.next.next;
                        tmp.next = current;

                        if( previous == null )
                        {
                            head = tmp;
                        }
                        else
                        {
                            previous.next = tmp;
                        }
                        previous = tmp;
                        swapped = true;
                    }
                    else
                    {
                        previous = current;
                        current = current.next;
                    }
                }
            } while( swapped );
        }
    }

    // 형식 인수를 사용해서 IComparable<T>를 구현하는 간단한 클래스입니다.
    // 일반적인 List에 저장되는 객체의 일반적인 디자인 패턴입니다.
    public class Person : IComparable<Person>
    {
        string name;
        int age;

        public Person( string s, int i )
        {
            name = s;
            age = i;
        }

        // This will cause list elements to be sorted on age values.
        public int CompareTo( Person p )
        {
            return age - p.age;
        }

        public override string ToString()
        {
            return name + ":" + age;
        }

        // Must implement Equals.
        // IComparable 제약 조건시 Equals 반드시 오버라이드         
        public bool Equals( Person p )
        {
            return ( age == p.age );
        }
    }

}
