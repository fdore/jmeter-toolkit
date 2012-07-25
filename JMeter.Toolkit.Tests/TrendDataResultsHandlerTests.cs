using System;
using System.Collections.Generic;
using JMeter.Toolkit.Engine.Processor;
using JMeter.Toolkit.Services.Spec;
using JMeter.Toolkit.Tests.Helpers;
using NUnit.Framework;

namespace JMeter.Toolkit.Tests
{
    [TestFixture]
    public class TrendDataResultsHandlerTests
    {
        [Test]
        public void TestCanHandleShouldReturnFalseIfAllRequestsAreFromTheSameDate()
        {
            var handler = new TrendDataResultsHandler();
            var dataResults = new List<RequestDataResults>();
            var date = DateTime.Now;
            int count = 1000;
            for (int i = 0; i < count; i++)
            {
                dataResults.Add(new RequestDataResults { Date = date });
            }
            var result = handler.CanHandle(dataResults);
            Assert.That(result == false);
        }

        [Test]
        public void TestProcessResultsShouldReturnEmptyListIfAllRequestsAreFromTheSameDate()
        {
            var handler = new TrendDataResultsHandler();
            var dataResults = new List<RequestDataResults>();
            var date = DateTime.Now;
            const int count = 1000;
            for (int i = 0; i < count; i++)
            {
                dataResults.Add(new RequestDataResults { Date = date });
            }
            var result = handler.ProcessResults(dataResults);
            Assert.That(result.Length == 0);
        }

        [Test]
        public void TestCanHandleShouldReturnTrueIfAllRequestsAreFromDifferentDates()
        {
            var handler = new TrendDataResultsHandler();
            var dataResults = new List<RequestDataResults>();
            const int count = 1000;
            for (int i = 0; i < count; i++)
            {
                dataResults.Add(new RequestDataResults { Date = DateTime.Now.AddDays(i) });
            }
            var result = handler.CanHandle(dataResults);
            Assert.That(result);
        }

        [Test]
        public void TestProcessResultsShouldReturnCorrectNumberOfCharts()
        {
            var handler = new TrendDataResultsHandler();
            const int requestCount = 7;
            const int iterations = 50;
            var results = new DataResultsBuilder().AddSamples(requestCount, iterations, 100, 300, true).Build();
            var charts = handler.ProcessResults(results);
            Assert.That(charts.Length == requestCount);
        }

        [Test]
        public void TestProcessResultsShouldReturnCorrectInformation()
        {
            var handler = new TrendDataResultsHandler();
            const int requestCount = 1;
            const int iterations = 50;
            var results = new DataResultsBuilder().AddSamples("Test/", requestCount, iterations, 100, 300, true).Build();
            var charts = handler.ProcessResults(results);
            Assert.That(charts[0].Request == "Test/0");
            Assert.That(charts[0].Data != null);
        }

         
    }
}
