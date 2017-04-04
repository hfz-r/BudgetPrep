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

        public USER GetValidUser(string UserName, string Password)
        {
            try
            {
                string encstr = Security.Encrypt(Password);
                return db.USERS.Where(x => x.UserName == UserName && x.UserPassword == encstr).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertUsers(USER _USER)
        {
            try
            {
                if (db.USERS.Where(x => x.UserName.Equals(_USER.UserName)).Count() > 0)
                {
                    return false;
                }
                else
                {
                    _USER.UserPassword = Security.Encrypt(_USER.UserPassword);

                    db.USERS.Add(_USER);
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
