using Entities.Model.All;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.AnualyPlanTwo
{
    public class AnualyPlanTwoReadDTO
    {
        public int a_o_id { get; set; }
        public LocomativeInformation locomative_name { get; set; }
        public int sections_reprair_number { get; set; }
        public string reprairtype { get; set; }

    }
}
