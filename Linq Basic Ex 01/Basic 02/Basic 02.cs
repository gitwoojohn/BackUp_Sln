using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_02
{
    class Program
    {
        static void Main( string[] args )
        {
            Galaxy ga = new Galaxy();
            ga.DoIterateList();
        }
    }

    public class Galaxy
    {
        public string Name { get; set; }
        public int MegaLightYears { get; set; }

        public void DoIterateList()
        {
            IterateThroughList();
        }

        private void IterateThroughList()
        {
            var theGalaxies = new List<Galaxy>
        {
            new Galaxy() { Name="Tadpole", MegaLightYears=400},
            new Galaxy() { Name="Pinwheel", MegaLightYears=25},
            new Galaxy() { Name="Milky Way", MegaLightYears=0},
            new Galaxy() { Name="Andromeda", MegaLightYears=3}
        };

            foreach (Galaxy theGalaxy in theGalaxies)
            {
                Console.WriteLine( theGalaxy.Name + "  " + theGalaxy.MegaLightYears );
            }
        }
    }
}
