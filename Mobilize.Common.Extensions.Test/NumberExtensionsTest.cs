using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mobilize.Common.Extensions.Test
{
    [TestClass]
    public class NumberExtensionsTest
    {
        [TestMethod]
        public void TestIsPrime()
        {
            int[] primes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109 };
            int[] nonPrimes = new int[] { 4, 6, 8, 9, 10, 12, 14, 15, 16, 18, 20, 21, 22, 24, 25, 26, 27, 28, 30, 32, 33, 34, 35, 36, 38, 39, 40, 42, 44, 45, 46, 48 };

            foreach (int prime in primes)
            {
                Assert.IsTrue(prime.IsPrime());
            }

            foreach (int nonPrime in nonPrimes)
            {
                Assert.IsFalse(nonPrime.IsPrime());
            }
        }

        [TestMethod]
        public void TestToFileSize()
        {
            long oneMegabyte = 1048576;
            Assert.AreEqual("1MB", oneMegabyte.ToFileSize());

            long fortyGigabytes = 42949672960;
            Assert.AreEqual("40GB", fortyGigabytes.ToFileSize());
        }
    }
}
