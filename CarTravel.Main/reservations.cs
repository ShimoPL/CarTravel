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
    
    public partial class reservations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public reservations()
        {
            this.payments = new HashSet<payments>();
        }
    
        public long reservationId { get; set; }
        public string status { get; set; }
        public int client { get; set; }
        public System.DateTime createDate { get; set; }
        public System.DateTime startDate { get; set; }
        public System.DateTime endDate { get; set; }
        public string comment { get; set; }
        public Nullable<System.DateTime> modifiedOn { get; set; }
        public Nullable<int> modifiedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<payments> payments { get; set; }
        public virtual users users { get; set; }
        public virtual users users1 { get; set; }
    }
}
