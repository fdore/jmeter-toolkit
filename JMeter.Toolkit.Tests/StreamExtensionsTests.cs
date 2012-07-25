using System.IO;
using System.Text;
using JMeter.Toolkit.Engine.Extensions;
using NUnit.Framework;

namespace JMeter.Toolkit.Tests
{
   
    [TestFixture]
    public class StreamExtensionsTests
    {
        [TestCase("Here is some content")]
        public void TestReadContent(string input)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var result = stream.ReadContent();
            Assert.That(result == input, "Expected: {0}, but was: {1}", input, result);
        }
    }
}
