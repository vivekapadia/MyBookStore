using MyBookStore.Models.Models;
using MyBookStore.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Repository
{
    public class CategoryRepository : BaseRepository
    {
        public ListResponse<Category> AllCategories(int pageIndex, int pageSize, string keyword)
        {
            keyword = keyword?.ToLower()?.Trim();
            var query = _context.Categories.Where(c => keyword == null || c.Name.ToLower().Contains(keyword)).AsQueryable();
            var totalRecords = query.Count();

            List<Category> categories = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new ListResponse<Category>()
            {
                Results = categories,
                TotalRecords = totalRecords,
            };
        }

        public Category GetCategory(int id)
        {
            if (id > 0)
            {
                return _context.Categories.FirstOrDefault(c => c.Id == id);
            }

            return null;
        }

        public Category AddCategory(Category category)
        {
            if (category == null )
            {
                return null;
            }

            Category exist = GetCategory(category.Id);
            if (exist == null)
            {
                var entry = _context.Categories.Add(category);
                _context.SaveChanges();
                return entry.Entity;
            }

            return null;

            
        }

        public Category UpdateCategory(Category category)
        {
            if (category == null || category.Id <= 0)
            {
                return null;
            }

            Category exist = GetCategory(category.Id);
            if (exist != null)
            {
                var entry = _context.Categories.Update(category);
                _context.SaveChanges();
                return entry.Entity;
            }
            
            return null;
        }

        public bool DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }
    }
}
