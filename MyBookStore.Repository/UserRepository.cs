//All the business logic FILE

// to use properties from respective files
using MyBookStore.Models.ViewModels;
using MyBookStore.Models.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Repository
{
    public class UserRepository : BaseRepository
    {
        public User Login(LoginModel model)
        {
            if (model == null)
            {
                return null;
            }
            return _context.Users.FirstOrDefault(c => c.Email.Equals(model.Email.ToLower()) && c.Password.Equals(model.Password));
        }

        public User Register(RegisterModel model)
        {
            User user = new User()
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email.ToLower(),
                Password = model.Password,
                Roleid = model.Roleid,
            };

            var entry = _context.Users.Add(user);
            _context.SaveChanges();
            return entry.Entity;
        }

        public User GetUser(int id)
        {
            if (id > 0)
            {
                return _context.Users.FirstOrDefault(w => w.Id == id);
            }

            return null;
        }

        public ListResponse<User> GetUsers(int pageIndex, int pageSize, string keyword)
        {
            keyword = keyword?.ToLower().Trim();
            var query = _context.Users.Where(c
                => String.IsNullOrEmpty(keyword)
                || c.Firstname.ToLower().Contains(keyword)
                || c.Lastname.ToLower().Contains(keyword)
            ).AsQueryable();

            int totalRecords = query.Count();
            IEnumerable<User> users = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new ListResponse<User>()
            {
                Results = users,
                TotalRecords = totalRecords,
            };
        }

        public User UpdateUser(User model)
        {
            if (model == null || model.Id <= 0)
            {
                return null;
            }

            User exist = GetUser(model.Id);
            if (exist != null)
            {
                var entry = _context.Update(model);
                _context.SaveChanges();

                return entry.Entity;
            }

            return null;
        }

        public bool DeleteUser(User model)
        {
            if (model == null || model.Id <= 0)
            {
                return false;
            }

            _context.Users.Remove(model);
            _context.SaveChanges();
            return true;
        }

        
    }
}
