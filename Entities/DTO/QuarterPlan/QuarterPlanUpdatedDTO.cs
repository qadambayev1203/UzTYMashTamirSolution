using Entities.Model.All;
using Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.QuarterPlan
{
    public class QuarterPlanUpdatedDTO
    {
        public LocomativeInformation locomative_name { get; set; }
        public string locomative_number { get; set; }
        public OrganizationType organization { get; set; }
        public ReprairType reprair_id { get; set; }
        public int month_of_reprair { get; set; }
        public int section_num { get; set; }
        public DateTime information_confirmed_date { get; set; }
        public int quarter { get; set; }
        public DateTime plan_year { get; set; }
        public int section_0 { get; set; }
    }
}
