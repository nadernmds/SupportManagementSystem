//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SupportManagemantSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TicketFile
    {
        public long ticketFileID { get; set; }
        public string fileName { get; set; }
        public Nullable<long> requestID { get; set; }
        public Nullable<long> responseID { get; set; }
        public Nullable<long> messageID { get; set; }
    
        public virtual Message Message { get; set; }
        public virtual Request Request { get; set; }
        public virtual Response Response { get; set; }
    }
}
