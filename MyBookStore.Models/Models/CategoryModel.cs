using MyBookStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Models.Models
{
    public class CategoryModel
    {
        // default constructor
        public CategoryModel() { }

        // custom constructor
        //public CategoryModel(int id, string name)
        //{
        //    Id = id;
        //    Name = name;
        //}

        public CategoryModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
