using System;
using System.Collections.Generic;
using System.Linq;
using JMeter.Toolkit.Engine.Extensions;
using JMeter.Toolkit.Engine.XSD;
using JMeter.Toolkit.Services.Spec;
using testResults = JMeter.Toolkit.Engine.XSD.testResults;

namespace JMeter.Toolkit.Engine.Processor
{
    public class TestResultsProcessor
    {
        private testResults _testResults;
        private IList<RequestDataResults> _dataResults; 

        public TestResultsProcessor(testResults testResults)
        {
            _testResults = testResults;
        }

        private double[] CalculateDistribution(IEnumerable<testResultsHttpSample> dataResults)
        {
            if(dataResults.Count() >= 10)
            {
                var sortedList = dataResults.OrderBy(x => x.t);
                int totalResults = 10;
                int count = sortedList.Count() / totalResults;
                var results = new double[totalResults];
                for (int i = 0; i < totalResults; i++)
                {
                    results[i] = sortedList.Skip(i * count).Take(count).Average(x => x.t);
                }
                return results;
            }
            return new double[0];
        }

        /// <summary>
        /// Generate DataResults containing various information like average response time, number of users... from testResults
        /// </summary>
        public void CreateDataResults()
        {
            _dataResults = new List<RequestDataResults>();
            var query = _testResults.httpSample.GroupBy(x => x.lb);
            query.ToList().ForEach(group =>
                                          {
                                              var first = group.First();
                                              int firstCount = 90*group.Count()/100;
                                              int secondCount = 80*group.Count()/100;
                                              var filteredList = group.OrderBy(x => x.t)
                                                                        .Take(firstCount)
                                                                        .OrderByDescending(x => x.t)
                                                                        .Take(secondCount);
                                              if (!filteredList.Any())
                                                  filteredList = group;
                                              var requestDataResults = new RequestDataResults
                                                                           {
                                                                               AverageResponseTime = Math.Round(group.Average(x => x.t), 0),
                                                                               MinResponseTime = Math.Round(group.Min(x => x.t), 0),
                                                                               MaxResponseTime = Math.Round(group.Max(x => x.t), 0),
                                                                               Request = first.lb,
                                                                               Date = first.ts.ToDateTime(),
                                                                               UserCount = first.ng,
                                                                               HostName = first.hn,
                                                                               MinResponseTimeExcludingTopDecile = Math.Round(filteredList.Min(x => x.t), 0),
                                                                               MaxResponseTimeExcludingBottomDecile = Math.Round(filteredList.Max(x => x.t), 0),
                                                                               AverageResponseTimeExcludingTopAndBottomDeciles = Math.Round(filteredList.Average(x => x.t), 0),
                                                                               ResponseTimeDistribution = CalculateDistribution(group.ToList())

                                                                           };
                                              _dataResults.Add(requestDataResults);
                                          });

        }

        public IEnumerable<RequestDataResults> DataResults { get { return _dataResults; } }

    }

   
}
