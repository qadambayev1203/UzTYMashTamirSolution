using Contracts.Plan;
using Entities.AllContext.QuarterPlanContex;
using Entities.Model.QuarterPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Plan
{
    public class QuarterPlanSqlRepo : IQuarterPlanRepository
    {
        private readonly QuaretrPlanContext _context;

        public QuarterPlanSqlRepo(QuaretrPlanContext quaretrPlanContext)
        {
            _context = quaretrPlanContext;
        }

        public IEnumerable<QuarterPlan> GetAllYearQuarterPlan(int year, int quarter, int queryNum)
        {
            var quarterPlans = _context.GetAllYearQuarterPlan(year, quarter, queryNum);
            return quarterPlans;
        }

        public QuarterPlan GetQuarterPlanByID(int id)
        {
            var quarterPlan = _context.GetQuarterPlanByID(id);
            return quarterPlan;
        }

        public string CreateQuarterPlan(QuarterPlan quarterPlan, int loginiduser)
        {
            return _context.CreateQuarterPlan(quarterPlan, loginiduser);
        }
        
        public void UpdateQuarterPlan(int id, QuarterPlan quarterPlan, int loginiduser)
        {
            _context.UpdateQuarterPlan(id, quarterPlan, loginiduser);
        }
        
        public void DeleteQuarterPlan(int id, int loginiduser)
        {
            _context.DeleteQuarterPlan(id, loginiduser);
        }

        public void UpdateQuarterPlanAdd(int id)
        {
            _context.UpdateQuarterPlanAdd(id);
        }
        public void UpdateQuarterPlanMonthAdd(int id)
        {
            _context.UpdateQuarterPlanMonthAdd(id);
        }


        //Two

        public IEnumerable<QuarterPlanTwo> GetAllYearQuarterPlanTwo(int year, int quarter, int queryNum)
        {
            var quarterPlansTwo = _context.GetAllYearQuarterPlanTwo(year,quarter,queryNum);
            return quarterPlansTwo;
        }
        
        public QuarterPlanTwo GetQuarterPlanTwoByID(int id)
        {
            var quarterPlanTwo = _context.GetQuarterPlanTwoByID(id);
            return quarterPlanTwo;
        }

        public string CreateQuarterPlanTwo(QuarterPlanTwo quarterPlan, int loginiduser)
        {
            return _context.CreateQuarterPlanTwo(quarterPlan, loginiduser);
        }
        
        public void DeleteQuarterPlanTwo(int id, int loginiduser)
        {
            _context.DeleteQuarterPlanTwo(id, loginiduser);
        }
        
        public void UpdateQuarterPlanTwo(int id, QuarterPlanTwo quarterPlan, int loginiduser)
        {
            _context.UpdateQuarterPlanTwo(id, quarterPlan, loginiduser);
        }

        
    }
}
