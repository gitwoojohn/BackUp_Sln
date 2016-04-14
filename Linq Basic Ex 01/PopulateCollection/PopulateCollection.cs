using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 메모리 내 데이터나 파일 시스템의 데이터를 아직 데이터베이스에 있는 
// 데이터와 조인하지 마십시오. 

// 데이터베이스 쿼리 및 다른 소스 형식에 대해 조인 작업을 정의할 수 있는 방식이 
// 다양하기 때문에 이러한 도메인 간 조인은 정의되지 않은 결과를 발생시킬 수 있습니다. 

//또한 이러한 작업은 데이터베이스의 데이터 양이 아주 큰 경우 
//메모리 부족 예외를 일으킬 수 있는 위험이 있습니다. 

// 데이터베이스의 데이터를 메모리 내 데이터에 조인하려면 먼저 
// 데이터베이스 쿼리에서 ToList 또는 ToArray를 호출한 다음 반환된 컬렉션에서 조인을 수행합니다.

namespace PopulateCollection
{
    // 다음 예제에서는 명명된 형식인 Student를 사용하여 .csv 형식으로 
    // 스프레드시트 데이터를 시뮬레이션하는 문자열의 두 메모리 내 컬렉션에서 
    // 병합된 데이터를 저장하는 방법을 보여 줍니다. 

    // 첫 번째 문자열 컬렉션은 학생 이름과 ID를 나타내며 두 번째 컬렉션은 
    // 첫 번째 열의 학생 ID 및 4개의 시험 점수를 나타냅니다. ID는 외래 키로 사용됩니다.
    class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ID { get; set; }
        public List<int> ExamScores { get; set; }
    }

    class PopulateCollection
    {
        static void Main()
        {
            // These data files are defined in How to: Join Content from 
            // Dissimilar Files (LINQ).

            // Each line of names.csv consists of a last name, a first name, and an
            // ID number, separated by commas. For example, Omelchenko,Svetlana,111
            string[] names = System.IO.File.ReadAllLines( @"../../../names.csv" );

            // Each line of scores.csv consists of an ID number and four test 
            // scores, separated by commas. For example, 111, 97, 92, 81, 60
            string[] scores = System.IO.File.ReadAllLines( @"../../../scores.csv" );

            // Merge the data sources using a named type.
            // var could be used instead of an explicit type. Note the dynamic
            // creation of a list of ints for the ExamScores member. We skip 
            // the first item in the split string because it is the student ID, 
            // not an exam score.
            IEnumerable<Student> queryNamesScores =
                from nameLine in names
                let splitName = nameLine.Split( ',' )
                from scoreLine in scores
                let splitScoreLine = scoreLine.Split( ',' )
                where splitName[2] == splitScoreLine[0]
                select new Student()
                {
                    FirstName = splitName[0],
                    LastName = splitName[1],
                    ID = Convert.ToInt32( splitName[2] ),
                    ExamScores = ( from scoreAsText in splitScoreLine.Skip( 1 )
                                   select Convert.ToInt32( scoreAsText ) ).
                                  ToList()
                };

            // Optional. Store the newly created student objects in memory
            // for faster access in future queries. This could be useful with
            // very large data files.
            List<Student> students = queryNamesScores.ToList();

            // Display each student's name and exam score average.
            foreach (var student in students)
            {
                Console.WriteLine( "The average score of {0} {1} is {2}.",
                    student.FirstName, student.LastName,
                    student.ExamScores.Average() );
            }

            Console.WriteLine( "\n=======================================\n " );

            // select 절에서 개체 이니셜라이저는 두 소스의 데이터를 사용하여 
            // 새 Student 개체를 각각 인스턴스화하는 데 사용됩니다.

            // 쿼리 결과를 저장할 필요가 없는 경우 익명 형식이 명명된 형식보다 더 편리할 수 있습니다. 
            // 쿼리가 실행되는 메서드 외부에 쿼리 결과를 전달하는 경우에는 명명된 형식이 필요합니다. 

            // 다음 예제에서는 명명된 형식 대신 익명 형식을 사용하여 위의 예제와 동일한 작업을 수행합니다.

            // Merge the data sources by using an anonymous type.
            // Note the dynamic creation of a list of ints for the
            // ExamScores member. We skip 1 because the first string
            // in the array is the student ID, not an exam score.
            var queryNamesScores2 =
                from nameLine in names
                let splitName = nameLine.Split( ',' )
                from scoreLine in scores
                let splitScoreLine = scoreLine.Split( ',' )
                where splitName[2] == splitScoreLine[0]
                select new
                {
                    First = splitName[0],
                    Last = splitName[1],
                    ExamScores = ( from scoreAsText in splitScoreLine.Skip( 1 )
                                   select Convert.ToInt32( scoreAsText ) )
                                  .ToList()
                };

            // Display each student's name and exam score average.
            foreach (var student in queryNamesScores2)
            {
                Console.WriteLine( "The average score of {0} {1} is {2}.",
                    student.First, student.Last, student.ExamScores.Average() );
            }
            //Keep console window open in debug mode
            Console.WriteLine( "Press any key to exit." );
            Console.ReadKey();
        }
    }
}
/* Output: 
    The average score of Omelchenko Svetlana is 82.5.
    The average score of O'Donnell Claire is 72.25.
    The average score of Mortensen Sven is 84.5.
    The average score of Garcia Cesar is 88.25.
    The average score of Garcia Debra is 67.
    The average score of Fakhouri Fadi is 92.25.
    The average score of Feng Hanying is 88.
    The average score of Garcia Hugo is 85.75.
    The average score of Tucker Lance is 81.75.
    The average score of Adams Terry is 85.25.
    The average score of Zabokritski Eugene is 83.
    The average score of Tucker Michael is 92.
 */