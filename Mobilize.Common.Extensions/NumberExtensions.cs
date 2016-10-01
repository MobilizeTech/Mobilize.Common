using System;
using System.Linq;

namespace Mobilize.Common.Extensions
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Formats a number into bytes
        /// </summary>
        /// <param name="l">Number to format</param>
        /// <returns>String representing size in bytes</returns>
        public static string ToFileSize(this long l)
        {
            if (l < 1024)
            {
                return (l).ToString("F0") + " bytes";
            }

            if (l < Math.Pow(1024, 2))
            {
                return (l / 1024).ToString("F0") + "KB";
            }

            if (l < Math.Pow(1024, 3))
            {
                return (l / Math.Pow(1024, 2)).ToString("F0") + "MB";
            }

            if (l < Math.Pow(1024, 4))
            {
                return (l / Math.Pow(1024, 3)).ToString("F0") + "GB";
            }

            if (l < Math.Pow(1024, 5))
            {
                return (l / Math.Pow(1024, 4)).ToString("F0") + "TB";
            }

            if (l < Math.Pow(1024, 6))
            {
                return (l / Math.Pow(1024, 5)).ToString("F0") + "PB";
            }

            return (l / Math.Pow(1024, 6)).ToString("F0") + "EB";
        }

        /// <summary>
        /// Checks if an integer is prime
        /// </summary>
        /// <param name="n">The integer to check</param>
        /// <returns>Boolean</returns>
        public static bool IsPrime(this int n)
        {
            if (n < 3) return (n == 2);
            return Enumerable.Range(2, (int)Math.Sqrt(n)).All(m => n % m != 0);
        }
    }
}