//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserPerjawatanWorkflow
    {
        public int UserPerjawatanWorkflowID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string GroupPerjawatanCode { get; set; }
        public string Status { get; set; }
    
        public virtual GroupPerjawatan GroupPerjawatan { get; set; }
        public virtual MasterUser MasterUser { get; set; }
    }
}
