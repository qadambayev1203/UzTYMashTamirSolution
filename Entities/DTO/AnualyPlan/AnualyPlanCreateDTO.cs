using Entities.Model.All;
using Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.AnualyPlan
{
    public class AnualyPlanCreateDTO
    {
        public LocomativeInformation locomative_name { get; set; }
        public double all_price { get; set; }
        public int sections_reprair_number { get; set; }
        public ReprairType reprair_type { get; set; }
        public DateTime information_confirmed_date { get; set; }
    }
}
