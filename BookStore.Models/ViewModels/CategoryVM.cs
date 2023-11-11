using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModels
{
    [ValidateNever]
    public class CategoryVM
    {
        public Category Category { get; set; }
        public IEnumerable<SelectListItem> CategoryNames { get; set; }
        public HistoryCategory History { get; set; }
        public DictionaryCategory Dictionary { get; set; }
        public FictionCategory Fiction { get; set; }
        public ChildrenCategory Children { get; set; }
        public ScientificCategory Scientific { get; set; }
        public IEnumerable<SelectListItem> LanguageList { get; set; }
    }
}