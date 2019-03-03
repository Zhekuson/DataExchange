using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnUser
{
    public class EventInfo
    {
        public int ID { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public int CreatorID { get; set; }
        public string Resources { get; set; }

        public List<int> Participants { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? EventDate { get; set; }

        public string Adress { get; set; }

        public string District { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
    }

   

}
