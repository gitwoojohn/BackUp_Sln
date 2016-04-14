using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class MergeStrings
{
    static void Main( string[] args )
    {
        //Put text files in your solution folder
        string[] fileA = System.IO.File.ReadAllLines( @"../../../names1.txt" );
        string[] fileB = System.IO.File.ReadAllLines( @"../../../names2.txt" );

        //Simple concatenation and sort. Duplicates are preserved.
        IEnumerable<string> concatQuery = fileA.Concat( fileB ).OrderBy( s => s );

        // Pass the query variable to another function for execution.
        OutputQueryResults( concatQuery, "Simple concatenate and sort. Duplicates are preserved:" );

        // Concatenate and remove duplicate names based on
        // default string comparer.
        IEnumerable<string> uniqueNamesQuery = fileA.Union( fileB ).OrderBy( s => s );
        OutputQueryResults( uniqueNamesQuery, "Union removes duplicate names:" );

        // Find the names that occur in both files (based on
        // default string comparer).
        IEnumerable<string> commonNamesQuery = fileA.Intersect( fileB );
        OutputQueryResults( commonNamesQuery, "Merge based on intersect:" );

        // Find the matching fields in each list. Merge the two 
        // results by using Concat, and then
        // sort using the default string comparer.
        string nameMatch = "Garcia";

        IEnumerable<String> tempQuery1 =
            from name in fileA
            let n = name.Split( ',' )
            where n[0] == nameMatch
            select name;

        IEnumerable<string> tempQuery2 =
            from name2 in fileB
            let n2 = name2.Split( ',' )
            where n2[0] == nameMatch
            select name2;

        IEnumerable<string> nameMatchQuery = tempQuery1.Concat( tempQuery2 ).OrderBy( s => s );
        OutputQueryResults( nameMatchQuery, 
            String.Format( "Concat based on partial name match \"{0}\":", nameMatch ) );

        // Keep the console window open in debug mode.
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