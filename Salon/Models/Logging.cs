
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
    
public partial class Logging
{

    public int LogID { get; set; }

    public string FunctionName { get; set; }

    public string ControllerName { get; set; }

    public string UserID { get; set; }

    public System.DateTime LogDateTime { get; set; }



    public virtual AspNetUsers AspNetUsers { get; set; }

}

}
