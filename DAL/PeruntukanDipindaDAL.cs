﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PeruntukanDipindaDAL
    {
        BudgetPrep db = new BudgetPrep();

        public IQueryable<PeruntukanDipinda> GetAccountCodes()
        {
            //db.Database.ExecuteSqlCommand();

            return db.PeruntukanDipindas.Select(x => x);
        }

        public PeruntukanDipinda GetParentAccountCode(PeruntukanDipinda BudgetAccount)
        {
            return db.PeruntukanDipindas.Where(x => x.BudgetAccount == BudgetAccount.BudgetAccount).FirstOrDefault();
        }

        public bool InsertAccountCode(PeruntukanDipinda objAccountCode)
        {
            try
            {
                //PeruntukanDipinda PA = new PeruntukanDipinda();
                db.PeruntukanDipindas.Add(objAccountCode);
                db.SaveChanges();

                BPEventLog bpe = new BPEventLog();
                bpe.Object = "PeruntukanDipinda";
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
                bpe.Object = "PeruntukanDipinda";
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

        public bool UpdateAccountCode(PeruntukanDipinda objAccountCode)
        {
            PeruntukanDipinda obj = db.PeruntukanDipindas.Where(x => x.BudgetAccount == objAccountCode.BudgetAccount).FirstOrDefault();
            string changes = new EventLogDAL().ObjectDifference(obj, objAccountCode);
            try
            {
                if (obj != null && changes != string.Empty)
                {
                    //obj.AccountCode1 = objAccountCode.AccountCode1;
                    obj.BudgetAccount = objAccountCode.BudgetAccount;
                    obj.Description = objAccountCode.Description;
                    obj.BudgetAccKey = objAccountCode.BudgetAccKey;
                    obj.BudgetLedgerKey = objAccountCode.BudgetLedgerKey;
                    obj.BudgetType = objAccountCode.BudgetType;
                    obj.BudgetYear = objAccountCode.BudgetYear;
                    obj.BudgetAmount = objAccountCode.BudgetAmount;
                    obj.Type = objAccountCode.Type;
                    db.SaveChanges();

                    BPEventLog bpe = new BPEventLog();
                    bpe.Object = "PeruntukanDipinda";
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
                bpe.Object = "PeruntukanDipinda";
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