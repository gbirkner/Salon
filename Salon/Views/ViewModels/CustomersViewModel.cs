using System;
using System.ComponentModel.DataAnnotations;

namespace Salon.Views.ViewModels
{
    [Serializable]
    public class CustomersViewModel
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Straße")]
        public string Street { get; set; }

        [Display(Name = "PLZ")]
        public string PostalCode { get; set; }

        [Display(Name = "Land")]
        public string Country { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Display(Name = "Stadt")]
        public string City { get; set; }
    }
}