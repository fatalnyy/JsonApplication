using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonApp.Models
{
    public class Json<T>
    {
        public static IEnumerable<T> ConvertJsonToCollectionOfObjects(string jsonString)
        {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
        }
        public static string FormatJsonString(IEnumerable<T> collection)
        {
            return JsonConvert.SerializeObject(collection, Formatting.Indented);
        }
    }
}
