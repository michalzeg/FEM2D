using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class DoubleExtensionMethods
    {
        /// <summary>
        /// Maximum difference allowed in comparing two doubles to determine if they are equal 
        /// </summary>
        public const double MaximumDifferenceAllowed = 0.0000001;

        /// <summary>
        /// Method for checking if two doubles are equal with difference given by <see cref="MaximumDifferenceAllowed"/>
        /// </summary>
        /// <param name="initialValue">The first double value</param>
        /// <param name="value">The second double value</param>
        /// <returns>Returns true if the given number are equal with difference given by <see cref="MaximumDifferenceAllowed"/></returns>
        public static bool IsApproximatelyEqualTo(this double initialValue, double value)
        {
            var result = DoubleExtensionMethods.IsApproximatelyEqualTo(initialValue, value, MaximumDifferenceAllowed);
            return result;
        }

        /// <summary>
        /// Method for checking if two doubles are equal with the given difference/>
        /// </summary>
        /// <param name="initialValue">The first double value</param>
        /// <param name="value">The second double value</param>
        /// <param name="maximumDifferenceAllowed">The difference between two double to determine if they are equal</param>
        /// <returns>Returns true if the given number are equal with the given difference</returns>
        public static bool IsApproximatelyEqualTo(this double initialValue, double value, double maximumDifferenceAllowed)
        {
            return (Math.Abs(initialValue - value) < maximumDifferenceAllowed);
        }

        public static bool IsApproximatelyEqualToZero(this double initialValue)
        {
            return IsApproximatelyEqualTo(initialValue, 0d);
        }

        /// <summary>
        /// Rounds the given double to the 2 digits
        /// </summary>
        /// <param name="initialValue">The number to round</param>
        /// <returns>Rounded number</returns>
        public static double Round(this double initialValue)
        {
            return DoubleExtensionMethods.Round(initialValue, 2);
        }

        /// <summary>
        /// Rounds the given double to the given number of digits
        /// </summary>
        /// <param name="initialValue">The number to round</param>
        /// <param name="numberOfDigits">The number of digits the number is round to</param>
        /// <returns>Rounded number</returns>
        public static double Round(this double initialValue, int numberOfDigits)
        {
            return (Math.Round(initialValue, numberOfDigits));
        }

        /// <summary>
        /// Check if the given double is NaN
        /// </summary>
        /// <param name="initialValue">The number</param>
        /// <returns>Returns true if given double is NaN</returns>
        public static bool IsNaN(this double initialValue)
        {
            return double.IsNaN(initialValue);
        }

        /// <summary>
        /// Simplified method to calculate the power of the given number
        /// </summary>
        /// <param name="base">The base of the power</param>
        /// <param name="exponent">The exponent</param>
        /// <returns></returns>
        public static double Power(this double @base,double exponent)
        {
            return Math.Pow(@base, exponent);
        }
    }
}
