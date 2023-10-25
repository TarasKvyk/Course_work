using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Descrition { get; set; }
        [Required]
        public string KeyWords { get; set; }
        [ValidateNever]
        public string IconUrl { get; set;  }
    }
}
