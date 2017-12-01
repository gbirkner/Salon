using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salon.Models
{
    [MetadataType(typeof(Connections))]
    public class ConnectionsMetaData
    {
        [Key]
        public int ConnectionId;

        public int ConnectionTypeId;

        public int CustomerId;

        [Display(Name = "Connection")]
        public string Title;

        public string Description;
    }

    public partial class Connections
    {
    }
}