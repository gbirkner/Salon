using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    
    public class ConnectionsMetaData
    {
        [Key]
        [Required]
        [Display(Name = "Kontakt")]
        public int ConnectionId;

       

        [Display(Name = "Kontakttyp")]
        [Required]
        public int ConnectionTypeId;

        [Display(Name = "Kunde")]
        [Required]
        public int CustomerId;

        [Display(Name = "Kontakt")]
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Title;

        [Display(Name = "Beschreibung")]
        [StringLength(500)]
        public string Description;

        [Display(Name = "Kontakttyp")]
        public virtual ConnectionTypes ConnectionTypes { get; set; }

        [Display(Name = "Kunde")]
        public virtual Customers Customers { get; set; }

    }

    [MetadataType(typeof(ConnectionsMetaData))]
    public partial class Connections
    {

    }


}