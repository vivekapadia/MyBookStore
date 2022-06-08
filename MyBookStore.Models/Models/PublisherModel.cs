using MyBookStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Models.Models
{
    public class PublisherModel
    {
        public PublisherModel() { }
        public PublisherModel(Publisher model) 
        {
            Id = model.Id;
            Name = model.Name;
            Address = model.Address;
            Contact = model.Contact;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
    }
}
