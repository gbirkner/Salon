using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    
    public class GendersMetaData
    {
        [Key]
        [Required]
        public int GenderID { get; set; }

        [Required]
        [Display(Name = "Geschlecht")]
        [StringLength(20, MinimumLength = 1)]
        public string GenderTitle { get; set; }
    }

    [MetadataType(typeof(GendersMetaData))]
    public partial class Genders
    {

    }


}