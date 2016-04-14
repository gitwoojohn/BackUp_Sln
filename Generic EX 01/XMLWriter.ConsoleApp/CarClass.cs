using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLWriter.ConsoleApp
{
    [Serializable()]
    public class CarClass
    {
        [XmlElement( "StockNumber" )]
        public string StockNumber { get; set; }

        [XmlElement( "Make" )]
        public string Make { get; set; }

        [XmlElement( "Model" )]
        public string Model { get; set; }

        public void Deserialize_Function()
        {
            CarCollection cars = null;
            string path = @"C:\Temp\cars.xml";

            XmlSerializer serializer = new XmlSerializer( typeof( CarCollection ) );

            StreamReader reader = new StreamReader( path );
            cars = ( CarCollection )serializer.Deserialize( reader );
            reader.Close();
        }
    }
    [Serializable()]
    [XmlRoot( "CarCollection" )]
    public class CarCollection
    {
        [XmlArray( "Cars" )]
        [XmlArrayItem( "Car", typeof( CarClass ) )]
        public CarClass[] Car { get; set; }
    }

}

/// 편집 - 선택하여 붙여넣기 - JSON, XML 클래스로 붙여넣기 
/// 아래 코드 자동 생성(Serialization, DeSerialization 위한 코드 자동 생성 )
/// <remarks/>
[XmlType( AnonymousType = true )]
[XmlRoot( Namespace = "", IsNullable = false )]
public partial class CarCollection
{

    private CarCollectionCar[] carsField;

    /// <remarks/>
    [XmlArrayItem( "Car", IsNullable = false )]
    public CarCollectionCar[] Cars
    {
        get
        {
            return this.carsField;
        }
        set
        {
            this.carsField = value;
        }
    }
}

/// <remarks/>
[XmlType( AnonymousType = true )]
public partial class CarCollectionCar
{

    private ushort stockNumberField;

    private string makeField;

    private string modelField;

    /// <remarks/>
    public ushort StockNumber
    {
        get
        {
            return this.stockNumberField;
        }
        set
        {
            this.stockNumberField = value;
        }
    }

    /// <remarks/>
    public string Make
    {
        get
        {
            return this.makeField;
        }
        set
        {
            this.makeField = value;
        }
    }

    /// <remarks/>
    public string Model
    {
        get
        {
            return this.modelField;
        }
        set
        {
            this.modelField = value;
        }
    }
}

