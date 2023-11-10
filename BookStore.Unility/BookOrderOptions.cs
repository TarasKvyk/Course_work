using BookStore.Models;
using System.Dynamic;

namespace BookStore.Unility
{
    public static class BookOrderOptions
    {
        private static List<(int id, string option)> OrderOptionsList = new List<(int, string)>
        {
            (1, "Alphabetic A-Z"),
            (2, "Alphabetic Z-A"),
            (3, "Price L->H"),
            (4, "Price H->L"),
            (5, "The Newest")
        };

        public static IEnumerable<(int, string)> GetOrderOptions()
        {
            return OrderOptionsList.ToList();
        }
    }
}
