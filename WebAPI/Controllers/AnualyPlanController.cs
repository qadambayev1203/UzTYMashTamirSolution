using AutoMapper;
using Contracts.Plan;
using Entities.DTO.AnualyPlan;
using Entities.DTO.AnualyPlanOne;
using Entities.DTO.AnualyPlanTwo;
using Entities.Model.All;
using Entities.Model.AnualyPlan;
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
    [Route("api/anualyplan")]
    [ApiController]
    [Authorize]

    public class AnualyPlanController : ControllerBase
    {
        private readonly IAnualyPlanRepository _repository;
        private readonly IMapper _mapper;
        private static int onlineIdUser = LoginUserId.loginId;
        public AnualyPlanController(IAnualyPlanRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }



        // Anualy Plan 

        [Authorize(Roles = "Admin")]
        [HttpPost("createanualyplan")]
        public IActionResult CreateAnualyPlan(AnualyPlanCreateDTO planCreateDTO)
        {
            if (planCreateDTO == null)
            {
                return NoContent();
            }
            var anualyPlanModel = _mapper.Map<AnualyPlan>(planCreateDTO);

            var a = _repository.CreateAnualyPlan(anualyPlanModel, onlineIdUser);

            //var anualyPlanReadDto = _mapper.Map<AnualyPlanReadDTO>(anualyPlanModel);
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
        [HttpGet("getallanualyplan")]
        public IActionResult GetAnualyPlan(int year, int queryNum)
        {
            var anualyPlans = _repository.GetAllYearAnualyPlan(year, queryNum);

            return Ok(_mapper.Map<IEnumerable<AnualyPlanReadDTO>>(anualyPlans));
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getbyidanualyplan/{id}")]
        public IActionResult GetAnualyPlanById(int id)
        {
            var anualyPlan = _repository.GetAnualyPlanByID(id);

            if (anualyPlan == null || anualyPlan.status == StatusEnum.deleted)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AnualyPlanReadDTO>(anualyPlan));
        }




        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteanualyplan/{id}")]
        public IActionResult DeleteAnualyPlan(int id)
        {
            var anualyPlan = _repository.GetAnualyPlanByID(id);

            if (anualyPlan == null)
            {
                return NotFound();
            }

            _repository.DeleteAnualyPlan(id, onlineIdUser);

            return Ok();

        }



        [Authorize(Roles = "Admin")]
        [HttpPut("updateanualyplan/{id}")]
        public IActionResult UpdateAnualyPlan(int id, AnualyPlanUpdatedDTO anualyPlanUpdatedDTO)
        {
            var anualyPlanCheck = _repository.GetAnualyPlanByID(id);

            if (anualyPlanCheck == null)
            {
                return NotFound();
            }
            AnualyPlan mapAnualyPlan = _mapper.Map<AnualyPlan>(anualyPlanUpdatedDTO);
            _repository.UpdateAnualyPlan(id, mapAnualyPlan, onlineIdUser);
            AnualyPlan updateAnualyPlan = _repository.GetAnualyPlanByID(id);
            return Ok(_mapper.Map<AnualyPlanReadDTO>(updateAnualyPlan));
        }




        // Anualy Plan One

        [Authorize(Roles = "Admin")]
        [HttpPost("createanualyplanone")]
        public IActionResult CreateAnualyOnePlan(int year)
        {
            int queryNum = 0;
            IEnumerable<AnualyPlan> anualyPlans = _repository.GetAllYearAnualyPlan(year, queryNum);
            IEnumerable<AnualyPlan> res = _repository.GetAllYearAnualyOnePlan(year, queryNum);
            foreach (var item in anualyPlans)
            {
                if (item.status != StatusEnum.addition)
                {                    
                    int k = 0;
                    foreach (var item1 in res)
                    {
                        if (item1.anualy_id == item.anualy_id)
                        {
                            _repository.UpdateAnualyPlanOneAdd(item.anualy_id);
                            k++;
                        }
                    }

                    if (k==0)
                    {
                        AnualyPlanOneCreateDTO oneCreateDTO = new AnualyPlanOneCreateDTO
                        {
                            anualy_id = item.anualy_id,
                            information_confirmed_date = item.information_confirmed_date,
                            month_plan = new MonthPlan
                            {
                                Yanvar = 0,
                                Fevral = 0,
                                Mart = 0,
                                Aprel = 0,
                                May = 0,
                                Iyun = 0,
                                Iyul = 0,
                                Avgust = 0,
                                Sentyabr = 0,
                                Oktyabr = 0,
                                Noyabr = 0,
                                Dekabr = 0,
                            },
                        };
                        var anualyPlanModel = _mapper.Map<AnualyPlan>(oneCreateDTO);
                        _repository.CreateAnualyOnePlan(anualyPlanModel, onlineIdUser);
                        _repository.UpdateAnualyPlanOneAdd(anualyPlanModel.anualy_id);
                    }
                }
            }

            return Created("", "");

        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getallanualyplanone")]
        public IActionResult GetAnualyOnePlan(int year, int queryNum)
        {
            var anualyPlans = _repository.GetAllYearAnualyOnePlan(year, queryNum);

            return Ok(_mapper.Map<IEnumerable<AnualyPlanOneReadDTO>>(anualyPlans));
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getbyidanualyplanone/{id}")]
        public IActionResult GetAnualyPlanOneById(int id)
        {
            var anualyPlan = _repository.GetAnualyPlanOneByID(id);

            if (anualyPlan == null || anualyPlan.status == StatusEnum.deleted)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AnualyPlanOneReadDTO>(anualyPlan));
        }




        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteanualyplanone/{id}")]
        public IActionResult DeleteAnualyOnePlan(int id)
        {
            var anualyPlan = _repository.GetAnualyPlanOneByID(id);

            if (anualyPlan == null)
            {
                return NotFound();
            }

            _repository.DeleteAnualyOnePlan(id, onlineIdUser);

            return Ok();

        }



        [Authorize(Roles = "Admin")]
        [HttpPut("updateanualyplanone/{id}")]
        public IActionResult UpdateAnualyPlanOne(int id, AnualyPlanOneUpdatedDTO anualyPlanUpdatedDTO)
        {
            var anualyPlanCheck = _repository.GetAnualyPlanOneByID(id);

            if (anualyPlanCheck == null)
            {
                return NotFound();
            }
            AnualyPlan mapAnualyPlan = _mapper.Map<AnualyPlan>(anualyPlanUpdatedDTO);
            _repository.UpdateAnualyOnePlan(id, mapAnualyPlan, onlineIdUser);
            AnualyPlan updateAnualyPlan = _repository.GetAnualyPlanOneByID(id);
            return Ok(_mapper.Map<AnualyPlanOneReadDTO>(updateAnualyPlan));
        }




        // Anualy Plan Two
        [Authorize(Roles = "Admin")]
        [HttpGet("getallanualyplantwo")]
        public IActionResult GetAnualyTwoPlan(int year, int queryNum)
        {
            var anualyPlans = _repository.GetAllYearAnualyTwoPlan(year, queryNum);

            return Ok(_mapper.Map<IEnumerable<AnualyPlanTwoReadDTO>>(anualyPlans));
        }


    }
}
