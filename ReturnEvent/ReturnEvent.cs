using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GoodyDataLib.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ReturnEvent
{
    public static class ReturnEvent
    {
        public static string eventRequest =
            "https://vk.com/away.php?to=https%3A%2F%2Fgoody.im%2Fapi%2Fv1%2Fevents%2Flocation%3Flat%3D55.784683%26lon%3D37.55752%26radius%3D15%26makeaton_user_id%3D34&cc_key=";
       
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");


            WebRequest webRequest =WebRequest.Create(eventRequest);
            WebResponse response = (WebResponse)webRequest.GetResponse();
            // parse query parameter
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string res = sr.ReadToEnd();
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;
            JSONEventParser parser = new JSONEventParser();
            parser = JsonConvert.DeserializeObject<JSONEventParser>(res);
            EventInfo ev = new EventInfo()
            {
                Adress = parser.address,
                ID = parser.ID,
                Longitude = parser.Longitude,
                Latitude = parser.Latitude,
                CreatorID = parser.creator_id,
                CreatedAt = parser.created_at,
                UpdatedAt = parser.updated_at,
                EventDate = parser.EventDate,
                Description = parser.description,
                Resources = parser.resources,
                State = parser.state


            };

            return
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(ev), Encoding.UTF8, "application/json")
            };
        }
    }
}
