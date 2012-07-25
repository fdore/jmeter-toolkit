using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace JMeter.Toolkit.Engine
{
    public class XmlSerializer<T>
    {
        public T Deserialize(Stream stream)
        {
            using(var reader = new StreamReader(stream))
            {
                string xml = reader.ReadToEnd();
                var stringReader = new StringReader(xml);
                var xmlSerializer = new XmlSerializer(typeof(T));
                return ((T)(xmlSerializer.Deserialize(XmlReader.Create(stringReader))));
            }
        }

        public T Deserialize(XDocument xDocument)
        {
            var memStream = new MemoryStream();
            xDocument.Save(memStream);
            return Deserialize(memStream);
        }
    }
}
