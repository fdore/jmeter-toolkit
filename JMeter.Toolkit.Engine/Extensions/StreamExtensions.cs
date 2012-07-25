using System.IO;
using System.Text;

namespace JMeter.Toolkit.Engine.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Convert stream to string
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadContent(this Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
