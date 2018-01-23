﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salon.Models;
using System.Data.Entity;

namespace Salon.Models.Statistics
{
    public sealed partial class CustomerStatistics
    {
        private SalonEntities dbref;

        public DbSet<Models.Customers> Customers { get; set; }
        public DbSet<Models.Visits> Visits { get; set; }
        public DbSet<Models.Treatments> Treatments { get; set; }
        public DbSet<Models.Cities> Cities { get; set; }
        public DbSet<Models.Countries> Countries { get; set; }
        public DbSet<Models.Connections> Connections { get; set; }
        public DbSet<Models.ConnectionTypes> ConnectionTypes { get; set; }
        public DbSet<Models.VisitTasks>VisitTasks { get; set; }

        public string GetCity(int? cityId)
        {
            var cityname = from v in Cities
                           where v.CityId == cityId
                           select v.Title;

            if (cityname.FirstOrDefault() == null)
            {
                return "";
            }
            else
            {
                return cityname.First();
            }            
        }

        public List<string> GetAllCities()
        {
            var allCities = from c in Customers
                            join v in Cities on c.CityId equals v.CityId
                            select v.Title;


            allCities = allCities.Distinct();
            List<String> cityList = allCities.ToList();
            cityList.Sort();

            return cityList;
        }

        public string LastTreatment(int customerID)
        {
            var result = from vt in VisitTasks
                         join v in Visits on vt.VisitId equals v.VisitId
                         join t in Treatments on vt.TreatmentId equals t.TreatmentId
                         where v.CustomerId == customerID
                         orderby v.Created descending
                         select t.Title;
            
            if(result.FirstOrDefault() != null)
            {
                return result.FirstOrDefault();
            }
            else
            {
                return "keine Behandlung vorhanden";
            }
            
        }
        
        public List<CustomerConnections> GetConnections(int customerID)
        {
            var cust = Connections.Include(t => t.ConnectionTypes);
            var customerConnections = from c in cust where c.CustomerId == customerID
                                      select new CustomerConnections
                                      {
                                          ConnectionType = c.ConnectionTypes.Title,
                                          ConnectionDescription = c.Description,
                                          ConnectionValue = c.Title
                                      };

            if (customerConnections != null)
            {
                return customerConnections.ToList();
            }
            else
            {
                return new List<CustomerConnections>();
            }
        }

        // gets the treatments of the last visit of a customer
        public List<string> GetLastVisit(int customerID)
        {            
            var lastVisitIds = from v in Visits
                         where customerID == v.CustomerId
                         orderby v.Created descending
                         select v.VisitId;

            int visitId = lastVisitIds.FirstOrDefault();

            var lastTreatments = from vt in VisitTasks
                                 join t in Treatments on vt.TreatmentId equals t.TreatmentId                                 
                                 where vt.VisitId == visitId
                                 select new { treatmentName = t.Title };

            if (lastTreatments != null)
            {
                List<string> lt = new List<string>();
                foreach (var item in lastTreatments)
                {
                    if (!lt.Contains(item.treatmentName))
                    {
                        lt.Add(item.treatmentName);
                    }                    
                }

                return lt;
            }
            else
            {
                // if customer has no treatments
                return null;
            }
        }

        public CustomerStatistics(SalonEntities conn)
        {
            dbref = conn;
            Customers = dbref.Customers;
            Visits = dbref.Visits;
            Treatments = dbref.Treatments;
            Cities = dbref.Cities;
            Countries = dbref.Countries;
            Connections = dbref.Connections;
            ConnectionTypes = dbref.ConnectionTypes;
            VisitTasks = dbref.VisitTasks;
        }
    }

    public partial class CustomerConnections
    {
        public string ConnectionType { get; set; }
        public string ConnectionValue { get; set; }
        public string ConnectionDescription { get; set; }        
    }
}