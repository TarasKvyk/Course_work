using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Author
    { 
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } 
        public string Surname { get; set; }
        public string Country { get; set; }
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }
    }
}
