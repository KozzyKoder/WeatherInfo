using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class MathExtensions
    {
        public static int? Floor(double? value)
        {
            if (value.HasValue)
            {
                return Convert.ToInt32(Math.Round(value.Value));
            }

            return null;
        }
    }
}
