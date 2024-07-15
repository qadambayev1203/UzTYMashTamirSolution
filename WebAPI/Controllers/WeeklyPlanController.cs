using AutoMapper;
using Contracts.Plan;
using Entities.DTO.WeeklyPlan;
using Entities.Model.WeeklyPlanModel;
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
    public class WeeklyPlanController : ControllerBase
    {
        private readonly IWeeklyPlanRepository _repository;
        private readonly IMapper _mapper;
        private static int onlineIdUser = LoginUserId.loginId;

        public WeeklyPlanController(IWeeklyPlanRepository planRepository, IMapper mapper)
        {
            _repository = planRepository;
            _mapper = mapper;
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("createweeklyplan")]
        public IActionResult CreateWeeklyPlan(WeeklyPlanCreateDTO planCreateDTO)
        {
            if (planCreateDTO == null)
            {
                return NoContent();
            }
            var weeklyPlanModel = _mapper.Map<WeeklyPlan>(planCreateDTO);

            var a=_repository.CreateWeekluPlan(weeklyPlanModel, onlineIdUser);

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
        [HttpGet("getallweeklyplan")]
        public IActionResult GetWeeklyPlan(DateTime week_date, int queryNum)
        {
            var weeklyPlans = _repository.GetAllYearWeekluPlan(week_date,queryNum);

            return Ok(_mapper.Map<IEnumerable<WeeklyPlanReadDTO>>(weeklyPlans));
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getbyidweeklyplan/{id}")]
        public IActionResult GetWeeklyPlanById(int id)
        {
            var weeklyPlan = _repository.GetWeekluPlanByID(id);

            if (weeklyPlan == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WeeklyPlanReadDTO>(weeklyPlan));
        }




        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteweeklyplan/{id}")]
        public IActionResult DeleteWeeklyPlan(int id)
        {
            var weeklyPlan = _repository.GetWeekluPlanByID(id);

            if (weeklyPlan == null)
            {
                return NotFound();
            }

            _repository.DeleteWeekluPlan(id, onlineIdUser);

            return Ok();

        }



        [Authorize(Roles = "Admin")]
        [HttpPut("updateweeklyplan/{id}")]
        public IActionResult UpdateWeeklyPlan(int id, WeeklyPlanUpdatedDTO weeklyPlanUpdatedDTO)
        {
            var weeklyPlanCheck = _repository.GetWeekluPlanByID(id);

            if (weeklyPlanCheck == null)
            {
                return NotFound();
            }
            WeeklyPlan mapWeeklyPlan = _mapper.Map<WeeklyPlan>(weeklyPlanUpdatedDTO);
            _repository.UpdateWeekluPlan(id, mapWeeklyPlan, onlineIdUser);
            WeeklyPlan updateWeeklyPlan = _repository.GetWeekluPlanByID(id);
            return Ok(_mapper.Map<WeeklyPlanReadDTO>(updateWeeklyPlan));
        }


    }
}
