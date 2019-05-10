using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Collections;
using System.Reflection;
using System.Data;
using Base_Data;
using FITI_EPS;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using FOX.DAL;
using FOX.COMMON;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;

public partial class Login : System.Web.UI.Page
{
    Base_DataDataContext dc = new Base_DataDataContext(ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString());
    string UserInfoStrCon = ConfigurationManager.ConnectionStrings["UserInfoConnectionString"].ConnectionString;
    FITI_EPSDataContext db = new FITI_EPSDataContext();
    EIP.Framework.Security objSecurity = new EIP.Framework.Security();      //解密

    string userID="";
    string resultsTrue = "";
    string CAstaffNo = "";
    string staffNo = "";
    string result_GetSerialNumberString = "";
    string msg = "";
    int effectRow = 0;
    string getPrivResult = "";
    string UserPW = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (!IsPostBack)
        //{          
            //Session.Abandon();      //清空全部的 session
            MyLibs.encrypt.Base64 base64 = new MyLibs.encrypt.Base64();
            string url = Request.Url.ToString();
            if (url.IndexOf("uid") > -1)
            {
                if (Request.QueryString["uid"] != null && Request.QueryString["uid"] != "")
                {
                    userID = base64.base64decode(Request.QueryString["uid"].ToString().Trim());//用戶ID
                }
                else if (Request.QueryString["inuid"] != null && Request.QueryString["inuid"] != "")
                {
                    userID = Request.QueryString["inuid"];
                }
                Session.Add("UserID", userID);
                getPrivResult = getPriv(userID);
                Response.Redirect("FormList.aspx");
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
        if (LoginUser == "viewer")
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
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('該帳號不存在或狀態異常!');window.location.href='http://10.56.69.77/FitiGroup/'", true);
            return "NO_SYS_PRIV";
        }

    }

    protected void login_Click(object sender, EventArgs e)
    {
        string UserID = Request["UserID"];
        string UserPS = Request["UserPS"];
        //string RememberMe = Request["RememberMe"]; 
        //CAstaffNo = Session["CAstaffNo"] == null ? "EXCEPTION_STAFF" : Session["CAstaffNo"].ToString().Trim();

        if (RememberMe.Checked)
        {
            Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(30);
            Response.Cookies["UserPS"].Expires = DateTime.Now.AddDays(30);
            Response.Cookies["UserID"].Value = this.UserID.Text;
            Response.Cookies["UserPS"].Value = this.UserPS.Text;
        }


        if (UserID != null)
        {
            #region
            /*
            if ((CAstaffNo != "") && (CAstaffNo.ToUpper() != UserID.ToUpper()))
            {
                Session["CASTATUS"] = "CA_NO_MATCH";
            }

            if ("PK0100"== UserID.ToUpper())
            {
                Session["CASTATUS"] = "CA_OK";
                Session["CAstaffNo"] = "UserID.ToUpper()";
                Session["CAString"] = "CAIs100";
            }
                //根據登錄用戶userid取得其角色
                var user_role = from role in db.BRM_USER_ROLE where role.UserID.Equals(UserID) select role;
             */
            //查詢登錄用戶信息
            System.Data.DataTable dt = new DataTable();
            string sqlstr = "select UserName,PASSWORD,EMail from dbo.EmployeeBase where UserID='" + UserID + "'";
            SqlConnection con = new SqlConnection(UserInfoStrCon);
            con.Open();

            SqlDataAdapter myda = new SqlDataAdapter(sqlstr, con);
            myda.Fill(dt);
            con.Close();


            //DBAccessProc.setConnStr(UserInfoStrCon);//設定連接字串
            //dt = DBAccessProc.GetDataTable("Enotes_PK0338_CheckUserPassword", 0, 1, "", UserID, ",", out msg, out effectRow);

            if (dt.Rows.Count != 0)
            {
                string SearchPS = dt.Rows[0]["PASSWORD"].ToString();
                if (UserPS == SearchPS)
                {
                    
                    Session.Add("UserID", UserID);
                    Session.Add("UserName", dt.Rows[0]["UserName"].ToString());
                    Session.Add("UserPW", SearchPS);
                    //    Session.Add("Dept", dt.Rows[0]["Dept"].ToString());
                    // Response.Redirect("../FormList.aspx");                    
                    Response.Redirect("FormList.aspx");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('登錄密碼錯誤!');window.location.href=window.location.href", true);
                }

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('該帳號不存在或狀態異常!');window.location.href=window.location.href", true);
            }
            
            //ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('此文件的流程站別已存在！');window.location.href='BTM_MyWorking.aspx'</script>");
           // return;

            /*
                var login_check = from login in dc.BRM_USER_INFO where login.UserID.Equals(UserID) select login;
                //查詢登錄用戶所擁有權限
                var permissions = from permission in db.V_EPS_UserRoleAuthority where permission.UserID.Equals(UserID) select permission;

                //定義三個List對象用于存放用戶對系統、頁面及頁面上button的操作權限
                List<String> systm = new List<string>();
                List<String> pages = new List<string>();
                List<String> funcs = new List<string>();

                //三個foreach循環將用linq抓出的內容加到List對象中
                foreach (var item in permissions)
                {
                    systm.Add(item.SystemName);
                }
                foreach (var item in permissions)
                {
                    pages.Add(item.PageName);
                }
                foreach (var item in permissions)
                {
                    funcs.Add(item.ElementName);
                }

                //如果用戶名密碼驗證通過就將抓到的信息放到session中，供其它頁面調用
                if (login_check.ToArray().Count() > 0)
                {
                    var login_ps = from loginps in dc.BRM_USER_INFO where loginps.PASSWORD.Equals(UserPS) select loginps; //查詢使用者密碼

                    if (login_ps.ToArray().Count() > 0)
                    {
                        Session.Add("UserID", login_check.First().UserID);
                        Session.Add("UserName", login_check.First().Name);
                        Session.Add("Dept", login_check.First().DeptCode);
                        if (user_role.ToArray().Count() > 0)
                        {
                            Session.Add("UserRole", user_role.First().RoleID);
                            Session.Add(Session["UserID"] + "_Systm", systm);
                            Session.Add(Session["UserID"] + "_Pages", pages);
                            Session.Add(Session["UserID"] + "_Funcs", funcs);

                            
                            if (login_check.First().Name.IndexOf("門衛") >= 0)
                            {
                                Response.Redirect("../TruckIntoList.aspx");
                            }
                            else
                            {
                                Response.Redirect("../FormList.aspx");
                            }
                            

                            Response.Redirect("../FormList.aspx");

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('無該系統權限!');window.location.href=window.location.href", true);
                        }

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('登錄密碼錯誤!');window.location.href=window.location.href", true);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('該帳號不存在或狀態異常!');window.location.href=window.location.href", true);
                }
               */
            
            #endregion
        }

    }

    protected void btnForget_Click(object sender, EventArgs e)
    {
        string UserID = Request["UserID"];
        System.Data.DataTable dt = new DataTable();
        DBAccessProc.setConnStr(UserInfoStrCon);//設定連接字串
        dt = DBAccessProc.GetDataTable("Enotes_PK0338_CheckUserPassword", 0, 1, "", UserID, ",", out msg, out effectRow);

        if (dt.Rows.Count != 0)
        {
            string email = dt.Rows[0]["EMail"].ToString();

            MailAddress from = new MailAddress("P1266@foxsemicon.com", "System Admin", System.Text.Encoding.UTF8);//MailAddress("寄件者郵件地址", "寄件者顯示的名稱",編碼方式);
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);//MailMessage(寄信者, 收信者)

            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;//E-mail編碼
            message.Subject = "System Password Notice";//E-mail主旨

            string User_PS = dt.Rows[0]["EMail"].ToString();
            message.Body = "<font face='Arial'>RFQ System<br>Account：<font color='blue'><b>" + UserID + "</b></font><br>Password：<font color='blue'><b>" + User_PS + "</b></font><br><br>System Notice Time：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</font>";//E-mail內容

            SmtpClient smtpClient = new SmtpClient("10.56.69.12", 25);//設定E-mail Server和port
            smtpClient.Send(message);
            ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('密碼已發送至您的信箱');window.location='Login.aspx'</script>");
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('該帳號不存在或狀態異常!');window.location.href=window.location.href", true);
        }
    }

    protected void btnModify_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(UserID.Text))
        {
            Response.Redirect("Modify_Password.aspx?UserID=" + UserID.Text);
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script language='javascript' defer>alert('Please Input User ID！');</script>);");
        }

    }

}