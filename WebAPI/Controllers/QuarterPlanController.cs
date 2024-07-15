using AutoMapper;
using Contracts.Plan;
using Entities.DTO.QuarterPlan;
using Entities.DTO.QuarterPlanTwo;
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
    [Route("api/quarterplan")]
    [ApiController]
    public class QuarterPlanController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuarterPlanRepository _repository;
        private static int onlineIdUser = LoginUserId.loginId;

        public QuarterPlanController(IQuarterPlanRepository quarterPlan, IMapper mapper)
        {
            _repository = quarterPlan;
            _mapper = mapper;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("createquareterplan")]
        public IActionResult CreateQuarterPlan(QuarterPlanCreateDTO planCreateDTO)
        {
            if (planCreateDTO == null)
            {
                return NoContent();
            }

            var quarterPlanModel = _mapper.Map<QuarterPlan>(planCreateDTO);

            var a=_repository.CreateQuarterPlan(quarterPlanModel, onlineIdUser);

            if (a == "Created")
            {
                return Created("", a);
            }
            else
            {
                return new NoContentResult();
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getallquarterplan")]
        public IActionResult GetQuarterPlan(int year, int quarter, int queryNum)
        {
            var quarterPlans = _repository.GetAllYearQuarterPlan(year, quarter, queryNum);

            return Ok(_mapper.Map<IEnumerable<QuarterPlanReadDTO>>(quarterPlans));
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getbyidquarterplan/{id}")]
        public IActionResult GetQuarterPlanById(int id)
        {
            var quarterPlan = _repository.GetQuarterPlanByID(id);

            if (quarterPlan == null || quarterPlan.status == StatusEnum.deleted)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<QuarterPlanReadDTO>(quarterPlan));
        }




        [Authorize(Roles = "Admin")]
        [HttpDelete("deletequarterplan/{id}")]
        public IActionResult DeleteQuarterPlan(int id)
        {
            var quarterPlan = _repository.GetQuarterPlanByID(id);

            if (quarterPlan == null)
            {
                return NotFound();
            }

            _repository.DeleteQuarterPlan(id, onlineIdUser);

            return Ok();

        }



        [Authorize(Roles = "Admin")]
        [HttpPut("updatequarterplan/{id}")]
        public IActionResult UpdateQuarterPlan(int id, QuarterPlanUpdatedDTO planUpdatedDTO)
        {
            var quarterPlanCheck = _repository.GetQuarterPlanByID(id);

            if (quarterPlanCheck == null)
            {
                return NotFound();
            }
            QuarterPlan quarterPlan = _mapper.Map<QuarterPlan>(planUpdatedDTO);
            _repository.UpdateQuarterPlan(id, quarterPlan, onlineIdUser);
            QuarterPlan updateQuarterPlan = _repository.GetQuarterPlanByID(id);
            return Ok(_mapper.Map<QuarterPlanReadDTO>(updateQuarterPlan));
        }



        //  Two

        [Authorize(Roles = "Admin")]
        [HttpPost("createquareterplantwo")]
        public IActionResult CreateQuarterPlanTwo(int year, int quarter)
        {
            int queryNum = 0;
            IEnumerable<QuarterPlan> quarters = _repository.GetAllYearQuarterPlan(year, quarter, queryNum);
            IEnumerable<QuarterPlanTwo> planTwos = _repository.GetAllYearQuarterPlanTwo(year, quarter, queryNum);

            foreach (var item in quarters)
            {
                if (item.status != StatusEnum.addition)
                {
                    int k = 0;
                    foreach (var item1 in planTwos)
                    {
                        if (item1.quarter_id == item.quarter_id)
                        {
                            _repository.UpdateQuarterPlanAdd(item.quarter_id);
                            k++;
                        }
                    }

                    if (k == 0)
                    {
                        QuarterPlanTwoCreateDTO twoCreateDTO = new QuarterPlanTwoCreateDTO
                        {
                            quarter_id=item.quarter_id,
                            section_1=0,
                            section_2=0,
                            section_3=0,
                            section_4=0,
                            information_confirmed_date=item.information_confirmed_date
                        };
                        var quarterPlan = _mapper.Map<QuarterPlanTwo>(twoCreateDTO);
                        _repository.CreateQuarterPlanTwo(quarterPlan, onlineIdUser);
                        _repository.UpdateQuarterPlanAdd(twoCreateDTO.quarter_id);
                    }
                }
            }


            return Created("", "");
           
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getallquarterplantwo")]
        public IActionResult GetQuarterPlanTwo(int year, int quarter, int queryNum)
        {
            var quarterPlans = _repository.GetAllYearQuarterPlanTwo(year, quarter, queryNum);

            return Ok(_mapper.Map<IEnumerable<QuarterPlanTwoReadDTO>>(quarterPlans));
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getbyidquarterplantwo/{id}")]
        public IActionResult GetQuarterPlanTwoById(int id)
        {
            var quarterPlan = _repository.GetQuarterPlanTwoByID(id);

            if (quarterPlan == null || quarterPlan.status == StatusEnum.deleted)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<QuarterPlanTwoReadDTO>(quarterPlan));
        }




        [Authorize(Roles = "Admin")]
        [HttpDelete("deletequarterplantwo/{id}")]
        public IActionResult DeleteQuarterPlanTwo(int id)
        {
            var quarterPlan = _repository.GetQuarterPlanTwoByID(id);

            if (quarterPlan == null)
            {
                return NotFound();
            }

            _repository.DeleteQuarterPlanTwo(id, onlineIdUser);

            return Ok();

        }



        [Authorize(Roles = "Admin")]
        [HttpPut("updatequarterplantwo/{id}")]
        public IActionResult UpdateQuarterPlanTwo(int id, QuarterPlanTwoUpdatedDTO planUpdatedDTO)
        {
            var quarterPlanCheck = _repository.GetQuarterPlanTwoByID(id);

            if (quarterPlanCheck == null)
            {
                return NotFound();
            }
            QuarterPlanTwo quarterPlan = _mapper.Map<QuarterPlanTwo>(planUpdatedDTO);
            _repository.UpdateQuarterPlanTwo(id, quarterPlan, onlineIdUser);
            QuarterPlanTwo updateQuarterPlan = _repository.GetQuarterPlanTwoByID(id);
            return Ok(_mapper.Map<QuarterPlanTwoReadDTO>(updateQuarterPlan));
        }

    }
}
