using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CSVFiles
{
    static void Main( string[] args )
    {
        // Create the IEnumerable data source
        string[] lines = System.IO.File.ReadAllLines( @"../../../spreadsheet1.csv" );

        // Create the query. Put field 2 first, then
        // reverse and combine fields 0 and 1 from the old field
        IEnumerable<string> query =
            from line in lines
            let x = line.Split( ',' )
            orderby x[2]
            select x[2] + ", " + ( x[1] + " " + x[0] );

        // Execute the query and write out the new file. Note that WriteAllLines
        // takes a string[], so ToArray is called on the query.
        System.IO.File.WriteAllLines( @"../../../spreadsheet2.csv", query.ToArray() );

        Console.WriteLine( "Spreadsheet2.csv written to disk. Press any key to exit" );
        Console.ReadKey();
    }
}