﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Configuration;
using System.Data;
using BP.Classes;
using DAL;
using Newtonsoft.Json;
using System.Web.UI.WebControls;

namespace BP
{
    /// <summary>
    /// Summary description for FileUploadHandler
    /// </summary>
    public class FileUploadHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            List<object> ListExcluded = new List<object>();

            try
            {
                HttpPostedFile objFile = context.Request.Files["upload"];
                if (objFile != null)
                {
                    string FolderPath = WebConfigurationManager.AppSettings["UploadFolderPath"];

                    if (!Directory.Exists(FolderPath))
                        Directory.CreateDirectory(FolderPath);

                    string FileName = DateTime.Now.ToString("dd'-'MM'-'yyyy HHmmss") + "_" + System.IO.Path.GetFileName(objFile.FileName);
                    string FullFileName = System.IO.Path.Combine(FolderPath, FileName);
                    objFile.SaveAs(FullFileName);

                    if (Path.GetExtension(FileName) == ".csv")
                    {
                        DataSet ds = new ReportHelper().ExcelToDataSet(FullFileName, Path.GetExtension(FileName));

                        string source = context.Request.QueryString["source"];
                        if (source == "AccountCode")
                        {
                            AccountCodeFileUpload(ds, ref ListExcluded);
                        }
                        else if (source == "ServicesGroup")
                        {
                            ServicesGroupFileUpload(ds, ref ListExcluded);
                        }
                        else if (source == "SegmenDetails")
                        {
                            SegmentDetailsFileUpload(ds, ref ListExcluded);
                        }
                        else if (source == "BudgetMengurus")
                        {
                            BudgetMengurusFileUpload(ds, ref ListExcluded);
                        }
                    }
                }

                objFile = null;
            }
            catch (Exception ex)
            {
                object ReturnObj;
                ListExcluded.Add(ReturnObj = new
                {
                    status = "Error",
                    message = "An error occurred - " + ex.Message
                });
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(ListExcluded, Formatting.Indented));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void AccountCodeFileUpload(DataSet ds, ref List<object> ListExcluded)
        {
            object ReturnObj = new object();

            List<string> lstErrors = new List<string>();
            DataTable dt = new DataTable();

            dt = new ReportHelper().Validate<AccountCodeImport>(ds, "", ref lstErrors);

            if (lstErrors.Count == 0)
            {
                List<AccountCode> AccountCodesData = (List<AccountCode>)HttpContext.Current.Session["AccountCodesData"];
                List<AccountCode> UploadedData = new ReportHelper().DataTableToList<AccountCodeImport>(dt).Select(x => new AccountCode() 
                {
                    AccountCode1 = Convert.ChangeType(x.AccountCode, typeof(string)).ToString(),
                    AccountDesc = x.AccountDesc,
                    Status = ((x.Status == "Active") ? "A" : "D"),
                    ParentAccountCode = ((Convert.ChangeType(x.UpperLevel, typeof(string)).ToString() == "0") ? "" :
                        Convert.ChangeType(x.UpperLevel, typeof(string)).ToString())
                }).ToList();

                if (UploadedData.Count > 0)
                {
                    foreach (AccountCode item in UploadedData)
                    {
                        AccountCode objAccountCode = new AccountCode();

                        if (AccountCodesData.Where(x => x.AccountCode1 == item.AccountCode1).Count() > 0)
                        {
                            ReturnObj = new
                            {
                                status = "Error",
                                message = "Account Code **" + item.AccountCode1 + "** already exists. It`ll be excluded."
                            };
                        }
                        else
                        {
                            objAccountCode.AccountCode1 = item.AccountCode1;
                            objAccountCode.AccountDesc = item.AccountDesc;
                            objAccountCode.Status = item.Status;
                            objAccountCode.ParentAccountCode = item.ParentAccountCode;
                            objAccountCode.CreatedBy = new UsersDAL().GetValidUser(HttpContext.Current.User.Identity.Name).UserID;
                            objAccountCode.CreatedTimeStamp = DateTime.Now;
                            objAccountCode.ModifiedBy = new UsersDAL().GetValidUser(HttpContext.Current.User.Identity.Name).UserID;
                            objAccountCode.ModifiedTimeStamp = DateTime.Now;

                            if (new AccountCodeDAL().InsertAccountCode(objAccountCode))
                            {
                                ReturnObj = new
                                {
                                    status = "Success",
                                    message = "Account Code **" + item.AccountCode1 + "** uploaded successfully"
                                };
                            }
                            else
                            {
                                ReturnObj = new
                                {
                                    status = "Failure",
                                    message = "An error occurred while uploading Account Code"
                                };
                            }
                        }

                        ListExcluded.Add(ReturnObj);
                    }
                }
            }
            else
            {
                ListExcluded.Add(ReturnObj = new
                {
                    status = "Error",
                    message = lstErrors.Aggregate((a, b) => a + "<br/>" + b)
                });
            }
        }

