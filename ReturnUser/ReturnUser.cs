using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using AeroORMFramework;
using GoodyDataLib.Models;
using System.Collections.Generic;
using TransformLibrary;
using Newtonsoft.Json;
using System.Text;

namespace ReturnUser
{
    
    public static class ReturnUser
    {
        public const string connectionString = "Data Source=zhekusonsql.database.windows.net; " +
            "Initial Catalog = GoodyBase;  Integrated Security = True;       " +
            " User ID = zhekuson; Password=wasd#1580; Trusted_Connection = False;  Encrypt=True;";
        [FunctionName("ReturnUser")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Userinfo triggered");
            string column = null;
            string value= null;
            // parse query parameter
            column = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "column", true) == 0)
                .Value;
            value= req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "value", true) == 0)
                .Value;
        
            List<UserInfo> records=null;
            AeroORMFramework.Connector connector = new Connector(connectionString);
            if (value==null && column == null)
            {
                records = connector.GetAllRecords<UserInfo>();
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(records), Encoding.UTF8, "application/json")
                };
            }

            if (value != null && column!=null)
            {
                records = connector.GetRecords<UserInfo>(column, value);
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
