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
    
    public partial class TICKET_THREADS
    {
        public int id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string account { get; set; }
        public string ip { get; set; }
        public int time { get; set; }
        public short status { get; set; }
        public string byadmin { get; set; }
        public string byadminip { get; set; }
        public Nullable<int> modtime { get; set; }
    }
}