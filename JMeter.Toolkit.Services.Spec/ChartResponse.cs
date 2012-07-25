using System.Runtime.Serialization;

namespace JMeter.Toolkit.Services.Spec
{
    [DataContract]
    public class ChartResponse
    {
        [DataMember]
        public Chart[] Charts { get; set; }

        [DataMember]
        public RequestDataResults[] DataResults { get; set; }
    }

    [DataContract]
    public class Chart
    {
        [DataMember]
        public byte[] Data { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Request { get; set; }
    }

    
}
