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
    
    public partial class PUS_ITEMLERI_PAKET
    {
        public PUS_ITEMLERI_PAKET()
        {
            this.PUS_ITEMLERI = new HashSet<PUS_ITEMLERI>();
        }
    
        public int PID { get; set; }
        public int PItem1 { get; set; }
        public int Adet1 { get; set; }
        public int PItem2 { get; set; }
        public int Adet2 { get; set; }
        public int PItem3 { get; set; }
        public int Adet3 { get; set; }
        public int PItem4 { get; set; }
        public int Adet4 { get; set; }
        public int PItem5 { get; set; }
        public int Adet5 { get; set; }
    
        public virtual ICollection<PUS_ITEMLERI> PUS_ITEMLERI { get; set; }
    }
}