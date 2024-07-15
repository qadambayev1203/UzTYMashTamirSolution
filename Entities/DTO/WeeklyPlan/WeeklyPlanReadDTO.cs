using Entities.Model.All;
using Entities.Model.User;
using Entities.Model.WeeklyPlanModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.WeeklyPlan
{
    public class WeeklyPlanReadDTO
    {
        public int weekly_id { get; set; }
        public WeeklyWork weekly_work { get; set; }
        public LocomativeInformation loco_inf { get; set; }
        public string fuel_type { get; set; }
        public string locomative_number { get; set; }
        public int number_of_locomotives { get; set; }
        public DateTime week_date { get; set; }
    }
}
