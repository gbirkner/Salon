
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Salon.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Steps
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Steps()
    {

        this.StepOptions = new HashSet<StepOptions>();

        this.TreatmentSteps = new HashSet<TreatmentSteps>();

    }


    public int StepId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public bool isSensitive { get; set; }

    public bool isActive { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<StepOptions> StepOptions { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<TreatmentSteps> TreatmentSteps { get; set; }

}

}