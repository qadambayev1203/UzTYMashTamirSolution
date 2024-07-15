using Entities.Model.QuarterPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.QuarterPlanTwo
{
    public class QuarterPlanTwoReadDTO
    {
        public int q_pt_id { get; set; }
        public QuarterPlanReaded quarter_plan { get; set; }
        public int section_1 { get; set; }
        public int section_2 { get; set; }
        public int section_3 { get; set; }
        public int section_4 { get; set; }
        public DateTime information_confirmed_date { get; set; }
    }
}
