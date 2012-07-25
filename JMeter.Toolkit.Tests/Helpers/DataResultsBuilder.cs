using System;
using System.Collections.Generic;
using JMeter.Toolkit.Services.Spec;

namespace JMeter.Toolkit.Tests.Helpers
{
    public class DataResultsBuilder
    {
        private IList<RequestDataResults> _dataResults;

        public DataResultsBuilder()
        {
            _dataResults = new List<RequestDataResults>();
        }

        public DataResultsBuilder AddSamples(int requestCount, int iterations, int minValue, int maxValue, bool changeDate)
        {
            return AddSamples("Test/", requestCount, iterations, minValue, maxValue, changeDate);
        }

        public DataResultsBuilder AddSamples(string requestBase, int requestCount, int iterations, int minValue, int maxValue, bool changeDate)
        {
            var now = DateTime.Now;
            for (int i = 0; i < requestCount; i++)
            {
                var request = requestBase + i;

                var value = new Random().Next(minValue, maxValue);
                for (int j = 0; j < iterations; j++)
                {
                    var date = now;
                    if (changeDate)
                        date = now.AddDays(j);
                    _dataResults.Add(new RequestDataResults
                                         {
                                             AverageResponseTime = value,
                                             Date = date,
                                             HostName = "localhost",
                                             MinResponseTime = minValue,
                                             MaxResponseTime = maxValue,
                                             Request = request
                                         });
                }
            }
            return this;
        }

        public IList<RequestDataResults> Build()
        {
            return _dataResults;
        }
    }
}