using System;
using System.Collections.Specialized;
using System.IO;

namespace ConsoleApplication1
{
    // root.GetDirectories("*.*", System.IO.SearchOption.AllDirectories);

    //위 방식의 단점은 지정된 루트 아래에 있는 하위 디렉터리 중 하나가 DirectoryNotFoundException 
    //또는 UnauthorizedAccessException을 발생시키면 전체 메서드가 실패하고 
    //디렉터리가 반환되지 않는다는 것입니다.

    //이것은 GetFiles 메서드를 사용할 경우에도 마찬가지입니다.
    //특정 하위 폴더에서 이러한 예외를 처리해야 한다면
    //다음 예제에서 볼 수 있는 것처럼 디렉터리 트리를 수동으로 탐색해야 합니다.

    public class RecursiveFileSearch
    {
        static StringCollection log = new StringCollection();

        static void Main()
        {
            // Start with drives if you have to search the entire computer.
            string[] drives = Environment.GetLogicalDrives();

            foreach (string drive in drives)
            {
                DriveInfo driveInfo = new DriveInfo( drive );

                // Here we skip the drive if it is not ready to be read. This
                // is not necessarily the appropriate action in all scenarios.
                if (!driveInfo.IsReady)
                {
                    Console.WriteLine( "The drive {0} could not be read", driveInfo.Name );
                    continue;
                }
                DirectoryInfo rootDir = driveInfo.RootDirectory;
                WalkDirectoryTree( rootDir );
            }

            // Write out all the files that could not be processed.
            Console.WriteLine( "Files with restricted access:" );
            foreach (string logMsg in log)
            {
                Console.WriteLine( logMsg );
            }
            // Keep the console window open in debug mode.
            Console.WriteLine( "Press any key" );
            Console.ReadKey();
        }

        static void WalkDirectoryTree( DirectoryInfo root )
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                // *.mp3 , *.wav 
                files = root.GetFiles( "*.wav" );
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                log.Add( e.Message );
            }

            catch ( DirectoryNotFoundException e )
            {
                Console.WriteLine( e.Message );
            }

            if (files != null)
            {
                foreach ( FileInfo file in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    //Console.WriteLine( file.FullName );
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach ( DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree( dirInfo );
                }
            }
        }
    }
}
