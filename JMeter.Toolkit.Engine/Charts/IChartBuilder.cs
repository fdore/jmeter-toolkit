using System.Collections.Generic;
using System.IO;
using JMeter.Toolkit.Services.Spec;

namespace JMeter.Toolkit.Engine.Charts
{
    public interface IChartBuilder
    {
        string Title { get; set; }
        void Generate(Stream stream, IEnumerable<RequestDataResults> dataResults);
    }
}
