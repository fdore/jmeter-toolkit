using System;

namespace JMeter.Toolkit.Engine.Extensions
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Converts milliseconds since midnight Jan 1, 1970 UTC into date
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this double val)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return epoch.AddMilliseconds(val);
        }
    }
}
