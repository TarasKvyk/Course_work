using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class FictionCategory : Category
    {
        [Required]
        public string LiteraryFormat { get; set; }
    }
}
