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
        public DbSet<Models.Visits> Visits { get; set; }
        public DbSet<Models.Treatments> Treatments { get; set; }
        public DbSet<Models.Cities> Cities { get; set; }
        public DbSet<Models.Countries> Countries { get; set; }
        public DbSet<Models.Connections> Connections { get; set; }
        public DbSet<Models.ConnectionTypes> ConnectionTypes { get; set; }

        public string GetCity(string postalCode, string countryId)
        {
            var cityname = from v in Cities
                           where v.PostalCode == postalCode && v.CountryId == countryId
                           select v.Title;

            return cityname.First();
        }

        public CustomerConnections GetConnections(int customerID)
        {
            var cust = Connections.Include(t => t.ConnectionTypes);
            var customerConnections = from c in cust
                                      select new CustomerConnections
                                      {
                                          ConnectionType = c.ConnectionTypes.Title,
                                          ConnectionDescription = c.Description,
                                          ConnectionValue = c.Title
                                      };

            //var customerConnections = from c in Connections
            //                          join
            //                          ct in ConnectionTypes on c.ConnectionTypeId equals ct.ConnectionTypeId
            //                          where c.CustomerId == customerID
            //                          select new CustomerConnections(ct.Title ?? "", c.Title ?? "", c.Description ?? "");

            return customerConnections.First();
        }

        public CustomerStatistics(DbSet<Models.Customers> customers, DbSet<Models.Visits> visits, DbSet<Models.Treatments> treatments,
            DbSet<Models.Cities> cities, DbSet<Models.Countries> countries, DbSet<Connections> connections, DbSet<ConnectionTypes> connectionTypes)
        {
            Customers = customers;
            Visits = visits;
            Treatments = treatments;
            Cities = cities;
            Countries = countries;
            Connections = connections;

        }
    }

    public partial class CustomerConnections
    {
        public string ConnectionType { get; set; }
        public string ConnectionValue { get; set; }
        public string ConnectionDescription { get; set; }

        /*public CustomerConnections(string connectionType, string connectionValue, string connectionDescription)
        {
            ConnectionType = connectionType;
            ConnectionValue = connectionValue;
            ConnectionDescription = connectionDescription;
        }*/
    }
}