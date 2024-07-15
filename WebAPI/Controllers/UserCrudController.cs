using AutoMapper;
using Contracts.UserCrud;
using Entities.DTO.User.UserCrud;
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
    [Route("api/usercrud")]
    [ApiController]
    [Authorize]
    public class UserCrudController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private static int onlineIdUser=LoginUserId.loginId;
        public UserCrudController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("createuser")]
        public IActionResult CreateUser(UserCreateDTO userCreateDTO)
        {
            var userModel = _mapper.Map<User>(userCreateDTO);

            _repository.CreateUser(userModel, onlineIdUser);

            var userReadDto = _mapper.Map<UserReadDTO>(userModel);

            return Created("", userReadDto);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getalluser")]
        public IActionResult GetUsers()
        {
            var users = _repository.GetAllUser();

            return Ok(_mapper.Map<IEnumerable<UserReadDTO>>(users));
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("getbyiduser/{id}")]
        public IActionResult GetUserById(int id)
        {
           
            var user = _repository.GetUserByID(id);

            if (user == null || user.status==StatusEnum.deleted)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserReadDTO>(user));
        }




        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteuser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _repository.GetUserByID(id);

            if (user == null)
            {
                return NotFound();
            }

            _repository.DeleteUser(id, onlineIdUser);

            return Ok();

        }



        [Authorize(Roles = "Admin")]
        [HttpPut("updateuser/{id}")]
        public IActionResult UpdateUser(int id,UserUpdatedDTO user)
        {
           
            var userCheck = _repository.GetUserByID(id);

            if (userCheck == null)
            {
                return NotFound();
            }
            User mapUser=_mapper.Map<User>(user);
            _repository.UpdateUser(id,mapUser, onlineIdUser);
            User updateUser = _repository.GetUserByID(id);
            return Ok(_mapper.Map<UserReadDTO>(updateUser));
        }
    }
}
