using Microsoft.AspNetCore.Mvc;
using MyBookStore.Models.Models;
using MyBookStore.Models.ViewModels;
using MyBookStore.Repository;
using System;

//for Select
using System.Linq;
using System.Net;

namespace MyBookStore.Api.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : Controller
    {
        RoleRepository _roleRepository = new RoleRepository();

        [Route("list")]
        [HttpGet]
        [ProducesResponseType(typeof(RoleModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetRoles()
        {
            try
            {
                ListResponse<Role> roles = _roleRepository.AllRoles();
                ListResponse<RoleModel> listResponse = new ListResponse<RoleModel>()
                {
                    Results = roles.Results.Select(c => new RoleModel(c)),
                    TotalRecords = roles.TotalRecords,
                };

                return Ok(listResponse);
            }
            catch(Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(RoleModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(String), (int)HttpStatusCode.NotFound)]
        public IActionResult GetRole(int id)
        {
            try
            {
                Role role = _roleRepository.GetRole(id);

                if (role == null)
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "No, Such User - Please provide correct information");

                RoleModel roleModel = new RoleModel(role);
                return Ok(roleModel);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(RoleModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(String), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddRole(RoleModel model)
        {
            try
            {
                if(model == null || model.Id <=0)
                {
                    return BadRequest("provide correct information");
                }

                Role role = new Role()
                {
                    Id = model.Id,
                    Name = model.Name
                };
                Role response = _roleRepository.AddRole(role);

                if (response == null)
                {
                    return BadRequest("Cant Add Role - Already Id Exist");
                }

                RoleModel roleModel = new RoleModel(response);
                return Ok(roleModel);
                
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }

            
        }

        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(RoleModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(String), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateRole(RoleModel model)
        {
            try
            {
                // check provied model is null
                if (model != null)
                {
                    var updateRole = _roleRepository.GetRole(model.Id);
                    if (updateRole == null)
                    {
                        return BadRequest("No Such ROLE found");
                    }

                    updateRole.Id = model.Id;
                    updateRole.Name = model.Name;

                    var response = _roleRepository.UpdateRole(updateRole);
                    RoleModel roleModel = new RoleModel(response);

                    return Ok(roleModel);
                }

                return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "Null Passed - provide correct information");
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }           
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [ProducesResponseType(typeof(String), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(String), (int)HttpStatusCode.NotFound)]
        public IActionResult DeleteRole(int id)
        {
            try
            {
                if (id == 1)
                {
                    return StatusCode(HttpStatusCode.Conflict.GetHashCode(), "Admin Cant be Removed");
                }
                else if(id > 1)
                {
                    var success = _roleRepository.DeleteRole(id);
                    if (success)
                    {
                        return Ok("Role Deleted");
                    }
                }
                return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "No, Such Role - provide correct information");
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
    }
}
