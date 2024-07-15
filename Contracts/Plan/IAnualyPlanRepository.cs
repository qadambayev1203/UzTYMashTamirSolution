using Entities.Model.AnualyPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Plan
{
    public interface IAnualyPlanRepository
    {
        //Anualy Plan
        public IEnumerable<AnualyPlan> GetAllYearAnualyPlan(int year, int queryNum);

        public AnualyPlan GetAnualyPlanByID(int id);

        public string CreateAnualyPlan(AnualyPlan anualyPlan, int loginiduser);

        public void UpdateAnualyPlan(int id, AnualyPlan anualyPlan, int loginiduser);

        public void DeleteAnualyPlan(int id, int loginiduser);
        public void UpdateAnualyPlanOneAdd(int id);


        //Anualy Plan One

        public IEnumerable<AnualyPlan> GetAllYearAnualyOnePlan(int year, int queryNum);

        public AnualyPlan GetAnualyPlanOneByID(int id);

        public string CreateAnualyOnePlan(AnualyPlan anualyPlan, int loginiduser);

        public void UpdateAnualyOnePlan(int id, AnualyPlan anualyPlan, int loginiduser);

        public void DeleteAnualyOnePlan(int id, int loginiduser);



        //Anualy PlanTwo


        public IEnumerable<AnualyPlan> GetAllYearAnualyTwoPlan(int year, int queryNum);

    }
}