        public void ServicesGroupFileUpload(DataSet ds, ref List<object> ListExcluded)
        {
            object ReturnObj = new object();

            List<string> lstErrors = new List<string>();
            DataTable dt = new DataTable();

            dt = new ReportHelper().Validate<ServiceCodeImport>(ds, "", ref lstErrors);

            if (lstErrors.Count == 0)
            {
                List<GroupPerjawatan> ServiceGroupData = (List<GroupPerjawatan>)HttpContext.Current.Session["GroupPerjawatansData"];
                List<GroupPerjawatan> UploadedData = new ReportHelper().DataTableToList<ServiceCodeImport>(dt).Select(x => new GroupPerjawatan()
                {
                    GroupPerjawatanCode = Convert.ChangeType(x.ServiceCode, typeof(string)).ToString(),
                    GroupPerjawatanDesc = x.ServiceDesc,
                    Status = ((x.Status == "Active") ? "A" : "D"),
                    ParentGroupPerjawatanID = ((Convert.ChangeType(x.UpperLevel, typeof(string)).ToString() == "0") ? "" :
                        Convert.ChangeType(x.UpperLevel, typeof(string)).ToString())
                }).ToList();

                if (UploadedData.Count > 0)
                {
                    foreach (GroupPerjawatan item in UploadedData)
                    {
                        GroupPerjawatan objGroupPerjawatan = new GroupPerjawatan();

                        if (ServiceGroupData.Where(x => x.GroupPerjawatanCode == item.GroupPerjawatanCode).Count() > 0)
                        {
                            ReturnObj = new
                            {
                                status = "Error",
                                message = "Service Group **" + item.GroupPerjawatanCode + "** already exists. It`ll be excluded."
                            };
                        }
                        else
                        {
                            objGroupPerjawatan.GroupPerjawatanCode = item.GroupPerjawatanCode;
                            objGroupPerjawatan.GroupPerjawatanDesc = item.GroupPerjawatanDesc;
                            objGroupPerjawatan.Status = item.Status;
                            objGroupPerjawatan.ParentGroupPerjawatanID = item.ParentGroupPerjawatanID;
                            objGroupPerjawatan.CreatedBy = new UsersDAL().GetValidUser(HttpContext.Current.User.Identity.Name).UserID;
                            objGroupPerjawatan.CreatedTimeStamp = DateTime.Now;
                            objGroupPerjawatan.ModifiedBy = new UsersDAL().GetValidUser(HttpContext.Current.User.Identity.Name).UserID;
                            objGroupPerjawatan.ModifiedTimeStamp = DateTime.Now;

                            if (new GroupPerjawatanDAL().InsertGroupPerjawatan(objGroupPerjawatan))
                            {
                                ReturnObj = new
					            {
						            status = "Success",
                                    message = "Service Group **" + item.GroupPerjawatanCode + "** uploaded successfully"
					            };
                            }
                            else
                            {
                                ReturnObj = new
                                {
                                    status = "Failure",
                                    message = "An error occurred while uploading Service Group"
                                };
                            }
                        }

                        ListExcluded.Add(ReturnObj);
                    }
                }
            }
            else
            {
                ListExcluded.Add(ReturnObj = new
                {
                    status = "Error",
                    message = lstErrors.Aggregate((a, b) => a + "<br/>" + b)
                });
            }
        }

