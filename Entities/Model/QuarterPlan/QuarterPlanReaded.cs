using Entities.Model.All;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.QuarterPlan
{
    public class QuarterPlanReaded
    {
        public LocomativeInformation locomative_name { get; set; }
        public string locomative_number { get; set; }
        public string reprair_type { get; set; }
        public string monthofreprair { get; set; }
        public int section_num { get; set; }
        public string locomativeFuelType { get; set; }
        public int quarter { get; set; }
        public DateTime plan_year { get; set; }

    }
}
