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
    
    public partial class PUS_ITEMLERI
    {
        public int id { get; set; }
        public Nullable<int> PID { get; set; }
        public bool ExtraItem { get; set; }
        public string ItemIsmi { get; set; }
        public string ItemResmi { get; set; }
        public int itemkodu { get; set; }
        public int adet { get; set; }
        public Nullable<int> ucret { get; set; }
        public string type { get; set; }
        public int alindi { get; set; }
        public string aciklama { get; set; }
        public string islev { get; set; }
        public string sure { get; set; }
        public string kullanim { get; set; }
        public string kosul { get; set; }
        public byte reqtitle { get; set; }
        public bool ItemVer { get; set; }
        public bool KomutCalistir { get; set; }
        public string Komut { get; set; }
    
        public virtual PUS_ITEMLERI_PAKET PUS_ITEMLERI_PAKET { get; set; }
    }
}