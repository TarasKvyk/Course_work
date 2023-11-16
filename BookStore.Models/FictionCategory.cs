using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    // Клас художня література, що є похідним від Category
    public class FictionCategory : Category
    {
        [Required]
        [DisplayName("Literary Format")]
        public string LiteraryFormat { get; set; }
    }
}
