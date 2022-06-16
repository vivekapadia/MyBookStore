using MyBookStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Models.Models
{
    public class CartModel
    {
        public CartModel() { }

        public CartModel(Cart cart)
        {
            Id = cart.Id;
            Quantity = cart.Quantity;
            BookId = cart.Bookid;
            UserId = cart.Userid;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }

    }
}
