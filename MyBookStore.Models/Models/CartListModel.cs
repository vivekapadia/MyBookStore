using MyBookStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Models.Models
{
    public class CartListModel
    {
        public CartListModel() { }

        public CartListModel(Cart cart)
        {
            Id = cart.Id;
            Quantity = cart.Quantity;
            UserId = cart.Userid;
            book = new BookModel(cart.Book);
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }

        public BookModel book { get; set; }

    }
}
