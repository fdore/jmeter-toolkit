using System;
using System.IO;
using JMeter.Toolkit.Engine.Charts;
using JMeter.Toolkit.Services.Spec;
using NUnit.Framework;
using System.Collections.Generic;

namespace JMeter.Toolkit.Tests
{
    [TestFixture]
    public class ChartBuilderTests
    {
        [Test]
        public void TestGenerateShouldReturnImage()
        {
            var chartBuilder = new ChartBuilder();
            var stream = new MemoryStream();
            var dataResults = new List<RequestDataResults>();
            int count = 3;
            for (int i = 0; i < count;i++ )
            {
                dataResults.Add(new RequestDataResults() { AverageResponseTime = 100 * i, Date = DateTime.Now, Request = "Request" + i });
            }
                
            chartBuilder.Generate(stream, dataResults);
            Assert.That(stream.Length > 0);
        }
    }
}
