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
    
    public partial class LOG_PURCHASES
    {
        public int bid { get; set; }
        public int item { get; set; }
        public string account { get; set; }
        public System.DateTime datetime { get; set; }
        public string itemname { get; set; }
        public Nullable<int> itemcount { get; set; }
        public Nullable<int> itemworth { get; set; }
        public byte itemcurrency { get; set; }
    }
}
