using System;
using JMeter.Toolkit.Engine.XSD;

namespace JMeter.Toolkit.Tests.Helpers
{
    public class TestResultsBuilder
    {
        private testResults _testResults;

        public TestResultsBuilder()
        {
            _testResults = new testResults();
        }

        public TestResultsBuilder AddSamples(string request, int count, double expectedAverage)
        {
            for (int i = 0; i < count; i++)
            {
                var salt = new Random().Next(500);
                var value1 = expectedAverage - salt;
                var value2 = expectedAverage + salt;
                _testResults.httpSample.Add(new testResultsHttpSample { t = value1, lb = request, ts = 1321978933941 });
                _testResults.httpSample.Add(new testResultsHttpSample { t = value2, lb = request });
            }
            return this;
        }

        public TestResultsBuilder AddSamples(string request, int count, double minValue, double maxValue)
        {
            for (int i = 0; i < count; i++)
            {
                var value = new Random().Next((int)minValue, (int)maxValue);
                _testResults.httpSample.Add(new testResultsHttpSample { t = value, lb = request, ts = 1321978933941 });
            }
            return this;
        }

        public testResults Build()
        {
            return _testResults;
        }
    }
}
