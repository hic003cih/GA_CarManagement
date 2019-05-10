using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FOX.DAL;
using Base_Data;
using FITI_EPS;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Text;
using FOX.COMMON;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Net.Mail;
using System.Collections;
using System.Reflection;


public partial class AutoLogin : System.Web.UI.Page
{
    string userID = "";
    string formID = "";
    string formNo = "";
    string formType = "";
    string formTypeName = "";
    string status = "";
    string sysid = "006";
    string tableid = "01";

    //定義存放執行sql時相關變量
    string msg = "";
    int rowCount = 0;
    int effectRow = 0;

    //用于證書
    string resultsTrue = "";
    string CAstaffNo = "";
    string staffNo = "";

    //定義用于存放連接字串
    string strCon = "";
    EIP.Framework.Security objSecurity = new EIP.Framework.Security();      //解密
    Base_DataDataContext dc = new Base_DataDataContext(ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString());
    string UserInfoStrCon = ConfigurationManager.ConnectionStrings["UserInfoConnectionString"].ConnectionString;
    FITI_EPSDataContext db = new FITI_EPSDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        strCon = ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString();
        MyLibs.encrypt.Base64 base64 = new MyLibs.encrypt.Base64();

        string getPrivResult = "";
        string action = "";
        int FormInFlowYN=-1;
        string url = Request.Url.ToString();
        if (Request.QueryString["uid"] == null || Request.QueryString["uid"] == "")
        {
            if (Session["UserID"] == null)
            {

                Response.Redirect("http://10.56.69.77/GA_CarManagement");
            }
            else
            {
                userID = Session["UserID"].ToString();
            }
            
            getPrivResult = getPriv(userID);
            Response.Redirect("../FormList.aspx");     //新登入 
        }
        else
        {
            
                userID = base64.base64decode(Request.QueryString["uid"].ToString().Trim());//用戶ID

                if (Request.QueryString["fid"] != null && Request.QueryString["fid"] != "")
                { formID = base64.base64decode(Request.QueryString["fid"].ToString().Trim()); }//表單ID

                if (Request.QueryString["action"] != null && Request.QueryString["action"] != "")
                { action = Request.QueryString["action"].ToString().Trim(); }//登錄入口或資料來源


                if (Request.QueryString["ftype"] != null && Request.QueryString["ftype"] != "")
                { formType = Request.QueryString["ftype"].ToString().Trim(); }//表單ID

                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                { status = Request.QueryString["status"].ToString().Trim(); }//表單status

                 string tmp_page = "";
                if (formType.IndexOf("FormCarApply_") > -1)
                {
                    tmp_page = "FormCarApply.aspx";
                }



            //checkCA(userID);
            
           FormInFlowYN = GetEFormInFlowYN(formID, userID, formType);
        
            getPrivResult = getPriv(userID);
            if (getPrivResult == "GET_PRIV_OK")
            {

                if (formID == "")
                {
                    Response.Redirect("../FormList.aspx");
                }
                if (FormInFlowYN == 1)
                {
                    getEFormInFlowInfo(formID, userID, formType); 

             //        Response.Redirect("../" + tmp_page + "?formID=" + formID + "&formNo=" + formNo + "&formType=" + formType + "&formTypeName=" + formTypeName + "&status=" + status + "&uid=" + userID);
                    Response.Redirect("../" + tmp_page + "?formID=" + base64.base64encode(formID) + "&formNo=" + base64.base64encode(formNo) + "&formType=" + base64.base64encode(formType) + "&uid=" + base64.base64encode(userID));
                }
                else if (FormInFlowYN == 0)
                {
                    getEFormNotInFlowInfo(formID, formType);

            //        Response.Redirect("../" + tmp_page + "?formID=" + formID + "&formNo=" + formNo + "&formType=" + formType + "&formTypeName=" + formTypeName + "&status=" + status + "&uid=" + userID);
                    Response.Redirect("../" + tmp_page + "?formID=" + base64.base64encode(formID) + "&formNo=" + base64.base64encode(formNo) + "&formType=" + base64.base64encode(formType) + "&uid=" + base64.base64encode(userID));
                }

            }
            
        }

    }




    public string getPriv(string LoginUser)
    {
       
        if (LoginUser != "viewer")
        {
            System.Data.DataTable dt = new DataTable();
            string sqlstr = "select UserName,PASSWORD,EMail from dbo.EmployeeBase where UserID='" + LoginUser + "'";
            SqlConnection con = new SqlConnection(UserInfoStrCon);
            con.Open();

            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, con);
            myda.Fill(dt);
            con.Close();

            if (dt.Rows.Count != 0)
            {
                Session.Add("UserID", LoginUser);
                Session.Add("UserName", dt.Rows[0]["UserName"].ToString());
                return "GET_PRIV_OK";

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('該帳號不存在或狀態異常!');window.location.href='http://10.56.69.77:8300/Login/Login.aspx'", true);
                return "NO_SYS_PRIV";
            }
        }
        else
        {
            return "GET_PRIV_OK";
        }
    }


    //根據FormID獲取該表單的相關信息
    public void getEFormInFlowInfo(string FormID, string UserID, string formType)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();

        sbval.Append(FormID).Append(",")
            .Append(UserID).Append(",")
             .Append(formType).Append(",");

        try
        {
            //將查詢數據以DataTable形式取出
            dt =
                 DBAccessProc.GetDataTable(
                   "Enotes_PK0338_AutoLogin_GetFormInFlowInfo", 0, 1, "", sbval.ToString(), ",",
                   out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("Enotes_PK0338_AutoLogin_GetFormInFlowInfo Failure:" + msg);
            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    formNo = dr["FormNo"].ToString().Trim();
                    formType = dr["FormType"].ToString().Trim();
                    formTypeName = dr["FormTypeName"].ToString().Trim();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_AutoLogin_GetFormInFlowInfo function error,Detail:" + ex.Message);
        }
    }
  

    //判斷表單是否在流程中
    public int GetEFormInFlowYN(string FormID, string UserID, string formType)
    {
        int inFlowYN = 0;

        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();

        sbval.Append(FormID).Append(",")
           .Append(UserID).Append(",")
           .Append(formType).Append(",");

        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_AutoLogin_InFlowYN", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("Enotes_PK0338_AutoLogin_InFlowYN Failure:" + msg);
                return -1;   
            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    inFlowYN = Convert.ToInt32(dr["InFlowYN"]);
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_AutoLogin_InFlowYN function error,Detail:" + ex.Message);
        }

        return inFlowYN;
    }


    //根據FormID獲取該表單的相關信息
    public void getEFormNotInFlowInfo(string FormID,string formType)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();

        sbval.Append(FormID).Append(",")
            .Append(formType).Append(",");

        try
        {
            //將查詢數據以DataTable形式取出
            dt =
                 DBAccessProc.GetDataTable(
                   "GA_Car_PK0338_AutoLogin_GetFormNotInFlowInfo", 0, 1, "", sbval.ToString(), ",",
                   out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("GA_Car_PK0338_AutoLogin_GetFormNotInFlowInfo Failure:" + msg);
            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    formNo = dr["FormNo"].ToString().Trim();
                    formType = dr["FormType"].ToString().Trim();
                    formTypeName = dr["FormTypeName"].ToString().Trim();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_AutoLogin_GetFormNotInFlowInfo error,Detail:" + ex.Message);
        }
    }
    /*
    public string checkPW(string LoginUser, string UserPW)
    {
        //查詢登錄用戶信息
        var login_check = from login in dc.BRM_USER_INFO where login.UserID.Equals(userID) select login;

        //如果用戶名密碼驗證通過就將抓到的信息放到session中，供其它頁面調用
        if (login_check.ToArray().Count() > 0)
        {
            var login_ps = from loginps in dc.BRM_USER_INFO where loginps.PASSWORD.Equals(UserPW) select loginps; //查詢使用者密碼

            if (login_ps.ToArray().Count() > 0)
            {

                return "CH_PW_OK";
            }
            else
            {

                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('登錄密碼錯誤!');window.location.href='http://10.56.69.77:6020/Login/Login.aspx'", true);
                return "PW_WR";

            }
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('該帳號不存在或狀態異常!');window.location.href='http://10.56.69.77:6020/Login/Login.aspx'", true);
            return "NO_USER";
        }
    }
    */
}