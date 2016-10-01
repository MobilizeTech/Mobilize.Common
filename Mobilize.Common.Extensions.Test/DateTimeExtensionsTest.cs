using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mobilize.Common.Extensions.Test
{
    [TestClass]
    public class DateTimeExtensionsTest
    {
        private const int i = 15;

        [TestMethod]
        public void TestCalculateAgeInYearsPast()
        {
            DateTime dt = DateTime.Now.AddYears(-i);
            Assert.AreEqual(i, dt.CalculateAgeInYears());
        }

        [TestMethod]
        public void TestCalculateAgeInYearsNow()
        {
            DateTime dt = DateTime.Now;
            Assert.AreEqual(0, dt.CalculateAgeInYears());
        }

        [TestMethod]
        public void TestCalculateAgeInYearsFuture()
        {
            DateTime dt = DateTime.Now.AddYears(i);
            Assert.AreEqual(0, dt.CalculateAgeInYears());
        }

        [TestMethod]
        public void TestToReadableTimeMinutes()
        {
            DateTime dt = DateTime.Now.AddMinutes(-i);
            Assert.AreEqual(string.Format("{0} minutes ago", i), dt.ToReadableTime());
        }

        [TestMethod]
        public void TestToReadableTimeYears()
        {
            DateTime dt = DateTime.Now.AddYears(-i);
            Assert.AreEqual(string.Format("{0} years ago", i), dt.ToReadableTime());
        }

        [TestMethod]
        public void TestToReadableTimeSingleMinute()
        {
            DateTime dt = DateTime.Now.AddMinutes(-1);
            Assert.AreEqual(string.Format("a minute ago", i), dt.ToReadableTime());
        }

        [TestMethod]
        public void TestToReadableTimeSingleDay()
        {
            DateTime dt = DateTime.Now.AddDays(-1);
            Assert.AreEqual(string.Format("yesterday", i), dt.ToReadableTime());
        }
    }
}