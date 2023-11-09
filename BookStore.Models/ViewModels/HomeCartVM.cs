using BookStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Book> BookList { get; set; }
        public IEnumerable<SelectListItem> AuthorList { get; set; }
        public int AuthorId { get; set; }
    }
}
