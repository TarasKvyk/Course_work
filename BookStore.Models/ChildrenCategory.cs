using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    // Клас - дитяча категорія, що є похідним від Category
    public class ChildrenCategory : Category
    {
        [Required]
        [DisplayName("Purpose Age")]
        public string PurposeAge { get; set; }
    }
}
