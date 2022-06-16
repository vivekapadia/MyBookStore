// Web APIs FILE
// The primary or most commonly-used HTTP methods are POST, GET, PUT, PATCH, and DELETE.
// These methods correspond to create, read, update, and delete (or CRUD) operations, respectively.

using Microsoft.AspNetCore.Mvc;

// to use properties from respective files
using MyBookStore.Models.ViewModels;
using MyBookStore.Models.Models;
using MyBookStore.Repository;


// For HttpStatusCode
using System.Net;

// For Exception
using System;

using System.Linq;

namespace MyBookStore.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
    public class UserController : ControllerBase
    {
        UserRepository _userRepository = new UserRepository();

        [HttpGet]
        [Route("list")]

        public IActionResult GetUsers(int pageIndex = 1, int pageSize = 10, string keyword = "")
        {
            try
            {
                ListResponse<User> users = _userRepository.GetUsers(pageIndex, pageSize, keyword);
                ListResponse<UserModel> listResponse = new ListResponse<UserModel>()
                {
                    Results = users.Results.Select(u => new UserModel(u)),
                    TotalRecords = users.TotalRecords,
                };

                return Ok(listResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }

        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
        public IActionResult GetUser(int id)
        {
            try
            {
                User user = _userRepository.GetUser(id);

                if (user == null)
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "No, Such User - Please provide correct information");

                UserModel userModel = new UserModel(user);
                return Ok(userModel);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(String), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(String), (int)HttpStatusCode.NotFound)]
        public IActionResult UpdateUser(UpdateProfileModel model)
        {
            try
            {
                if (model != null)
                {
                    var user = _userRepository.GetUser(model.Id);
                    if (user == null)
                        return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "User not found");

                    user.Firstname = model.Firstname;
                    user.Lastname = model.Lastname;
                    user.Email = model.Email;
                    user.Password = model.NewPassword;

                    User isSaved = _userRepository.UpdateUser(user);


                    if (isSaved == null)
                    {
                        return BadRequest("Not Updated");
                    }
                    UserModel updatedModel = new UserModel(isSaved);
                    return Ok(updatedModel);
                }

                return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "No, Such User - Please provide correct information");
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(String), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(String), (int)HttpStatusCode.OK)]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                if (id > 0)
                {
                    var user = _userRepository.GetUser(id);
                    if (user == null)
                        return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "User not found");

                    var isDeleted = _userRepository.DeleteUser(user);
                    if (isDeleted)
                    {
                        return StatusCode(HttpStatusCode.OK.GetHashCode(), "User detail deleted successfully");
                    }
                }

                return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "No, Such User - Please provide correct information");
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

    }
    
}
