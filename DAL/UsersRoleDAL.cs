using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UsersRoleDAL
    {
        BPEntities db = new BPEntities();

        public List<MasterRole> GetRoles()
        {
            return db.MasterRoles.Select(x => x).ToList();
        }
    }
}
