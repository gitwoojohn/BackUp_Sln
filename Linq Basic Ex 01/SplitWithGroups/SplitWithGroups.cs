using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SplitWithGroups
{
    class SplitWithGroups
    {
        static void Main( string[] args )
        {
            string[] fileA = File.ReadAllLines( @"../../../names1.txt" );
            string[] fileB = File.ReadAllLines( @"../../../names2.txt" );

            // Concatenate and remove duplicate names based on
            // default string comparer
            var mergeQuery = fileA.Union( fileB );

            // Group the names by the first letter in the last name.
            var groupQuery = from name in mergeQuery
                             let n = name.Split( ',' )
                             group name by n[ 0 ][ 0 ] into g
                             orderby g.Key
                             select g;

            // Create a new file for each group that was created
            // Note that nested foreach loops are required to access
            // individual items with each group.
            foreach( var g in groupQuery )
            {
                // Create the new file name.
                string fileName = @"../../../testFile_" + g.Key + ".txt";

                // Output to display.
                Console.WriteLine( g.Key );

                // Write file.
                using( StreamWriter sw = new StreamWriter( fileName ) )
                {
                    foreach( var item in g )
                    {
                        sw.WriteLine( item );
                        // Output to console for example purposes.
                        Console.WriteLine( "   {0}", item );
                    }
                }
            }
            // Keep console window open in debug mode.
            Console.WriteLine( "Files have been written. Press any key to exit" );
            Console.ReadKey();
        }
    }
}
/* Output: 
    A
       Aw, Kam Foo
    B
       Bankov, Peter
       Beebe, Ann
    E
       El Yassir, Mehdi
    G
       Garcia, Hugo
       Guy, Wey Yuan
       Garcia, Debra
       Gilchrist, Beth
       Giakoumakis, Leo
    H
       Holm, Michael
    L
       Liu, Jinghao
    M
       Myrcha, Jacek
       McLin, Nkenge
    N
       Noriega, Fabricio
    P
       Potra, Cristina
    T
       Toyoshima, Tim
 */
