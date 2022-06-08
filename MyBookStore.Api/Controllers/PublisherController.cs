using Microsoft.AspNetCore.Mvc;
using MyBookStore.Models.Models;
using MyBookStore.Models.ViewModels;
using MyBookStore.Repository;
using System;
using System.Linq;
using System.Net;

namespace MyBookStore.Api.Controllers
{
    [ApiController]
    [Route("api/publisher")]
    public class PublisherController : Controller
    {
        PublisherRepository _publisherRepository = new PublisherRepository();

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(OkObjectResult), (int)HttpStatusCode.OK)]
        public IActionResult GetPublishers(int pageIndex = 1, int pageSize = 10, string keyword = "")
        {
            if (pageIndex < 1 || pageSize < 1)
            {
                return BadRequest("Provide Correct Information");
            }
            ListResponse<Publisher> listResponse = _publisherRepository.AddPublisher(pageIndex, pageSize, keyword);
            ListResponse<PublisherModel> publicers = new ListResponse<PublisherModel>()
            {
                Results = listResponse.Results.Select(c => new PublisherModel(c)),
                TotalRecords = listResponse.TotalRecords,
            };
            return Ok(publicers);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(BadRequestObjectResult),(int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(NotFoundObjectResult),(int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OkObjectResult),(int)HttpStatusCode.OK)]
        public IActionResult GetPublisher(int id)
        {
            try
            {
                if(id <= 0)
                {
                    return BadRequest("Provide Correct Information");
                }
                Publisher publisher = _publisherRepository.GetPublisher(id);

                if(publisher == null)
                {
                    return NotFound("No Such Publisher");
                }

                PublisherModel publisherModel = new PublisherModel(publisher);
                return Ok(publisherModel);
            }
            catch(Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(),ex.Message);
            }
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(BadRequestObjectResult),(int)HttpStatusCode.BadRequest)]
        [ProducesResponseType (typeof(OkObjectResult),(int)HttpStatusCode.OK)]
        public IActionResult AddPublisher(PublisherModel model)
        {
            try
            {
                if(model == null || model.Id <= 0)
                {
                    return BadRequest("Provide Correct Information");
                }

                Publisher publisher = new Publisher()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Address = model.Address,
                    Contact = model.Contact,
                };

                var response = _publisherRepository.AddPublisher(publisher);
                if(response == null)
                {
                    return BadRequest("Already Exist");
                }

                PublisherModel publisherModel = new PublisherModel(response);
                return Ok(publisherModel);
            }
            catch(Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult UpdatePublisher(PublisherModel model)
        {
            if(model == null || model.Id <= 0)
            {
                return BadRequest("Provide Correct Information");
            }

            Publisher exist = _publisherRepository.GetPublisher(model.Id);
            if(exist == null)
            {
                return NotFound("No Such Publisher");
            }

            exist.Name = model.Name;
            exist.Address = model.Address;
            exist.Contact = model.Contact;

            Publisher publisher = _publisherRepository.UpdatePublisher(exist);
            PublisherModel publisherModel = new PublisherModel(publisher);
            return Ok(publisherModel);
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [ProducesResponseType(typeof(String), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult DeletePublisher(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id cant be less than or equal to zero");

                var response = _publisherRepository.DeletePublisher(id);
                if (response)
                {
                    return Ok("Publisher with ID : " + id + " is deleted Successfully");
                }
                else
                {
                    return BadRequest("No Such Publisher");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }

        }
    }
}
