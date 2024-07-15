using Entities.Model.All;
using Entities.Model.QuarterPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.MonthPlanOne
{
    public class MonthPlanOneReadDTO
    {
        public int month_id { get; set; }
        public int quarter_id { get; set; }
        public QuarterPlanReaded quarterPlanTwo { get; set; }
        public Designations designations { get; set; }
        public int section_true { get; set; }
    }
}
