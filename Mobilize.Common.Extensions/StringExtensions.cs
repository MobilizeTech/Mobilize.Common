using System;
using System.Linq;

namespace Mobilize.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Syntactical sugar for "String.IsNullOrEmpty(x)"
        /// </summary>
        /// <param name="this">Extends the string</param>
        /// <returns>True if null or empty, otherwise false.</returns>
        public static bool IsBlank(this string value)
        {
            return String.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Extracts digits from string
        /// </summary>
        /// <param name="value">String to extract</param>
        /// <returns>String with only digits</returns>
        public static string ExtractDigits(this string value)
        {
            return new string(value?.Where(c => char.IsDigit(c)).ToArray());
        }

        /// <summary>
        /// Provides an overload to String.Contains to specify a StringComparison (i.e. allows for case-insensitive searches)
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="toCheck">String to check for</param>
        /// <param name="comparisonType">Type of comparison to use</param>
        /// <returns>Boolean</returns>
        public static bool Contains(this string source, string toCheck, StringComparison comparisonType)
        {
            if (source == null)
            {
                throw new ArgumentNullException("this");
            }

            return source.IndexOf(toCheck, comparisonType) >= 0;
        }
    }
}