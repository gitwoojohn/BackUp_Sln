using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDelegate
{
    public class Person { }
    public class Employee : Person { }
    public partial class Program
    {
        static Employee FindByTitle( string title )
        {
            return new Employee();
        }
        static void AddToContacts( Person person )
        {
            // This method add a Person object to a contact list.
        }
        static void Test()
        {
            // 공변을 사용하지 않고 대리자 인스턴스 생성.
            Func<string, Employee> findEmployee = FindByTitle;

            // Person이 반환될 것으로 기대되지만 Employee가 반환되도록 지정할 수 있다.
            Func<string, Person> findPerson = FindByTitle;

            // Employee를 Person에 할당( Employee가 Person 상속 하기 때문에 공변 )
            // 덜 파생적인 형식을 반환하는 것을 대리자는 더 파생적인 형식으로 반환할 수 있게 지정할 수 있다.
            findPerson = findEmployee;

            // 반공변 ( Contravariant )
            Action<Person> addPersonToContacts = AddToContacts;

            // Employee 매개변수를 가지고 있는 Action 대리자 메서드가 동작할 것 같지만
            // Person 매개변수를 가지고 있는 메소드를 지정할 수 있다. 왜냐하면
            // Employee는 Person에서 파생 되었기 때문이다.
            Action<Employee> addEmployeeToContacts = AddToContacts;

            // 덜 파생적인 매개변수를 가지고 대리자가 더 파생적인 매개변수를 가지고 있는 
            // 대리자를 받아들이고 대리자를 지정할 수 있다.
            addEmployeeToContacts = addPersonToContacts;
        }
        private static void ActionPrint( string str, int value )
        {
            Action<string, int> action = ( stringArg, intArg )
                =>
            { Console.WriteLine( "string - {0}, integer - {1}", stringArg, intArg ); };

            action( str, value );
        }
    }
}
