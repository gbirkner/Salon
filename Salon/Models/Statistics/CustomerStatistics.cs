using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salon.Models;

namespace Salon.Models.Statistics
{
    public sealed partial class CustomerStatistics
    {
        public List<Models.Customers> Customers { get; set; }

        public CustomerStatistics(List<Models.Customers> customers)
        {
            Customers = customers;
        }
    }
}