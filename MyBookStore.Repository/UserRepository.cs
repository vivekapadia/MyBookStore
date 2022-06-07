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
                return _context.Users.Where(w => w.Id == id).FirstOrDefault();
            }

            return null;
        }

        public List<User> GetUsers(int pageIndex, int pageSize, string keyword)
        {
            var users = _context.Users.AsQueryable();

            if (pageIndex > 0)
            {
                if (string.IsNullOrEmpty(keyword) == false)
                {   
                    users = users.Where(w => w.Firstname.ToLower().Contains(keyword.ToLower()) || w.Lastname.ToLower().Contains(keyword.ToLower()));
                }

                var userList = users.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return userList;
            }

            return null;
        }

        public bool UpdateUser(User model)
        {
            // check for valid conditions
            if (model.Id > 0)
            {
                _context.Update(model);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool DeleteUser(User model)
        {
            _context.Users.Remove(model);
            _context.SaveChanges();
            return true;
        }

        
    }
}
