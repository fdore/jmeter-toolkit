using System;
using JMeter.Toolkit.Engine.Extensions;
using NUnit.Framework;

namespace JMeter.Toolkit.Tests
{
    [TestFixture]
    public class DoubleExtensionsTests
    {
        [Test]
        public void TestToDateTime()
        {
            double timestamp = 1321978933000;
            var expectedDate = new DateTime(2011, 11, 22, 16, 22, 13);
            var result = timestamp.ToDateTime();
            Assert.That(result == expectedDate);
        }
    }
}
