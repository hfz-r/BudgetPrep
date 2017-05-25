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

<<<<<<< HEAD
        public bool InsertUserRole(JuncUserRole objUserRole)
        {
            try
            {
                foreach (JuncUserRole obj in db.JuncUserRoles.Where(x => x.UserID == objUserRole.UserID))
                {
                    db.JuncUserRoles.Remove(obj);
=======
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
>>>>>>> fa2a2893ae1d7e783d8591f454ef428f3a40756b
                }

                if (db.MasterRoles.Where(x=>x.RoleID==objUserRole.RoleID && x.RoleStatus != objUserRole.Status).Count() > 0)
                {
                    UpdateStatusOnRole(objUserRole);
                }

                db.JuncUserRoles.Add(objUserRole);
                db.SaveChanges();

                BPEventLog bpe = new BPEventLog();
<<<<<<< HEAD
                bpe.Object = "UserRoles";
=======
                bpe.Object = "UsersRole";
>>>>>>> fa2a2893ae1d7e783d8591f454ef428f3a40756b
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
<<<<<<< HEAD
                bpe.Object = "UserRoles";
=======
                bpe.Object = "UsersRole";
>>>>>>> fa2a2893ae1d7e783d8591f454ef428f3a40756b
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
<<<<<<< HEAD

        public bool DeleteUserRole(string username,string[] roles)
        {
            try
            {
                int uid = DAL.UsersDAL.StaticUserId(0, username).UserID;

                for (int i = 0; i < roles.Count(); i++)
                {
                    MasterRole objMasterRole = GetRoles().Where(x => x.RoleName == roles[i]).FirstOrDefault();
                    if (db.JuncUserRoles.Where(x => x.RoleID == objMasterRole.RoleID).Count() > 0)
                    {
                        foreach (JuncUserRole objUserRole in db.JuncUserRoles.Where(x => x.UserID == uid))
                        {
                            db.JuncUserRoles.Remove(objUserRole);
                        }
                    }
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
=======
>>>>>>> fa2a2893ae1d7e783d8591f454ef428f3a40756b
    }
}
