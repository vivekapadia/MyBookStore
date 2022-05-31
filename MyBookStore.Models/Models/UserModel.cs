using System;
using System.Collections.Generic;

// for Required
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookStore.Models.Models
{
    public class UserModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int Roleid { get; set; }
    }
}
