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

        public bool InsertMasterRole(MasterRole objMasterRole)
        {
            try
            {
                db.MasterRoles.Add(objMasterRole);
                db.SaveChanges();

                BPEventLog bpe = new BPEventLog();
                bpe.Object = "UserRole";
                bpe.ObjectName = objMasterRole.RoleName;
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Success";
                bpe.Status = "A";
                bpe.CreatedTimeStamp = DateTime.Now;
                new EventLogDAL().AddEventLog(bpe);

                return true;
            }
            catch (Exception ex)
            {
                BPEventLog bpe = new BPEventLog();
                bpe.Object = "UserRole";
                bpe.ObjectName = objMasterRole.RoleName;
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Failure";
                bpe.Status = "A";
                bpe.CreatedTimeStamp = DateTime.Now;
                new EventLogDAL().AddEventLog(bpe);

                throw ex;
            }
        }
    }
}
