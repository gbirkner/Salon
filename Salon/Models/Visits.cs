
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
    
public partial class Visits
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Visits()
    {

        this.Pictures = new HashSet<Pictures>();

        this.VisitTasks = new HashSet<VisitTasks>();

    }


    public int VisitId { get; set; }

    public int Duration { get; set; }

    public Nullable<int> CustomerId { get; set; }

    public string ModifiedBy { get; set; }

    public System.DateTime Modified { get; set; }

    public string CreatedBy { get; set; }

    public System.DateTime Created { get; set; }

    public Nullable<int> RoomId { get; set; }

    public string TeacherId { get; set; }



    public virtual AspNetUsers AspNetUsers { get; set; }

    public virtual AspNetUsers AspNetUsers1 { get; set; }

    public virtual AspNetUsers AspNetUsers2 { get; set; }

    public virtual Customers Customers { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Pictures> Pictures { get; set; }

    public virtual Rooms Rooms { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<VisitTasks> VisitTasks { get; set; }

}

}
