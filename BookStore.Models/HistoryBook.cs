using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class HistoryBook : Category
    {
        [Required]
        public string Period { get; set; }
    }
}
