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
    
    public partial class JuncRolePage
    {
        public int JuncRolePageID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<int> PageID { get; set; }
        public string Status { get; set; }
    
        public virtual MasterPage MasterPage { get; set; }
        public virtual MasterRole MasterRole { get; set; }
    }
}
