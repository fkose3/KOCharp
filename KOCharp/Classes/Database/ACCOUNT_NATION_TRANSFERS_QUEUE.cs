//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KOCharp.Classes.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class ACCOUNT_NATION_TRANSFERS_QUEUE
    {
        public decimal ID { get; set; }
        public string AccountID { get; set; }
        public Nullable<byte> Process { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }
    }
}