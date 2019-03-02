using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using GoodyDataLib.Models;
using System.Collections.Generic;
using AeroORMFramework;
using Newtonsoft.Json;
using System.Text;

namespace ReturnAchievementInfo
{
    public static class ReturnAchievementInfo
    {
        public const string connectionString = "Data Source=zhekusonsql.database.windows.net; " +
           "Initial Catalog = GoodyBase;  Integrated Security = True;       " +
           " User ID = zhekuson; Password=wasd#1580; Trusted_Connection = False;  Encrypt=True;";
        [FunctionName("ReturnAchievementInfo")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
         
            string column = req.GetQueryNameValuePairs()
             .FirstOrDefault(q => string.Compare(q.Key, "column", true) == 0)
             .Value;
            string value = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "value", true) == 0)
                .Value;
            List<AchievmentInfo> records = null;
            AeroORMFramework.Connector connector = new Connector(connectionString);
            if (value == null && column == null)
            {
                records = connector.GetAllRecords<AchievmentInfo>();
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(records), Encoding.UTF8, "application/json")
                };
            }

            if (value != null && column != null)
            {
                records = connector.GetRecords<AchievmentInfo>(column, value);
            }
            else
            {
                log.Info("Error occured");
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(records), Encoding.UTF8, "application/json")
            };



        }
    }
}
