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
    
    public partial class payments
    {
        public int paymentId { get; set; }
        public int clientId { get; set; }
        public long reservationId { get; set; }
        public decimal offerPrice { get; set; }
        public Nullable<decimal> prepaid { get; set; }
        public decimal lastPrice { get; set; }
        public System.DateTime paymentDate { get; set; }
        public Nullable<System.DateTime> paidOn { get; set; }
        public Nullable<System.DateTime> prepaidedOn { get; set; }
    
        public virtual users users { get; set; }
        public virtual reservations reservations { get; set; }
    }
}