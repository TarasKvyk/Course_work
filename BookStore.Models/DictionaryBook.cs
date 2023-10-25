using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class DictionaryBook : Category
    {
        [Required]
        public string NativeLanguage { get; set; }
        [Required]
        public string IntoLanguage { get; set; }
    }
}
