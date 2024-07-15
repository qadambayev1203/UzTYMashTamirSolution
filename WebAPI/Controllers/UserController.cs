using AutoMapper;
using Contracts;
using Entities.DTO.User;
using Entities.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/authorizatsion")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly IOptions<AppSettings> appSettings;
        private readonly IMapper mapper;
        private readonly JwtSecurityTokenHandler securityTokenHandler;

        public UserController(IRepositoryManager repositoryManager, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            this.repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
            this.appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
            securityTokenHandler = new JwtSecurityTokenHandler();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult< UserAuthInfoDTO>> LoginAsync([FromBody] UserCredentials credentials, CancellationToken cancelationToken)
        {
            UserAuthInfoDTO authInfo = new UserAuthInfoDTO();
            if(credentials==null)
            {
                return BadRequest("No data");
            }
            var user= await repositoryManager.User.LoginAsync(credentials.Login, credentials.Password, false, cancelationToken);
            if (user != null)
            {
                LoginUserId.loginId = user.id;
                var key = Encoding.ASCII.GetBytes(appSettings.Value.SecretKey);
                var tokenDescriptoir = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                            new Claim(ClaimTypes.GivenName, user.firstname),
                            new Claim(ClaimTypes.Name, user.lastname),
                            new Claim(ClaimTypes.Role, user.role.ToString()),
                        }
                       ),
                    Expires = DateTime.UtcNow.AddDays(2),
                    SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };
                var securityToken = securityTokenHandler.CreateToken(tokenDescriptoir);
                authInfo.Token = securityTokenHandler.WriteToken(securityToken);
                authInfo.UserDetails = mapper.Map<UserDTO>(user);
            }
            if (string.IsNullOrEmpty(authInfo?.Token) || user.status==StatusEnum.deleted)
            {
                return Unauthorized("Error login or password");
            }
            return Ok(authInfo);
        }
    }
}
