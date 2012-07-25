using System;
using System.Collections.Generic;
using JMeter.Toolkit.Engine.Processor;
using JMeter.Toolkit.Services.Spec;
using JMeter.Toolkit.Tests.Helpers;
using NUnit.Framework;

namespace JMeter.Toolkit.Tests
{
    [TestFixture]
    public class ComparisonDataResultsHandlerTests
    {

        [Test]
        public void TestCanHandleShouldReturnTrueIfAllRequestsAreFromTheSameDate()
        {
            var handler = new ComparisonDataResultsHandler();
            var dataResults = new List<RequestDataResults>();
            var date = DateTime.Now;
            int count = 1000;
            for (int i = 0; i < count; i++)
            {
                dataResults.Add(new RequestDataResults { Date = date});
            }
            var result = handler.CanHandle(dataResults);
            Assert.That(result);
        }

        [Test]
        public void TestCanHandleShouldReturnFalseIfAllRequestsAreFromDifferentDates()
        {
            var handler = new ComparisonDataResultsHandler();
            var dataResults = new List<RequestDataResults>();
            int count = 1000;
            for (int i = 0; i < count; i++)
            {
                dataResults.Add(new RequestDataResults { Date = DateTime.Now.AddDays(i) });
            }
            var result = handler.CanHandle(dataResults);
            Assert.That(result == false);
        }

        [Test]
        public void TestProcessResultsShouldReturnEmptyListOfChartsIfComparisonDataResultsHandlerCantHandlerData()
        {
            var handler = new ComparisonDataResultsHandler();
            var dataResults = new List<RequestDataResults>();
            int count = 1000;
            for (int i = 0; i < count; i++)
            {
                dataResults.Add(new RequestDataResults { Date = DateTime.Now.AddDays(i) });
            }
            var result = handler.ProcessResults(dataResults);
            Assert.That(result.Length == 0);
        }

        [Test]
        public void TestProcessResultsShouldReturnCorrectNumberOfCharts()
        {
            var handler = new ComparisonDataResultsHandler();
            int requestCount = 7;
            int iterations = 50;
            var results = new DataResultsBuilder().AddSamples(requestCount, iterations, 100, 300, false).Build();
            var charts = handler.ProcessResults(results);
            Assert.That(charts.Length == 1);
        }

        [Test]
        public void TestProcessResultsShouldReturnCorrectInformation()
        {
            var handler = new ComparisonDataResultsHandler();
            int requestCount = 1;
            int iterations = 50;
            var results = new DataResultsBuilder().AddSamples("Test/", requestCount, iterations, 100, 300, false).Build();
            var charts = handler.ProcessResults(results);
            Assert.That(charts[0].Data != null);
        }
    }
}
