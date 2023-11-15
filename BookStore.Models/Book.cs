using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Range(0, 5000, ErrorMessage = "Page count must be greater than 0 and lower or equal to 5000")]
        [DisplayName("Pages")]
        public int PageCount { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Available book count must be greater than 0 and lower or equal to 100")]
        [DisplayName("Available book count")]
        public int AvailableCount { get; set; }
        [Required]
        public string Title { get; set; }
        [DisplayName("Book Description")]
        public string? Description { get; set; }
        [Required]
        [Range(0, 2023, ErrorMessage = "Year must be lower or equal to 2023")]
        public int Year { get; set; }
        [Required]
        [Range(0, 10000, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }
        [Required]
        public string Language { get; set; }

        [ValidateNever]
        public string? ImageUrl { get; set; }

        [Required]
        [DisplayName("Author")]
        public int? AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        [ValidateNever]
        public Author? Author { get; set; }

        [Required]
        [DisplayName("Category")]
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }
    }
}
