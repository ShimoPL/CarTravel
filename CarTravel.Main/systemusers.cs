//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarTravel.Main
{
    using System;
    using System.Collections.Generic;
    
    public partial class systemusers
    {
        public int loginId { get; set; }
        public int userId { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    
        public virtual users users { get; set; }
    }
}
