using Microsoft.AspNetCore.Mvc;

using MyBookStore.Models.Models;
using MyBookStore.Models.ViewModels;
using MyBookStore.Repository;

// For HttpStatusCode
using System.Net;

// For Exception
using System;

// for EnableCors
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;

namespace MyBookStore.Api.Controllers
{
    [ApiController]
    [EnableCors("bookstore")]
    [Route("api/public")]
    public class LoginRegisterController : ControllerBase
    {
        UserRepository _repository = new UserRepository();

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            try
            {
                User user = _repository.Login(model);
                if (user == null)
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "User not found");

                return StatusCode(HttpStatusCode.OK.GetHashCode(), user);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }

        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            try
            {
                User user = _repository.Register(model);
                if (user == null)
                    return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "Bad Request");

                return StatusCode(HttpStatusCode.OK.GetHashCode(), user);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }

        }

        
    }
}

