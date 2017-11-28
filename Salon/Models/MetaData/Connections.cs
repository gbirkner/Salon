using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    [MetadataType(typeof(Connections))]
    partial class ConnectionsMetaData
    {
        [Key]
        public int ConnectionId { get; set; }

        public int ConnectionTypeId { get; set; }

        public int CustomerId { get; set; }

        [Display(Name = "Connection")]
        public string Title { get; set; }

        public string Description { get; set; }
    }

    public partial class Connections
    {
    }
}