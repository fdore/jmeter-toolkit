using System.Runtime.Serialization;

namespace JMeter.Toolkit.Services.Spec
{
    [DataContract]
    public class JMeterLogsRequest
    {
        [DataMember]
        public string Logs { get; set; }
    }
}
