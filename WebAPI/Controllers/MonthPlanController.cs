using AutoMapper;
using Contracts.Plan;
using Entities.DTO.MonthPlan;
using Entities.DTO.MonthPlanOne;
using Entities.Model.MonthPlanModel;
using Entities.Model.QuarterPlan;
using Entities.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthPlanController : ControllerBase
    {
        private readonly IMonthPlanRepository _repository;
        private readonly IMapper _mapper;
        private readonly IQuarterPlanRepository _repositoryQuarter;
        private static int onlineIdUser = LoginUserId.loginId;
        public MonthPlanController(IMonthPlanRepository repository, IMapper mapper, IQuarterPlanRepository quarter)
        {
            _repository = repository;
            _mapper = mapper;
            _repositoryQuarter = quarter;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getallmonthplan")]
        public IActionResult GetMonthPlan(int year, int quarter,int month, int queryNum)
        {
            var quarterPlans = _repository.GetAllYearMonthPlan(year, quarter,month, queryNum);

            return Ok(_mapper.Map<IEnumerable<MonthPlanDTO>>(quarterPlans));
        }




        //Month

        [Authorize(Roles = "Admin")]
        [HttpPost("createmonthplanone")]
        public IActionResult CreateMonthPlanOne(int year, int quarter,int month)
        {
            int queryNum = 0;
            IEnumerable<QuarterPlan> result = _repositoryQuarter.GetAllYearQuarterPlan(year, quarter, queryNum);
            IEnumerable<MonthPlanOne> planOnes = _repository.GetAllYearMonthPlanOne(year, quarter, month, queryNum);

            foreach (var item in result)
            {
                if (item.month_status != StatusEnum.addition && item.month_of_reprair==month)
                {
                    int k = 0;
                    foreach (var item1 in planOnes)
                    {
                        if (item1.quarter_id == item.quarter_id)
                        {
                            _repositoryQuarter.UpdateQuarterPlanMonthAdd(item.quarter_id);
                            k++;
                        }
                    }

                    if (k == 0)
                    {
                        MonthPlanOneCreateDTO twoCreateDTO = new MonthPlanOneCreateDTO
                        {
                            quarter_id = item.quarter_id,
                            
                        };
                        var quarterPlan = _mapper.Map<MonthPlanOne>(twoCreateDTO);
                        _repository.CreateMonthPlanOne(quarterPlan, onlineIdUser);
                        _repositoryQuarter.UpdateQuarterPlanMonthAdd(twoCreateDTO.quarter_id);
                    }
                }
            }


           
                return Created("", "");
            
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getallmonthplanone")]
        public IActionResult GetMonthPlanOne(int year, int quarter,int month, int queryNum)
        {
            var monthPlans = _repository.GetAllYearMonthPlanOne(year, quarter,month, queryNum);

            return Ok(_mapper.Map<IEnumerable<MonthPlanOneReadDTO>>(monthPlans));
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getbyidmonthplanone/{id}")]
        public IActionResult GetMonthPlanOneById(int id)
        {
            var monthPlan = _repository.GetMonthPlanOneByID(id);

            if (monthPlan == null || monthPlan.status == StatusEnum.deleted)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MonthPlanOneReadDTO>(monthPlan));
        }




        [Authorize(Roles = "Admin")]
        [HttpDelete("deletemonthplanone/{id}")]
        public IActionResult DeleteMonthPlanOne(int id)
        {
            var monthPlan = _repository.GetMonthPlanOneByID(id);

            if (monthPlan == null)
            {
                return NotFound();
            }

            _repository.DeleteMonthPlanOne(id, onlineIdUser);

            return Ok();

        }



        [Authorize(Roles = "Admin")]
        [HttpPut("updatemonthplanone/{id}")]
        public IActionResult UpdateMonthPlanOne(int id, MonthPlanOneUpdatedDTO planUpdatedDTO)
        {
            var monthPlancheck = _repository.GetMonthPlanOneByID(id);

            if (monthPlancheck == null)
            {
                return NotFound();
            }
            MonthPlanOne quarterPlan = _mapper.Map<MonthPlanOne>(planUpdatedDTO);
            _repository.UpdateMonthPlanOne(id, quarterPlan, onlineIdUser);
            MonthPlanOne updateMonthPlan = _repository.GetMonthPlanOneByID(id);
            return Ok(_mapper.Map<MonthPlanOneReadDTO>(updateMonthPlan));
        }
    }
}
