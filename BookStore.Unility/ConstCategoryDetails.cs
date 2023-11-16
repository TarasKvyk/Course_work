using BookStore.Models;
using System.Dynamic;

namespace BookStore.Unility
{
    // Клас, що містить перелік категорій і їх назви
    public static class ConstCategoryDetails
    {
        private static List<(Type Type, string TypeName)> CategoryNames = new List<(Type, string)>
        {
            (typeof(HistoryCategory), nameof(HistoryCategory)),
            (typeof(ChildrenCategory), nameof(ChildrenCategory)),
            (typeof(ScientificCategory), nameof(ScientificCategory)),
            (typeof(FictionCategory), nameof(FictionCategory)),
            (typeof(DictionaryCategory), nameof(DictionaryCategory))
        };

        public static IEnumerable<(Type, string)> GetCategoryValues()
        {
            return CategoryNames.ToList();
        }
    }
}
