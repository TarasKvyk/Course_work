﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    // Клас категорія
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Specialization { get; set; }
        [DisplayName("Category Description")]
        public string CategoryDescrition { get; set; }
        [DisplayName("Key words")]
        public string KeyWords { get; set; }
    }
}
