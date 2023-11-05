using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
    public class AuthorVM
    {
        public Author Author { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }

    }
}
