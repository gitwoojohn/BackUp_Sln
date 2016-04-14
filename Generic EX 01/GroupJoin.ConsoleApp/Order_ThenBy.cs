using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupJoin.ConsoleApp
{
    class Order_ThenBy
    {
        // The element type of the data source.
        public class Student
        {
            public string First { get; set; }
            public string Last { get; set; }
            public int ID { get; set; }
        }

        public static List<Student> GetStudents()
        {
            // Use a collection initializer to create the data source. Note that each element
            //  in the list contains an inner sequence of scores.
            List<Student> students = new List<Student>
            {
               new Student {First="Svetlana", Last="Omelchenko", ID=111},
               new Student {First="Claire", Last="O'Donnell", ID=112},
               new Student {First="Sven", Last="Mortensen", ID=113},
               new Student {First="Desar", Last="Garcia", ID=114},
               new Student {First="Debra", Last="Garcia", ID=115}
            };
            return students;
        }
        public void Multiple_OrderBy()
        {
            // Create the data source.
            List<Student> students = GetStudents();

            // Now create groups and sort the groups. The query first sorts the names
            // of all students so that they will be in alphabetical order after they are
            // grouped. The second orderby sorts the group keys in alpha order.            
            var sortedGroups =
                from student in students
                orderby student.Last, student.First
                group student by student.Last[ 0 ] into newGroup
                orderby newGroup.Key
                select newGroup;

            // Execute the query.
            Console.WriteLine( Environment.NewLine + "sortedGroups:" );
            foreach( var studentGroup in sortedGroups )
            {
                Console.WriteLine( studentGroup.Key );
                foreach( var student in studentGroup )
                {
                    Console.WriteLine( "   {0}, {1}", student.Last, student.First );
                }
            }

            string[] fruits = { "grape", "passionfruit", "banana", "apple",
                      "orange", "raspberry", "mango", "blueberry" };

            // Sort the strings first by their length and then 
            // alphabetically by passing the identity selector function.
            IEnumerable<string> query =
                fruits.AsQueryable()
                .OrderBy( fruit => fruit.Length ).ThenBy( fruit => fruit );

            foreach( string fruit in query )
                Console.WriteLine( fruit );


        }
    }
}