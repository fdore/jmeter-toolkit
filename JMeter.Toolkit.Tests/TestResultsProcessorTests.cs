using System;
using System.Diagnostics;
using System.Linq;
using JMeter.Toolkit.Engine.Processor;
using JMeter.Toolkit.Engine.XSD;
using JMeter.Toolkit.Tests.Helpers;
using NUnit.Framework;

namespace JMeter.Toolkit.Tests
{
    [TestFixture]
    public class TestResultsProcessorTests
    {
        [Test]
        public void TestAverageResponseTime()
        {
            const int count = 1000;
            const int median = 500;
            const string request = "/Test.hml";
            var testResults = new TestResultsBuilder().AddSamples(request, count, median).Build();
            
            var processor = new TestResultsProcessor(testResults);
            processor.CreateDataResults();
            var result =
                processor.DataResults.AsQueryable().SingleOrDefault(x => x.Request == request).AverageResponseTime;
            Assert.That(result == median, "Expected: {0}, but was {1}", median, result);
        }

        [Test]
        public void TestMinResponseTime()
        {
            const int minValue = 500;
            const string request = "/Test.hml";
            var testResults = new TestResultsBuilder().AddSamples(request, minValue, 2 * minValue).Build();
            testResults.httpSample.Add(new testResultsHttpSample { t = minValue, lb = request, ts = 1321978933941 });

            var processor = new TestResultsProcessor(testResults);
            processor.CreateDataResults();
            var result =
                processor.DataResults.AsQueryable().SingleOrDefault(x => x.Request == request).MinResponseTime;
            Assert.That(result == minValue, "Expected: {0}, but was {1}", minValue, result);
        }

        [Test]
        public void TestMaxResponseTime()
        {
            const int count = 1000;
            const int maxValue = 800;
            const string request = "/Test.hml";
            var testResults = new TestResultsBuilder().AddSamples(request, count, maxValue / 2, maxValue).Build();
            testResults.httpSample.Add(new testResultsHttpSample { t = maxValue, lb = request, ts = 1321978933941 });

            var processor = new TestResultsProcessor(testResults);
            processor.CreateDataResults();
            var result =
                processor.DataResults.AsQueryable().SingleOrDefault(x => x.Request == request).MaxResponseTime;
            Assert.That(result == maxValue, "Expected: {0}, but was {1}", maxValue, result);
        }

        [Test]
        public void TestAverageResponseTimeWhithMultipleRequests()
        {
            const int count = 1000;
            const int median1 = 100;
            const int median2 = 250;
            const int median3 = 500;
            const string request1 = "/Test.hml"; 
            const string request2 = "/Test/Request.aspx";
            const string request3 = "/Test2/Page.aspx";
            var testResults = new TestResultsBuilder().AddSamples(request1, count, median1)
                .AddSamples(request2, count, median2)
                .AddSamples(request3, count, median3)
                .Build();

            var processor = new TestResultsProcessor(testResults);
            processor.CreateDataResults();
            var result1 =
                processor.DataResults.AsQueryable().SingleOrDefault(x => x.Request == request1).AverageResponseTime;
            Assert.That(result1 == median1, "Expected: {0}, but was {1}", median1, result1);
            var result2 =
               processor.DataResults.AsQueryable().SingleOrDefault(x => x.Request == request2).AverageResponseTime;
            Assert.That(result2 == median2, "Expected: {0}, but was {1}", median2, result2);
            var result3 =
               processor.DataResults.AsQueryable().SingleOrDefault(x => x.Request == request3).AverageResponseTime;
            Assert.That(result3 == median3, "Expected: {0}, but was {1}", median3, result3);
        }

        [Test]
        public void TestPerformanceCreateDataResults()
        {
            const string request1 = "/Test.hml";
            const string request2 = "/Test/Request.aspx";
            const string request3 = "/Test2/Page.aspx";
            const int count = 10000;
            const int median1 = 100;
            const int median2 = 250;
            const int median3 = 500;
            var testResults = new TestResultsBuilder().AddSamples(request1, count, median1)
                 .AddSamples(request2, count, median2)
                 .AddSamples(request3, count, median3)
                 .Build();

            var sw = new Stopwatch();
            var processor = new TestResultsProcessor(testResults);
            sw.Start();
            processor.CreateDataResults();
            sw.Stop();
            Console.WriteLine("CreateDataResults - Ellapsed: " + sw.ElapsedMilliseconds);
           
        }

        [Test]
        public void TestMinResponseTimeExcludingTopDecile()
        {
            var testResults = new testResults();

            const string request = "/Test.hml";
            const int start = 1;
            for (int i = start; i < 11; i++)
            {
                testResults.httpSample.Add(new testResultsHttpSample { t = i, lb = request, ts = 1321978933941 });
            }

            var processor = new TestResultsProcessor(testResults);
            processor.CreateDataResults();
            var result = processor.DataResults.First().MinResponseTimeExcludingTopDecile;
            const int expectedResult = start + 1;
            Assert.That(result == expectedResult, "Expected: {0}, but was {1}", expectedResult, result);
        }

        [Test]
        public void TestMaxResponseTimeExcludingBottomDecile()
        {
            var testResults = new testResults();

            const string request = "/Test.hml";
            const int end = 11;
            for (int i = 1; i < end; i++)
            {
                testResults.httpSample.Add(new testResultsHttpSample { t = i, lb = request, ts = 1321978933941 });
            }

            var processor = new TestResultsProcessor(testResults);
            processor.CreateDataResults();
            var result = processor.DataResults.First().MaxResponseTime;
            const int expectedResult = end-1;
            Assert.That(result == expectedResult, "Expected: {0}, but was {1}", expectedResult, result);
        }

        [Test]
        public void TestAverageResponseTimeExcludingTopAndBottomDeciles()
        {
            var testResults = new testResults();

            const string request = "/Test.hml";
            const int end = 10;
            for (int i = 1; i < end; i++)
            {
                testResults.httpSample.Add(new testResultsHttpSample { t = i, lb = request, ts = 1321978933941 });
            }

            var processor = new TestResultsProcessor(testResults);
            processor.CreateDataResults();
            var result = processor.DataResults.First().AverageResponseTimeExcludingTopAndBottomDeciles;
            const int expectedResult = 5;
            Assert.That(result == expectedResult, "Expected: {0}, but was {1}", expectedResult, result);
        }

    }
}
