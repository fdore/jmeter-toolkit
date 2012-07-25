using System;
using System.Runtime.Serialization;

namespace JMeter.Toolkit.Services.Spec
{
    [DataContract]
    public class RequestDataResults
    {
        /// <summary>
        /// Host name
        /// </summary>
        [DataMember]
        public string HostName { get; set; }

        /// <summary>
        /// Date of the requests
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Request string
        /// </summary>
        [DataMember]
        public string Request { get; set; }

        /// <summary>
        /// Average response time
        /// </summary>
        [DataMember]
        public double AverageResponseTime { get; set; }

        /// <summary>
        /// Minimum response time
        /// </summary>
        [DataMember]
        public double MinResponseTime { get; set; }

        /// <summary>
        /// Maximum response excluding the 10% slowest requests
        /// </summary>
        [DataMember]
        public double MaxResponseTimeExcludingBottomDecile { get; set; }

        /// <summary>
        /// Minimum response time excluding the 10% fastest requests
        /// </summary>
        [DataMember]
        public double MinResponseTimeExcludingTopDecile { get; set; }

        /// <summary>
        /// Average response time excluding the 10% fastest and 10% slowest requests
        /// </summary>
        [DataMember]
        public double AverageResponseTimeExcludingTopAndBottomDeciles { get; set; }

        /// <summary>
        /// Maxiumum response time
        /// </summary>
        [DataMember]
        public double MaxResponseTime { get; set; }

        /// <summary>
        /// Number of users simulated
        /// </summary>
        [DataMember]
        public int UserCount { get; set; }

        /// <summary>
        /// Distribution of the response time
        /// Takes the 10% slowest requests, and calculate the average, and so on, up to the 10% fastest
        /// Returns an array of 10 values 
        /// </summary>
        [DataMember]
        public double[] ResponseTimeDistribution { get; set; }
    }
}
