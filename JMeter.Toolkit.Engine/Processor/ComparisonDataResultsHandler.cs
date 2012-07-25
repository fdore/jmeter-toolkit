using System.Collections.Generic;
using System.IO;
using System.Linq;
using JMeter.Toolkit.Engine.Charts;
using JMeter.Toolkit.Services.Spec;

namespace JMeter.Toolkit.Engine.Processor
{
    public class ComparisonDataResultsHandler : DataResultsHandler
    {
        protected override Chart[] HandleResults(IEnumerable<RequestDataResults> results)
        {
            var builder = new ChartBuilder();
            var stream = new MemoryStream();
            builder.Generate(stream, results);
            return new[] { new Chart { Data = stream.GetBuffer(), Name = string.Empty}};
        }

        public override bool CanHandle(IEnumerable<RequestDataResults> results)
        {
            var query = results.GroupBy(x => x.Date);
            if (query.Count() > 1)
                return false;
            return true;
        }
    }
}
