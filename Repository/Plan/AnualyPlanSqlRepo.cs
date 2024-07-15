using Contracts.Plan;
using Entities.AllContext.AnualyPlanContext;
using Entities.Model.AnualyPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Plan
{
    public class AnualyPlanSqlRepo : IAnualyPlanRepository
    {
        private readonly AnualyPlanContext _context;
        public AnualyPlanSqlRepo(AnualyPlanContext context)
        {
            _context = context;
        }



        //Anualy Plan
        public string CreateAnualyPlan(AnualyPlan anualyPlan, int loginiduser)
        {
            return _context.CreateAnualyPlan(anualyPlan, loginiduser);
        }

        public void DeleteAnualyPlan(int id, int loginiduser)
        {
            _context.DeleteAnualyPlan(id, loginiduser);
        }

        public IEnumerable<AnualyPlan> GetAllYearAnualyPlan(int year, int queryNum)
        {
            var anualyPlans = _context.GetAllYearAnualyPlan(year,queryNum);
            return anualyPlans;
        }

        public AnualyPlan GetAnualyPlanByID(int id)
        {
            var anualyPlan = _context.GetAnualyPlanByID(id);
            return anualyPlan;
        }

        public void UpdateAnualyPlan(int id, AnualyPlan anualyPlan, int loginiduser)
        {
            _context.UpdateAnualyPlan(id, anualyPlan, loginiduser);
        }

        public void UpdateAnualyPlanOneAdd(int id)
        {
            _context.UpdateAnualyPlanOneAdd(id);
        }



        //AnualyPlan Plan One


        public string CreateAnualyOnePlan(AnualyPlan anualyPlan, int loginiduser)
        {
           return _context.CreateAnualyOnePlan(anualyPlan, loginiduser);
        }

        public void DeleteAnualyOnePlan(int id, int loginiduser)
        {
            _context.DeleteAnualyOnePlan(id, loginiduser);
        }

        public IEnumerable<AnualyPlan> GetAllYearAnualyOnePlan(int year, int queryNum)
        {
            var anualyPlan = _context.GetAllYearAnualyOnePlan(year,queryNum);
            return anualyPlan;
        }

        public AnualyPlan GetAnualyPlanOneByID(int id)
        {
            var anulayPlan = _context.GetAnualyPlanOneByID(id);
            return anulayPlan;
        }

        public void UpdateAnualyOnePlan(int id, AnualyPlan anualyPlan, int loginiduser)
        {
            _context.UpdateAnualyOnePlan(id, anualyPlan, loginiduser);
        }




        //Anualy Plan Two
        public IEnumerable<AnualyPlan> GetAllYearAnualyTwoPlan(int year, int queryNum)
        {
            var anualyPlanTwo = _context.GetAllYearAnualyTwoPlan(year,queryNum);
            return anualyPlanTwo;
        }

        
    }
}