        public void SegmentDetailsFileUpload(DataSet ds, ref List<object> ListExcluded)
        {
            object ReturnObj = new object();

            List<string> lstErrors = new List<string>();
            DataTable dt = new DataTable();

            dt = new ReportHelper().Validate<SegmentDetailImport>(ds, ((Segment)HttpContext.Current.Session["SelectedSegment"]).ShapeFormat, ref lstErrors);

            if (lstErrors.Count == 0)
            {
                List<SegmentDetail> SegmentDetailData = (List<SegmentDetail>)HttpContext.Current.Session["SegmentDetailsData"];
                List<SegmentDetail> UploadedData = new ReportHelper().DataTableToList<SegmentDetailImport>(dt).Select(x => new SegmentDetail()
                {
                    SegmentID = ((Segment)HttpContext.Current.Session["SelectedSegment"]).SegmentID,
					DetailCode = x.DetailCode,
					DetailDesc = x.DetailDesc,
					Status = ((x.Status == "Active") ? "A" : "D")
                }).ToList();

                if (UploadedData.Count > 0)
                {
                    foreach (SegmentDetail item in UploadedData)
                    {
                        SegmentDetail objSegmentDetail = new SegmentDetail();

                        if (SegmentDetailData.Where(x => x.DetailCode == item.DetailCode).Count() > 0)
                        {
                            ReturnObj = new
                            {
                                status = "Error",
                                message = "Segment Details **" + item.DetailCode + "** already exists. It`ll be excluded."
                            };
                        }
                        else
                        {
                            objSegmentDetail.SegmentID = item.SegmentID;
                            objSegmentDetail.DetailCode = item.DetailCode;
                            objSegmentDetail.DetailDesc = item.DetailDesc;
                            objSegmentDetail.Status = item.Status;
                            objSegmentDetail.ParentDetailID = 0;
                            objSegmentDetail.CreatedBy = new UsersDAL().GetValidUser(HttpContext.Current.User.Identity.Name).UserID;
                            objSegmentDetail.CreatedTimeStamp = DateTime.Now;
                            objSegmentDetail.ModifiedBy = new UsersDAL().GetValidUser(HttpContext.Current.User.Identity.Name).UserID;
                            objSegmentDetail.ModifiedTimeStamp = DateTime.Now;

                            if (new SegmentDetailsDAL().InsertSegmentDetail(objSegmentDetail))
                            {
                                ReturnObj = new
                                {
                                    status = "Success",
                                    message = "Segment Details **" + item.DetailCode + "** uploaded successfully"
                                };
                            }
                            else
                            {
                                ReturnObj = new
                                {
                                    status = "Failure",
                                    message = "An error occurred while uploading Segment Details"
                                };
                            }
                        }

                        ListExcluded.Add(ReturnObj);
                    }
                }
            }
            else
            {
                ListExcluded.Add(ReturnObj = new
                {
                    status = "Error",
                    message = lstErrors.Aggregate((a, b) => a + "<br/>" + b)
                });
            }
        }

        public void BudgetMengurusFileUpload(DataSet ds, ref List<object> ListExcluded)
        {
            List<string> lstErrors = new List<string>();
            DataTable dt = new DataTable();

            GridView gvAccountCodes = (GridView)HttpContext.Current.Session["gvAccountCodes"];
            dt = new ReportHelper().ValidateBudgetImport<BudgetMengurusSetup>(ds, gvAccountCodes, ref lstErrors);

            if (lstErrors.Count == 0)
            {
                List<AccountCode> AccountCodesData = ((List<AccountCode>)HttpContext.Current.Session["AccountCodesData"])
                    .Where(x => !String.IsNullOrEmpty(x.ParentAccountCode)).ToList();

                foreach (DataRow row in dt.Rows)
                {
                    List<object> MessageModel = new List<object>();

                    if (AccountCodesData.Where(x => x.AccountCode1 == Convert.ToString(row["AccountCode"])).Select(y => y.AccountCode1).Count() > 0)
                    {
                        new BudgetMengurusSetup().MatchingData(row, ref MessageModel);
                        ListExcluded.AddRange(MessageModel);
                    }
                    else
                    {
                        ListExcluded.Add(new 
                        {
                            status = "Error",
                            message = "Account Code not match. Please check and re-try."
                        });
                    }
                }
            }
            else
            {
                ListExcluded.Add(new
                {
                    status = "Error",
                    message = lstErrors.Aggregate((a, b) => a + "<br/>" + b)
                });
            }
        }
    }
}