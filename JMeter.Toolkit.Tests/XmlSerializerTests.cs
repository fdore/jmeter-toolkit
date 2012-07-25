using System.IO;
using System.Text;
using JMeter.Toolkit.Engine;
using JMeter.Toolkit.Engine.XSD;
using NUnit.Framework;

namespace JMeter.Toolkit.Tests
{
    [TestFixture]
    public class XmlSerializerTests
    {
        private string file =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<testResults version=""1.2"">
	<httpSample t=""81"" lt=""80"" ts=""1321978933939"" s=""true"" lb=""/EN/Flights/Add"" rc=""200"" rm=""OK"" tn=""Thread Group 1-5"" dt=""text"" by=""17443"" ng=""9"" na=""9"">
	  <assertionResult>
		<name>Response Assertion</name>
		<failure>false</failure>
		<error>false</error>
	  </assertionResult>
	</httpSample>	
	<httpSample t=""131"" lt=""130"" ts=""1321978933958"" s=""true"" lb=""/EN/Flights/Add"" rc=""200"" rm=""OK"" tn=""Thread Group 1-7"" dt=""text"" by=""17443"" ng=""10"" na=""10"">
	  <assertionResult>
		<name>Response Assertion</name>
		<failure>false</failure>
		<error>false</error>
	  </assertionResult>
	</httpSample>
	<httpSample t=""96"" lt=""96"" ts=""1321978934025"" s=""true"" lb=""/EN/Flights/Add"" rc=""200"" rm=""OK"" tn=""Thread Group 1-8"" dt=""text"" by=""17443"" ng=""10"" na=""10"">
	  <assertionResult>
		<name>Response Assertion</name>
		<failure>false</failure>
		<error>false</error>
	  </assertionResult>
	</httpSample>
</testResults>";

        [Test]
        public void TestDeserializeStream()
        {
            var xmlSerializer = new XmlSerializer<testResults>();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(file));
            var result = xmlSerializer.Deserialize(stream);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.httpSample.Count == 3);
        }

    }
}
