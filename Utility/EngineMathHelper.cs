using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeyStoneEngine.Utility
{
    public static class EngineMathHelper
    {
        public static int RoundDown(this int integer, int size)
        {
            return (int)(Math.Floor((double)(integer / size)) * size);
        }

        public static int RoundUp(this int integer, int size)
        {
            return (int)(Math.Ceiling((double)(integer / size)) * size);
        }

        public static float RoundDown(this float integer, int size)
        {
            return (int)(Math.Floor((double)(integer / size)) * size);
        }

        public static float RoundUp(this float integer, int size)
        {
            return (int)(Math.Ceiling((double)(integer / size)) * size);
        }
    }
}
