//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Salon.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TreatmentSteps
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TreatmentSteps()
        {
            this.VisitTasks = new HashSet<VisitTasks>();
        }
    
        public int TreatmentId { get; set; }
        public int StepId { get; set; }
        public int StepOrder { get; set; }
        public int Duration { get; set; }
    
        public virtual Steps Steps { get; set; }
        public virtual Treatments Treatments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VisitTasks> VisitTasks { get; set; }
    }
}