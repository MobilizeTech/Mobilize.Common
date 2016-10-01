using System;

namespace Mobilize.Common.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Determine how many years old
        /// </summary>
        /// <param name="dt">DateTime to check</param>
        /// <returns>Number of years old</returns>
        public static int CalculateAgeInYears(this DateTime dt)
        {
            var age = DateTime.Now.Year - dt.Year;
            if (DateTime.Now < dt.AddYears(age))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }

        /// <summary>
        /// Convert to a string representation of how long ago (e.g. '35 minutes ago' or '2 months ago')
        /// </summary>
        /// <param name="dt">DateTime to convert</param>
        /// <returns>String representation of how long ago</returns>
        public static string ToReadableTime(this DateTime dt)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.ToUniversalTime().Ticks);
            double delta = ts.TotalSeconds;
            if (delta < 60)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 120)
            {
                return "a minute ago";
            }
            if (delta < 2700) // 45 * 60
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 5400) // 90 * 60
            {
                return "an hour ago";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "yesterday";
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                return ts.Days + " days ago";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }
}