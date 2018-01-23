using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salon.Models {
    public partial class VisitTasks {
        public Treatments getTreatment() {
            return new SalonEntities().Treatments.Find(this.TreatmentId);
        }
    }
}