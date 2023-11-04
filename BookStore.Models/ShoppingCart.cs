using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Book Book { get; set; }

        [Range(0, 1000, ErrorMessage = "Enter a value between 1 and 1000")]
        public int Count { get; set; }

        //public string ApplicationUserId { get; set; }
        //[ForeignKey("ApplicationUserId")]
        //[ValidateNever]
        //public ApplicationUser ApplicationUser { get; set; }

        [NotMapped] // do not create column in a database
        public double Price { get; set; }

        //public int  Count { get; set; }
     }
}
