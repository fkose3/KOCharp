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
    
    public partial class RENTAL_ITEM_LIST
    {
        public int nRentalIndex { get; set; }
        public Nullable<int> nItemIndex { get; set; }
        public Nullable<short> sDurability { get; set; }
        public Nullable<long> nSerialNumber { get; set; }
        public Nullable<byte> byRegType { get; set; }
        public Nullable<byte> byItemType { get; set; }
        public Nullable<byte> byClass { get; set; }
        public Nullable<short> sRentalTime { get; set; }
        public Nullable<int> nRentalMoney { get; set; }
        public string strLenderCharID { get; set; }
        public string strLenderAcID { get; set; }
        public string strBorrowerCharID { get; set; }
        public string strBorrowerAcID { get; set; }
        public Nullable<System.DateTime> timeLender { get; set; }
        public Nullable<System.DateTime> timeRegister { get; set; }
    }
}
