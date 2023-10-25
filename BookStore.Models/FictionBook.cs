using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Fiction : Category
    {
        [Required]
        public string LiteraryFormat { get; set; }
    }
}
