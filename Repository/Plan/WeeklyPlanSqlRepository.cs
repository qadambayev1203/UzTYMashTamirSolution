using Contracts.Plan;
using Entities.AllContext.WeeklyPlanContex;
using Entities.Model.WeeklyPlanModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Plan
{
    public class WeeklyPlanSqlRepository : IWeeklyPlanRepository
    {
        private readonly WeeklyPlanContext _context;

        public WeeklyPlanSqlRepository(WeeklyPlanContext planContext)
        {
            _context = planContext;
        }


        public string CreateWeekluPlan(WeeklyPlan weeklyPlan, int loginiduser)
        {
            return _context.CreateWeekluPlan(weeklyPlan, loginiduser);
        }

        public void DeleteWeekluPlan(int id, int loginiduser)
        {
            _context.DeleteWeekluPlan(id,loginiduser);
        }

        public IEnumerable<WeeklyPlan> GetAllYearWeekluPlan(DateTime week_date, int queryNum)
        {
            var weeklyPlans = _context.GetAllYearWeekluPlan(week_date, queryNum);
            return weeklyPlans;
        }

        public WeeklyPlan GetWeekluPlanByID(int id)
        {
            var weeklyPlan = _context.GetWeekluPlanByID(id);
            return weeklyPlan;
        }

        public void UpdateWeekluPlan(int id, WeeklyPlan weeklyPlan, int loginiduser)
        {
            _context.UpdateWeekluPlan(id, weeklyPlan, loginiduser);
        }
    }
}
