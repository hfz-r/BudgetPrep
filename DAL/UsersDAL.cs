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
            string pwd = Security.Encrypt(Answer.ToUpper());
            return db.MasterUsers.Where(x => x.UserStatus == "A" && x.UserName == UserName && x.SecAnswer.Trim() == pwd).FirstOrDefault();
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

        public bool InsertUsers(MasterUser objMasterUser)
        {
            try
            {
                if (db.MasterUsers.Where(x => x.UserName.Equals(objMasterUser.UserName)).Count() > 0)
                {
                    return false;
                }
                else
                {
                    //objMasterUser.UserPassword = Security.Encrypt(objMasterUser.UserPassword);
                    objMasterUser.SecAnswer = Security.Encrypt(objMasterUser.SecAnswer.ToUpper());

                    db.MasterUsers.Add(objMasterUser);
                    db.SaveChanges();

                    BPEventLog bpe = new BPEventLog();
                    bpe.Object = "User - New User";
                    bpe.ObjectName = objMasterUser.UserName;
                    //string mw = (lstAccountCode.Count == 0) ? string.Empty : lstAccountCode.Select(x => x.AccountCode).Aggregate((x, y) => x + "," + y);
                    //string pw = (lstServiceCode.Count == 0) ? string.Empty : lstServiceCode.Select(x => x.GroupPerjawatanCode).Aggregate((x, y) => x + "," + y);
                    //bpe.ObjectChanges = "<tr><td>Mengurus Workflow</td><td>New</td><td>" + mw + "</td></tr><tr><td>Perjawatan Workflow</td><td>New</td><td>" + pw + "</td></tr>";
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

        public bool UpdateUsers(MasterUser objMasterUser)
        {
            MasterUser objuser = db.MasterUsers.Where(x => x.UserID == objMasterUser.UserID).FirstOrDefault();
            string changes = new EventLogDAL().ObjectDifference(objuser, objMasterUser);

            try
            {
                if (objuser != null)
                {
                    objuser.UserName = objMasterUser.UserName;
                    objuser.FullName = objMasterUser.FullName;
                    objuser.UserEmail = objMasterUser.UserEmail;
                    objuser.UserIC = objMasterUser.UserIC;
                    objuser.Department = objMasterUser.Department;
                    objuser.Position = objMasterUser.Position;
                    objuser.UserPhoneNo = objMasterUser.UserPhoneNo;
                    objuser.UserStatus = objMasterUser.UserStatus;
                    objuser.ModifiedBy = objMasterUser.ModifiedBy;
                    objuser.ModifiedTimeStamp = objMasterUser.ModifiedTimeStamp;

                    db.SaveChanges();

                    BPEventLog bpe = new BPEventLog();
                    bpe.Object = "User - Updated";
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
                    if (!string.IsNullOrEmpty(objMasterUser.SecAnswer)) objuser.SecAnswer = Security.Encrypt(objMasterUser.SecAnswer.ToUpper());
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

        public Nullable<int> GetUserID(string username)
        {
            return GetValidUser(username).UserID;
        }

        public void DeleteUsers(string username)
        {
            int? UserId = GetUserID(username);
            if (UserId != null)
            {
                db.MasterUsers.Remove(new MasterUser() { UserID = UserId.GetValueOrDefault() });
            }
        }

    }
}
