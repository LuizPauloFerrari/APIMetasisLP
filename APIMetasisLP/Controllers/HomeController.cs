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
    [Route("v1/account")]
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
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";

    }
}
