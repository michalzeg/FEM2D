using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class StringExtensionMethods
    {
        public static string ToFormatedString(this double initialValue)
        {
            return initialValue.Round().ToString("F");
        }

        public static string ToDot(this string value)
        {
            return value.Replace(',', '.');
        }
    }
}
