using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ChildrenCategory : Category
    {
        [Required]
        public int PurposeAge { get; set; }
    }
}
