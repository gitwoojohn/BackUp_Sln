using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 이 예제에서는 자동으로 구현된 속성 집합을 캡슐화만 하는, 변경 불가능한 
// 간단한 클래스를 만드는 방법을 보여 줍니다. 

// 참조 형식 구문을 사용해야만 하는 경우 구조체 대신 이러한 종류의 구문을 사용합니다.

// 자동으로 구현된 속성에는 get 및 set 접근자가 모두 필요합니다. 
// set 접근자를 private로 선언하여 클래스를 변경 불가능하게 만듭니다. 

// 그러나 private set 접근자를 선언할 때는 개체 이니셜라이저를 사용하여 
// 속성을 초기화할 수 없습니다. 생성자나 팩터리 메서드를 사용해야 합니다.

// 컴파일러에서는 자동으로 구현된 각 속성에 대한 지원 필드를 만듭니다. 
// 필드는 소스 코드에서 직접 액세스할 수 없습니다.

namespace Contact
{
    // This class is immutable. After an object is created,
    // it cannot be modified from outside the class. It uses a
    // constructor to initialize its properties.
    class Contact
    {
        // Read-only properties.
        public string Name { get; private set; }
        public string Address { get; private set; }

        // Public constructor.
        public Contact( string contactName, string contactAddress )
        {
            Name = contactName;
            Address = contactAddress;
        }
    }

    // This class is immutable. After an object is created,
    // it cannot be modified from outside the class. It uses a
    // static method and private constructor to initialize its properties.   
    public class Contact2
    {
        // Read-only properties.
        public string Name { get; private set; }
        public string Address { get; private set; }

        // Private constructor.
        private Contact2( string contactName, string contactAddress )
        {
            Name = contactName;
            Address = contactAddress;
        }

        // Public factory method.
        public static Contact2 CreateContact( string name, string address )
        {
            return new Contact2( name, address );
        }
    }

    class Program
    {
        static void Main( string[] args )
        {
            // Some simple data sources.
            string[] names = {"Terry Adams","Fadi Fakhouri", "Hanying Feng", 
                              "Cesar Garcia", "Debra Garcia"};
            string[] addresses = {"123 Main St.", "345 Cypress Ave.", "678 1st Ave",
                                  "12 108th St.", "89 E. 42nd St."};

            // Simple query to demonstrate object creation in select clause.
            // Create Contact objects by using a constructor.
            var query1 = from i in Enumerable.Range( 0, 5 )
                         select new Contact( names[i], addresses[i] );

            // List elements cannot be modified by client code.
            var list = query1.ToList();
            foreach (var contact in list)
            {
                Console.WriteLine( "{0}, {1}", contact.Name, contact.Address );
            }

            // Create Contact2 objects by using a static factory method.
            var query2 = from i in Enumerable.Range( 0, 5 )
                         select Contact2.CreateContact( names[i], addresses[i] );

            // Console output is identical to query1.
            var list2 = query2.ToList();

            // List elements cannot be modified by client code.
            // CS0272:
            // list2[0].Name = "Eugene Zabokritski"; 

            // Keep the console open in debug mode.
            Console.WriteLine( "Press any key to exit." );
            Console.ReadKey();  
        }
    }
}

/* Output:
    Terry Adams, 123 Main St.
    Fadi Fakhouri, 345 Cypress Ave.
    Hanying Feng, 678 1st Ave
    Cesar Garcia, 12 108th St.
    Debra Garcia, 89 E. 42nd St.
*/
