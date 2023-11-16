using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    // Категорія - словник, що є похідним від Category
    public class DictionaryCategory : Category
    {
        [Required]
        [DisplayName("Native Lanquage")]
        public string NativeLanguage { get; set; }
        [Required]
        [DisplayName("Into Lanquage")]
        public string IntoLanguage { get; set; }
    }
}
