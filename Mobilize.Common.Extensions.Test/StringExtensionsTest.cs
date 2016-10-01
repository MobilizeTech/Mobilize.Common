using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mobilize.Common.Extensions.Test
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void TestIsBlank()
        {
            string @null = null;
            string empty = "";
            string whitespace = "       ";
            string words = "purple monkey dishwasher";
            Assert.IsTrue(@null.IsBlank());
            Assert.IsTrue(empty.IsBlank());
            Assert.IsTrue(whitespace.IsBlank());
            Assert.IsFalse(words.IsBlank());
        }

        [TestMethod]
        public void TestExtractDigits()
        {
            string s = "1a2b3C4/5*6p7X8w9J0@";
            Assert.AreEqual("1234567890", s.ExtractDigits());
        }

        [TestMethod]
        public void TestContainsCaseInsensitive()
        {
            string s = "The quick brown fox jumps over the lazy dog";
            Assert.IsTrue(s.Contains("BROWN", StringComparison.CurrentCultureIgnoreCase));
        }
    }
}