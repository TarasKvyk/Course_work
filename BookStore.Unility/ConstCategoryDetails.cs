using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models;

namespace BookStore.Unility
{
    public static class ConstCategoryDetails
    {
        public static List<string> CategoryNames = new List<string>()
        {
             nameof(HistoryCategory),
             nameof(ChildrenCategory),
             nameof(ScientificCategory),
             nameof(FictionCategory),
             nameof(DictionaryCategory)
        };
    }
}
