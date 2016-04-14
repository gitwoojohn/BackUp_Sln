using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BenchMark.HashSet.ConsoleApp
{
    class Program
    {
        const int _max = 10000000;
        static void Main( string[] args )
        {
            Random r = new Random();
            List<string> iNumber = new List<string>();

            // 77메가 텍스트 파일 생성
            //for( int i = 0; i < _max; i++ )
            //{
            //    iNumber.Add( Convert.ToString( r.Next( 1, 1000000 ) ) );
            //    //r.NextBytes(_bytes);
            //}
            //File.WriteAllLines( @"C:\Temp\10Number.txt", iNumber.ToArray() );

            //using( FileStream xmlfs = new FileStream( @"C:\Temp\10Number.xml", FileMode.Create ) )
            //{
            //    using(xmlst)
            //    XmlWriter writer = new XmlWriter( xmlfs );
            //};
            XmlTextWriter write = new XmlTextWriter( @"C:\Temp\10Number.xml", Encoding.UTF8 );
            //write.WriteStartElement( "XMLExample" );
            write.WriteStartElement( "Product" );
            for( int i = 0; i < 3; i++ )
            {
                write.WriteStartElement( "Number" );
                write.WriteString( i.ToString() );
                write.WriteEndElement();
            }
            write.WriteEndElement();
            write.WriteEndElement();
            write.Close();

            FileStream fs = null;
            string[] intHash = new string[ _max ];
            try
            {
                fs = new FileStream( @"C:\Temp\10Number.txt", FileMode.Open );
                using( StreamReader reader = new StreamReader( fs ) )
                {
                    string line;
                    int i = 0;
                    while( ( line = reader.ReadLine() ) != null )
                    {

                        intHash[ i++ ] = line;
                    }
                }
            }
            finally
            {
                if( fs != null )
                    fs.Dispose();
            }

            var h = new HashSet<string>( StringComparer.Ordinal );
            var d = new Dictionary<string, bool>( StringComparer.Ordinal );
            var a = new string[] { "a", "b", "c", "d", "longer", "words", "also" };

            var s1 = Stopwatch.StartNew();
            for( int i = 0; i < _max; i++ )
            {
                //foreach( string s in a )
                //{
                //    h.Add( s );
                //    h.Contains( s );
                //}
                h.Add( intHash[ i ] );
                h.Contains( intHash[ i ] );
            }
            s1.Stop();
            var s2 = Stopwatch.StartNew();
            for( int i = 0; i < _max; i++ )
            {
                foreach( string s in a )
                {
                    d[ s ] = true;
                    d.ContainsKey( s );
                }
            }
            s2.Stop();
            Console.WriteLine( h.Count );
            Console.WriteLine( d.Count );

            Console.WriteLine( ( s1.Elapsed.TotalMilliseconds * 1000000 / _max ).ToString( "0.00 ns" ) );
            Console.WriteLine( ( s2.Elapsed.TotalMilliseconds * 1000000 / _max ).ToString( "0.00 ns" ) );
            Console.Read();
        }
    }
}
