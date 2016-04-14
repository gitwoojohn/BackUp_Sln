using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_04_WordCount
{
    class CountWords
    {
        static void Main()
        {
            string text = @"Historically, the world of data and the world of objects" +
              @" have not been well integrated. Programmers work in C# or Visual Basic" +
              @" and also in SQL or XQuery. On the one side are concepts such as classes," +
              @" objects, fields, inheritance, and .NET Framework APIs. On the other side" +
              @" are tables, columns, rows, nodes, and separate languages for dealing with" +
              @" them. Data types often require translation between the two worlds; there are" +
              @" different standard functions. Because the object world has no notion of query, a" +
              @" query can only be represented as a string without compile-time type checking or" +
              @" IntelliSense support in the IDE. Transferring data from SQL tables or XML trees to" +
              @" objects in memory is often tedious and error-prone.";

            // 예제 1 - 단어(Word) 카운트
            string searchTerm = "data";

            //Convert the string into an array of words
            string[] source = text.Split( new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries );

            // Create and execute the query. It executes immediately 
            // because a singleton value is produced.
            // Use ToLowerInvariant to match "data" and "Data" 
            var matchQuery = from word in source
                             where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                             select word;

            // Count the matches.
            int wordCount = matchQuery.Count();
            Console.WriteLine( "{0} occurrences(s) of the search term \"{1}\" were found.", wordCount, searchTerm );
            Console.WriteLine("\n====================================================================\n");

            // 예제 2 - 문장(Sentences) 카운트
            // Split the text block into an array of sentences.
            string[] sentences = text.Split( new char[] { '.', '?', '!' } );

            string[] wordsToMatch = { "Historically", "data", "integrated" };
            // Find sentences that contain all the terms in the wordsToMatch array.
            // Note that the number of terms to match is not specified at compile time.
            var sentenceQuery = from sentence in sentences
                                let w = sentence.Split( new char[] { '.', '?', '!', ' ', ';', ':', ',' },
                                                        StringSplitOptions.RemoveEmptyEntries )
                                where w.Distinct().Intersect( wordsToMatch ).Count() == wordsToMatch.Count()
                                select sentence;

            // Execute the query. Note that you can explicitly type
            // the iteration variable here even though sentenceQuery
            // was implicitly typed. 
            foreach (string str in sentenceQuery)
            {
                Console.WriteLine( str );
            }

            // Keep console window open in debug mode
            Console.WriteLine( "Press any key to exit" );
            Console.ReadKey();
        }
    }
}