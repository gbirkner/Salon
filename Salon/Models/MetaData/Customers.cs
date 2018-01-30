using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    
    public class CustomersMetaData
    {
        [Key]
        [Required]
        [Display(Name = "Kunde")]
        public int CustomerId { get; set; }

        [Display(Name = "Vorname")]
        [StringLength(255)]
        public string FName { get; set; }

        [Required]
        [Display(Name = "Nachname")]
        [StringLength(255, MinimumLength = 1)]
        public string LName { get; set; }

        [Display(Name = "Straße")]
        [StringLength(255)]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Aktiv")]
        public bool isActive { get; set; }

        [Display(Name = "Beschreibung")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Bilder")]
        public bool allowImages { get; set; }

        [Required]
        [Display(Name = "sensible Daten")]
        public bool allowSensitive { get; set; }


        [Display(Name = "geändert von")]
        public string ModifiedBy { get; set; }

        [Display(Name = "geändert am")]
        public System.DateTime Modified { get; set; }


        [Display(Name = "erstellt von")]
        public string CreatedBy { get; set; }

        [Display(Name = "erstellt am")]
        public System.DateTime Created { get; set; }

        [Required]
        [Display(Name = "Geschlecht")]
        public int GenderID { get; set; }

        [Display(Name = "Stadt")]
        public int? CityId { get; set; }
    }

    [MetadataType(typeof(CustomersMetaData))]
    public partial class Customers
    {
        public string NameFull
        {
            get
            {
                return FName + " " + LName;
            }
        }

    }


}