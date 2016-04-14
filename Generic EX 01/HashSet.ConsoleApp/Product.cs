using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HashSet.ConsoleApp
{
    /// <summary>
    /// C#에서는 equals
    /// </summary>
    public class Product : IEquatable<Product>
    {
        public int Number { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool Equals( Product other )
        {
            //Check whether the compared object is null. 
            if( Object.ReferenceEquals( other, null ) ) return false;

            //Check whether the compared object references the same data. 
            if( Object.ReferenceEquals( this, other ) ) return true;

            //Check whether the products' properties are equal. 
            //return Code.Equals( other.Code ) && Name.Equals( other.Name );

            //Check whether the products' properties are equal. 
            return Number.Equals( other.Number );

        }
        // If Equals() returns true for a pair of objects  
        // then GetHashCode() must return the same value for these objects. 
        public override int GetHashCode()
        {
            //Get hash code for the Name field if it is not null. 
            int hashProductName = Name == null ? 0 : Name.GetHashCode();

            //Get hash code for the Code field. 
            int hashProductCode = Code.GetHashCode();

            // Get hash number for the Number field.
            int hashProductNumber = Number.GetHashCode();

            //Calculate the hash Number for the product. 
            return hashProductName ^ hashProductNumber;
        }
        public void distinctProduct_02()
        {
            #region HashSet을 사용해서 중복 제거
            HashSet<string> UniqueNumber =
                new HashSet<string>( File.ReadAllLines( @"C:\Temp\10Number.txt" ), StringComparer.Ordinal );

            List<int> intList = UniqueNumber.Select( i => int.Parse( i ) ).ToList();
            intList.Sort();
            #endregion HashSet을 사용해서 중복 제거 끝

            distinctProduct_01();

            XmlSerializer reader = new XmlSerializer( typeof( Product ) );
            StreamReader file = new StreamReader( @"C:\Temp\10Number.xml" );
            Product overview = ( Product )reader.Deserialize( file );
            file.Close();

            Console.WriteLine( overview.Number );
            Console.ReadLine();
            //Exclude duplicates.
            //IEnumerable<Product> noduplicates = products.Distinct();

            //foreach( var product in noduplicates )
            //    Console.WriteLine( "{0,-7} : {1}", product.Name, product.Number );
        }
        public void distinctProduct_01()
        {
            Product[] products = {
                new Product { Name = "apple", Number = 9 },
                new Product { Name = "orange", Number = 4 },
                new Product { Name = "apple", Number = 9 },
                new Product { Name = "lemon", Number = 12 } };

            //Exclude duplicates.
            IEnumerable<Product> noduplicates = products.Distinct();

            foreach( var product in noduplicates )
                Console.WriteLine( "{0,-7} : {1}", product.Name, product.Number );
        }
        /// <summary>
        /// XML Serialization and DeSerialization
        /// </summary>
        public void ReadXML()
        {
            // First write something so that there is something to read ...
            var b = new Product { Number = 1, Name = "TestName", Code = 1 };
            var writer = new XmlSerializer( typeof( Product ) );
            var wfile = new StreamWriter( @"C:\Temp\SerializationOverview.xml" );
            writer.Serialize( wfile, b );
            wfile.Close();

            // Now we can read the serialized book ...
            XmlSerializer reader = new XmlSerializer( typeof( Product ) );
            StreamReader file = new StreamReader( @"C:\Temp\SerializationOverview.xml" );
            Product overview = ( Product )reader.Deserialize( file );
            file.Close();

            Console.WriteLine( overview.Number );

        }
    }
}
