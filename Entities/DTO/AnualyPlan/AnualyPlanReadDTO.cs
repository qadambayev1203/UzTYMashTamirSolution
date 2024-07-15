using Entities.Model.All;
using Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.AnualyPlan
{
    public class AnualyPlanReadDTO
    {
        public int anualy_id { get; set; }
        public LocomativeInformation locomative_name { get; set; }
        public double all_price { get; set; }
        public int sections_reprair_number { get; set; }
        public string reprairtype { get; set; }
        public DateTime information_confirmed_date { get; set; }
        public DateTime information_entered_date { get; set; }
        public DateTime information_modified_date { get; set; }
    }
}
