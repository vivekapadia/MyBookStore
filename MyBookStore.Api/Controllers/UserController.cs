// Web APIs FILE

using MyBookStore.Repository;
using Microsoft.AspNetCore.Mvc;

// For HttpStatusCode
using System.Net;

// For Exception
using System;


namespace MyBookStore.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        UserRepository _repository = new UserRepository();

        [HttpGet]
        [Route("list")]
        public IActionResult GetUsers(int pageIndex = 1, int pageSize = 10, string keyword = "")
        {
            try
            {
                var users = _repository.GetUsers(pageIndex, pageSize, keyword);

                if (users == null)
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "No, Such User - Please provide correct information");

                return StatusCode(HttpStatusCode.OK.GetHashCode(), users);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }

        }


    }
}
