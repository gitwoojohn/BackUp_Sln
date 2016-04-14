using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace ObservableController_App
{
    class Person
    {
        private string _Name;
        private int _Age;

        public Person( string Name, int Age )
        {
            _Name = Name;
            _Age = Age;
        }
    }
    // 특수화 클래스
    class People : ObservableCollection<Person>
    {
        public People()
        {
            CollectionChanged += LogChange;
        }
        /// <summary>
        /// 클래스의 데이터가 추가, 삭제, 변경 또는 이동되거나 전체 목록이 새로 고쳐질 때 동작 .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LogChange( object sender, NotifyCollectionChangedEventArgs e )
        {
            // e.Action -> NotifyCollectionChangedAction.Add/Remove/Replace/Move/Reset..
            Console.WriteLine( "CollectionChanged : reason={0}", e.Action.ToString() );
            var people = sender as ObservableCollection<Person>;
            foreach( var person in people )
            {
                Console.WriteLine( "{0}", person );
            }
        }
    }
    class Program
    {
        static void Main( string[] args )
        {
            var people = new People()
            {
                new Person("준환", 42),
                new Person("신영", 38),
                new Person("서연", 11),
                new Person("은서", 5)
            };

            foreach( var person in people )
            {
                Console.WriteLine( "{0}", person );
            }

            Console.WriteLine( "가족을 추가합니다.." );
            people.Add( new Person( "가은", 7 ) );

            Console.WriteLine( "가족을 삭제합니다.." );
            people.RemoveAt( people.Count - 1 );

            Console.WriteLine( "가족을 클리어합니다..." );
            people.Clear();

            Console.ReadLine();
        }
    }
}
