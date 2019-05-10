using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Configuration;
using FOX.DAL;
using System.Text;
using System.Web.Services;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using FOX.COMMON;
using System.Net;
/// <summary>
/// CountAccount 的摘要描述
/// </summary>
public class FileUploadFTP
{
    string msg = "";
    int effectRow = 0;
    int rowcount = 0;
    string strCon = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString;
    public void Upload(string filename, string filePath, string tmp_id, string sysid, string tableid, string userID, string attfieldname)     //filename 为本地文件的绝对路径,serverDir为服务器上的目录
    {
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
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetFTPInfo_GA", 0, 1, "", "", "{?+?}", out msg, out effectRow);

            if (msg != "")
            {
            //    Response.Write("Enotes_PK0338_GetFTPInfo_Q Error:" + msg);
            }

        }
        catch (Exception ex)
        {
        //    Response.Write("Enotes_PK0338_GetFTPInfo_Q function error:" + ex.Message);
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

        MakeFTPDir(ftpServerIP, filePath + "/", ftpUserID, ftpPassword, "");   //建立資料夾MakeFTPDir(ftp各AP資料夾名,儲存路徑,ftp帳號,ftp密碼,"")

        FileInfo fileInf = new FileInfo(filename);

        filename = checkFile(tmp_id, sysid, tableid, attfieldname, fileInf.Name);   //確認是否需要改名

        string uri = string.Format("ftp://{0}/{1}/{2}", ftpServerIP, filePath, filename);
        FtpWebRequest reqFTP;
        // 根据uri创建FtpWebRequest对象 
        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
        // ftp用户名和密码
        reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
        // 默认为true，连接不会被关闭
        // 在一个命令之后被执行
        reqFTP.KeepAlive = false;
        // 指定执行什么命令
        reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
        // 指定数据传输类型
        reqFTP.UseBinary = true;
        // 上传文件时通知服务器文件的大小
        reqFTP.ContentLength = fileInf.Length;
        // 缓冲大小设置为2kb
        int buffLength = 2048;
        byte[] buff = new byte[buffLength];
        int contentLen;
        // 打开一个文件流 (System.IO.FileStream) 去读上传的文件
        FileStream fs = fileInf.OpenRead();
        try
        {
            // 把上传的文件写入流
            Stream strm = reqFTP.GetRequestStream();
            // 每次读文件流的2kb
            contentLen = fs.Read(buff, 0, buffLength);
            // 流内容没有结束
            while (contentLen != 0)
            {
                // 把内容从file stream 写入 upload stream
                strm.Write(buff, 0, contentLen);
                contentLen = fs.Read(buff, 0, buffLength);
            }
            // 关闭两个流
            strm.Close();
            fs.Close();
        }
        catch (Exception ex)
        {
            // MessageBox.Show(ex.Message, "Upload Error");
        //    Response.Write("DN_PK0338_ftpFileUpload Error：" + ex.Message);
        }

        StringBuilder sbval = new StringBuilder();
        try
        {

            sbval.Append(tmp_id).Append("{?+?}")
                .Append(sysid).Append("{?+?}")
                .Append(tableid).Append("{?+?}")
                .Append(filePath).Append("{?+?}")
                .Append(filename).Append("{?+?}")
                    .Append(userID).Append("{?+?}")
                    .Append(userID).Append("{?+?}")
                    .Append(attfieldname).Append("{?+?}");
            sbval.ToString();

            DBAccessProc.ExecuteSQL("Enotes_PK0338_InsertUploadFiles", " ", sbval.ToString(), "{?+?}", out msg, out rowcount, out effectRow);
            sbval.Clear();

            if (msg != "")
            {
            //    Response.Write("Enotes_PK0338_InsertUploadFilestoSQL Error:" + msg);
                return;
            }

        }
        catch (Exception ex)
        {
         //   Response.Write("Enotes_PK0338_InsertUploadFilestoSQL function error:" + ex.Message);
        }



    }

    public string checkFile(string fid, string sysid, string tableid, string attfieldname, string filename)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();

        try
        {
            sbval.Append(fid).Append("{?+?}")
            .Append(sysid).Append("{?+?}")
            .Append(tableid).Append("{?+?}")
            .Append(filename).Append("{?+?}")
                .Append(attfieldname).Append("{?+?}");
            sbval.ToString();
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_CheckAttFile", 0, 1, "", sbval.ToString(), "{?+?}", out msg, out effectRow);

            if (msg != "")
            {
             //   Response.Write("Enotes_PK0338_CheckAttFile Error:" + msg);
            }

        }
        catch (Exception ex)
        {
         //   Response.Write("Enotes_PK0338_CheckAttFile function error:" + ex.Message);
            return filename;
        }

        if (dt.Rows.Count != 0)
        {
            string[] nocollection = filename.Split('.');
            filename = nocollection[0];
            filename = filename + "_01." + nocollection[1];
        }

        return filename;
    }

    //確認ftp是否有相對應的資料夾
    public static void MakeFTPDir(string ftpAddress, string pathToCreate, string login, string password, string ftpProxy = null)
    {
        FtpWebRequest reqFTP = null;
        Stream ftpStream = null;

        string[] subDirs = pathToCreate.Split('/');

        string currentDir = string.Format("ftp://{0}", ftpAddress);

        foreach (string subDir in subDirs)
        {
            try
            {
                currentDir = currentDir + "/" + subDir;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(currentDir);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(login, password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
            }
        }
    }
    public string checkfile(string formtype, string node)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        string returnstr = "";
        try
        {
            sbval.Append(formtype.Trim()).Append("{?,?}")
                .Append(node).Append("{?,?}")
                ;
            sbval.ToString();
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetCheckFileSql", 0, 1, "", sbval.ToString(), "{?,?}", out msg, out effectRow);
            sbval.Clear();
            if ("" != msg)
            {
                return "";
            }

            if (dt.Rows.Count != 0)
            {
                returnstr = dt.Rows[0][0].ToString();
                if (returnstr == null)
                {
                    returnstr = "";
                }
            }
            return returnstr;

        }
        catch (Exception ex)
        {
            sbval.Clear();
        }
        return returnstr;
    }


}
