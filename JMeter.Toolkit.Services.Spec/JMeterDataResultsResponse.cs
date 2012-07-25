using System.Runtime.Serialization;

namespace JMeter.Toolkit.Services.Spec
{
    [DataContract]
    public class JMeterDataResultsResponse
    {
        [DataMember]
        public RequestDataResults[] DataResults { get; set; }
    }
}
