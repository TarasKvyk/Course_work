using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Author
    {
        [Key] 
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Surname { get; set; }
        public string Country { get; set; }
    }
}
