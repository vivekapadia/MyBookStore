using MyBookStore.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Repository
{
    public class BaseRepository
    {
        protected readonly MyBookStoreContext _context = new MyBookStoreContext();
    }
}
