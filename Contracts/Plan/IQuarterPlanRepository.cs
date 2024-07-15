using Entities.Model.QuarterPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Plan
{
    public interface IQuarterPlanRepository
    {
        public IEnumerable<QuarterPlan> GetAllYearQuarterPlan(int year,int quarter, int queryNum);

        public QuarterPlan GetQuarterPlanByID(int id);

        public string CreateQuarterPlan(QuarterPlan quarterPlan, int loginiduser);

        public void UpdateQuarterPlan(int id, QuarterPlan quarterPlan, int loginiduser);

        public void DeleteQuarterPlan(int id, int loginiduser);
        public void UpdateQuarterPlanAdd(int id);
        public void UpdateQuarterPlanMonthAdd(int id);




        // Two


        public IEnumerable<QuarterPlanTwo> GetAllYearQuarterPlanTwo(int year, int quarter, int queryNum);

        public QuarterPlanTwo GetQuarterPlanTwoByID(int id);

        public string CreateQuarterPlanTwo(QuarterPlanTwo quarterPlan, int loginiduser);

        public void UpdateQuarterPlanTwo(int id, QuarterPlanTwo quarterPlan, int loginiduser);

        public void DeleteQuarterPlanTwo(int id, int loginiduser);

    }
}
