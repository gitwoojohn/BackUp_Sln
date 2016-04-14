using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartGeneric
{
    public struct Complex
    {
        public int real;
        public int imaginary;

        public Complex( int real, int imaginary )
        {
            this.real = real;
            this.imaginary = imaginary;
        }

        // Declare which operator to overload (+), the types 
        // that can be added (two Complex objects), and the 
        // return type (Complex):
        public static Complex operator +( Complex c1, Complex c2 )
        {
            return new Complex( c1.real + c2.real, c1.imaginary + c2.imaginary );
        }
        public static Complex operator -( Complex c1, Complex c2 )
        {
            return new Complex( c1.real - c2.real, c1.imaginary - c2.imaginary );
        }
        // Override the ToString method to display an complex number in the suitable format:
        public override string ToString()
        {
            return ( String.Format( "{0} + {1}i", real, imaginary ) );
        }

    }

}