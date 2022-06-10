using MyBookStore.Models.Models;
using MyBookStore.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Repository
{
    public class RoleRepository : BaseRepository
    {
        public ListResponse<Role> AllRoles()
        {
            List<Role> roleDetails = _context.Roles.Where(w => w.Id != 0).ToList();
            var totalRecords = roleDetails.Count();

            return new ListResponse<Role>()
            {
                Results = roleDetails,
                TotalRecords = totalRecords,
            };
        }

        public Role GetRole(int id)
        {
            if (id > 0)
            {
                return _context.Roles.FirstOrDefault(w => w.Id == id);
            }

            return null;
            
        }

        public Role AddRole(Role role)
        {
            if (role == null )
            {
                return null;
            }

            Role exist = GetRole(role.Id);

            if(exist == null)
            {
                var entry = _context.Roles.Add(role);
                _context.SaveChanges();
                return entry.Entity;
            }

            return null;
        }

        public Role UpdateRole (Role role)
        {
            if(role == null || role.Id <= 0)
            {
                return null;
            }

            Role exist = GetRole(role.Id);

            if (exist != null)
            {
                var entry = _context.Roles.Update(role);
                _context.SaveChanges();
                return entry.Entity;
            }

            return null;
        }

        public bool DeleteRole(int id)
        {
            Role exist = GetRole(id);

            if (exist == null)
                return false;

            _context.Roles.Remove(exist);
            _context.SaveChanges();
            return true;
        }
    }
}
