using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortedSet.ConsoleApp
{
    class Program
    {
        static void Main( string[] args )
        {
            /*
                1. SortedSet<T> 개체에 요소가 삽입되거나 삭제하더라도 성능에 영향을 주지않고 정렬된 순서를 유지 관리 합니다.
                2. 중복 요소가 허용 되지 않습니다.
                3. 기존 항목의 정렬 값 변경을 지원 하지 않습니다.
                4. 스레드로부터 안전한 ImmutableSortedSet<T>가 있습니다.

                다음 예제는 SortedSet<T>개체를 사용하여 만든 클래스 IComparer<T> 매개 변수로 합니다.            
                이 비교자 (ByFileExtension) 해당 확장명으로 파일 이름 목록을 정렬 하는데 사용 됩니다.

                이 예제에는 파일 이름들을 미디어의 정렬된 집합을 만들고, 원치 않는 요소를 제거, 
                다양한 요소를 확인 및 다른 정렬방식의 집합으로 집합들을 비교 하는 방법을 보여줍니다.
            */
            try
            {
                // Get a list of the files to use for the sorted set.
                IEnumerable<string> files1 =
                Directory.EnumerateFiles(@"C:\Temp", "*", SearchOption.AllDirectories);

                // Create a sorted set using the ByFileExtension comparer.
                SortedSet<string> mediaFiles1 = new SortedSet<string>(new ByFileExtension());

                // Note that there is a SortedSet constructor that takes an IEnumerable,
                // but to remove the path information they must be added individually.
                foreach( string f in files1 )
                {
                    mediaFiles1.Add( f.Substring( f.LastIndexOf( @"\" ) + 1 ) );
                }

                // Remove elements that have non-media extensions.
                // See the 'isDoc' method.
                Console.WriteLine( "Remove docs from the set..." );
                Console.WriteLine( "\tCount before: {0}", mediaFiles1.Count.ToString() );
                mediaFiles1.RemoveWhere( isDoc );
                Console.WriteLine( "\tCount after: {0}", mediaFiles1.Count.ToString() );


                Console.WriteLine();

                // List all the avi files.
                SortedSet<string> aviFiles = mediaFiles1.GetViewBetween("avi", "avj");

                Console.WriteLine( "AVI files:" );
                foreach( string avi in aviFiles )
                {
                    Console.WriteLine( "\t{0}", avi );
                }

                Console.WriteLine();

                // Create another sorted set.
                IEnumerable<string> files2 =
                Directory.EnumerateFiles(@"E:\FileDown", "*", SearchOption.AllDirectories);

                SortedSet<string> mediaFiles2 = new SortedSet<string>(new ByFileExtension());

                foreach( string f in files2 )
                {
                    mediaFiles2.Add( f.Substring( f.LastIndexOf( @"\" ) + 1 ) );
                }

                // Remove elements in mediaFiles1 that are also in mediaFiles2.
                Console.WriteLine( "Remove duplicates (of mediaFiles2) from the set..." );
                Console.WriteLine( "\tCount before: {0}", mediaFiles1.Count.ToString() );
                mediaFiles1.ExceptWith( mediaFiles2 );
                Console.WriteLine( "\tCount after: {0}", mediaFiles1.Count.ToString() );

                Console.WriteLine();

                Console.WriteLine( "List of mediaFiles1:" );
                foreach( string f in mediaFiles1 )
                {
                    Console.WriteLine( "\t{0}", f );
                }

                // Create a set of the sets.
                IEqualityComparer<SortedSet<string>> comparer = SortedSet<string>.CreateSetComparer();

                HashSet<SortedSet<string>> allMedia =  new HashSet<SortedSet<string>>(comparer);
                allMedia.Add( mediaFiles1 );
                allMedia.Add( mediaFiles2 );
            }
            catch( IOException ioEx )
            {
                Console.WriteLine( ioEx.Message );
            }

            catch( UnauthorizedAccessException AuthEx )
            {
                Console.WriteLine( AuthEx.Message );
            }
            Console.ReadLine();
        }
        // Defines a predicate delegate to use
        // for the SortedSet.RemoveWhere method.
        private static bool isDoc( string s )
        {
            if( s.ToLower().EndsWith( ".txt" ) ||
                s.ToLower().EndsWith( ".doc" ) ||
                s.ToLower().EndsWith( ".xls" ) ||
                s.ToLower().EndsWith( ".xlsx" ) ||
                s.ToLower().EndsWith( ".pdf" ) ||
                s.ToLower().EndsWith( ".doc" ) ||
                s.ToLower().EndsWith( ".docx" ) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    // Defines a comparer to create a sorted set
    // that is sorted by the file extensions.
    public class ByFileExtension : IComparer<string>
    {
        string xExt, yExt;

        CaseInsensitiveComparer caseiComp = new CaseInsensitiveComparer();

        public int Compare( string x, string y )
        {
            // Parse the extension from the file name. 
            xExt = x.Substring( x.LastIndexOf( "." ) + 1 );
            yExt = y.Substring( y.LastIndexOf( "." ) + 1 );

            // Compare the file extensions. 
            int vExt = caseiComp.Compare(xExt, yExt);
            if( vExt != 0 )
            {
                return vExt;
            }
            else
            {
                // The extension is the same, 
                // so compare the filenames. 
                return caseiComp.Compare( x, y );
            }
        }
    }
}
