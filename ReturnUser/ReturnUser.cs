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
  /// <summary>
  /// ����� ������� ��� ��������� � ���
  /// </summary>
    public static class ReturnUser
    {
        public const decimal disp = 0.0005m;
        public static string FunctionURL =
           "https://goody.im/api/v1/events/";
        [FunctionName("ReturnUser")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            try
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
                decimal latt = 0;
                decimal longi = 0;



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
                    request = WebRequest.Create(FunctionURL + "region" +
                    $"?radius={Radius.ToString()}" +
                    $"&makeaton_user_id={makeaton_user_id}"
                    + $"&name={name}");
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


                        eventsList.Add(new EventInfo
                        {
                            Address = parser.address,
                            ID = parser.id,
                            Longitude = longi,
                            Latitude = latt,
                            CreatorID = parser.user_id,
                            CreatedAt = parser.created_at,
                            UpdatedAt = parser.updated_at,
                            EventDate = parser.EventDate,
                            Description = parser.description,
                            Resources = parser.resources,
                            State = parser.state,
                            District = "",
                            AuthorAge = parser.author_age
                        });
                    }
                }
                else
                {


                    JsonDistrictParser parser = new JsonDistrictParser();
                    parser = JsonConvert.DeserializeObject<JsonDistrictParser>(result);


                    foreach (JSONEventParser pars in parser.all)
                    {


                        eventsList.Add(new EventInfo
                        {
                            Address = pars.address,
                            ID = pars.id,
                            Longitude = longi,
                            Latitude = latt,
                            CreatorID = pars.user_id,
                            CreatedAt = pars.created_at,
                            UpdatedAt = pars.updated_at,
                            EventDate = pars.EventDate,
                            Description = pars.description,
                            Resources = pars.resources,
                            State = pars.state,
                            District = name,
                            AuthorAge = pars.author_age

                        });
                    }
                }


                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(eventsList), Encoding.UTF8, "application/json")
                };
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
        }
    }
}
