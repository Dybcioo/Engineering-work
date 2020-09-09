//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Document
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Document()
        {
            this.ParcelMoving = new HashSet<ParcelMoving>();
        }
    
        public int id { get; set; }
        public System.Guid code { get; set; }
        public bool exposure { get; set; }
        public Nullable<decimal> summary { get; set; }
        public int quantity { get; set; }
        public int idTypeOfDocument { get; set; }
        public int idWarehouse { get; set; }
        public Nullable<int> idEmployee { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual TypeOfDocument TypeOfDocument { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParcelMoving> ParcelMoving { get; set; }
    }
}
