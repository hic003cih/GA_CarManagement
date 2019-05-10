using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FOX.DAL;
using Base_Data;
//using FITI_EPS;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Text;
using FOX.COMMON;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;

public partial class AutoLogin : System.Web.UI.Page
{
    string userID = "";
    string formID = "";
    string formNo = "";
    string formType = "";
    string formTypeName = "";
    string status = "";

    //定義存放執行sql時相關變量
    string msg = "";
    int rowCount = 0;
    int effectRow = 0;
    string url = "";
    //用于證書
    string resultsTrue = "";
    string CAstaffNo = "";
    string staffNo = "";

    //定義用于存放連接字串
    string strCon = "";
    EIP.Framework.Security objSecurity = new EIP.Framework.Security();      //解密
    BaseDataContext dc = new BaseDataContext(ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString());
    string UserInfoStrCon = ConfigurationManager.ConnectionStrings["UserInfoConnectionString"].ConnectionString;
    FITI_EPSDataContext db = new FITI_EPSDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        strCon = ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString();
        MyLibs.encrypt.Base64 base64 = new MyLibs.encrypt.Base64();

        string getPrivResult = "";
        
        if (!IsPostBack)
        {
            if (Session["UserID"] == null)
            {
                if (Request["GUD"].ToString().Trim() == null)        //由 eHR Portal 傳入
                {
                    Response.Redirect("http://10.56.69.77:8000/");
                }
                else
                {
                    //string a = objSecurity.DecryptQueryString(Request["GUD"].ToString().Trim());
                    Session.Add("UserID", objSecurity.DecryptQueryString(Request["GUD"].ToString().Trim()));
                    

                }
            }
            userID = Session["UserID"].ToString().Trim();
            getPrivResult = getPriv(userID);
            //Response.Write(userID);
            Response.Redirect("../FormPCBApply.aspx");            
        }


    }
    public string getPriv(string LoginUser)
    {
      //  Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('" + LoginUser + "');", true);
        System.Data.DataTable dt = new DataTable();
        string sqlstr = "select UserName,PASSWORD,EMail from dbo.EmployeeBase where UserID='" + LoginUser + "'";
        SqlConnection con = new SqlConnection(UserInfoStrCon);
        con.Open();

        SqlDataAdapter myda = new SqlDataAdapter(sqlstr, con);
        myda.Fill(dt);
        con.Close();

        //System.Data.DataTable dt = new DataTable();
        //DBAccessProc.setConnStr(UserInfoStrCon);//設定連接字串
        //dt = DBAccessProc.GetDataTable("Enotes_PK0338_CheckUserPassword", 0, 1, "", LoginUser, ",", out msg, out effectRow);
        if (LoginUser=="viewer")
        {
           Session.Add("UserID", LoginUser);
            Session.Add("UserName", LoginUser);
           return "GET_PRIV_OK";
         }
        else if (dt.Rows.Count != 0)
        {
         //   string SearchPS = dt.Rows[0]["PASSWORD"].ToString();
            Session.Add("UserID", LoginUser);
            Session.Add("UserName", dt.Rows[0]["UserName"].ToString());
          //  Session.Add("Dept", dt.Rows[0]["DeptCode"].ToString());
            return "GET_PRIV_OK";

        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('該帳號不存在或狀態異常!');window.location.href='http://10.56.69.77:8000/'", true);
            return "NO_SYS_PRIV";
        }

    }

    public string checkPW(string LoginUser, string UserPW)
    {
        //查詢登錄用戶信息
        var login_check = from login in dc.BRM_USER_INFO where login.UserID.Equals(userID) select login;

        //如果用戶名密碼驗證通過就將抓到的信息放到session中，供其它頁面調用
        if (login_check.ToArray().Count() > 0)
        {
            var login_ps = from loginps in dc.BRM_USER_INFO where loginps.password.Equals(UserPW) select loginps; //查詢使用者密碼

            if (login_ps.ToArray().Count() > 0)
            {

                return "CH_PW_OK";
            }
            else
            {

                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('登錄密碼錯誤!');window.location.href='http://10.56.69.77:8000/'", true);
                return "PW_WR";

            }
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('該帳號不存在或狀態異常!');window.location.href='http://10.56.69.77:8000/'", true);
            return "NO_USER";
        }
    }

}