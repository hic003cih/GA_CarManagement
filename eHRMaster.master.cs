using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FOX.DAL;
using System.Data;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Caching;
//using CacheExtensionMethod;

public partial class Master : System.Web.UI.MasterPage
{
    EIP.Framework.Security objSecurity = new EIP.Framework.Security();      //加解密   
    MyLibs.encrypt.Base64 base64 = new MyLibs.encrypt.Base64(); //加解密 
    string userId;
    string getuid;  //文件UserID
    string strCon = "";
    string msg = "";
    int effectRow = 0;
    string userid = "";
    string deptCode = "";
    string uid = "";
    string gud = "";
    string upw;
    string sysname = "GA_GoodsRequest";
    //＊＊＊＊＊＊＊＊DataSet＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    BaseDataContext bc = new BaseDataContext();
    Base_DataDataContext login_db = new Base_DataDataContext();
    HRDataContext db = new HRDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        //RemoveAllCache.CacheClear();  //清除 cache
        //將該類注冊為ajax類型
        Ajax.Utility.RegisterTypeForAjax(typeof(Master));
        Server.ScriptTimeout = 3600;

        strCon = ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString();
        if (!IsPostBack)
        {
            getuid = "";
            string url = Request.Url.ToString();

            if (url.IndexOf("uid") > -1)

            {
                getuid = Request.QueryString["uid"].ToString().Trim();
                try
                {
                    getuid = base64.base64decode(Request.QueryString["uid"].ToString().Trim()); //解密
                }
                catch
                {
                    getuid = Request.QueryString["uid"].ToString().Trim();
                }
            }

            if (getuid == "")        //由 eHR Portal 傳入
             {
               if (Session["UserID"] == null)
               {

                Response.Redirect("http://10.56.69.77/GA_CarManagement");
               }
                else
               {
                   getuid = Session["UserID"].ToString().Trim();
               }
              
             }
             else
              {
                  Session.Add("UserID", getuid);
              }
            userId = getuid;

            if (userId == "viewer")
            {
                this.Label3.Text = "viewer";
            }
            else
            {

         //       if (Session["UserName"] == null)
         //       {
                    string cc = GetUserName(userId);
                    Session.Add("UserName", cc);
         //       }
                this.Label3.Text = (String)Session["UserName"].ToString().Trim() + "(" + (String)Session["UserID"].ToString().Trim() + ")";
            }
            //setAreaList();   //取得主廠區list
            
        }
        

    }

    public string GetUserName(string user)
    {
        //string user = this.TextBox1.Text.ToString();
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        string UserName = "";//聲明一個變量接受查詢數據
        try
        {
            //設定連接字串
            DBAccessProc.setConnStr(strCon);
            sbval.Append(user).Append(","); //定義變量用於獲取變更的參數

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetUserName", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    UserName = dr["Name"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetUserName error,Detail:" + ex.Message);
            sbrep.Clear();
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("Enotes_PK0338_GetUserName Failure:" + msg);

        }

        sbrep.Clear();
        sbval.Clear();

        return UserName;
    }

    //eHR 平台內 Session UserID 整合
    protected void Inuid(object sender, EventArgs e)
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;

        if (context.Session["UserID"] == null)
        {
            Response.Redirect("/FitiGroup/Login.aspx");
        }
        else
        {
            gud = Session["UserID"].ToString();
            Response.Redirect("/FitiGroup/Login.aspx?inuid=" + gud);
        }
    }
    //京鼎國內外出差單session
    protected void Fiti_Click(object sender, EventArgs e)
    {
        //Session["FactoryArea"] = "Fiti";
        Response.Redirect("FormBTApply.aspx");
    }
    //protected void DDLApType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string areaname = "";
    //    areaname = DDLApType.SelectedValue.ToString();//選擇的處級單位
    //    SetFactoryNameList(areaname);
    //}
    //點選人員用:設定主廠區欄位DropDownList綁定
    
    //聯動---根據廠區帶出子廠區

    //用戶登錄后，根據其角色去查詢'待處理表單'的個數
    [Ajax.AjaxMethod]
    protected int getFormListCountByUserID()
    {
        int formCount = 0;
        //當前登錄人的角色
        //       string userRole = Session["UserRole"].ToString();

        userid = Session["UserID"].ToString();
        //userid = "PK0338";
        //DataTable dt = new DataTable();
        //StringBuilder sbval = new StringBuilder();
        //sbval.Append(userid).Append(",");
        DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        StringBuilder sbrep = new StringBuilder();
        sbval.Append(userid).Append(",");
        sbrep.Append(sysname + ".dbo.V_EPS_NeedSign");
        try
        {
            //DBAccessProc.setConnStr(strCon);
            //dt = DBAccessProc.GetDataTable("ENotes_PK0338_NeedMyActionQty", 0, 1, ",", sbval.ToString(), ",", out msg, out effectRow);

            //if ("" != msg)
            //{
            //    Response.Write("ENotes_PK0338_NeedMyActionQty Failure:" + msg);
            //    return 0;
            //}
            //foreach (DataRow dr in dt.Rows)
            //{
            //    formCount = Convert.ToInt32(dr["FormCount"].ToString());
            //}
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("ENotes_P1266_NeedMyActionQty", 0, 1, sbrep.ToString(), sbval.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("ENotes_P1266_NeedMyActionQty Failure:" + msg);
                return 0;
            }
            foreach (DataRow dr in dt.Rows)
            {
                formCount = Convert.ToInt32(dr["FormCount"].ToString());
            }
        }
        catch (Exception ex)
        {
            Response.Write("ENotes_PK0338_NeedMyActionQty function error,Detail:" + ex.Message);
        }

        return formCount;
    }

    protected void logout(object sender, EventArgs e)
    {
        Session.Abandon();  //清空 Sesion
        Response.Redirect("http://10.56.69.77/GA_CarManagement");
    }


    //判斷當前人員是否是管理員
    public string CheckSysManageman(string tableid, string userid)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        try
        {
            DBAccessProc.setConnStr(strCon);//設定連接字串
            sbval.Append(tableid).Append(",") //定義變量用於獲取變更的參數
                .Append(userid).Append(","); ;

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_CheckSysmanForMainview", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            sbval.Clear();
            if (dt.Rows.Count != 0)
            {
                return "Y";
            }
            else
            {
                return "N";
            }
            
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_CheckSysmanForMainview function error,Detail:" + ex.Message);
            sbval.Clear();
            return "N";
        }   
    }
}
