using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 이름이 같은 파일이 두 개 이상의 폴더에 위치하는 경우가 있습니다. 
// 예를 들어 Visual Studio 설치 폴더의 여러 폴더에 readme.htm 파일이 있습니다. 

// 이 예제에서는 지정한 루트 폴더에서 이러한 중복 파일 이름에 
// 대해 쿼리하는 방법을 보여 줍니다. 

// 두 번째 예제에서는 크기와 작성 시간도 일치하는 파일을 쿼리하는 방법을 보여 줍니다.

// 첫 번째 쿼리에서는 간단한 키를 사용하여 일치하는 항목을 확인합니다. 
// 이 쿼리에서는 이름은 같지만 내용은 다를 수 있는 파일을 찾습니다. 

// 두 번째 쿼리에서는 복합 키를 사용하여 FileInfo 개체의 
// 세 가지 속성에 대한 일치 여부를 확인합니다. 

// 이 쿼리는 이름이 같고 내용이 유사하거나 동일한 파일을 찾는 것과 아주 유사합니다.

namespace QueryDuplicateFileNames
{
    class QueryDuplicateFileNames
    {
        static void Main( string[] args )
        {
            //string sFolder1 = @"c:\program files\Microsoft Visual Studio 12.0\";
            //string sFolder2 = @"c:\program files\Microsoft Visual Studio 12.0\Common7";

            string sFolder1 = @"C:\program files\Microsoft Visual Studio 12.0\";
            string sFolder2 = @"c:\program files\Microsoft Visual Studio 12.0\Common7";

            // Uncomment QueryDuplicates2 to run that query.
            QueryDuplicates(sFolder1);
            QueryDuplicates2(sFolder2);

            // Keep the console window open in debug mode.
            Console.WriteLine( "Press any key to exit." );
           // Console.ReadKey();
        }

        static void QueryDuplicates(string startFolder)
        {
            // Change the root drive or folder if necessary
            //string startFolder = strFolder;

            // Take a snapshot of the file system.
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo( startFolder );          

            try
            {
                // This method assumes that the application has discovery permissions
                // for all folders under the specified path.
                IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles( "*.*", System.IO.SearchOption.AllDirectories );

                // used in WriteLine to keep the lines shorter
                int charsToSkip = startFolder.Length;

                // var can be used for convenience with groups.
                var queryDupNames =
                    from file in fileList
                    group file.FullName.Substring( charsToSkip ) by file.Name into fileGroup
                    where fileGroup.Count() > 1
                    select fileGroup;

                // Pass the query to a method that will
                // output one page at a time.
                PageOutput<string, string>( queryDupNames );
            }
            catch (UnauthorizedAccessException)
            {
                //Console.WriteLine( e.Message );
            }
        }

        // A Group key that can be passed to a separate method.
        // Override Equals and GetHashCode to define equality for the key.
        // Override ToString to provide a friendly name for Key.ToString()
        class PortableKey
        {
            public string Name { get; set; }
            public DateTime CreationTime { get; set; }
            public long Length { get; set; }

            public override bool Equals( object obj )
            {
                PortableKey other = (PortableKey)obj;
                return other.CreationTime == this.CreationTime &&
                       other.Length == this.Length &&
                       other.Name == this.Name;
            }

            public override int GetHashCode()
            {
                string str = String.Format( "{0}{1}{2}", this.CreationTime, this.Length, this.Name );
                return str.GetHashCode();
            }
            public override string ToString()
            {
                return String.Format( "{0} {1} {2}", this.Name, this.Length, this.CreationTime );
            }
        }

        // 파일 이름, 생성일자, 크기가 같은 파일 쿼리
        static void QueryDuplicates2(string startFolder)
        {
            // Change the root drive or folder if necessary.
            //string startFolder = strFolder;  //@"c:\program files\Microsoft Visual Studio 12.0\Common7";

            // Make the the lines shorter for the console display
            int charsToSkip = startFolder.Length;

            try
            {
                // Take a snapshot of the file system.
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo( startFolder );
                IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles( "*.*", System.IO.SearchOption.AllDirectories );

                // Note the use of a compound key. Files that match
                // all three properties belong to the same group.
                // A named type is used to enable the query to be
                // passed to another method. Anonymous types can also be used
                // for composite keys but cannot be passed across method boundaries
                // 
                var queryDupFiles =
                    from file in fileList
                    group file.FullName.Substring( charsToSkip ) by
                        new PortableKey { Name = file.Name, CreationTime = file.CreationTime, Length = file.Length } into fileGroup
                    where fileGroup.Count() > 1
                    select fileGroup;

                var list = queryDupFiles.ToList();

                int i = queryDupFiles.Count();

                PageOutput<PortableKey, string>( queryDupFiles );
            }
            catch (UnauthorizedAccessException)
            {
            }
        }


        // A generic method to page the output of the QueryDuplications methods
        // Here the type of the group must be specified explicitly. "var" cannot
        // be used in method signatures. This method does not display more than one
        // group per page.
        private static void PageOutput<K, V>( IEnumerable<System.Linq.IGrouping<K, V>> groupByExtList )
        {
            // Flag to break out of paging loop.
            bool goAgain = true;

            // "3" = 1 line for extension + 1 for "Press any key" + 1 for input cursor.
            int numLines = Console.WindowHeight - 3;

            // Iterate through the outer collection of groups.
            foreach (var filegroup in groupByExtList)
            {
                // Start a new extension at the top of a page.
                int currentLine = 0;

                // Output only as many lines of the current group as will fit in the window.
                do
                {
                    Console.Clear();
                    Console.WriteLine( "Filename = {0}", filegroup.Key.ToString() == String.Empty ? "[none]" : filegroup.Key.ToString() );

                    // Get 'numLines' number of items starting at number 'currentLine'.
                    var resultPage = filegroup.Skip( currentLine ).Take( numLines );

                    //Execute the resultPage query
                    foreach (var fileName in resultPage)
                    {
                        Console.WriteLine( "\t{0}", fileName );
                    }

                    // Increment the line counter.
                    currentLine += numLines;

                    // Give the user a chance to escape.
                    Console.WriteLine( "Press any key to continue or the 'End' key to break..." );
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.End)
                    {
                        goAgain = false;
                        break;
                    }
                } while (currentLine < filegroup.Count());

                if (goAgain == false)
                    break;
            }
        }
    }
}