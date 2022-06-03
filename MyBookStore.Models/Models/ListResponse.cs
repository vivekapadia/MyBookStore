// generic : same class used in multiple way for different objects 
using System.Collections.Generic;

namespace MyBookStore.Models.Models
{
    public class ListResponse<T> where T : class
    {
        public IEnumerable<T> Results { get; set; }

        public int TotalRecords { get; set; }
    }
}
