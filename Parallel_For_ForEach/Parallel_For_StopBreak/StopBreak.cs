using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_For_StopBreak
{
    class StopBreak
    {
        static void Main( string[] args )
        {
            int N = 1000;
            Parallel.For( 0, N, ( i, loopState ) =>
            {
                Console.WriteLine( i );
                if( i == 50 )
                {
                    // Stop()은 모든 반복 작업이 필요 없을때 반복 루프가 남아 있더라도 중지 
                    // Break()는 현재 반복을 완료하고 이전에 실행 되었던 반복을 다 완료후 중지
                    loopState.Break(); 
                    
                }
            } );

        }
    }
}
