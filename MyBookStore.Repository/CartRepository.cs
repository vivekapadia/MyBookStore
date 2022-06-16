using Microsoft.EntityFrameworkCore;
using MyBookStore.Models.Models;
using MyBookStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Repository
{
    public class CartRepository : BaseRepository
    {
        public ListResponse<Cart> GetCartItems(int userID ,int pageIndex, int pageSize, string keyword)
        {
            keyword = keyword?.ToLower()?.Trim();
            var query = _context.Carts.Include(c => c.Book).Where(c => c.Userid == userID && ( keyword == null || c.Book.Name.ToLower().Contains(keyword) ) ).AsQueryable();
            int totalReocrds = query.Count();
            List<Cart> carts = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new ListResponse<Cart>()
            {
                Results = carts,
                TotalRecords = totalReocrds,
            };

        }

        public Cart GetCarts(int id)
        {
            return _context.Carts.FirstOrDefault(c => c.Id == id);
        }

        public Cart AddCart(Cart cart)
        {
            if (cart == null)
            {
                return null;
            }

            Cart exist = _context.Carts.FirstOrDefault(c => c.Userid == cart.Userid && c.Bookid == cart.Bookid);
            if (exist == null)
            {
                var entry = _context.Carts.Add(cart);
                _context.SaveChanges();
                return entry.Entity;
            }

            return null;
        }

        public Cart UpdateCart(Cart category)
        {
            var entry = _context.Carts.Update(category);
            _context.SaveChanges();
            return entry.Entity;
        }

        public bool DeleteCart(int id)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.Id == id);
            if (cart == null)
                return false;

            _context.Carts.Remove(cart);
            _context.SaveChanges();
            return true;
        }
    }
}
