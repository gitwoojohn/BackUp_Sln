using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 다음 예제에서는 키에 부울 값을 사용하여 결과를 
// 두 개의 그룹으로 나누는 방법을 보여 줍니다. 

// group 절의 하위 식에서 값이 생성됩니다.

namespace Linq_Group_Example
{
    class GroupSample
    {
        // The element type of the data source.
        public class Student
        {
            public string First { get; set; }
            public string Last { get; set; }
            public int ID { get; set; }
            public List<int> Scores;
        }
        public static List<Student> GetStudents()
        {
            // Use a collection initializer to create the data source. Note that each element
            //  in the list contains an inner sequence of scores.
            List<Student> students = new List<Student>
        {
           new Student {First="Svetlana", Last="Omelchenko", ID=111, Scores= new List<int> {97, 72, 81, 60}},
           new Student {First="Claire", Last="O'Donnell", ID=112, Scores= new List<int> {75, 84, 91, 39}},
           new Student {First="Sven", Last="Mortensen", ID=113, Scores= new List<int> {99, 89, 91, 95}},
           new Student {First="Cesar", Last="Garcia", ID=114, Scores= new List<int> {72, 81, 65, 84}},
           new Student {First="Debra", Last="Garcia", ID=115, Scores= new List<int> {97, 89, 85, 82}} 
        };
            return students;
        }

        static void Main( string[] args )
        {
            // Obtain the data source.
            List<Student> students = GetStudents();

            // Group by true or false.
            // Query variable is an IEnumerable<IGrouping<bool, Student>>
            var booleanGroupQuery =
                from student in students
                group student by ( student.Scores.Average() >= 80 ) into g
                orderby g.Key descending
                select g; //pass or fail!
                
            // Execute the query and access items in each group
            foreach (var studentGroup in booleanGroupQuery)
            {
                Console.WriteLine( studentGroup.Key == true ? "High averages" : "Low averages" );
                foreach (var student in studentGroup)
                {
                    Console.WriteLine( "   {0}, {1}:{2}", 
                        student.Last, student.First, student.Scores.Average() );
                }
            }
            // Keep the console window open in debug mode.
            Console.WriteLine( "Press any key to exit." );
            Console.ReadKey();
        }
    }
}

/* Output:  
  High averages
   Mortensen, Sven:93.5
   Garcia, Debra:88.25
  Low averages
   Omelchenko, Svetlana:77.5
   O'Donnell, Claire:72.25
   Garcia, Cesar:75.5
*/
