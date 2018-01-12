using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Salon.Models
{
    public class CitiesViewModel
    {

        [Key]
        public int CityId { get; set; }


        [Display(Name = "Land")]
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string CountryId { get; set; }

        [Required]
        [Display(Name = "Postleitzahl")]
        [StringLength(10, MinimumLength = 1)]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Stadt")]
        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; }

        [Display(Name = "Land")]
        [StringLength(255, MinimumLength = 1)]
        [Required]
        public string Country { get; set; }



        public virtual Countries Countries { get; set; }

        public virtual ICollection<Customers> Customers { get; set; }

    }
}