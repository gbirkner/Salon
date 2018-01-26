using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{

    public class ConnectionTypesMetaData
    {
        [Key]
        [Required]
        [Display(Name = "Kontakttyp")]
        public int ConnectionTypeId { get; set; }

        [Required]
        [Display(Name = "Kontakttyp")]
        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Beschreibung")]
        [StringLength(500)]
        public string Description { get; set; }


        public virtual ICollection<Connections> Connections { get; set; }
    }

    [MetadataType(typeof(ConnectionTypesMetaData))]
    public partial class ConnectionTypes
    {

    }


}