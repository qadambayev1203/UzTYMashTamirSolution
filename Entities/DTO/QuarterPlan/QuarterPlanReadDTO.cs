using Entities.Model.All;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.QuarterPlan
{
    public class QuarterPlanReadDTO
    {
        public int quarter_id { get; set; }
        public LocomativeInformation locomative_name { get; set; }
        public string locomative_number { get; set; }
        public OrganizationType organization { get; set; }
        public int section_num { get; set; }
        public string reprair_type { get; set; }
        public string monthofreprair { get; set; }
        public string locomativeFuelType { get; set; }
        public DateTime information_confirmed_date { get; set; }
        public int quarter { get; set; }
        public DateTime plan_year { get; set; }
        public int section_0 { get; set; }
    }
}
