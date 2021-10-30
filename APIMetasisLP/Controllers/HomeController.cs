using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APIMetasisLP.Entities;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using APIMetasisLP.Services;
using APIMetasisLP.Repositories;
using System.Net;
using System.Web;
using System.Net.Http;

namespace APIMetasisLP.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/account")]
    public class HomeController : Controller
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);
            var host = HttpContext.Request.Host.ToString();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            //string userRequest = System.Web.HttpContext.Current.Request.UserHostAddress;

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            var expires = DateTime.UtcNow.AddHours(2);
            return new
            {
                user,
                token,
                expires,
                host
            };
        }


        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public string Employee() => "Funcionário";

        [HttpGet]
        [Route("manager")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";

    }
}
