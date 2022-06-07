using Microsoft.AspNetCore.Mvc;
using MyBookStore.Models.Models;
using MyBookStore.Models.ViewModels;
using MyBookStore.Repository;
using System;

// Select()
using System.Linq;
using System.Net;

namespace MyBookStore.Api.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        CategoryRepository _categoryRepository = new CategoryRepository();

        [HttpGet]
        [Route("categoryist")]
        [ProducesResponseType(typeof(String), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetCategories(int pageIndex = 1, int pageSize = 10, string keyword = "")
        {
            try
            {
                var categories = _categoryRepository.AllCategories(pageIndex, pageSize, keyword);
                ListResponse<CategoryModel> listResponse = new ListResponse<CategoryModel>()
                {
                    Results = categories.Results.Select(c => new CategoryModel(c)),
                    TotalRecords = categories.TotalRecords,
                };

                return Ok(listResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(CategoryModel),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(String),(int)HttpStatusCode.BadRequest)]
        public IActionResult GetCategory(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id cant be less than or equal to zero");

                var category = _categoryRepository.GetCategory(id);
                CategoryModel categoryModel = new CategoryModel(category);

                return Ok(categoryModel);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult),(int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CategoryModel),(int)HttpStatusCode.OK)]
        public IActionResult AddCategory(CategoryModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Model is null");

                Category category = new Category()
                {
                    Id = model.Id,
                    Name = model.Name
                };
                var response = _categoryRepository.AddCategory(category);
                CategoryModel categoryModel = new CategoryModel(response);

                return Ok(categoryModel);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
            
        }

        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(CategoryModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateCategory(CategoryModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Model is null");

                Category category = new Category()
                {
                    Id = model.Id,
                    Name = model.Name
                };
                var response = _categoryRepository.UpdateCategory(category);
                CategoryModel categoryModel = new CategoryModel(response);

                return Ok(categoryModel);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
            
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [ProducesResponseType(typeof(String),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id cant be less than or equal to zero");

                var response = _categoryRepository.DeleteCategory(id);
                if (response)
                {
                    return Ok("Category with ID : " + id + " is deleted Successfully");
                }
                else
                {
                    return BadRequest("No Such Category");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
            
        }
    }
}
