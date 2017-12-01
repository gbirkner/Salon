using System;
using System.ComponentModel.DataAnnotations;

namespace Salon.Views.ViewModels
{
    public class CustomersViewModel
    {
        [Key]
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
    }
}