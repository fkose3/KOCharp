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
    
    public partial class GIRISPANEL_Result
    {
        public int ID { get; set; }
        public string strAccountID { get; set; }
        public string strPasswd { get; set; }
        public string strSealPasswd { get; set; }
        public string strClientIP { get; set; }
        public byte bPremiumType { get; set; }
        public Nullable<System.DateTime> dtPremiumTime { get; set; }
        public short sHours { get; set; }
        public Nullable<System.DateTime> dtCreateTime { get; set; }
        public Nullable<int> CashPoint { get; set; }
        public string email { get; set; }
        public string guvenlikcevap { get; set; }
        public string guvenliksoru { get; set; }
        public Nullable<int> TCashPoint { get; set; }
        public Nullable<byte> PusAdmin { get; set; }
        public int BonusCashPoint { get; set; }
        public string Privatekey { get; set; }
        public string Referencekey { get; set; }
        public bool GiveReferenceVal { get; set; }
    }
}