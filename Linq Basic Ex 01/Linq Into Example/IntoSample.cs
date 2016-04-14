using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 컨텍스트 키워드 into는 group, join 또는 select 절의 결과를 
// 새 식별자에 저장하는 임시 식별자를 만드는 데 사용할 수 있습니다. 

// 이 식별자 자체는 추가 쿼리 명령에 대한 생성기가 될 수 있습니다. 
// group 또는 select 절에서 사용되는 경우 새 식별자의 사용을 연속이라고도 합니다.

// 다음 예제에서는 IGrouping의 유추된 형식을 포함하는 
// 임시 식별자 fruitGroup을 활성화하도록 into 키워드를 사용하는 방법을 보여 줍니다. 

// 식별자를 사용하여 각 그룹에서 Count 메서드를 호출하고 
// 두 개 이상의 단어를 포함하는 해당 그룹만 선택할 수 있습니다.

namespace Linq_Into_Example
{
    class IntoSample
    {
        static void Main( string[] args )
        {
            // Create a data source.
            string[] words = { "apples", "blueberries", "oranges", "bananas", "apricots" };

            // Create the query.
            var wordGroups1 =
                from w in words
                group w by w[0] into fruitGroup
                where fruitGroup.Count() >= 2
                select new { FirstLetter = fruitGroup.Key, Words = fruitGroup.Count() };

            // Execute the query. Note that we only iterate over the groups, 
            // not the items in each group
            foreach (var item in wordGroups1)
            {
                Console.WriteLine( " {0} has {1} elements.", item.FirstLetter, item.Words );
            }

            // Keep the console window open in debug mode
            Console.WriteLine( "Press any key to exit." );
            Console.ReadKey();
        }
    }
}
/* Output:
   a has 2 elements.
   b has 2 elements.
*/

