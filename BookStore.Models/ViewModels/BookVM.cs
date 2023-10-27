using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Models.ViewModels
{
    public class BookVM
    {
        [ValidateNever]
        IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        IEnumerable<SelectListItem> AuthorList { get; set; }
        Book Book { get; set; }
    }
}
