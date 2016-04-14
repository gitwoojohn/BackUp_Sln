using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 그룹에 적용되는 추가 쿼리 논리가 없을 때 소스 데이터를 그룹으로 정렬하기 위한 표준 패턴을 보여 줍니다.
// 이를 비연속 그룹화라고 합니다. 

// 문자열 배열의 요소는 첫 글자에 따라 그룹화 됩니다. 쿼리 결과는 그룹의 각 항목을 포함하는
// IEnumerable<T> 컬렉션 및char 형식의 public Key 속성을 포함하는 IGrouping<TKey, TElement> 형식입니다.
// group 절의 결과는 시퀀스의 시퀀스입니다. 

// 따라서 반환된 각 그룹 내의 개별 요소에 액세스하려면 다음 예제와 같이 그룹 키를 반복하는 
// 루프 내부의 중첩 foreach 루프를 사용합니다.

namespace Linq_Group_Example_03
{
    class GroupSample3
    {
        static void Main( string[] args )
        {
            // Create a data source.
            string[] words = { "blueberry", "chimpanzee", "abacus", "banana", "apple", "cheese" };

            // Create the query.
            var wordGroups =
                from w in words
                // 주석 제거 하면 오름 차순(어센딩) 정렬
                group w by w[0]; //into g
                //orderby g.Key
                //select g;

            // 그룹에서 추가 논리 수행하는 예제
            // 각 그룹을 쿼리하여 키 값이 모음인 그룹만 선택 합니다.
            //var wordGroups2 =
            //    from w in words
            //    group w by w[0] into grps
            //    where ( grps.Key == 'a' || grps.Key == 'e' || grps.Key == 'i'
            //            || grps.Key == 'o' || grps.Key == 'u' )
            //    select grps;

            // Execute the query.
            foreach (var wordGroup in wordGroups)
            {
                Console.WriteLine( "Words that start with the letter '{0}':", wordGroup.Key );
                foreach (var word in wordGroup)
                {
                    Console.WriteLine( word );
                }
            }

            // Keep the console window open in debug mode
            Console.WriteLine( "Press any key to exit." );
            Console.ReadKey();
        }
    }
}
/* Output:
      Words that start with the letter 'b':
        blueberry
        banana
      Words that start with the letter 'c':
        chimpanzee
        cheese
      Words that start with the letter 'a':
        abacus
        apple
*/
