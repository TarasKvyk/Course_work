using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ScientificBook : Category
    {
        [Required]
        public string KnowledgeBranch { get; set; }
    }
}
