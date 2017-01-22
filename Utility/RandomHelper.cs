using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeyStoneEngine.Utility
{
    public static class RandomHelper
    {
        public static double NextDouble(this Random rand, double minimum, double maximum)
        {
            return rand.NextDouble() * (maximum - minimum) + minimum;
        }

        public static bool NextCoinFlip(this Random rand)
        {   //non inclusive, so either 0 or 1
            return rand.Next(2) == 0;
        }
    }
}
