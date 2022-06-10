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
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        BookRepository _bookRepository = new BookRepository();
        CategoryRepository _categoryRepository = new CategoryRepository();
        PublisherRepository _publisherRepository = new PublisherRepository();

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(CategoryModel), (int)HttpStatusCode.OK)]
        public IActionResult GetBooks(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                ListResponse<Book> books = _bookRepository.GetBooks(pageIndex, pageSize, keyword);

                ListResponse<BookModel> listResponse = new ListResponse<BookModel>()
                {
                    Results = books.Results.Select(c => new BookModel(c)),
                    TotalRecords = books.TotalRecords,
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
        [ProducesResponseType(typeof(BookModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound)]
        public IActionResult GetBook(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id cant be less than or equal to zero");

                Book book = _bookRepository.GetBook(id);
                if (book == null)
                    return NotFound("NO Such category");

                return Ok(new BookModel(book));
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
            
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(BookModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddBook(BookModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Model is null");

                var bookExist = _bookRepository.GetBook(model.Id);
                if (bookExist != null)
                {
                    return BadRequest("Already Book Exist with this ID");
                }

                var categoryExist = _categoryRepository.GetCategory(model.Categoryid);
                var publiserExist = _publisherRepository.GetPublisher(model.Publisherid);

                if(categoryExist == null || publiserExist == null)
                {
                    return BadRequest("category or publiser not found - Provide Correct Information");
                }

                Book book = new Book()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    Base64image = model.Base64image,
                    Categoryid = model.Categoryid,
                    Publisherid = model.Publisherid,
                    Quantity = model.Quantity,
                };


                var response = _bookRepository.AddBook(book);
                if (response == null)
                {
                    return BadRequest("Cant Add Book - Already Id Exist");
                }

                BookModel bookModel = new BookModel(response);

                return Ok(bookModel);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
            
        }

        [Route("update")]
        [HttpPut]
        [ProducesResponseType(typeof(BookModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateBook(BookModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Model is null");

                var bookExist = _bookRepository.GetBook(model.Id);
                if (bookExist == null)
                {
                    return NotFound("Book not Found");
                }

                Category categoryExist = _categoryRepository.GetCategory(model.Categoryid);
                Publisher publiserExist = _publisherRepository.GetPublisher(model.Publisherid);

                if (categoryExist == null || publiserExist == null)
                {
                    return BadRequest("category or publiser not found - Provide Correct Information");
                }

                Book bookUpdate = _bookRepository.GetBook(model.Id);

                if(bookUpdate == null)
                {
                    return NotFound("No Such Book Found");
                }

                bookUpdate.Id = model.Id;
                bookUpdate.Name = model.Name;
                bookUpdate.Price = model.Price;
                bookUpdate.Description = model.Description;
                bookUpdate.Base64image = model.Base64image;
                bookUpdate.Categoryid = model.Categoryid;
                bookUpdate.Publisherid = model.Publisherid;
                bookUpdate.Quantity = model.Quantity;

                Book response = _bookRepository.UpdateBook(bookUpdate);
                BookModel bookModel = new BookModel(response);

                return Ok(bookModel);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
            
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id is null");

                var bookExist = _bookRepository.GetBook(id);
                if (bookExist == null)
                {
                    return NotFound("Book not Found");
                }

                var response = _bookRepository.DeleteBook(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
    }
}
