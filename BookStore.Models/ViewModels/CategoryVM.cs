using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModels
{
    public class CategoryVM
    {
        public Category Category { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryNames { get; set; }
        [ValidateNever]
        public HistoryCategory History { get; set; }
        [ValidateNever]
        public DictionaryCategory Dictionary { get; set; }
        [ValidateNever]
        public FictionCategory Fiction { get; set; }
        [ValidateNever]
        public ChildrenCategory Children { get; set; }
        [ValidateNever]
        public ScientificCategory Scientific { get; set; }
    }
}
