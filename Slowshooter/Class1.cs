using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slowshooter
{
    static class IntExtensions
    {
        public static int Clamp(this int value, int min, int max)
        {
            return Math.Max(min, Math.Min(value, max));
        }
    }
}
