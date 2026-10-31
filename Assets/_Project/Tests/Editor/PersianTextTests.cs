using NUnit.Framework;
using Charso.Core;

namespace Charso.Tests
{
    public sealed class PersianTextTests
    {
        [TestCase("كتاب", "کتاب")]
        [TestCase("يكي", "یکی")]
        [TestCase("می\u200cروم", "میروم")]
        [TestCase("کِتاب", "کتاب")]
        [TestCase("۱۲۳", "123")]
        public void NormalizeForMatch_MapsPersianVariants(string input, string expected)
        {
            Assert.AreEqual(expected, PersianText.NormalizeForMatch(input));
        }
    }
}