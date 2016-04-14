using System;

namespace ExtendedMethod
{
    public static class MyExtension
    {
        public static int WordCount( this string str )
        {
            return str.Split( new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries ).Length;
        }
    }
}
