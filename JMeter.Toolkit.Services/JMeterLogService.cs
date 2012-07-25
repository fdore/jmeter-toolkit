using System.IO;
using System.Linq;
using System.Text;
using JMeter.Toolkit.Engine;
using JMeter.Toolkit.Engine.Processor;
using JMeter.Toolkit.Engine.XSD;
using JMeter.Toolkit.Services.Spec;

namespace JMeter.Toolkit.Services
{
    public class JMeterLogService : IJMeterLogService
    {
        private DataResultsHandler _handler;

        public JMeterLogService()
        {
            _handler = new TrendDataResultsHandler();
            _handler.AppendNextHandler(new ComparisonDataResultsHandler());
        }

        public JMeterDataResultsResponse ProcessLogs(JMeterLogsRequest request)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(request.Logs));
            // Deserialize logs
            var xmlSerializer = new XmlSerializer<testResults>();
            var testResults = xmlSerializer.Deserialize(stream);

            // create data results
            var dataProcessor = new TestResultsProcessor(testResults);
            dataProcessor.CreateDataResults();
            return new JMeterDataResultsResponse() {DataResults = dataProcessor.DataResults.ToArray()};
        }

        public ChartResponse GenerateCharts(JMeterDataResultsRequest request)
        {
            var results = _handler.ProcessResults(request.DataResults);
            return new ChartResponse {Charts = results.ToArray()};
        }

    }
}
