﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BP.Classes;
using OBSecurity;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web.Security;

namespace BP
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.HttpMethod == "POST")
                {
                    OnSubmittedFormSave(sender,e);
                }

                GetProfileData();
            }
            else
            {
                TabName.Value = Request.Form[TabName.UniqueID];
            }
        }

        protected void GetProfileData()
        {
            try
            {
                MasterUser user = new UsersDAL().GetValidUser(User.Identity.Name);

                if (user.UUID != null)
                {
                    Session["ProfileData"] = user;
                    PopulateProfile();
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void PopulateProfile()
        {
            MasterUser AuthUser = (MasterUser)Session["ProfileData"];

            string src = string.Empty;
            if (AuthUser.Image != null)
            {
                src = "~/ShowImage.ashx?UserId=" + AuthUser.UserID;
                
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopulateImage", "PopulateImage('" + src + "');", true);

            username.Value = AuthUser.UserName.Trim();
            fullname.Value = (!string.IsNullOrEmpty(AuthUser.FullName) ? AuthUser.FullName.Trim() : string.Empty);
            birthdate.Value = AuthUser.BirthDate.HasValue ? AuthUser.BirthDate.Value.ToString("dd-MM-yyyy") : DBNull.Value.ToString();
               
            if (AuthUser.Gender == "M")
            {
                rbMale.Checked = true;
            }
            else if (AuthUser.Gender == "F")
            {
                rbFemale.Checked = true;
            }

            comment.Value = (!string.IsNullOrEmpty(AuthUser.Comment) ? AuthUser.Comment.Trim() : string.Empty);
            email.Value = AuthUser.UserEmail.Trim();
            website.Value = (!string.IsNullOrEmpty(AuthUser.Website) ? AuthUser.Website.Trim() : string.Empty);
            phone.Value = (!string.IsNullOrEmpty(AuthUser.UserPhoneNo) ? AuthUser.UserPhoneNo.Trim() : string.Empty);
            question.InnerText = (!string.IsNullOrEmpty(AuthUser.SecQuestion) ? AuthUser.SecQuestion.Trim() : string.Empty);
            answer.InnerText = (AuthUser.SecAnswer != null && AuthUser.SecAnswer != string.Empty) ? Security.Decrypt(AuthUser.SecAnswer.Trim()) : string.Empty;
        }

        protected void OnSubmittedFormSave(object sender, EventArgs e)
        {
            List<dynamic> ReturnObj = new List<dynamic>();

            try
            {
                Byte[] imgByte = null;
                NameValueCollection nvc = Request.Form;

                MasterUser objMasterUser = new MasterUser();
                objMasterUser.UserName = nvc["ctl00$MainContent$username"];

                MembershipUser _User = Membership.GetUser(objMasterUser.UserName);
                if (_User == null)
                {
                    throw new Exception("Username " + HttpUtility.HtmlEncode(objMasterUser.UserName) + " not found. Please check the value and re-enter.");
                }
                else
                {
                    MasterUser _MasterUser = new UsersDAL().GetValidUser(_User.UserName);
                    if (_MasterUser.UUID == (Guid)_User.ProviderUserKey)
                    {
                        HttpPostedFile File = Request.Files["ctl00$MainContent$imgUpload"];
                        if (File != null && File.ContentLength > 0)
                        {
                            imgByte = new Byte[File.ContentLength];
                            File.InputStream.Read(imgByte, 0, File.ContentLength);

                            objMasterUser.Image = imgByte;
                        }
                        else
                        {
                            if (_MasterUser.Image != null)
                            {
                                objMasterUser.Image = _MasterUser.Image;
                            }
                        }

                        objMasterUser.UUID = (Guid)_User.ProviderUserKey;
                        objMasterUser.FullName = (!string.IsNullOrEmpty(nvc["ctl00$MainContent$fullname"]) ? nvc["ctl00$MainContent$fullname"].Trim() : string.Empty);

                        string birthDate = (!string.IsNullOrEmpty(nvc["ctl00$MainContent$birthdate"]) ? nvc["ctl00$MainContent$birthdate"].Trim() : string.Empty);
                        if (birthDate != "")
                        {
                            objMasterUser.BirthDate = DateTime.ParseExact(birthDate, "dd-MM-yyyy", null);
                        }
                        else
                        {
                            objMasterUser.BirthDate = null;
                        }

                        if (!string.IsNullOrEmpty(nvc["ctl00$MainContent$Gender"]))
                        {
                            string selectedGender = nvc["ctl00$MainContent$Gender"].ToString();
                            objMasterUser.Gender = (selectedGender == "male" ? "M" : "F");
                        }

                        objMasterUser.Comment = (!string.IsNullOrEmpty(nvc["ctl00$MainContent$comment"]) ? nvc["ctl00$MainContent$comment"].Trim() : string.Empty);
                        objMasterUser.Website = (!string.IsNullOrEmpty(nvc["ctl00$MainContent$website"]) ? nvc["ctl00$MainContent$website"].Trim() : string.Empty);
                        objMasterUser.UserPhoneNo = nvc["ctl00$MainContent$phone"];

                        //email:new=update
                        if (_User.Email.Trim() != nvc["ctl00$MainContent$email"].Trim())
                        {
                            _User.Email = nvc["ctl00$MainContent$email"].Trim();
                            Membership.UpdateUser(_User);

                            objMasterUser.UserEmail = _User.Email;
                        }

                        //password:notnull=change()
                        if (!string.IsNullOrEmpty(nvc["ctl00$MainContent$oldpassword"]))
                        {
                            string oldpassword = string.Empty; 
                            string newpassword = string.Empty;

                            oldpassword = nvc["ctl00$MainContent$oldpassword"].Trim();

                            if (!string.IsNullOrEmpty(nvc["ctl00$MainContent$newpassword"]) && !string.IsNullOrEmpty(nvc["ctl00$MainContent$confirmpassword"]))
                            {
                                newpassword = nvc["ctl00$MainContent$newpassword"].Trim();
                            }
                            else
                            {
                                throw new Exception("A password-related error has occured. Please try again.");
                            }

                            if (_User.ChangePassword(oldpassword, newpassword))
                            {
                                objMasterUser.UserPassword = newpassword;

                                bool mail = MailHelper.SendMail(new MasterUser() 
                                { 
                                    UserEmail = _User.Email,
                                    FullName = objMasterUser.FullName,
                                    UserName = objMasterUser.UserName
                                }, 
                                newpassword);

                                ReturnObj.Add(new
                                {
                                    source = "Password Updated",
                                    message = "Password successfully updated. Your new password will be sent to your email id : "
                                        + new Helper().EmailClipper(_User.Email)
                                });
                            }
                            else
                            {
                                throw new Exception("A password-related error has occured. Please check your old password and Try Again!");
                            }
                        }

                        //sec. question/answer:notnull=change()
                        if ((_MasterUser.SecQuestion.Trim() != nvc["ctl00$MainContent$question"].Trim()) ||
                            (Security.Decrypt(_MasterUser.SecAnswer.Trim()) != nvc["ctl00$MainContent$answer"].Trim()))
                        {
                            string question = string.Empty;
                            string answer = string.Empty;

                            if (!string.IsNullOrEmpty(nvc["ctl00$MainContent$question"]) && !string.IsNullOrEmpty(nvc["ctl00$MainContent$answer"]))
                            {
                                question = nvc["ctl00$MainContent$question"].Trim();
                                answer = nvc["ctl00$MainContent$answer"].Trim();
                            }
                            else
                            {
                                throw new Exception("A security-related error has occured. Please try again.");
                            }

                            if (_User.ChangePasswordQuestionAndAnswer(_User.GetPassword(Security.Decrypt(_MasterUser.SecAnswer.Trim())), question, answer))
                            {
                                objMasterUser.SecQuestion = question;
                                objMasterUser.SecAnswer = answer;

                                ReturnObj.Add(new
                                {
                                    source = "Security Details Updated",
                                    message = "Security details (question/answer) successfully updated."
                                });
                            }   
                        }

                        objMasterUser.ModifiedBy = new UsersDAL().GetValidUser(HttpContext.Current.User.Identity.Name).UserID;
                        objMasterUser.ModifiedTimeStamp = DateTime.Now;

                        if (new UsersDAL().UpdateProfileUser(objMasterUser))
                        {
                            ReturnObj.Add(new
                            {
                                source = "Profile Updated",
                                message = "Profile successfully updated."
                            });

                            string json = JsonConvert.SerializeObject(ReturnObj, Formatting.Indented);
                            string script = "var data = " + json + ";";
                            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "dataVar", script, true);

                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "output", "ShowOutput('" + _MasterUser.UserID + "');", true);
                        }
                        else
                        {
                            throw new Exception("An error occurred while updating user profile");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }
    }
}