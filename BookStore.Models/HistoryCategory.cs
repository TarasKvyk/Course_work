using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    // Клас історична категорія, що є похідним від Category
    public class HistoryCategory : Category
    {
        [Required]
        public string Period { get; set; }
    }
}
