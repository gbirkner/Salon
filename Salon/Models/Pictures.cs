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
    
    public partial class Pictures
    {
        public int PictureId { get; set; }
        public byte[] Photo { get; set; }
        public bool isSketch { get; set; }
        public int VisitId { get; set; }
        public string Description { get; set; }
    
        public virtual Visits Visits { get; set; }
    }
}
