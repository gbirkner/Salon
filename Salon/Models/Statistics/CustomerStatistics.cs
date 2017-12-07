using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salon.Models;
using System.Data.Entity;

namespace Salon.Models.Statistics
{
    public sealed partial class CustomerStatistics
    {
        public DbSet<Models.Customers> Customers { get; set; }
        public DbSet<Models.Visits>Visits { get; set; }
        public DbSet<Models.Treatments>Treatments { get; set; }

        public void Sort()
        {

        }

        public CustomerStatistics(DbSet<Models.Customers> customers)
        {
            Customers = customers;
        }
    }
}