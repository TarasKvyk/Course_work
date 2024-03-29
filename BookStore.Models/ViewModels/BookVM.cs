﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModels
{
    // View model для передавання на view кількох об'єктів разом із Book
    public class BookVM
    {
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> AuthorList { get; set; }
        public Book Book { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> LanguageList { get; set; }
    }
}
