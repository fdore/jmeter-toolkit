using System.Runtime.Serialization;

namespace JMeter.Toolkit.Services.Spec
{
    [DataContract]
    public class JMeterDataResultsRequest
    {
        [DataMember]
        public RequestDataResults[] DataResults { get; set; }
    }
}
