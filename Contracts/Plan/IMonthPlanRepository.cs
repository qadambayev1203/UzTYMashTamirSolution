using Entities.Model.MonthPlanModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Plan
{
    public interface IMonthPlanRepository
    {
        // Month
        public IEnumerable<MonthPlanOne> GetAllYearMonthPlan(int year,int quarter,int month, int queryNum);


        //Month one
        public IEnumerable<MonthPlanOne> GetAllYearMonthPlanOne(int year, int quarter, int month, int queryNum);

        public MonthPlanOne GetMonthPlanOneByID(int id);

        public string CreateMonthPlanOne(MonthPlanOne monthPlan, int loginiduser);

        public void UpdateMonthPlanOne(int id, MonthPlanOne monthPlan, int loginiduser);

        public void DeleteMonthPlanOne(int id, int loginiduser);
    }
}
