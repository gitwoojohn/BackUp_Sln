using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

/*
-----------------------------------------------------------------------------------------------------------
where T : struct 타입 매개변수는 반드시 Nullable을 제외한 값형(value type)이어야만 한다.
-----------------------------------------------------------------------------------------------------------
where T : class	 타입 매개변수는 반드시 참조형(reference type)이어야만 한다.
-----------------------------------------------------------------------------------------------------------
where T : new()	 타입 매개변수는 반드시 public이고 매개변수가 없는 생성자를 갖고 있어야 한다. 
                 그리고 다른 제약 조건이랑 같이 사용될때는 new()가 맨뒤에 와야한다.
-----------------------------------------------------------------------------------------------------------
where T : <base class name>	 타입 매개변수는 반드시 명시된 클래스를 상속 해야한다.
-----------------------------------------------------------------------------------------------------------
where T : <interface name>	 타입 매개변수는 반드시 명시된 인터페이스이거나, 명시된 인터페이스를 구현해야 한다.
-----------------------------------------------------------------------------------------------------------
where T : U	 T에 해당되는 매개변수는 반드시 U에 해당되는 매개변수 타입이거나, U를 상속한 타입이어야 한다. 
-----------------------------------------------------------------------------------------------------------
*/
namespace GenericMethodExam
{
    class Serializer
    {
        internal static bool Serialize<T>( T source, string fileName )
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer( typeof( T ) );
                Stream stream = new FileStream( fileName, FileMode.Create,
                       FileAccess.Write, FileShare.None );
                serializer.Serialize( stream, source );
                stream.Close();

                return true;
            }
            catch( Exception ex )
            {
                throw ex;
            }
        }

        // where T : class, new()
        internal static T Deserialize<T>( string fileName ) where T : class, new()
        {
            T target;
            FileInfo _settingFile = new FileInfo( fileName );
            if( !_settingFile.Exists )
            {
                return new T();
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer( typeof( T ) );
                Stream stream = new FileStream( fileName, FileMode.Open,
                       FileAccess.Read, FileShare.None );
                target = serializer.Deserialize( stream ) as T;
                stream.Close();

                return target;
            }
            catch( Exception ex )
            {
                throw ex;
            }
        }
    }
}
