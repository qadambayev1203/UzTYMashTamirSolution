using AutoMapper;
using Contracts.Plan;
using Entities.DTO.DaylyPlan;
using Entities.Model.DaylyPlanModel;
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
    public class DaylyPlanController : ControllerBase
    {
        private readonly IDaylyPlanRepository _repository;
        private readonly IMapper _mapper;
        private static int onlineIdUser = LoginUserId.loginId;

        public DaylyPlanController(IDaylyPlanRepository planRepository, IMapper mapper)
        {
            _repository = planRepository;
            _mapper = mapper;
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("createdaylyplan")]
        public IActionResult CreateDaylyPlan(DaylyPlanCreatedDTO planCreateDTO)
        {
            if (planCreateDTO == null)
            {
                return NoContent();
            }
            var daylyPlanModel = _mapper.Map<DaylyPlan>(planCreateDTO);

            var a=_repository.CreateDaylyPlan(daylyPlanModel, onlineIdUser);

            if (a == "Created")
            {
                return Created("", a);
            }
            else
            {
                return new NoContentResult();
            }
        }



        //[Authorize(Roles = "Admin")
        [HttpGet("getalldaylyplan")]
        public IActionResult GetDaylyPlan(DateTime day_date, int queryNum)
        {
            var daylyPlans = _repository.GetAllYearDaylyPlan(day_date, queryNum);

            return Ok(_mapper.Map<IEnumerable<DaylyPlanReadedDTO>>(daylyPlans));
        }



        //[Authorize(Roles = "Admin")]
        [HttpGet("getbyiddaylyplan/{id}")]
        public IActionResult GetDaylyPlanById(int id)
        {
            var daylyPlan = _repository.GetDaylyPlanByID(id);

            if (daylyPlan == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DaylyPlanReadedDTO>(daylyPlan));
        }




        [Authorize(Roles = "Admin")]
        [HttpDelete("deletedaylyplan/{id}")]
        public IActionResult DeleteDaylyPlan(int id)
        {
            var daylyPlan = _repository.GetDaylyPlanByID(id);

            if (daylyPlan == null)
            {
                return NotFound();
            }

            _repository.DeleteDaylyPlan(id, onlineIdUser);

            return Ok();

        }



        [Authorize(Roles = "Admin")]
        [HttpPut("updatedaylyplan/{id}")]
        public IActionResult UpdateDaylyPlan(int id, DaylyPlanUpdatedDTO daylyPlanUpdatedDTO)
        {
            var daylyPlanCheck = _repository.GetDaylyPlanByID(id);

            if (daylyPlanCheck == null)
            {
                return NotFound();
            }
            DaylyPlan mapDaylyPlan = _mapper.Map<DaylyPlan>(daylyPlanUpdatedDTO);
            _repository.UpdateDaylyPlan(id, mapDaylyPlan, onlineIdUser);
            DaylyPlan updateDaylyPlan = _repository.GetDaylyPlanByID(id);
            return Ok(_mapper.Map<DaylyPlanReadedDTO>(updateDaylyPlan));
        }

    }
}
