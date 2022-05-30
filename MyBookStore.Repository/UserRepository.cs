//All the business logic FILE

using MyBookStore.Models.ViewModels;
using MyBookStore.Models.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Repository
{
    public class UserRepository

    {
        MyBookStoreContext _context = new MyBookStoreContext();

        public List<User> GetUsers(int pageIndex, int pageSize, string keyword)
        {
            var users = _context.Users.AsQueryable();

            if (pageIndex > 0)
            {
                if (string.IsNullOrEmpty(keyword) == false)
                {
                    users = users.Where(w => w.FirstName.ToLower().Contains(keyword.ToLower()) || w.LastName.ToLower().Contains(keyword.ToLower()));
                }

                var userList = users.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return userList;
            }

            return null;
        }

        public User Login(LoginModel model)
        {
            return _context.Users.FirstOrDefault(c => c.Email.Equals(model.Email.ToLower()) && c.Password.Equals(model.Password));
        }

        public User Register(RegisterModel model)
        {
            User user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
            };

            var entry = _context.Users.Add(user);
            _context.SaveChanges();
            return entry.Entity;
        }

    }
}
