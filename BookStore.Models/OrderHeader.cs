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
    public class OrderHeader
    {
		public int Id { get; set; }

		public DateTime OrderDate { get; set; }
		public DateTime ShippingDate { get; set; }
		public double OrderTotal { get; set; }

		public string? OrderStatus { get; set; }
		public string? Carrier { get; set; }
		public string? TrackingNumber { get; set; }

		[Required]
		[StringLength(15, MinimumLength = 3)]
		public string Name { get; set; }
		[Required]
		[StringLength(10, MinimumLength = 10)]
		public string PhoneNumber { get; set; }
		[Required]
		[StringLength(25, MinimumLength = 3)]
		public string StreetAddress { get; set; }
		[Required]
		[StringLength(20, MinimumLength = 3)]
		public string City { get; set; }
		[Required]
		[StringLength(20, MinimumLength = 3)]
		public string State { get; set; }
		[Required]
		[StringLength(10, MinimumLength = 3)]
		public string PostalCode { get; set; }
	}
}
