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
    
    public partial class HistoryOfParcel
    {
        public int id { get; set; }
        public System.DateTime date { get; set; }
        public int idParcel { get; set; }
        public int idStatus { get; set; }
        public Nullable<int> idWarehouse { get; set; }
    
        public virtual Warehouse Warehouse { get; set; }
        public virtual Status Status { get; set; }
    }
}
