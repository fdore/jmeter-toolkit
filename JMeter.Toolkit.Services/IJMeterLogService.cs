using System.ServiceModel;
using JMeter.Toolkit.Services.Spec;

namespace JMeter.Toolkit.Services
{
    [ServiceContract]
    public interface IJMeterLogService
    {
        [OperationContract]
        JMeterDataResultsResponse ProcessLogs(JMeterLogsRequest request);

        [OperationContract]
        ChartResponse GenerateCharts(JMeterDataResultsRequest request);

    }

}
