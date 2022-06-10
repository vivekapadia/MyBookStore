using MyBookStore.Models.Models;
using MyBookStore.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Repository
{
    public class BookRepository : BaseRepository
    {
        public ListResponse<Book> GetBooks(int pageIndex, int pageSize, string keyword)
        {
            keyword = keyword?.ToLower()?.Trim();
            var query = _context.Books.Where(c => keyword == null || c.Name.ToLower().Contains(keyword)).AsQueryable();
            int totalReocrds = query.Count();
            List<Book> categories = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new ListResponse<Book>()
            {
                Results = categories,
                TotalRecords = totalReocrds,
            };
        }

        public Book GetBook(int id)
        {
            if (id > 0)
            {
                return _context.Books.FirstOrDefault(c => c.Id == id);
            }
            return null;
        }

        public Book AddBook(Book book)
        {
            if (book == null)
            {
                return null;
            }

            Book exist = GetBook(book.Id);
            if (exist == null)
            {
                var entry = _context.Books.Add(book);
                _context.SaveChanges();
                return entry.Entity;
            }

            return null;
        }

        public Book UpdateBook(Book book)
        {
            if (book == null || book.Id <= 0)
            {
                return null;
            }

            Book exist = GetBook(book.Id);

            if (exist != null)
            {
                var entry = _context.Books.Update(book);
                _context.SaveChanges();
                return entry.Entity;
            }

            return null;
        }

        public bool DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(c => c.Id == id);
            if (book == null)
                return false;

            _context.Books.Remove(book);
            _context.SaveChanges();
            return true;
        }
    }
}
