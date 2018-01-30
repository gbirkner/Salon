using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    
    public class CitiesMetaData
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

    }

    [MetadataType(typeof(CitiesMetaData))]
    public partial class Cities
    {
     

    }


}