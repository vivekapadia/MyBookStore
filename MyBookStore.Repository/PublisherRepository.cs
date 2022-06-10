using MyBookStore.Models.Models;
using MyBookStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Repository
{
    public class PublisherRepository : BaseRepository
    {
        public ListResponse<Publisher> AddPublisher(int pageIndex, int pageSize, string keyword)
        {
            keyword = keyword?.ToLower()?.Trim();
            var query = _context.Publishers.Where(c => 
                String.IsNullOrEmpty(keyword) 
                || c.Name.ToLower().Contains(keyword) 
                || c.Address.ToLower().Contains(keyword) 
                || c.Contact.ToLower().Contains(keyword)
                ).AsQueryable();

            var totalRecords = query.Count();
            IEnumerable<Publisher> publisherList = query.Skip((pageIndex - 1)*pageSize).Take(pageSize);

            return new ListResponse<Publisher>()
            {
                Results = publisherList,
                TotalRecords = totalRecords,
            };
        }
        public Publisher GetPublisher(int id)
        {
            if(id <= 0)
            {
                return null;
            }
            var publisher = _context.Publishers.FirstOrDefault(c => c.Id == id);
            if(publisher == null)
            {
                return null;
            }
            return publisher;
        }
        public Publisher AddPublisher(Publisher model)
        {
            if(model == null )
            {
                return null;
            }

            var exist = _context.Publishers.FirstOrDefault(c => c.Id == model.Id);
            if(exist == null)
            {
                var entry = _context.Publishers.Add(model);
                _context.SaveChanges();
                return entry.Entity;
            }

            return null;
        }

        public Publisher UpdatePublisher(Publisher model)
        {
            if(model == null || model.Id <= 0)
            {
                return null;
            }

            Publisher exist = GetPublisher(model.Id);
            if(exist != null)
            {
                var entry = _context.Publishers.Update(model);
                _context.SaveChanges();
                return entry.Entity;
            }

            return null;
        }

        public bool DeletePublisher(int id)
        {
            Publisher exist = GetPublisher(id);

            if (exist == null)
                return false;

            _context.Publishers.Remove(exist);
            _context.SaveChanges();
            return true;
        }
    }
}
