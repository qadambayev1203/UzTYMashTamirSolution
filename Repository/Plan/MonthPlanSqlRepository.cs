using Contracts.Plan;
using Entities.AllContext.MonthPlanContext;
using Entities.Model.MonthPlanModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Plan
{
    public class MonthPlanSqlRepository : IMonthPlanRepository
    {
        private readonly MonthPlanContex _context;
        public MonthPlanSqlRepository(MonthPlanContex monthPlan)
        {
            _context = monthPlan;
        }
        //Month
        public IEnumerable<MonthPlanOne> GetAllYearMonthPlan(int year, int quarter, int month, int queryNum)
        {

            var monthPlans = _context.GetAllYearMonthPlan(year, quarter, month, queryNum);
            return monthPlans;
        }





        //Month One
        public string CreateMonthPlanOne(MonthPlanOne monthPlan, int loginiduser)
        {
            return _context.CreateMonthPlanOne(monthPlan, loginiduser);
        }

        public void DeleteMonthPlanOne(int id, int loginiduser)
        {
            _context.DeleteMonthPlanOne(id, loginiduser);
        }

        public IEnumerable<MonthPlanOne> GetAllYearMonthPlanOne(int year, int quarter, int month, int queryNum)
        {
            var monthPlansOne = _context.GetAllYearMonthPlanOne(year, quarter, month, queryNum);
            return monthPlansOne;
        }

        public MonthPlanOne GetMonthPlanOneByID(int id)
        {
            var monthPlanOne = _context.GetMonthPlanOneByID(id);
            return monthPlanOne;
        }

        public void UpdateMonthPlanOne(int id, MonthPlanOne monthPlan, int loginiduser)
        {
            _context.UpdateMonthPlanOne(id, monthPlan, loginiduser);
        }
    }
}
