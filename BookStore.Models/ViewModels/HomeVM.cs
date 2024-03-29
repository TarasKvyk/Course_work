﻿using BookStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models.ViewModels
{
    // View model для передавання на view кількох об'єктів на головну сторінку
    public class HomeVM
    {
        public List<Book> BookList { get; set; }
        public IEnumerable<SelectListItem> AuthorList { get; set; }
        public int AuthorId { get; set; }

        public IEnumerable<SelectListItem> OrderOptionsList { get; set; }
        public int OrderOptionId { get; set; }

        public List<string> ChosenLanguages { get; set; }
        public List<string> AvailableLanguages { get; set; }

        public List<int?> ChosenCategoryIds { get; set; }
        public List<Category?> AvailableCategories { get; set; }

        public string SearchQuery { get; set; }

        [ValidateNever]
        [NotMapped]
        [Range(0, 10000, ErrorMessage = "The Price must be in the range from 0 to 10000.")]
        public int MinPrice { get; set; } = 0;

        [NotMapped]
        [Range(0, 10000, ErrorMessage = "The price must be in the range from 0 to 10000.")]
        public int MaxPrice { get; set; } = 9999;

        public int BooksPerPage { get; set; } = 6;
        public int PageNumber { get; set; } = 2;
        public int CurrentPageNumber { get; set; } = 1;
    }
}
