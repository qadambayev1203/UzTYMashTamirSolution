using Entities.Model.All;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.AnualyPlanOne
{
    public class AnualyPlanOneCreateDTO
    {
        public int anualy_id { get; set; }
        public DateTime information_confirmed_date { get; set; }
        public Model.All.MonthPlan month_plan { get; set; }
    }
}
