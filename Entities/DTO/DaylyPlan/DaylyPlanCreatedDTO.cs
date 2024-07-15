using Entities.Model.All;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.DaylyPlan
{
    public class DaylyPlanCreatedDTO
    {
        public int loco_id { get; set; }
        public string locomative_number { get; set; }
        public ReprairType reprair_id { get; set; }
        public int org_id { get; set; }
        public string do_work { get; set; }
        public string comment { get; set; }
        public DateTime day_date { get; set; }
        
    }
}
