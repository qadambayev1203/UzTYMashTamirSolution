using Entities.Model.All;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.DaylyPlan
{
    public class DaylyPlanReadedDTO
    {
        public int dayly_id { get; set; }
        public LocomativeInformation loco_inf { get; set; }
        public string fuel_type { get; set; }
        public string locomative_number { get; set; }
        public string reprair_type { get; set; }
        public OrganizationType organization { get; set; }
        public string do_work { get; set; }
        public string comment { get; set; }
        public DateTime day_date { get; set; }

    }
}
