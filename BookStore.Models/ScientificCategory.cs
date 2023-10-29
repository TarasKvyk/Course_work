using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ScientificCategory : Category
    {
        [Required]
        [DisplayName("Knowledge Branch")]
        public string KnowledgeBranch { get; set; }
    }
}
