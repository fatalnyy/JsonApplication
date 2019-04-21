using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JsonApp.Models
{
    public class Xml<T>
    {
        public static void SerializeObjectToXML(T objectToSerialize, string filePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                xs.Serialize(sw, objectToSerialize);
            }
        }
    }
}
