using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointStrings
{
    class JoinStrings
    {
        static void Main()
        {
            // Join content from dissimilar files that contain
            // related information. File names.csv contains the student
            // name plus an ID number. File scores.csv contains the ID 
            // and a set of four test scores. The following query joins
            // the scores to the student names by using ID as a
            // matching key.

            string[] names = System.IO.File.ReadAllLines( @"../../../names.csv" );
            string[] scores = System.IO.File.ReadAllLines( @"../../../scores.csv" );

            // Name:    Last[0],       First[1],  ID[2]
            //          Omelchenko,    Svetlana,  11
            // Score:   StudentID[0],  Exam1[1]   Exam2[2],  Exam3[3],  Exam4[4]
            //          111,           97,        92,        81,        60

            // This query joins two dissimilar spreadsheets based on common ID value.
            // Multiple from clauses are used instead of a join clause
            // in order to store results of id.Split.
            IEnumerable<string> scoreQuery1 =
                from name in names
                let nameFields = name.Split( ',' )
                from id in scores
                let scoreFields = id.Split( ',' )
                where nameFields[2] == scoreFields[0]
                select nameFields[0] + "," + scoreFields[1] + "," + scoreFields[2]
                       + "," + scoreFields[3] + "," + scoreFields[4];

            // Pass a query variable to a method and execute it
            // in the method. The query itself is unchanged.
            OutputQueryResults( scoreQuery1, "Merge two spreadsheets:" );

            // Keep console window open in debug mode.
            Console.WriteLine( "Press any key to exit" );
            Console.ReadKey();
        }
        static void OutputQueryResults( IEnumerable<string> query, string message )
        {
            Console.WriteLine( System.Environment.NewLine + message );
            foreach (string item in query)
            {
                Console.WriteLine( item );
            }
            Console.WriteLine( "{0} total names in list", query.Count() );
        }
    }
    /* Output:
    Merge two spreadsheets:
    Adams, 99, 82, 81, 79
    Fakhouri, 99, 86, 90, 94
    Feng, 93, 92, 80, 87
    Garcia, 97, 89, 85, 82
    Garcia, 35, 72, 91, 70
    Garcia, 92, 90, 83, 78
    Mortensen, 88, 94, 65, 91
    O'Donnell, 75, 84, 91, 39
    Omelchenko, 97, 92, 81, 60
    Tucker, 68, 79, 88, 92
    Tucker, 94, 92, 91, 91
    Zabokritski, 96, 85, 91, 60
    12 total names in list
     */

}
