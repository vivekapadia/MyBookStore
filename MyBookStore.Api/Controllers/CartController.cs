using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MyBookStore.Models.Models;
using MyBookStore.Models.ViewModels;
using MyBookStore.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BookStore.Api.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly CartRepository _cartRepository = new CartRepository();

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(ListResponse<CartModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetCartItems(int userID,int pageIndex = 1, int pageSize = 10 ,string keyword="")
        {
            ListResponse<Cart> carts = _cartRepository.GetCartItems(userID,pageIndex, pageSize,keyword);
            ListResponse<CartListModel> listResponse = new ListResponse<CartListModel>()
            {
                Results = carts.Results.Select(c => new CartListModel(c)),
                TotalRecords = carts.TotalRecords,
            };

            return Ok(listResponse);
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(CartModel), (int)HttpStatusCode.OK)]
        public IActionResult GetCategory(int id)
        {
            var cartItem = _cartRepository.GetCarts(id);
            CartModel cartModel = new CartModel(cartItem);

            return Ok(cartModel);
        }

        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(CartModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddCart(CartModel model)
        {
            if (model == null)
                return BadRequest();

            Cart cart = new Cart()
            {
                Quantity = 1,
                Bookid = model.BookId,
                Userid = model.UserId
            };
            cart = _cartRepository.AddCart(cart);

            if (cart == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Already Item Exist in Cart");
            }

            return Ok(new CartModel(cart));
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(CartModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateCart(CartModel model)
        {
            if (model == null)
                return BadRequest();

            Cart cart = new Cart()
            {
                Id = model.Id,
                Quantity = model.Quantity,
                Bookid = model.BookId,
                Userid = model.UserId
            };
            cart = _cartRepository.UpdateCart(cart);

            return Ok(new CartModel(cart));
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult DeleteCart(int id)
        {
            if (id == 0)
                return BadRequest();

            bool response = _cartRepository.DeleteCart(id);
            return Ok(response);
        }
    }
}
