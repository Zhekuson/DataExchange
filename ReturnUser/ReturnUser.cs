using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReturnUser
{
  
    public static class ReturnUser
    {
        public const decimal disp = 0.0005m;
        public static string FunctionURL =
           "https://goody.im/api/v1/events/";
        [FunctionName("ReturnUser")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            string Latitude = req.GetQueryNameValuePairs()
            .FirstOrDefault(q => string.Compare(q.Key, "lat", true) == 0)
            .Value;
            string Longitude = req.GetQueryNameValuePairs()
            .FirstOrDefault(q => string.Compare(q.Key, "lon", true) == 0)
            .Value;
            string Radius = req.GetQueryNameValuePairs()
            .FirstOrDefault(q => string.Compare(q.Key, "radius", true) == 0)
            .Value;
            string makeaton_user_id = req.GetQueryNameValuePairs()
            .FirstOrDefault(q => string.Compare(q.Key, "makeathon_user_id", true) == 0)
            .Value;
            string name = req.GetQueryNameValuePairs()
            .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
            .Value;

            Random random = new Random();
            decimal latt=0;
            decimal longi=0;


            WebRequest request;
            if (name == null)
            {
                decimal.TryParse(Latitude, out latt);
                decimal.TryParse(Longitude, out longi);
                string latString = Latitude.ToString().Replace(',', '.');
                string lonString = Longitude.ToString().Replace(',', '.');
                request = WebRequest.Create(FunctionURL + $"?lat={latString}&" +
                $"lon={lonString}&radius={Radius.ToString()}" +
                $"&makeaton_user_id={makeaton_user_id}");
            }
            else
            {
                request = WebRequest.Create(FunctionURL +"region" +
                $"?radius={Radius.ToString()}" +
                $"&makeaton_user_id={makeaton_user_id}"
                +$"&name={name}");
            }
            WebResponse resp = request.GetResponse();

            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string result = sr.ReadToEnd();
            List<EventInfo> eventsList = new List<EventInfo>();
            if (name == null)
            {
                List<JSONEventParser> parserList = new List<JSONEventParser>();
                parserList = JsonConvert.DeserializeObject<List<JSONEventParser>>(result);

          
                foreach (JSONEventParser parser in parserList)
                {

                    latt += ((decimal)Math.Pow(-1, random.Next(2))) * (decimal)random.NextDouble() * disp;
                    longi += ((decimal)Math.Pow(-1, random.Next(2))) * (decimal)random.NextDouble() * disp;
                    eventsList.Add(new EventInfo
                    {
                        Adress = parser.address,
                        ID = parser.ID,
                        Longitude = longi,
                        Latitude = latt,
                        CreatorID = parser.creator_id,
                        CreatedAt = parser.created_at,
                        UpdatedAt = parser.updated_at,
                        EventDate = parser.EventDate,
                        Description = parser.description,
                        Resources = parser.resources,
                        State = parser.state,
                        District = parser.district
                    });
                }
            }
            else
            {
                //JSONEventParser parser = new JSONEventParser();
                
                JsonDistrictParser parser = new JsonDistrictParser();
                parser = JsonConvert.DeserializeObject<JsonDistrictParser>(result);
                

                foreach (JSONEventParser pars in parser.all)
                {

                    latt += ((decimal)Math.Pow(-1, random.Next(2))) * (decimal)random.NextDouble() * disp;
                    longi += ((decimal)Math.Pow(-1, random.Next(2))) * (decimal)random.NextDouble() * disp;
                    eventsList.Add(new EventInfo
                    {
                        Adress = parser.address,
                        ID = parser.ID,
                        Longitude = longi,
                        Latitude = latt,
                        CreatorID = parser.creator_id,
                        CreatedAt = parser.created_at,
                        UpdatedAt = parser.updated_at,
                        EventDate = parser.EventDate,
                        Description = parser.description,
                        Resources = parser.resources,
                        State = parser.state,
                        District = parser.district
                    });
                }
            }
           

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(eventsList), Encoding.UTF8, "application/json")
            };
        }
    }
}
