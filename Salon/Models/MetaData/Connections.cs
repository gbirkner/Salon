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
        public int ConnectionId;

        public int ConnectionTypeId;

        [Display(Name = "Kunde")]
        public int CustomerId;

        [Display(Name = "Kontakt")]
        public string Title;

        [Display(Name = "Beschreibung")]
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