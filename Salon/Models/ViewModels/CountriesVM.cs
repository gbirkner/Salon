using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Salon.Models
{
    public class CountriesViewModel
    {
        [Key]
        [Display(Name = "Länderkürzel")]
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string CountryId { get; set; }

        [Display(Name = "Land")]
        [StringLength(255, MinimumLength = 1)]
        [Required]
        public string Title { get; set; }

        public virtual ICollection<Cities> Cities { get; set; }

    }
}