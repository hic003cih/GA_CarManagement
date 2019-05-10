using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Linq.SqlClient;
using System.Web.Script.Serialization;
using System.Data.OleDb;
//連接database,需添加以下三行
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Configuration;
using FOX.DAL;
using System.Text;
using System.Web.Services;
using System.IO;
using FOX.COMMON;
using System.Net;

public partial class FTPView : System.Web.UI.Page
{
    string strCon = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString;
    string filePath = "";
 //   string fileExtens = "";
    string fileName = "";
    string msg = "";
    int effectRow = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["FileID"] == null) return;

        filePath = Server.UrlDecode(Request.QueryString["FileID"].ToString().Trim());
        fileName = Server.UrlDecode(Request.QueryString["fileName"].ToString().Trim());
    //    fileExtens = Server.UrlDecode(Request.QueryString["fileExtens"].ToString().Trim());


        //string ftpServerIP = "10.97.51.49";//服务器ip
        //string ftpUserID = "ftp_ks";//用户名
        //string ftpPassword = "fuyao";//密码
        
            string ftpServerIP = "";//服务器ip
            string ftpUserID = "";//用户名
            string ftpPassword = "";//密码
            System.Data.DataTable dt = new DataTable();
            try
            {
                DBAccessProc.setConnStr(strCon);
                dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetFTPInfo_Q", 0, 1, "", "", "{?+?}", out msg, out effectRow);

                if (msg != "")
                {
                    Response.Write("Enotes_PK0338_GetFTPInfo_Q Error:" + msg);
                }

            }
            catch (Exception ex)
            {
                Response.Write("Enotes_PK0338_GetFTPInfo_Q function error:" + ex.Message);
            }

            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ftpServerIP = dr["RoleID"].ToString().Trim();
                    ftpUserID = dr["UserID"].ToString().Trim();
                    ftpPassword = dr["RoleName"].ToString().Trim();
                }
            }


        string uri = string.Format("ftp://{0}/{1}/{2}", ftpServerIP, filePath, fileName);

        try
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            //Enter FTP Server credentials.
            request.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;

            //Fetch the Response and read it into a MemoryStream object.
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            using (MemoryStream stream = new MemoryStream())
            {
                //Download the File.
                response.GetResponseStream().CopyTo(stream);
                fileName = HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8).ToString(); //下載檔案中文名轉碼
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);


                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }
        }
        catch (WebException ex)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('檔案下載有誤，請與系統管理員聯繫!');", true);
            return;
        }
    }

}