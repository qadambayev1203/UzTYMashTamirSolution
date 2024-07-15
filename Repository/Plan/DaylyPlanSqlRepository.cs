using Contracts.Plan;
using Entities.AllContext.DaylyPlanContex;
using Entities.Model.DaylyPlanModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Plan
{
    public class DaylyPlanSqlRepository : IDaylyPlanRepository
    {
        private readonly DaylyPlanContext _context;

        public DaylyPlanSqlRepository(DaylyPlanContext daylyPlanContext)
        {
            _context = daylyPlanContext;
        }


        public string CreateDaylyPlan(DaylyPlan daylyPlan, int loginiduser)
        {
            return _context.CreateDaylyPlan(daylyPlan, loginiduser);
        }

        public void DeleteDaylyPlan(int id, int loginiduser)
        {
            _context.DeleteDaylyPlan(id, loginiduser);
        }

        public IEnumerable<DaylyPlan> GetAllYearDaylyPlan(DateTime day_date, int queryNum)
        {
            var daylyPlans = _context.GetAllYearDaylyPlan(day_date,queryNum);
            return daylyPlans;
        }

        public DaylyPlan GetDaylyPlanByID(int id)
        {
            var daylyPlan = _context.GetDaylyPlanByID(id);
            return daylyPlan;
        }

        public void UpdateDaylyPlan(int id, DaylyPlan daylyPlan, int loginiduser)
        {
            _context.UpdateDaylyPlan(id,daylyPlan,loginiduser);
        }
    }
}
