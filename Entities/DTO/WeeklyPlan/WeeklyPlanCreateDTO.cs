using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.WeeklyPlan
{
    public class WeeklyPlanCreateDTO
    {
        public int w_w_id { get; set; }
        public int loco_id { get; set; }
        public string locomative_number { get; set; }
        public int number_of_locomotives { get; set; }
        public DateTime week_date { get; set; }
    }
}
