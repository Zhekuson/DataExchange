using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReturnUser
{
    class JsonDistrictParser
    {
        public int id;
        public int ID;
        public int user_id;//To creator_id 

        public int creator_id;

        public decimal? lat;
        public decimal? lon;

        public decimal? Latitude;
        public decimal? Longitude;

        public DateTime created_at;
        public DateTime updated_at;
        public DateTime? date;//EvantDate 

        public DateTime? EventDate;

        public string address;
        public string aasm_state;//state 
        public string resources;
        public string description;

        public List<JSONEventParser> all;

        public string name;
        public string district;
        public string state;

        public void GetValues(string value)
        {
            JsonConvert.DeserializeObject(value);
            creator_id = user_id;
            EventDate = date;
            state = aasm_state;
            Latitude = lat;
            Longitude = lon;
            ID = id;
            district = name;

        }
    }
}
