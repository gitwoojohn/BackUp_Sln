using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupJoin.ConsoleApp
{
    class Person
    {
        public string Name { get; set; }
    }

    class Pet
    {
        public string Name { get; set; }
        public Person Owner { get; set; }
    }

    class Program
    {
        static void Main( string[] args )
        {
            Order_ThenBy ot = new Order_ThenBy();
            ot.Multiple_OrderBy();


            GroupJoinEx1();
            Console.ReadLine();
        }
        public static void GroupJoinEx1()
        {
            Person magnus = new Person { Name = "Hedlund, Magnus" };
            Person terry = new Person { Name = "Adams, Terry" };
            Person charlotte = new Person { Name = "Weiss, Charlotte" };

            Pet barley = new Pet { Name = "Barley", Owner = terry };
            Pet boots = new Pet { Name = "Boots", Owner = terry };
            Pet whiskers = new Pet { Name = "Whiskers", Owner = charlotte };
            Pet daisy = new Pet { Name = "Daisy", Owner = magnus };

            List<Person> peoples = new List<Person> { magnus, terry, charlotte };
            List<Pet> pets = new List<Pet> { barley, boots, whiskers, daisy };

            // Create a list where each element is an anonymous 
            // type that contains a person's name and 
            // a collection of names of the pets they own.
            var GroupJoinQuery =
                // people(TOuter), pets(TInner), person(outerKeySelector), 
                // pet:person(innerKeySelector), new { resultSelector }, TResult
                peoples.GroupJoin( pets,
                                  person => person,
                                  pet => pet.Owner,
                                  ( person, petCollection ) =>
                                    new
                                    {
                                        OwnerName = person.Name,
                                        Pets = petCollection.Select( pet => pet.Name )
                                    } ).OrderBy( o => o.OwnerName );

            foreach( var obj in GroupJoinQuery )
            {
                // Output the owner's name.
                Console.WriteLine( string.Format( "{0}:", obj.OwnerName ) );

                // Output each of the owner's pet's names.
                foreach( string pet in obj.Pets )
                {
                    Console.WriteLine( string.Format( "  {0}", pet ) );
                }
                Console.WriteLine();
            }

            Console.WriteLine( "\n---------------------------------------------------------------------\n" );
            var LinqGroupJoinQuery =
                from people in peoples
                join pet in pets on people equals pet.Owner into gKey
                select new
                {
                    OwnerName = people.Name,

                    // 방법 1. 
                    Pets = gKey

                    // 방법 2.
                    //Pets = from g in gKey
                    //       orderby g.Name
                    //       select g
                };

            foreach( var GroupQuery in LinqGroupJoinQuery )
            {
                Console.WriteLine( GroupQuery.OwnerName );
                foreach( var oPet in GroupQuery.Pets )
                {
                    Console.WriteLine( oPet.Name );
                }
            }
        }
    }
}
