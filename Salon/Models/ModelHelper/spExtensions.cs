using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Salon.Models {


    public partial class SalonEntities {

        public virtual int AnonymizeCustomerByDaysToInt() {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<int>("AnonymizeCustomerByDays").FirstOrDefault();
        }

        public virtual int AnonymizeUserByDaysToInt() {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<int>("AnonymizeUserByDays").FirstOrDefault();
        }

        public virtual int DeleteCustomerByDaysToInt() {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<int>("DeleteCustomerByDays").FirstOrDefault();
        }

        public virtual int DeleteUserByDaysToInt() {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<int>("DeleteUserByDays").FirstOrDefault();
        }

    }
}