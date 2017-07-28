using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OBSecurity;

namespace DAL
{
    public class UsersDAL
    {
        BPEntities db = new BPEntities();

        public MasterUser GetValidUser(string UserName, string Password="")
        {
            try
            {
                if (!String.IsNullOrEmpty(Password))
                {
                    //string encstr = Security.Encrypt(Password);
                    return db.MasterUsers.Where(x => x.UserStatus == "A" && x.UserName == UserName && x.UserPassword == Password).FirstOrDefault();
                }
                else
                {
                    return db.MasterUsers.Where(x => x.UserStatus == "A" && x.UserName == UserName).FirstOrDefault() ?? new MasterUser();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MasterUser GetUserDataByID(int UserId)
        {
            try
            {
                MasterUser user = db.MasterUsers.Where(x => x.UserID == UserId).FirstOrDefault();
                return user;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MasterUser VerifyAnswer(string UserName, string Answer)
        {
            string pwd = Security.Encrypt(Answer);
            return db.MasterUsers.Where(x => x.UserName == UserName && x.SecAnswer.Trim() == pwd).FirstOrDefault();
        }

        public bool ResetPassword(string UserName, string Password)
        {
            MasterUser user = db.MasterUsers.Where(x => x.UserName == UserName).FirstOrDefault();
            try
            {
                //string encstr = Security.Encrypt(Password);
                user.UserPassword = Password;
                user.ModifiedBy = user.UserID;
                user.ModifiedTimeStamp = DateTime.Now;
                db.SaveChanges();

                BPEventLog bpe = new BPEventLog();
                bpe.Object = "User - Reset Password";
                bpe.ObjectName = user.UserName;
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Success";
                bpe.Status = "A";
                bpe.CreatedBy = user.ModifiedBy;
                bpe.CreatedTimeStamp = user.ModifiedTimeStamp;
                new EventLogDAL().AddEventLog(bpe);
            }
            catch
            {
                BPEventLog bpe = new BPEventLog();
                bpe.Object = "User - Reset Password";
                bpe.ObjectName = user.UserName;
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Failure";
                bpe.Status = "A";
                bpe.CreatedBy = user.ModifiedBy;
                bpe.CreatedTimeStamp = user.ModifiedTimeStamp;
                new EventLogDAL().AddEventLog(bpe);

                return false;
            }

            return true;
        }

        public List<MasterUser> GetUsers()
        {
            return db.MasterUsers.Select(x => x).OrderBy(x => x.UserName).ToList();
        }

        public bool InsertUsers(MasterUser objMasterUser, 
            List<UserMengurusWorkflow> objUserMengurus, 
            List<UserPerjawatanWorkflow> objUserPerjawatan, 
            List<UserSegDtlWorkflow> objUserSegmentDetails, 
            ref int UserId)
        {
            try
            {
                if (db.MasterUsers.Where(x => x.UserName.Equals(objMasterUser.UserName)).Count() > 0)
                {
                    return false;
                }
                else
                {
                    objMasterUser.SecAnswer = Security.Encrypt(objMasterUser.SecAnswer);

                    db.MasterUsers.Add(objMasterUser);

                    foreach (UserMengurusWorkflow o in objUserMengurus)
                        db.UserMengurusWorkflows.Add(o);

                    foreach (UserPerjawatanWorkflow o in objUserPerjawatan)
                        db.UserPerjawatanWorkflows.Add(o);

                    foreach (UserSegDtlWorkflow o in objUserSegmentDetails)
                        db.UserSegDtlWorkflows.Add(o);

                    db.SaveChanges();

                    UserId = objMasterUser.UserID;

                    BPEventLog bpe = new BPEventLog();
                    bpe.Object = "User - New User";
                    bpe.ObjectName = objMasterUser.UserName;
                    string mw = (objUserMengurus.Count == 0) ? string.Empty : objUserMengurus.Select(x => x.AccountCode).Aggregate((x, y) => x + "," + y);
                    string pw = (objUserPerjawatan.Count == 0) ? string.Empty : objUserPerjawatan.Select(x => x.GroupPerjawatanCode).Aggregate((x, y) => x + "," + y);
                    bpe.ObjectChanges = "<tr><td>Mengurus Workflow</td><td>New</td><td>" + mw + "</td></tr><tr><td>Perjawatan Workflow</td><td>New</td><td>" + pw + "</td></tr>";
                    bpe.EventMassage = "Success";
                    bpe.Status = "A";
                    bpe.CreatedBy = objMasterUser.CreatedBy;
                    bpe.CreatedTimeStamp = objMasterUser.CreatedTimeStamp;
                    new EventLogDAL().AddEventLog(bpe);

                    return true;
                }
            }
            catch (Exception ex)
            {
                BPEventLog bpe = new BPEventLog();
                bpe.Object = "User - New User";
                bpe.ObjectName = objMasterUser.UserName;
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Failure";
                bpe.Status = "A";
                bpe.CreatedBy = objMasterUser.CreatedBy;
                bpe.CreatedTimeStamp = objMasterUser.CreatedTimeStamp;
                new EventLogDAL().AddEventLog(bpe);

                throw ex;
            }
        }

        public bool UpdateUsers(MasterUser objMasterUser, string Role, 
            List<UserMengurusWorkflow> objUserMengurus, 
            List<UserPerjawatanWorkflow> objUserPerjawatan,
            List<UserSegDtlWorkflow> objUserSegmentDetails)
        {
            MasterUser objuser = db.MasterUsers.Where(x => x.UserID == objMasterUser.UserID).FirstOrDefault();
            string changes = new EventLogDAL().ObjectDifference(objuser, objMasterUser);
            string rolechange = (db.JuncUserRoles.Where(x => x.UserID == objMasterUser.UserID).FirstOrDefault().RoleID == Convert.ToInt32(Role)) ?
                string.Empty : "<tr><td>RoleID</td><td>" + db.JuncUserRoles.Where(x => x.UserID == objMasterUser.UserID).FirstOrDefault().RoleID + "</td><td>"
                + Convert.ToInt32(Role) + "</td></tr>";

            try
            {
                if (objuser != null)
                {
                    objuser.UserName = objMasterUser.UserName;
                    objuser.FullName = objMasterUser.FullName;
                    objuser.UserEmail = objMasterUser.UserEmail;
                    objuser.UserIC = objMasterUser.UserIC;
                    objuser.Department = objMasterUser.Department;
                    objuser.UserPhoneNo = objMasterUser.UserPhoneNo;
                    objuser.Designation = objMasterUser.Designation;
                    objuser.Fax = objMasterUser.Fax;
                    objuser.OfficeAddress = objMasterUser.OfficeAddress;
                    objuser.PeriodOfService = objMasterUser.PeriodOfService;
                    objuser.PositionGrade = objMasterUser.PositionGrade;
                    objuser.Title = objMasterUser.Title;
                    objuser.UserStatus = objMasterUser.UserStatus;
                    objuser.ModifiedBy = objMasterUser.ModifiedBy;
                    objuser.ModifiedTimeStamp = objMasterUser.ModifiedTimeStamp;

                    string mwo = (objuser.UserMengurusWorkflows.Count() == 0) ? string.Empty : objuser.UserMengurusWorkflows.ToList().Select(x => x.AccountCode).Aggregate((x, y) => x + "," + y);
                    string pwo = (objuser.UserPerjawatanWorkflows.Count() == 0) ? string.Empty : objuser.UserPerjawatanWorkflows.ToList().Select(x => x.GroupPerjawatanCode).Aggregate((x, y) => x + "," + y);
                    string swo = (objuser.UserSegDtlWorkflows.Count() == 0) ? string.Empty : objuser.UserSegDtlWorkflows.ToList().Select(x => x.SegmentDetailID.ToString()).Aggregate((x, y) => x + "," + y);
                    string mw = (objUserMengurus.Count == 0) ? string.Empty : objUserMengurus.Select(x => x.AccountCode).Aggregate((x, y) => x + "," + y);
                    string pw = (objUserPerjawatan.Count == 0) ? string.Empty : objUserPerjawatan.Select(x => x.GroupPerjawatanCode).Aggregate((x, y) => x + "," + y);
                    string sw = (objUserSegmentDetails.Count == 0) ? string.Empty : objUserSegmentDetails.Select(x => x.SegmentDetailID.ToString()).Aggregate((x, y) => x + "," + y);

                    string wochanges = string.Empty;
                    if (mwo != mw)
                        wochanges = wochanges + "<tr><td>Mengurus Workflow</td><td>" + mwo + "</td><td>" + mw + "</td></tr>";
                    if (pwo != pw)
                        wochanges = wochanges + "<tr><td>Perjawatan Workflow</td><td>" + pwo + "</td><td>" + pw + "</td></tr>";
                    if (swo != sw)
                        wochanges = wochanges + "<tr><td>SegmentDetails Workflow</td><td>" + swo + "</td><td>" + sw + "</td></tr>";

                    if (objUserMengurus.Count() >= 0)
                    {
                        foreach (UserMengurusWorkflow o in db.UserMengurusWorkflows.Where(x => x.UserID == objMasterUser.UserID).ToList())
                            db.UserMengurusWorkflows.Remove(o);
                    }

                    if (objUserPerjawatan.Count() >= 0)
                    {
                        foreach (UserPerjawatanWorkflow o in db.UserPerjawatanWorkflows.Where(x => x.UserID == objMasterUser.UserID).ToList())
                            db.UserPerjawatanWorkflows.Remove(o);
                    }

                    if (objUserSegmentDetails.Count() >= 0)
                    {
                        foreach (UserSegDtlWorkflow o in db.UserSegDtlWorkflows.Where(x => x.UserID == objMasterUser.UserID).ToList())
                            db.UserSegDtlWorkflows.Remove(o);
                    }

                    foreach (UserMengurusWorkflow o in objUserMengurus)
                        db.UserMengurusWorkflows.Add(new UserMengurusWorkflow()
                        {
                            AccountCode = o.AccountCode,
                            UserID = objMasterUser.UserID,
                            Status = "A"
                        });
                    foreach (UserPerjawatanWorkflow o in objUserPerjawatan)
                        db.UserPerjawatanWorkflows.Add(new UserPerjawatanWorkflow()
                        {
                            GroupPerjawatanCode = o.GroupPerjawatanCode,
                            UserID = objMasterUser.UserID,
                            Status = "A"
                        });
                    foreach (UserSegDtlWorkflow o in objUserSegmentDetails)
                        db.UserSegDtlWorkflows.Add(new UserSegDtlWorkflow()
                        {
                            SegmentDetailID = o.SegmentDetailID,
                            UserID = objMasterUser.UserID,
                            Status = "A"
                        });

                    db.SaveChanges();

                    BPEventLog bpe = new BPEventLog();
                    bpe.Object = "User - Updated";
                    bpe.ObjectName = objMasterUser.UserName;
                    changes = changes + rolechange + wochanges;
                    bpe.ObjectChanges = changes;
                    bpe.EventMassage = "Success";
                    bpe.Status = "A";
                    bpe.CreatedBy = objMasterUser.ModifiedBy;
                    bpe.CreatedTimeStamp = objMasterUser.ModifiedTimeStamp;
                    new EventLogDAL().AddEventLog(bpe);
                }

                return true;
            }
            catch (Exception ex)
            {
                BPEventLog bpe = new BPEventLog();
                bpe.Object = "User - Updated";
                bpe.ObjectName = objMasterUser.UserName;
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Failure";
                bpe.Status = "A";
                bpe.CreatedBy = objMasterUser.ModifiedBy;
                bpe.CreatedTimeStamp = objMasterUser.ModifiedTimeStamp;
                new EventLogDAL().AddEventLog(bpe);

                throw ex;
            }
        }

        public bool UpdateProfileUser(MasterUser objMasterUser)
        {
            MasterUser objuser = db.MasterUsers.Where(x => x.UUID == objMasterUser.UUID).FirstOrDefault();
            string changes = new EventLogDAL().ObjectDifference(objuser, objMasterUser);

            try
            {
                if (objuser != null)
                {
                    objuser.FullName = objMasterUser.FullName;
                    objuser.Image = objMasterUser.Image;
                    objuser.BirthDate = objMasterUser.BirthDate;
                    objuser.Gender = objMasterUser.Gender;
                    objuser.Comment = objMasterUser.Comment;
                    objuser.Website = objMasterUser.Website;
                    objuser.UserPhoneNo = objMasterUser.UserPhoneNo;
                    if (!string.IsNullOrEmpty(objMasterUser.UserEmail)) objuser.UserEmail = objMasterUser.UserEmail;
                    if (!string.IsNullOrEmpty(objMasterUser.UserPassword)) objuser.UserPassword = objMasterUser.UserPassword;
                    if (!string.IsNullOrEmpty(objMasterUser.SecQuestion)) objuser.SecQuestion = objMasterUser.SecQuestion;
                    if (!string.IsNullOrEmpty(objMasterUser.SecAnswer)) objuser.SecAnswer = Security.Encrypt(objMasterUser.SecAnswer);
                    objuser.ModifiedBy = objMasterUser.ModifiedBy;
                    objuser.ModifiedTimeStamp = objMasterUser.ModifiedTimeStamp;

                    db.SaveChanges();

                    BPEventLog bpe = new BPEventLog();
                    bpe.Object = "User Profile - Updated";
                    bpe.ObjectName = objMasterUser.UserName;
                    //changes = changes + rolechange + wochanges;
                    bpe.ObjectChanges = changes;
                    bpe.EventMassage = "Success";
                    bpe.Status = "A";
                    bpe.CreatedBy = objMasterUser.ModifiedBy;
                    bpe.CreatedTimeStamp = objMasterUser.ModifiedTimeStamp;
                    new EventLogDAL().AddEventLog(bpe);
                }

                return true;
            }
            catch (Exception ex)
            {
                BPEventLog bpe = new BPEventLog();
                bpe.Object = "User Profile - Updated";
                bpe.ObjectName = objMasterUser.UserName;
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Failure";
                bpe.Status = "A";
                bpe.CreatedBy = objMasterUser.ModifiedBy;
                bpe.CreatedTimeStamp = objMasterUser.ModifiedTimeStamp;
                new EventLogDAL().AddEventLog(bpe);

                throw ex;
            }
        }

        public void DeleteUsers(string username)
        {
            int UserId = GetValidUser(username).UserID;
            if (UserId != 0)
            {
                foreach (MasterUser user in db.MasterUsers.Where(x => x.UserID == UserId))
                {
                    db.MasterUsers.Remove(user);
                }
            }
        }

        public static MasterUser StaticUserId(int userid = 0, string username = "")
        {
            MasterUser objMasterUser = new MasterUser();

            if (userid != 0)
            {
                objMasterUser = new UsersDAL().GetUsers().Where(x => x.UserID == userid).FirstOrDefault();
            }
            else if (username != "")
            {
                objMasterUser = new UsersDAL().GetUsers().Where(x => x.UserName == username).FirstOrDefault();
            }

            return objMasterUser;
        }

    }
}
