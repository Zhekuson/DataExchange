using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GoodyDataLib.Models;
namespace TransformLibrary
{
    public static class JsonTransform
    {
        public static string DataTypeToJson<GoodyDataLibType>(GoodyDataLibType ev)
        {
            return JsonConvert.SerializeObject(ev);
        }
        public static GoodyDataLibType JsonToDataType<GoodyDataLibType>(string json)
        {
            return JsonConvert.DeserializeObject<GoodyDataLibType>(json);                
        }
    
    }
}
