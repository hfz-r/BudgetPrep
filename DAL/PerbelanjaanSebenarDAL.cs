using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PerbelanjaanSebenarDAL
    {
        BudgetPrep db = new BudgetPrep();

        public IQueryable<PerbelanjaanSebenar> GetAccountCodes()
        {
            //db.Database.ExecuteSqlCommand();

            return db.PerbelanjaanSebenars.Select(x => x);
        }

        public PerbelanjaanSebenar GetParentAccountCode(PerbelanjaanSebenar BudgetAccount)
        {
            return db.PerbelanjaanSebenars.Where(x => x.BudgetAccount == BudgetAccount.BudgetAccount).FirstOrDefault();
        }

        public bool InsertAccountCode(PerbelanjaanSebenar objAccountCode)
        {
            try
            {
                List<AccountCode> data = new AccountCodeDAL().GetAccountCodes().ToList();
                objAccountCode.ParentAccountCode = data.Where(x => x.AccountCode1 == objAccountCode.BudgetAccount.Substring(17, 6)).Select(y => y.ParentAccountCode).FirstOrDefault();
                db.PerbelanjaanSebenars.Add(objAccountCode);
                db.SaveChanges();

                BPEventLog bpe = new BPEventLog();
                bpe.Object = "PerbelanjaanSebenar";
                bpe.ObjectName = objAccountCode.BudgetAccount;
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Success";
                bpe.Status = "A";
                //bpe.CreatedBy = objAccountCode.CreatedBy;
                //bpe.CreatedTimeStamp = objAccountCode.CreatedTimeStamp;
                new EventLogDAL().AddEventLog(bpe);
                return true;
            }
            catch (Exception ex)
            {
                BPEventLog bpe = new BPEventLog();
                bpe.Object = "PerbelanjaanSebenar";
                bpe.ObjectName = objAccountCode.BudgetAccount;
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Failure";
                bpe.Status = "A";
                //bpe.CreatedBy = objAccountCode.CreatedBy;
                //bpe.CreatedTimeStamp = objAccountCode.CreatedTimeStamp;
                new EventLogDAL().AddEventLog(bpe);
                return true;

                throw ex;
            }
        }

        public bool UpdateAccountCode(PerbelanjaanSebenar objAccountCode)
        {
            PerbelanjaanSebenar obj = db.PerbelanjaanSebenars.Where(x => x.BudgetAccount == objAccountCode.BudgetAccount).FirstOrDefault();
            string changes = new EventLogDAL().ObjectDifference(obj, objAccountCode);
            try
            {
                if (obj != null && changes != string.Empty)
                {
                    //obj.AccountCode1 = objAccountCode.AccountCode1;
                    List<AccountCode> data = new AccountCodeDAL().GetAccountCodes().ToList();
                    obj.ParentAccountCode = data.Where(x => x.AccountCode1 == objAccountCode.BudgetAccount.Substring(17, 6)).Select(y => y.ParentAccountCode).FirstOrDefault();
                    obj.BudgetAccount = objAccountCode.BudgetAccount;
                    obj.Description = objAccountCode.Description;
                    obj.BudgetAmount = objAccountCode.BudgetAmount;
                    obj.BudgetYear = objAccountCode.BudgetYear;
                    db.SaveChanges();

                    BPEventLog bpe = new BPEventLog();
                    bpe.Object = "PerbelanjaanSebenar";
                    bpe.ObjectName = objAccountCode.BudgetAccount;
                    bpe.ObjectChanges = string.Empty;
                    bpe.EventMassage = "Success";
                    bpe.Status = "A";
                    //bpe.CreatedBy = objAccountCode.CreatedBy;
                    //bpe.CreatedTimeStamp = objAccountCode.CreatedTimeStamp;
                    new EventLogDAL().AddEventLog(bpe);
                }
                return true;
            }
            catch (Exception ex)
            {
                BPEventLog bpe = new BPEventLog();
                bpe.Object = "PerbelanjaanSebenar";
                bpe.ObjectName = objAccountCode.BudgetAccount;
                bpe.ObjectChanges = string.Empty;
                bpe.EventMassage = "Failure";
                bpe.Status = "A";
                //bpe.CreatedBy = objAccountCode.CreatedBy;
                //bpe.CreatedTimeStamp = objAccountCode.CreatedTimeStamp;
                new EventLogDAL().AddEventLog(bpe);

                throw ex;
            }
        }
    }
}
