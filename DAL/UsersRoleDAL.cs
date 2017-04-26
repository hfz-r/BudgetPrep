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
                bpe.Object = "Roles";
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
                bpe.Object = "Roles";
                bpe.ObjectName = objMasterRole.RoleName;
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Failure";
                bpe.Status = "A";
                bpe.CreatedTimeStamp = DateTime.Now;
                new EventLogDAL().AddEventLog(bpe);

                throw ex;
            }
        }

        public bool UpdateMasterRole(MasterRole objMasterRole)
        {
            MasterRole obj = db.MasterRoles.Where(x => x.RoleID == objMasterRole.RoleID).FirstOrDefault();
            string changes = new EventLogDAL().ObjectDifference(obj, objMasterRole);
            try
            {
                if (obj != null)
                {
                    obj.RoleName = objMasterRole.RoleName;
                    obj.Description = objMasterRole.Description;
                    obj.RoleStatus = objMasterRole.RoleStatus;
                    db.SaveChanges();

                    BPEventLog bpe = new BPEventLog();
                    bpe.Object = "Roles";
                    bpe.ObjectName = objMasterRole.RoleName;
                    bpe.ObjectChanges = changes;
                    bpe.EventMassage = "Success";
                    bpe.Status = "A";
                    bpe.CreatedTimeStamp = DateTime.Now;
                    new EventLogDAL().AddEventLog(bpe);
                }
                return true;
            }
            catch (Exception ex)
            {
                BPEventLog bpe = new BPEventLog();
                bpe.Object = "Roles";
                bpe.ObjectName = objMasterRole.RoleName;
                bpe.ObjectChanges = changes;
                bpe.EventMassage = "Failure";
                bpe.Status = "A";
                bpe.CreatedTimeStamp = DateTime.Now;
                new EventLogDAL().AddEventLog(bpe);

                throw ex;
            }
        }

        public List<JuncUserRole> ListUserRole()
        {
            return db.JuncUserRoles.Select(x => x).ToList();
        }

        public bool UserRoleFunc(JuncUserRole objUserRole)
        {
            try
            {
                if (db.JuncUserRoles.Where(x => x.RoleID == objUserRole.RoleID && x.UserID == objUserRole.UserID).Count() == 0)
                {
                    if (db.JuncUserRoles.Where(x => x.UserID == objUserRole.UserID).Count() > 0)
                    {
                        foreach (JuncUserRole obj in ListUserRole().Where(x => x.UserID == objUserRole.UserID).ToList())
                        {
                            db.JuncUserRoles.Remove(obj);
                        }
                    }
                }
                else
                {
                    foreach (JuncUserRole obj in ListUserRole().Where(x => x.UserID == objUserRole.UserID).ToList())
                    {
                        db.JuncUserRoles.Remove(obj);
                    }
                }

                if (db.MasterRoles.Where(x=>x.RoleID==objUserRole.RoleID && x.RoleStatus != objUserRole.Status).Count() > 0)
                {
                    UpdateStatusOnRole(objUserRole);
                }

                db.JuncUserRoles.Add(objUserRole);
                db.SaveChanges();

                BPEventLog bpe = new BPEventLog();
                bpe.Object = "UsersRole";
                bpe.ObjectName = GetRoles().Where(x => x.RoleID == objUserRole.RoleID).Select(y => y.RoleName).FirstOrDefault();
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
                bpe.Object = "UsersRole";
                bpe.ObjectName = GetRoles().Where(x => x.RoleID == objUserRole.RoleID).Select(y => y.RoleName).FirstOrDefault();
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Failure";
                bpe.Status = "A";
                bpe.CreatedTimeStamp = DateTime.Now;
                new EventLogDAL().AddEventLog(bpe);

                throw ex;
            }
        }

        public bool UpdateStatusOnRole(JuncUserRole objUserRole)
        {
            try
            {
                MasterRole _role = db.MasterRoles.Where(x => x.RoleID == objUserRole.RoleID).FirstOrDefault();
                _role.RoleStatus = objUserRole.Status;
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
