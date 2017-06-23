<%@ WebHandler Language="C#" Class="ShowImage" %>

using System;
using System.Configuration;
using System.Web;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using DAL;

public class ShowImage : IHttpHandler 
{
    public void ProcessRequest(HttpContext context)
    {
       Int32 UserId;
       if (context.Request.QueryString["UserId"] != null)
           UserId = Convert.ToInt32(context.Request.QueryString["UserId"]);
       else
            throw new ArgumentException("No parameter specified");

       context.Response.ContentType = "image/jpeg";
       Stream strm = ShowEmpImage(UserId);
<<<<<<< HEAD

       if (strm != null)
       {
           byte[] buffer = new byte[4096];
           int byteSeq = strm.Read(buffer, 0, 4096);

           while (byteSeq > 0)
           {
               context.Response.OutputStream.Write(buffer, 0, byteSeq);
               byteSeq = strm.Read(buffer, 0, 4096);
           }
=======
       byte[] buffer = new byte[4096];
       int byteSeq = strm.Read(buffer, 0, 4096);

       while (byteSeq > 0)
       {
           context.Response.OutputStream.Write(buffer, 0, byteSeq);
           byteSeq = strm.Read(buffer, 0, 4096);
>>>>>>> fa2a2893ae1d7e783d8591f454ef428f3a40756b
       }        
       //context.Response.BinaryWrite(buffer);
    }

    public Stream ShowEmpImage(int UserId)
    {
        object UserImg = new UsersDAL().GetUserDataByID(UserId).Image;
        
        try
        {
            return new MemoryStream((byte[])UserImg);
        }
        catch
        {
            return null;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }


}