using System.Collections.Generic;
using System.IO;
using System.Linq;
using JMeter.Toolkit.Engine.Charts;
using JMeter.Toolkit.Services.Spec;

namespace JMeter.Toolkit.Engine.Processor
{
    public class TrendDataResultsHandler : DataResultsHandler
    {
      
        protected override Chart[] HandleResults(IEnumerable<RequestDataResults> results)
        {
            var charts = new List<Chart>();
            var query = results.GroupBy(r => r.Request);
            query.ToList().ForEach(group =>
            {
                int pos = group.Key.LastIndexOf('?');
                string name = group.Key;
                if (pos != -1)
                {
                    int count = charts.Count(c => c.Request == group.Key);
                    name = group.Key.Substring(0, pos);
                    if (count > 0)
                    {
                        name += count;
                    }

                }
                var stream = new MemoryStream();
                var trendChartBuilder = new TrendChartBuilder();
                trendChartBuilder.Generate(stream, group.ToList());
                charts.Add(new Chart { Data = stream.GetBuffer(), Name = name, Request = group.Key});
            });
            return charts.ToArray();
        }

        public override bool CanHandle(IEnumerable<RequestDataResults> results)
        {
            var query = results.GroupBy(x => x.Date.ToShortDateString());
            if (query.Count() > 1)
                return true;
            return false;
        }
    }
}
