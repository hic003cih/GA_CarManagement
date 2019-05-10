using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO; 

public partial class TextView : System.Web.UI.Page
{
    string fileFullName = "";
    string fileExtens = "";
    string fileName = "";
 //   string sname="";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["FileID"] == null) return;

        fileFullName = Server.UrlDecode(Request.QueryString["FileID"].ToString().Trim());
     //   sname = System.Web.HttpContext.Current.Server.MapPath("/");
       // fileFullName = sname+fileFullName;
        fileName = Path.GetFileName(fileFullName);

        fileExtens = Path.GetExtension(fileFullName).ToUpper();
        //Response.ClearContent();
        //Response.ClearHeaders();
        if (fileExtens == ".TXT")
        {
            //Response.Write(fileFullName);
            ShowText(fileFullName, fileName, fileExtens);
        }
        else if (fileExtens == ".PDF")
        {
            //Response.Write(fileName);
            ShowPDF(fileFullName, fileName, fileExtens);
        }
        else if (fileExtens == ".DOC")
        {
            //Response.Write(fileName);
            ShowWord2003(fileFullName, fileName, fileExtens);
        }
        else if (fileExtens == ".DOCX")
        {
            //Response.Write(fileName);
            ShowWord2007(fileFullName,fileName, fileExtens);
        }
        else if (fileExtens == ".XLS")
        {
            ShowExcel2003(fileFullName, fileName, fileExtens);
        }
        else if (fileExtens == ".XLSX")
        {
            ShowExcel2007(fileFullName,fileName, fileExtens);
        }
        else if (fileExtens == ".JPEG" || fileExtens == ".JPG" || fileExtens == ".JPE")
        {
            ShowJPG(fileFullName, fileName, fileExtens);
        }
        else if (fileExtens == ".PNG")
        {
            ShowPNG(fileFullName, fileName, fileExtens);
        }
        else if (fileExtens == ".TIFF" || fileExtens == ".TIF")
        {
            ShowTIF(fileFullName, fileName, fileExtens);
        }
        else if (fileExtens == ".BMP")
        {
            ShowBMP(fileFullName, fileName, fileExtens);
        }
        else if (fileExtens == ".GIF")
        {
            ShowGIF(fileFullName, fileName, fileExtens);
        }
        else if (fileExtens == ".ZIP")
        {
            ShowZIP(fileFullName, fileName, fileExtens);
        }
        else if (fileExtens == ".TAR")
        {
            ShowTAR(fileFullName, fileName, fileExtens);
        } 
        else if (fileExtens == ".PPT")
        {
            ShowTAR(fileFullName, fileName, fileExtens);
        }
        //ShowJPG();
    }

    /// <summary>
    /// 在網頁中顯示TEXT文件
    /// </summary>
    /// <param name="fileName"></param>
    private void ShowText(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "text/plain;charset=BIG5";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        Response.End();
    }

    /// <summary>
    /// 在網頁中顯示PDF文件
    /// </summary>
    private void ShowPDF(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "Application/pdf";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        //Response.Write(filePath);
        Response.End();
    }

    /// <summary>
    /// 在網頁中顯示WORD文件
    /// </summary>
    private void ShowWord2003(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/msword";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        Response.End();

    }
    /// <summary>
    /// 在網頁中顯示WORD文件
    /// </summary>
    private void ShowWord2007(string fileFullNamePara, string fileNamePara,string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        Response.End();

    }
    /// <summary>
    /// 在網頁中顯示EXCEL文件
    /// </summary>
    /// <param name="fileName"></param>
    private void ShowExcel2003(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/x-msexcel";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        Response.End();
    }
    /// <summary>
    /// 在網頁中顯示EXCEL文件
    /// </summary>
    /// <param name="fileName"></param>
    private void ShowExcel2007(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        Response.End();
    }

    //private void ShowJPG()
    //{
    //    Response.ClearContent();
    //    Response.ClearHeaders();
    //    Response.ContentType = "Application/jpeg";
    //    string filePath = MapPath("Data.jpg");
    //    Response.WriteFile(filePath);
    //    Response.End();
    //}

    /// <summary>
    /// 在網頁中顯示JPG文件
    /// </summary>
    private void ShowJPG(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "image/JPEG";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        //Response.Write(filePath);
        Response.End();
    }

    /// <summary>
    /// 在網頁中顯示PNG文件
    /// </summary>
    private void ShowPNG(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "image/PNG";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        //Response.Write(filePath);
        Response.End();
    }

    /// <summary>
    /// 在網頁中顯示TIF文件
    /// </summary>
    private void ShowTIF(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "image/tiff";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        //Response.Write(filePath);
        Response.End();
    }

    /// <summary>
    /// 在網頁中顯示BMP文件
    /// </summary>
    private void ShowBMP(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "image/BMP";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        //Response.Write(filePath);
        Response.End();
    }

    /// <summary>
    /// 在網頁中顯示TIF文件
    /// </summary>
    private void ShowGIF(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "image/GIF";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        //Response.Write(filePath);
        Response.End();
    }

    /// <summary>
    /// 在網頁中顯示ZIP文件
    /// </summary>
    /// <param name="fileName"></param>
    private void ShowZIP(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/zip;charset=BIG5";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        Response.End();
    }

    /// <summary>
    /// 在網頁中顯示TAR文件
    /// </summary>
    /// <param name="fileName"></param>
    private void ShowTAR(string fileFullNamePara, string fileNamePara, string fileExtensPara)
    {
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/x-tar;charset=BIG5";
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(fileNamePara, System.Text.UTF8Encoding.UTF8)));
        string filePath = Server.MapPath(fileFullNamePara);
        Response.WriteFile(filePath);
        Response.End();
    }
}