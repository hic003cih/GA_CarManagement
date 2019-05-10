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
public partial class FormCarApply : System.Web.UI.Page
{
    EIP.Framework.Security objSecurity = new EIP.Framework.Security();      //解密
    MyLibs.encrypt.Base64 base64 = new MyLibs.encrypt.Base64();
    string UserID = "";
    string getuid;  //文件UserID
    string UserName = "";
    string UserDept = "";
    string formName = "FormCarApply";  
    string sysname = "GA_CarManagerment";
    string formYear = "";
    string formMonthDay = "";
    string msg = "";
    int effectRow = 0;
    int rowcount = 0;
    string strCon = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ConnectionString;
    string nextNodeInfo = "";//下一節點信息
    string sysid = "006";
    string tableid = "01";
    //string formtype = "FITICVMS_01";
    string FormIDCol = "FormID";
    string formid = "";
    string tmp_0001 = "";
    string url = "";
    List<string> ARList = new List<string>();

    string ddlDeptCode = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Ajax.Utility.RegisterTypeForAjax(typeof(FormCarApply));
        

        if (!IsPostBack)
        {
      //      Session.Add("UserID", "PK0338");
            Session.Add("car_formID", "");
            Session.Add("car_formNo", "");
            tmp_0001=(Session["car_formNo"].ToString().Trim());
            Session.Add("car_IsSysmanager", "N");
            Session.Add("car_IsMyForm", "N");
            Session.Add("car_status", "");
            Session.Add("car_FormDate", "");
            Session.Add("car_currentNode", "");
            Session.Add("car_formtype", "");
            Session.Add("car_ARList", new List<string>());

            url = Request.Url.ToString();

            if (url.IndexOf("uid") > -1)
            {
                //       string cc = Request.QueryString["uid"].ToString().Trim();
                getuid = Request.QueryString["uid"].ToString().Trim();
                try
                {
                    getuid = base64.base64decode(Request.QueryString["uid"].ToString().Trim()); //解密
                }
                catch
                {
                    getuid = Request.QueryString["uid"].ToString().Trim();
                }

                if ((getuid == null) || (getuid == ""))
                {
                    Response.Redirect("http://10.56.69.77/GA_CarManagement?refer=" + base64.base64encode(url));
                }
                else
                {
                    Session.Add("UserID", getuid);
                    UserID = getuid;
                }

            }
            else if ((Session["UserID"] == null) || (Session["UserID"] == "viewer"))
            {

                Response.Redirect("http://10.56.69.77/GA_CarManagement?refer=" + base64.base64encode(url));

            }

            else
            {
                UserID = Session["UserID"].ToString().Trim();
            }
           
            checkmanager(sysid, UserID);


            //获取本頁面url值,若有參數傳過來,則表示是開啟已存在的文件,若沒有參數傳過來,則表示是新增文件

            if (url.IndexOf("formID") > -1)
            {

                string docid = Request.QueryString["formID"].ToString().Trim();
                if (Is32Formatted(docid))
                {
                    docid = Request.QueryString["formID"].ToString().Trim();        //如果為數字表示未加密碼
                }
                else
                {
                    docid = base64.base64decode(Request.QueryString["formID"].ToString().Trim());
                }

                string fno = Request.QueryString["formNo"].ToString().Trim();
                try
                {
                    fno = base64.base64decode(Request.QueryString["formNo"].ToString().Trim());
                }
                catch
                {
                    fno = Request.QueryString["formNo"].ToString().Trim();
                }

                string ftype = Request.QueryString["formType"].ToString().Trim();
                try
                {
                    ftype = base64.base64decode(Request.QueryString["formType"].ToString().Trim());
                }
                catch
                {
                    ftype = Request.QueryString["formType"].ToString().Trim();
                }
                Session.Add("car_formNo", fno);
                Session.Add("car_formID", docid);
                Session.Add("car_formtype", ftype);

                string tmp_node = GetCurrentNode(docid);
                Session.Add("car_currentNode", tmp_node);

                string tmpdeptcode = "";
                tmpdeptcode = GetNowManDeptCode();

                getFormList();                  //顯示文件內容

                if (tmpdeptcode == "")
                {
                    tmpdeptcode = ApplyManDeptCode.Text.ToString().Trim();
                }
                hid_OwnerDeptCode.Value = tmpdeptcode;   //設定當前簽核者(BaseUser)的部門代碼

                if (Label_ForStatus.Text == "")    //設定代理Status Label
                {
                    try
                    {
                        System.Data.DataTable dt = new DataTable();
                        StringBuilder sbval = new StringBuilder();
                        string tmpowner = hid_Ownerlist.Value.ToString().Trim();
                        string tmpcode = hid_OwnerDeptCode.Value.ToString().Trim();
                        string tmpano = "";
                        string tmpaname = "";
                        string sqlstr = "";
                        if (tmpcode == "")
                        {
                            sqlstr = "ENotes_PK0338_ChectAgent_1";
                        }
                        else
                        {
                            sqlstr = "ENotes_PK0338_ChectAgent_Dept";
                        }
                        sbval.Append(tmpowner).Append(",");
                        sbval.Append(tmpcode).Append(",");
                        dt = DBAccessProc.GetDataTable(sqlstr, 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);

                        if ("" != msg)
                        {
                            Response.Write("GA_CarManagerment_PK0338_ChectAgent Failure:" + msg);
                        }

                        if (dt.Rows.Count != 0)
                        {
                            foreach (DataRow dr1 in dt.Rows)
                            {
                                tmpano = dr1["AgentID"].ToString().Trim();
                                tmpaname = dr1["AgentName"].ToString().Trim();
                            }
                            Label_ForStatus.Text = "此表單目前在 " + tmpaname + "(" + tmpano + ")" + " 代理簽核中";
                            Panel_ForStatus.Visible = true;
                        }
                        else
                        {
                            Label_ForStatus.Text = "";
                            Panel_ForStatus.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("CarApply_PK0338_SetStatusLabel error,Detail:" + ex.Message);
                    }
                }

                if ((UserID == "") || tmp_node=="-1")
                {
                    string tmp_fa = FactoryArea.SelectedValue.ToString().Trim();
                    string tmp_ct = CarType.SelectedValue.ToString().Trim();
                    if ((tmp_fa != "") && (tmp_fa != "TW") && (tmp_ct == "OfficalCar"))
                    {
                        ((Panel)this.Form.FindControl("ContentPlaceHolder1").FindControl("Panel_Invoice")).Style.Add("display", "none");
                    }

                    if (Session["car_IsSysmanager"].ToString().Trim() == "Y")         //對已結案的文件,只允許管理員修改
                    {
                        SetAuthorization("-1", "Y");
                        this.td_reject.Visible = false;
                        this.td_approve.Visible = false;
                        this.td_approve0.Visible = false;
                        this.td_moreSign.Visible = false;
                    }
                    else
                    {
                        SetAuthorization("-1", "N");
                        this.save.Visible = false;
                        Panel_Action0.Visible = false;
                    }
           //         SetCarTypeList();
                }
                else
                {
          //          SetCarTypeList();
                    string lastAction = "";
          //          if (Request.QueryString["status"].ToString().Trim() != "未送簽")
                    if (tmp_node!="1")
                    {
                        lastAction = checkSignType();
                    }
                    
            //        getFormList();

                    string tmp_fa = FactoryArea.SelectedValue.ToString().Trim();
                    string tmp_ct = CarType.SelectedValue.ToString().Trim();
                    if ((tmp_fa != "") && (tmp_fa != "TW") && (tmp_ct == "OfficalCar"))
                    {
                        ((Panel)this.Form.FindControl("ContentPlaceHolder1").FindControl("Panel_Invoice")).Style.Add("display", "none");
                    }

                    string myFormID = "";
                    myFormID = GetMyFormID();

                    //以下根據權限控制動作的顯示
                    if (myFormID.IndexOf(docid) == -1)//formID不在myFormID中
                    {

                        if (Session["car_IsSysmanager"].ToString().Trim() == "Y")  //管理員開放儲存及新增新增客戶按鈕
                        {
                            Panel_Action0.Visible = false;
                        }
                        else
                        {
                            Panel_Action.Visible = false;  //一般人員關閉所有按鈕
                        }

                    }
                    else
                    {
                        if (tmp_node == "1")
                        {
                            Session.Add("car_IsMyForm", "Y");
                            this.td_reject.Visible = false;
                            this.td_approve.Visible = false;
                            this.td_moreSign.Visible = false;
                            Panel_Att0.Visible = true;         //副檔功能打開

                            //第一關,駁回和轉簽不可使用
                            if (lastAction == "COUNTERSIGN")
                            {
                                this.td_approve0.Visible = false;
                            }
                            else
                            {
                                this.td_approve.Visible = false;
                            }

                        }
                        else
                        {
                            this.td_approve0.Visible = false;
                            this.td_approve.Visible = true;
                            this.td_reject.Visible = true;
                            this.td_moreSign.Visible = true;
                        }

                    }
                    SetAuthorization(tmp_node, Session["car_IsSysmanager"].ToString().Trim());    //設定可編輯的panel
                    Session.Add("car_status", "edit");

                }

            }
            else
            {

                if ((UserID == "")||(UserID == "viewer"))   //若文件從mail中的link點開並且session沒有userid,則需要登陸
                {
                    Response.Redirect("http://10.56.69.77/GA_CarManagement?refer=" + base64.base64encode(url));

                }

                try
                {
                    SetCarTypeList();
                    Session.Add("car_currentNode", "1");
                    System.Data.DataTable dt = new DataTable();
                    StringBuilder sbval = new StringBuilder();
                    string UserPhone = "";
                    string UserLocation = "";
                    Session.Add("car_formtype", getFormType(sysid, tableid));

                    SetAuthorization("1", Session["car_IsSysmanager"].ToString().Trim());

                    if (ApplyManNo.Text.ToString() == "")
                    {
                        sbval.Append(UserID).Append(","); //定義變量用於獲取變更的參數

                        DBAccessProc.setConnStr(strCon);//設定連接字串
                        dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetApplyManInfo", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
                        if (dt.Rows.Count != 0)
                        {
                            UserName = dt.Rows[0]["Name"].ToString();
                            UserDept = dt.Rows[0]["Dept"].ToString();
                            UserPhone = dt.Rows[0]["Phone"].ToString();
                            UserLocation = dt.Rows[0]["WorkStation"].ToString();
                            ApplyManDeptCode.Text = dt.Rows[0]["DeptCode"].ToString();
                            
                        }
                        string date = DateTime.Today.ToString("yyyy-MM-dd");
                        ApplyDate.Text = date;
                        ApplyManNo.Text = UserID;
                        ApplyMan.Text = UserName;
                        ApplyDept.Text = UserDept;
                        ApplyArea.Text = UserLocation;
                        ApplyPhoneNo.Text = UserPhone;

                        hid_OwnerDeptCode.Value = ApplyManDeptCode.Text;   //設定當前簽核者(BaseUser)的部門代碼

                        Session.Add("car_status", "add");
                    }
                }

                catch (Exception ex)
                {
                    Response.Write("Enotes_PK0338_GetApplyManInfo error,Detail:" + ex.Message);
                }

                this.td_reject.Visible = false;
                this.td_approve.Visible = false;
                this.td_moreSign.Visible = false;
                this.td_approve0.Visible = true;
                this.save.Visible = true;

            }
 //           Page.ClientScript.RegisterStartupScript(this.GetType(), "", "showfactoryarea()", true);
            
            BindFlow();
        }


    }

    //儲存主文件
    protected void save_click(object sender, EventArgs e)
    {
      string  url = Request.Url.ToString();
        if ((Session["UserID"] == null) || (Session["UserID"] == "viewer"))
        {       
            Response.Redirect("http://10.56.69.77/GA_CarManagement?refer=" + base64.base64encode(url));

        }
        string tmp_name = "";

        string testct = CarType.SelectedValue.ToString();

        string sno = FormNo.Text;

        if ((sno == "") || (sno == "CA-??"))
        {
            sno = GetformNo("CA");
            FormNo.Text = sno;
            Session.Add("car_formNo", sno);
   //         FormNo.Text = Session["car_formNo"].ToString().Trim();
            approve.Visible = true;  //產生編碼後打開送審動作
        }
        tmp_FNo.Value = sno;

        string applyman = ApplyMan.Text.ToString().Trim();
        string applymanno = ApplyManNo.Text.ToString().Trim();
        string applydept = ApplyDept.Text.ToString().Trim();
        string applydate = ApplyDate.Text.ToString().Trim();
        string applyphoneno = ApplyPhoneNo.Text.ToString().Trim();
        string applyarea = ApplyArea.Text.ToString().Trim();

        string factoryarea = FactoryArea.SelectedValue.ToString().Trim();
        string feecode = FeeCode.Text.ToString().Trim();
        string manlist = ManList.Text.ToString().Trim();
        string mannum = ManNum.Text.ToString().Trim();
        string departuretime = Departuretime.Text.ToString().Trim();
        string prereturntime = Prereturntime.Text.ToString().Trim();
        string destination = Destination.Text.ToString().Trim();
        string phoneno = PhoneNo.Text.ToString().Trim();
        string plnum = Plnum.Text.ToString().Trim();
        string pldate = Pldate.Text.ToString().Trim();
        string cartype=CarType.SelectedValue.ToString();
        string subcarType = "";
        if (factoryarea == "TW") {
            subcarType = SubCarType.SelectedValue.ToString();

        }
        string purpose = Purpose.Text.ToString().Trim();
        string isshare = IsShare.SelectedValue.ToString().Trim();
        string drivername = DriverName.SelectedValue.ToString().Trim();
        tmp_name = tmp_DriverName.Value.ToString().Trim();
        if (tmp_name != "")
        {
            drivername = tmp_name;
        }
        string carno = CarNo.Text.ToString().Trim();
        tmp_name = tmp_CarNo.Value.ToString().Trim();
        if (tmp_name != "")
        {
            carno = tmp_name;
        }
        string driverphoneno = DriverPhoneNo.Text.ToString().Trim();
        tmp_name = tmp_DriverPhNo.Value.ToString().Trim();
        if (tmp_name != "")
        {
            driverphoneno = tmp_name;
        }
        string dismanno = DisManNo.Text.ToString().Trim();
        string dismanname = DisManName.Text.ToString().Trim();
        tmp_name = tmp_DisMan.Value.ToString().Trim();
        if (tmp_name != "")
        {
            dismanname = tmp_name;
        }
        string feedback_service=Feedback_Service.SelectedValue.ToString();
        string feedback_clean=Feedback_Clean.SelectedValue.ToString();
        string feedback_satisfied=Feedback_Satisfied.SelectedValue.ToString().Trim();
        string acfinishtime=AcFinishTime.Text.ToString().Trim();
        string acdeparturetime=AcDeparturetime.Text.ToString().Trim();
        string acreturntime=Acreturntime.Text.ToString().Trim();
        string kilometers=Kilometers.Text.ToString().Trim();
        string oilnum=OilNum.Text.ToString().Trim();
        string acmanlist=AcManList.Text.ToString().Trim();
        string acmannum=AcManNum.Text.ToString().Trim();
        string acdestination=AcDestination.Text.ToString().Trim();
        string acarrive=AcArrive.Text.ToString().Trim();
        string routes = Routes.Text.ToString().Trim();
        //===============追加欄位
        string preroutes = PreRoutes.Text.ToString().Trim();
        string prekilometers = PreKilometers.Text.ToString().Trim();

        string totalkilometers = TotalKilometers.Text.ToString().Trim();
        //================
        if (factoryarea == "TW")
        {
            preroutes = "";
            prekilometers = "";
            isshare = "";
            
            dismanno = "";
            dismanname = "";
            feedback_service = "";
            feedback_clean = "";
            feedback_satisfied = "";
            acfinishtime = "";
            if (cartype == "SelfDrive")
            {
                carno = "";
                subcarType = "";
                drivername = "";
                driverphoneno = "";
            }
            else if ((cartype != "SendCar") && (subcarType != "Toyota"))
            {
                carno = "";
                drivername = "";
                driverphoneno = "";
            }
           
        }
        else 
        {
            subcarType = "";
            totalkilometers = "";
            if (cartype == "SelfDrive")
            {
                isshare = "";
                carno = "";
                drivername = "";
                driverphoneno = "";
                dismanno = "";
                dismanname = "";
                feedback_service = "";
                feedback_clean = "";
                feedback_satisfied = "";
                acfinishtime = "";
            }
            else
            {
                preroutes = "";
                prekilometers = "";

                acdeparturetime = "";
                acreturntime = "";
                kilometers = "";
                oilnum = "";
                acmanlist = "";
                acmannum = "";
                acdestination = "";
                acarrive = "";
                routes = "";
            }
        
        }

         url = Request.Url.ToString();
        string FTforModify = "";
        if (url.IndexOf("formtype") > -1)
        {
            string ftype = Request.QueryString["formType"].ToString().Trim();
            try
            {
                ftype = base64.base64decode(Request.QueryString["formType"].ToString().Trim());
            }
            catch
            {
                ftype = Request.QueryString["formType"].ToString().Trim();
            }
            FTforModify = ftype;
        }
        else
        {
            FTforModify = Session["car_formtype"].ToString().Trim();
        }
        tmp_FType.Value = FTforModify;

        saveone(Session["car_formtype"].ToString().Trim(), Session["car_formNo"].ToString().Trim(), factoryarea, feecode, manlist, mannum, departuretime, prereturntime, destination, phoneno, plnum, pldate, cartype, subcarType, purpose, isshare, carno, drivername, driverphoneno, dismanno, dismanname, feedback_service, feedback_clean, feedback_satisfied, acfinishtime, acdeparturetime, acreturntime, kilometers, oilnum, acmanlist, acmannum, acdestination, acarrive, routes, applyman, applymanno, applydept, applydate, applyphoneno, applyarea, preroutes, prekilometers, totalkilometers);
        //saveone(FTforModify, sno, factoryarea, feecode, manlist, mannum, departuretime, prereturntime, destination, phoneno, plnum, pldate, cartype, subcarType, purpose, isshare, carno, drivername, driverphoneno, dismanno, dismanname, feedback_service, feedback_clean, feedback_satisfied, acfinishtime, acdeparturetime, acreturntime, kilometers, oilnum, acmanlist, acmannum, acdestination, acarrive, routes, applyman, applymanno, applydept, applydate, applyphoneno, applyarea, preroutes, prekilometers, totalkilometers);

    }


    ////儲存文件
    protected void saveone(string formtype, string formNo, string factoryarea, string feecode, string manlist, string mannum, string departuretime, string prereturntime, string destination, string phoneno, string plnum, string pldate, string cartype, string subcarType, string purpose, string isshare, string carno, string drivername, string driverphoneno, string dismanno, string dismanname, string feedback_service, string feedback_clean, string feedback_satisfied, string acfinishtime, string acdeparturetime, string acreturntime, string kilometers, string oilnum, string acmanlist, string acmannum, string acdestination, string acarrive, string routes, string applyman, string applymanno, string applydept, string applydate, string applyphoneno, string applyarea, string preroutes, string prekilometers, string totalkilometers)
    {

        string url = Request.Url.ToString();
        string IDforModify = "";
        string ssnode = "";
        string docid = "";

        if (url.IndexOf("formID") > -1) //添加這些判斷的原因是因為有人可能同時開兩張單,然後來回的儲存,session的方式容易串單,導致資料儲存異常
        {
            docid = Request.QueryString["formID"].ToString().Trim();
            if (Is32Formatted(docid))
            {
                docid = Request.QueryString["formID"].ToString().Trim();        //如果為數字表示未加密碼
            }
            else
            {
                docid = base64.base64decode(Request.QueryString["formID"].ToString().Trim());
            }
            IDforModify = docid;
            tmp_ID.Value = IDforModify;
            ssnode = GetCurrentNode(IDforModify);
            Session.Add("car_status", "edit");
        }
        else
        {
            if (formNo != "")
            {
                IDforModify = getFormID(formNo);
                if (IDforModify == "")
                {
                    Session.Add("car_status", "add");
                }
                else
                {
                    Session.Add("car_status", "edit");
                    tmp_ID.Value = IDforModify;
                }
            }
            else 
            {
                Session.Add("car_status", "add");
            }
            
            ssnode = "1";
        }
        
        
        string ssmag = Session["car_IsSysmanager"].ToString().Trim();
        StringBuilder sbval = new StringBuilder();
        ARList = GetAuthorization(ssnode, ssmag);
        try
        {
            LabelShow.Text = "";
            UserID = Session["UserID"].ToString().Trim();

            string sqlstring = "";
            string nowtimeStr = DateTime.Now.Hour.ToString();
            int nowtimeInt = int.Parse(nowtimeStr);

            if (Session["car_status"].ToString().Trim() == "add")
            {
                if (Session["car_IsSysmanager"].ToString().Trim() != "Y")
                {
                    if ((factoryarea == "") || (factoryarea == "0"))
                    {
                        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('廠區不能為空!');", true);
                        return;
                    }
                    if (departuretime == "")
                    {

                        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('出發時間不能為空!');", true);
                        return;
                    }
                    if (prereturntime == "")
                    {

                        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('預返時間不能為空!');", true);
                        return;
                    }
                    if (factoryarea != "TW")
                    {
                        if (nowtimeInt >= 16)
                        {
                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('每天下午四點後不能申請用車,請明天早上再試,謝謝!');", true);
                            return;
                        }
                        else
                        {
                            DateTime outtime = Convert.ToDateTime(Departuretime.Text.ToString());
                            DateTime nowtime = DateTime.Now;
                            TimeSpan span = outtime.Subtract(nowtime);
                            Double tmin = span.TotalMinutes;
                            if (tmin < 120)
                            {
                                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('時間錯誤,派車必須提前兩小時,謝謝!');", true);
                                return;
                            }
                        }

                    }

                    if ((cartype == "") || ((factoryarea == "TW") && (cartype == "OfficalCar") && (subcarType == "")))
                    {

                        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('用車類型不能為空!');", true);
                        return;
                    }

                    if (manlist == "")
                    {

                        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('用車人員不能為空!');", true);
                        return;
                    }
                    if (mannum == "")
                    {

                        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('用車人數不能為空!');", true);
                        return;
                    }

                    if (feecode == "")
                    {

                        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('費用代碼不能為空!');", true);
                        return;
                    }
                    if (phoneno == "")
                    {

                        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('用車人聯繫方式不能為空!');", true);
                        return;
                    }
                    if (destination == "")
                    {

                        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('目的地不能為空!');", true);
                        return;
                    }
                    if (purpose == "")
                    {

                        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('用車事由不能為空!');", true);
                        return;
                    }
                    if ((factoryarea != "TW") && (cartype == "SelfDrive"))
                    {
                        if (preroutes == "")
                        {
                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('預計自駕路線不能為空!');", true);
                            return;
                        }
                        if (prekilometers == "")
                        {
                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('預計公里數不能為空!');", true);
                            return;
                        }
                    }


                    if ((factoryarea == "TW") && (cartype == "OfficalCar") && (subcarType != "") && (departuretime != "") && (prereturntime != ""))
                    {
                        System.Data.DataTable dt = new DataTable();
                        sbval.Append(Session["car_formNo"].ToString().Trim()).Append(",")
                            .Append(subcarType).Append(",")
                            .Append(departuretime).Append(",")
                            .Append(prereturntime).Append(",");

                        dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_CheckCartype", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
                        if (dt.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('此公務車該時段已被占用,請選擇其他車!');", true);
                            return;
                        }
                        sbval.Clear();
                    }
                }


                sbval.Append(formNo).Append("{?,?}")
                    .Append(formtype).Append("{?,?}")
                    .Append(applymanno).Append("{?,?}")
                    .Append(applyman).Append("{?,?}")
                    .Append(applydate).Append("{?,?}")
                    .Append(applydept).Append("{?,?}")
                    .Append(applyphoneno).Append("{?,?}")
                    .Append(applyarea).Append("{?,?}");


                sbval.Append(factoryarea).Append("{?,?}")
                .Append(feecode).Append("{?,?}")
                .Append(manlist).Append("{?,?}")
                .Append(mannum).Append("{?,?}")
                .Append(departuretime).Append("{?,?}")
                .Append(prereturntime).Append("{?,?}")
                .Append(destination).Append("{?,?}")
                .Append(phoneno).Append("{?,?}")
                .Append(plnum).Append("{?,?}")
                .Append(pldate).Append("{?,?}")
                .Append(cartype).Append("{?,?}")
                .Append(subcarType).Append("{?,?}")
                .Append(purpose).Append("{?,?}")
                .Append(isshare).Append("{?,?}")
                .Append(carno).Append("{?,?}")
                .Append(drivername).Append("{?,?}")
                .Append(driverphoneno).Append("{?,?}")
                .Append(dismanno).Append("{?,?}")
                .Append(dismanname).Append("{?,?}")
                .Append(feedback_service).Append("{?,?}")
                .Append(feedback_clean).Append("{?,?}")
                .Append(feedback_satisfied).Append("{?,?}")
                .Append(acfinishtime).Append("{?,?}")
                .Append(acdeparturetime).Append("{?,?}")
                .Append(acreturntime).Append("{?,?}")
                .Append(kilometers).Append("{?,?}")
                .Append(oilnum).Append("{?,?}")
                .Append(acmanlist).Append("{?,?}")
                .Append(acmannum).Append("{?,?}")
                .Append(acdestination).Append("{?,?}")
                .Append(acarrive).Append("{?,?}")
                .Append(routes).Append("{?,?}")
                .Append(UserID).Append("{?,?}")
                .Append(sysid).Append("{?,?}")
                .Append(tableid).Append("{?,?}")

                .Append(preroutes).Append("{?,?}")
                .Append(prekilometers).Append("{?,?}")

                .Append(totalkilometers).Append("{?,?}")
                .Append(ApplyManDeptCode.Text.ToString().Trim()).Append("{?,?}")
                ;

                sbval.ToString();
                DBAccessProc.setConnStr(strCon);

                sqlstring = "GA_CarManagement_PK0338_ADD_3";

                DBAccessProc.ExecSQL(sqlstring, " ", sbval.ToString(), "{?,?}", out msg, out rowcount, out effectRow);

                sbval.Clear();
                IDforModify = getFormID(formNo);
                tmp_ID.Value = IDforModify;
            }
            else
            {

                string myFormID = "";
                    myFormID = GetMyFormID();

                    if (((Session["car_IsSysmanager"].ToString().Trim() != "Y") && (myFormID.IndexOf(IDforModify) > -1)) || (Session["car_IsSysmanager"].ToString().Trim() == "Y"))
                {
                    StringBuilder sbrep = new StringBuilder();
                    sqlstring = "GA_CarManagement_PK0338_Edit";

                    if (ARList.Count >= 0)
                    {
                        string strAR = "";
                        string strEdit = "";
                        for (int i = 0; i < ARList.Count; i++)
                        {
                            strAR = ARList[i];
                            switch (strAR)
                            {
                                case "Panel_ApplyCtrl":

                                    if (Session["car_IsSysmanager"].ToString().Trim() != "Y")
                                    {
                                        if ((factoryarea == "") || (factoryarea == "0"))
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('廠區不能為空!');", true);
                                            return;
                                        }
                                        if (departuretime == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('出發時間不能為空!');", true);
                                            return;
                                        }
                                        if (prereturntime == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('預返時間不能為空!');", true);
                                            return;
                                        }
                                        if (factoryarea != "TW")
                                        {
                                            if (nowtimeInt >= 16)
                                            {
                                                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('每天下午四點後不能申請用車,請明天早上再試,謝謝!');", true);
                                                return;
                                            }
                                            else
                                            {
                                                DateTime outtime = Convert.ToDateTime(Departuretime.Text.ToString());
                                                DateTime nowtime = DateTime.Now;
                                                TimeSpan span = outtime.Subtract(nowtime);
                                                Double tmin = span.TotalMinutes;
                                                if (tmin < 120)
                                                {
                                                    ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('時間錯誤,派車必須提前兩小時,謝謝!');", true);
                                                    return;
                                                }
                                            }

                                        }

                                        if ((cartype == "") || ((factoryarea == "TW") && (cartype == "OfficalCar") && (subcarType == "")))
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('用車類型不能為空!');", true);
                                            return;
                                        }

                                        if (manlist == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('用車人員不能為空!');", true);
                                            return;
                                        }
                                        if (mannum == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('用車人數不能為空!');", true);
                                            return;
                                        }

                                        if (feecode == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('費用代碼不能為空!');", true);
                                            return;
                                        }
                                        if (phoneno == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('用車人聯繫方式不能為空!');", true);
                                            return;
                                        }
                                        if (destination == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('目的地不能為空!');", true);
                                            return;
                                        }
                                        if (purpose == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('用車事由不能為空!');", true);
                                            return;
                                        }
                                        if ((factoryarea != "TW") && (cartype == "SelfDrive"))
                                        {
                                            if (preroutes == "")
                                            {
                                                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('預計自駕路線不能為空!');", true);
                                                return;
                                            }
                                            if (prekilometers == "")
                                            {
                                                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('預計公里數不能為空!');", true);
                                                return;
                                            }
                                        }

                                        if ((factoryarea == "TW") && (cartype == "OfficalCar") && (subcarType != "") && (departuretime != "") && (prereturntime != ""))
                                        {
                                            System.Data.DataTable dt = new DataTable();
                                            sbval.Append(Session["car_formNo"].ToString().Trim()).Append(",")
                                                .Append(subcarType).Append(",")
                                                .Append(departuretime).Append(",")
                                                .Append(prereturntime).Append(",");

                                            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_CheckCartype", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
                                            if (dt.Rows.Count > 0)
                                            {
                                                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('此公務車該時段已被占用,請選擇其他車!');", true);
                                                return;
                                            }
                                            sbval.Clear();
                                        }
                                    }
                                    string a1 = "'" + factoryarea + "'";
                                    string a2 = "'" + feecode + "'";
                                    string a3 = "'" + manlist + "'";
                                    string a4 = "'" + mannum + "'";
                                    string a5 = "'" + departuretime + "'";
                                    string a6 = "'" + prereturntime + "'";
                                    string a7 = "'" + destination + "'";
                                    string a8 = "'" + phoneno + "'";
                                    string a9 = "'" + plnum + "'";
                                    string a10 = "'" + pldate + "'";
                                    string a11 = "'" + cartype + "'";
                                    string a12 = "'" + subcarType + "'";
                                    string a13 = "'" + purpose + "'";
                                    string a14 = "'" + preroutes + "'";
                                    string a15 = "'" + prekilometers + "'";
                                    strEdit = strEdit + "FactoryArea=" + a1 + ", FeeCode=" + a2 + ", ManList=" + a3 + ", ManNum=" + a4 + ", Departuretime=" + a5 + ", Prereturntime=" + a6 + ", Destination=" + a7 + ", PhoneNo=" + a8 + ", Plnum=" + a9 + ", Pldate=" + a10 + ", CarType=" + a11 + ", SubCarType=" + a12 + ", Purpose=" + a13 + ", PreRoutes=" + a14 + ", PreKilometers=" + a15 + ", ";
                                    break;
                                case "Panel_DriverInfo":
                                    if (Session["car_IsSysmanager"].ToString().Trim() != "Y")
                                    {
                                        if ((factoryarea != "TW")&&(isshare == ""))
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇是否並車!');", true);
                                            return;
                                        }
                                        if (drivername == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇司機!');", true);
                                            return;
                                        }
                                        if (factoryarea != "TW")
                                        {
                                            if (dismanno == "")
                                            {

                                                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('乘車後評價人員不能為空!');", true);
                                                return;
                                            }
                                            else
                                            {
                                                string tmp_no = DisManNo.Text.Trim().Substring(0, 1).ToUpper();
                                                string tmp_str = "ACDFHIKNPUZ";
                                                if ((tmp_DisMan.Value == "" && DisManName.Text == "") || (tmp_str.IndexOf(tmp_no) <= -1))
                                                {
                                                    ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請輸入正確的乘車評價人員工號!');", true);
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    string b1 = "'" + isshare + "'";
                                    string b2 = "'" + carno + "'";
                                    string b3 = "'" + drivername + "'";
                                    string b4 = "'" + driverphoneno + "'";
                                    string b5 = "'" + dismanno + "'";
                                    string b6 = "'" + dismanname + "'";
                                    strEdit = strEdit + "IsShare=" + b1 + ", CarNo=" + b2 + ", DriverName=" + b3 + ", DriverPhoneNo=" + b4 + ", DisManNo=" + b5 + ", DisManName=" + b6 + ", ";
                                    break;
                                case "Panel_Feedback":
                                    if (Session["car_IsSysmanager"].ToString().Trim() != "Y")
                                    {
                                        if (feedback_service == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇服務!');", true);
                                            return;
                                        }
                                        if (feedback_clean == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇衛生!');", true);
                                            return;
                                        }
                                        if (feedback_satisfied == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇滿意度!');", true);
                                            return;
                                        }
                                        if (acfinishtime == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫實際用車完成時間!');", true);
                                            return;
                                        }
                                    }
                                    string c1 = "'" + feedback_service + "'";
                                    string c2 = "'" + feedback_clean + "'";
                                    string c3 = "'" + feedback_satisfied + "'";
                                    string c4 = "'" + acfinishtime + "'";
                                    strEdit = strEdit + "Feedback_Service=" + c1 + ", Feedback_Clean=" + c2 + ", Feedback_Satisfied=" + c3 + ", AcFinishTime=" + c4 + ", ";
                                    break;
                                case "Panel_ApplyCtrl_1":
                                    if ((Session["car_IsSysmanager"].ToString().Trim() != "Y") && (ssnode != "12" || (ssnode == "12" && LFactoryArea.Text == "TW" && LCarType.Text=="派車")))
                                    {
                                        if (acdeparturetime == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫實際出發時間!');", true);
                                            return;
                                        }
                                        if (acreturntime == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫實際返回時間!');", true);
                                            return;
                                        }
                                        if (kilometers == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫實際公里數!');", true);
                                            return;
                                        }
                                        if (oilnum == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫實際加油量!');", true);
                                            return;
                                        }
                                        if (acmanlist == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫實際用車人員!');", true);
                                            return;
                                        }
                                        if (acmannum == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫實際用車人數!');", true);
                                            return;
                                        }
                                        if (acdestination == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫實際出發地點!');", true);
                                            return;
                                        }
                                        if (acarrive == "")
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫實際到達地點!');", true);
                                            return;
                                        }
                                        if ((factoryarea != "TW") && (routes == ""))
                                        {

                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫自駕線路!');", true);
                                            return;
                                        }

                                        if ((factoryarea == "TW") && (totalkilometers == ""))
                                        {
                                            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫還車公里數!');", true);
                                            return;
                                        }
                                        /*
                                          DateTime dateTime;
                                          dateTime = Convert.ToDateTime(ApplyDate.Text.ToString());
                                          formYear = dateTime.Year.ToString();
                                          formMonthDay = string.Format("{0:MMdd}", dateTime);
                                          if (formYear != "" && formMonthDay != "" && Session["car_formID"].ToString().Trim() != "")
                                          {
                                              string attnum = getExistFiles("Attach_1", formYear, formMonthDay, Session["car_formID"].ToString().Trim());
                                              if (attnum == "")
                                              {
                                                  ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請上傳發票影本!');", true);
                                                  return;
                                              }
                                          }
                                         */
                                    }
                                    string d1 = "'" + acdeparturetime + "'";
                                    string d2 = "'" + acreturntime + "'";
                                    string d3 = "'" + kilometers + "'";
                                    string d4 = "'" + oilnum + "'";
                                    string d5 = "'" + acmanlist + "'";
                                    string d6 = "'" + acmannum + "'";
                                    string d7 = "'" + acdestination + "'";
                                    string d8 = "'" + acarrive + "'";
                                    string d9 = "'" + routes + "'";
                                    string d10 = "'" + totalkilometers + "'";
                                    strEdit = strEdit + "AcDeparturetime=" + d1 + ", Acreturntime=" + d2 + ", Kilometers=" + d3 + ", OilNum=" + d4 + ", AcManList=" + d5 + ", AcManNum=" + d6 + ", AcDestination=" + d7 + ", AcArrive=" + d8 + ", Routes=" + d9 + ", TotalKilometers=" + d10 + ", ";
                                    break;
                            }
                        }

                        if (strEdit != "")
                        {
                            strEdit = strEdit + "ModifyMan='" + UserID + "' " + " where FormID=@P1";
                            sbval.Append(IDforModify);

                            sbrep.Append(strEdit);
                            DBAccessProc.ExecuteSQL(sqlstring, sbrep.ToString(), sbval.ToString(), "{?+?}", out msg, out rowcount, out effectRow);

                            sbrep.Clear();
                            sbval.Clear();
                        }
                    }

                    Session.Add("car_status", "edit");
                    if (msg != "")
                    {
                        Response.Write("GA_CarManagement_PK0338_ADD/Edit Error:" + msg);
                    }
                    else
                    {
                        LabelShow.Text = "儲存成功!";
                    }

                    
                    if (drivername != "")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "setdrivername()", true);
                        DriverName.SelectedValue = drivername;
                        CarNo.Text = carno;
                        DriverPhoneNo.Text = driverphoneno;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('文件信息有誤,請將文件關閉後重開再試!');", true);
                    return;
                }
                
                
            }

        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagement_PK0338_ADD/Edit function error:" + ex.Message);
        }
    

    }

   
    //產生文件編號
    public string GetformNo(string APcode)
    {
        formid = FormNo.Text.ToString();
        if (formid == "")
        {
            StringBuilder strval = new StringBuilder();
            string year = DateTime.Today.ToString("yyyyMM");
            string max = "";
            string LCode = APcode + "-" + year + "%";

            strval.Append(LCode).Append(",");

            System.Data.DataTable dt = new DataTable();
            try
            {
                DBAccessProc.setConnStr(strCon);
                dt = DBAccessProc.GetDataTable("GA_CarManagement_PK0338_getformno", 0, 1, "", strval.ToString(), ",", out msg, out effectRow);

                if ("" != msg)
                {
                    Response.Write("GA_CarManagement_PK0338_getformno Failure:" + msg);
                    return "";
                }

                if (dt.Rows.Count!=0)
                {
                    max = dt.Rows[0][1].ToString().Trim();
                }

                if (max == "")
                {
                    max = APcode + "-" + year + "001";
                }
                else
                {
                    max = APcode + "-" + max;
                }

            }
            catch (Exception ex)
            {
                Response.Write("GA_CarManagement_PK0338_getformno function error,Detail:" + ex.Message);
            }

            return max;
        }
        else
        {
            return formid;
        }
    }


    //送審
    protected void submit_Click(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            url = Request.Url.ToString();
            Response.Redirect("http://10.56.69.77/GA_CarManagement?refer=" + base64.base64encode(url));
        }
        else
        {
            UserID = Session["UserID"].ToString().Trim();
        }

        save_click(sender, e);

        string ssformid = tmp_ID.Value.ToString().Trim();
        string ssNo = tmp_FNo.Value.ToString().Trim();

        if (ssformid == "")
        {
            StringBuilder sbval = new StringBuilder();
            //sql查詢 formid
            System.Data.DataTable dt = new DataTable();
            sbval.Append(ssNo).Append(",");
            try
            {
                //將查詢數據以DataTable形式取出
                dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_GetFormidForFirstSave", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
                if (dt.Rows.Count != 0)
                {
                    //  aa.formID = dt.Rows[0][0].ToString();
                    Session.Add("car_formID", dt.Rows[0][0].ToString());

                    //aa.formtype = dt.Rows[0][1].ToString();
                }
                if ("" != msg)
                {
                    Response.Write("GA_CarManagerment_PK0338_GetFormidForFirstSave Failure:" + msg);
                }
            }
            catch (Exception ex)
            {
                Response.Write("GA_CarManagerment_PK0338_GetFormidForFirstSave error,Detail:" + ex.Message);
            }

        }

        if (ssformid == "" || ssformid == "0")
        {
            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請點儲存動作儲存文件,謝謝!');", true);
            return;
        }


        string Approve_nextNodeInfo = GetNextNodeInfo_dept(tmp_FType.Value.ToString().Trim(), ssformid, UserID, "Approved",  ssNo, Session["car_FormDate"].ToString().Trim(), hid_OwnerDeptCode.Value.ToString().Trim());


        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", " window.showModalDialog('FormSignApprove.aspx','" + Approve_nextNodeInfo + "','dialogWidth:670px;dialogHeight:450px;center:yes;edge:raised;scroll:no;status:no');window.location.href = 'FormList.aspx';", true);


    }

    //同意
    protected void approve_Click(object sender, EventArgs e)
    {
       url = Request.Url.ToString();
         if (Session["UserID"] == null)
        {

            Response.Redirect("http://10.56.69.77/GA_CarManagement?refer=" + base64.base64encode(url));
        }
        else
        {
            UserID = Session["UserID"].ToString().Trim();
        }

         string formNo = FormNo.Text.ToString().Trim();
         string fdate = ApplyDate.Text.ToString();
         string FTforModify = "";
         string IDforModify = "";
         if (url.IndexOf("formType") > -1)
         {
             string ftype = Request.QueryString["formType"].ToString().Trim();
             try
             {
                 ftype = base64.base64decode(Request.QueryString["formType"].ToString().Trim());
             }
             catch
             {
                 ftype = Request.QueryString["formType"].ToString().Trim();
             }
             FTforModify = ftype;
         }
         else
         {
             FTforModify = Session["car_formtype"].ToString().Trim();
         }


         if (url.IndexOf("formID") > -1) //添加這些判斷的原因是因為有人可能同時開兩張單,然後來回的儲存,session的方式容易串單,導致資料儲存異常
         {
             string docid = Request.QueryString["formID"].ToString().Trim();
             if (Is32Formatted(docid))
             {
                 docid = Request.QueryString["formID"].ToString().Trim();        
             }
             else
             {
                 docid = base64.base64decode(Request.QueryString["formID"].ToString().Trim());
             }
             IDforModify = docid;
         }
         else
         {
             
             if (formNo != "")
             {
                 IDforModify = getFormID(formNo);
                 Session.Add("car_formID", IDforModify);
             }
             else
             {
                 IDforModify = Session["car_formID"].ToString().Trim();
             }
         }

         

         //if (Approve_nextNodeInfo != null && Approve_nextNodeInfo != "")
         //{
             save_click(sender, e);

             string Approve_nextNodeInfo = GetNextNodeInfo_dept(FTforModify, IDforModify, UserID, "Approved", formNo, fdate, hid_OwnerDeptCode.Value.ToString().Trim());

             ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", " window.showModalDialog('FormSignApprove.aspx','" + Approve_nextNodeInfo + "','dialogWidth:670px;dialogHeight:450px;center:yes;edge:raised;scroll:no;status:no');window.location.href = 'FormList.aspx';", true);
         //}
         //else
         //{
         //    ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('當前文件信息有誤,請將文件關閉後至待簽文件中重新開啟再試,謝謝!');", true);
         //    return;
         //}
     
        
    }

    //駁回
    protected void reject_Click(object sender, EventArgs e)
    {
        Response.Redirect("FormList.aspx");
    }

    //轉簽
    protected void dispatch_Click(object sender, EventArgs e)
    {
        Response.Redirect("FormList.aspx");
    }

    //會簽
    protected void moreSign_Click(object sender, EventArgs e)
    {
        Response.Redirect("FormList.aspx");
    }


    //Ajax註冊的方法內無法用Request.QueryString[]/session 獲取到值。此法為折中法
    //後臺->前臺->後臺
    [Ajax.AjaxMethod]
    public string BackFormInfo()
    {

        string url = Request.Url.ToString();

        Session.Add("car_formNo", FormNo.Text.ToString().Trim());
        UserID = Session["UserID"].ToString().Trim();//获取当前登录用户

        if (url.IndexOf("?") <= -1 && Session["car_formNo"].ToString().Trim() != "" && Session["car_formNo"].ToString().Trim() != "CA-??")
        {
            DateTime dateTime;
            dateTime = Convert.ToDateTime(ApplyDate.Text.ToString());
            formYear = dateTime.Year.ToString();
            formMonthDay = string.Format("{0:MMdd}", dateTime);

            if (Session["car_formID"].ToString().Trim() == null || Session["car_formID"].ToString().Trim() == "")
            {
                //sql查詢 formid
                System.Data.DataTable dt = new DataTable();
                StringBuilder sbrep = new StringBuilder();
                sbrep.Append(sysname + ".dbo." + formName);

                try
                {
                    //將查詢數據以DataTable形式取出
                    dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetFormidForFirstSave", 0, 1, sbrep.ToString(), Session["car_formNo"].ToString().Trim(), ",", out msg, out effectRow);
                    if (dt.Rows.Count != 0)
                    {
                        Session.Add("car_formID", dt.Rows[0][0].ToString());
                    }
                    if ("" != msg)
                    {
                        Response.Write("Enotes_PK0338_GetFormidForFirstSave Failure:" + msg);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("Enotes_PK0338_GetFormidForFirstSave error,Detail:" + ex.Message);
                }
                //aa.formID= 

            }
        }

        //獲取人員所在部門編碼
       // ddlDeptCode = (Master.FindControl("ddlDept") as DropDownList).SelectedValue.ToString();
        ddlDeptCode = hid_OwnerDeptCode.Value.ToString().Trim();

        //     aa.FormDate = ApplyDate.Text.ToString();
        Session.Add("car_FormDate", ApplyDate.Text.ToString());

        return Session["car_formID"].ToString().Trim() + "|||" + Session["car_formNo"].ToString().Trim() + "|||" + UserID + "|||"
            + formYear + "|||" + formMonthDay + "|||" + Session["car_formtype"].ToString().Trim()
            + "|||" + Session["car_FormDate"].ToString().Trim() + "|||" + ddlDeptCode;
        
    }

    //GetWFNextNodeInfo;
    /*參數:EXEC dbo.GetNextNodeInfo 
     * @FormTable = 'TEST.dbo.EPS_EFORM_LIST', @FormIDCol = 'FormID', 
     * @FormType='' @FormIDValue = '',@UserID = '', @Action = '', @MSG = '' 
     */
    [Ajax.AjaxMethod]
    //取得下一站流程信息
    public string GetNextNodeInfo(string fType, string fID, string fUserID, string fAction, string fDeptCode, string fFormNo, string fFormDate)//f->form
    {

        fDeptCode.ToString();

        // string FormTable = "FormCarApply_Main";


        StringBuilder sbval = new StringBuilder();
        try
        {
            sbval.Append(formName).Append(",").Append(FormIDCol).Append(",")
                 .Append(fType).Append(",").Append(fID).Append(",")
                 .Append(fUserID).Append(",").Append(fAction).Append(",");
            //.Append(fDeptCode).Append(",");
            sbval.ToString();
            DataTable dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetNextNodeInfo", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);

            sbval.Clear();

            if (msg != "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('" + msg + "');", true);
                return "";
            }
            if (dt.Rows.Count!=0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    nextNodeInfo = dr["NodeName"].ToString().Trim() + "{?+?}"//當前流程
                            + dr["NextSeq"].ToString().Trim() + "_" + dr["NextName"].ToString().Trim() + "{?+?}"//下一站點_下一站點名稱
                            + dr["NextOwnerListName"].ToString().Trim() + "{?+?}"//下一簽核人員
                            + dr["NextOwnerList"].ToString().Trim() + "{?+?}"//下一簽核List
                            + dr["PreSelectNode"].ToString().Trim() + "{?+?}"//預選節點
                            + dr["PreNodeName"].ToString().Trim() + "{?+?}"//預選節點名稱
                            + dr["SelectList"].ToString().Trim() + "{?+?}"//選簽列表人員工號
                            + dr["SelectListShow"].ToString().Trim() + "{?+?}"//選簽列表人員姓名
                            + dr["LastAction"].ToString().Trim() + "{?+?}";//判斷是會簽，轉簽。
                }
                nextNodeInfo += fType + "{?+?}" + fID + "{?+?}" + fDeptCode + "{?+?}" + "formNO=" + fFormNo + ";formDate=" + fFormDate + "{?+?}" + formName + "{?+?}" + sysname;
                nextNodeInfo.ToString();
            }
        }
        catch (Exception e)
        {
            Response.Write("Enotes_PK0338_GetNextNodeInfo function error:" + e.Message);
        }

        return nextNodeInfo;
    }

    [Ajax.AjaxMethod]
    public string GetNextNodeInfo_dept(string fType, string fID, string fUserID, string fAction, string fFormNo, string fFormDate, string UserDeptCode)//f->form
    {

        //fDeptCode.ToString();
        string fDeptCode = "";  //以前電簽的參數,下面的function要用到,所以保留了

        // string FormTable = "FormCarApply_Main";


        StringBuilder sbval = new StringBuilder();
        try
        {
            sbval.Append(formName).Append(",").Append(FormIDCol).Append(",")
                 .Append(fType).Append(",").Append(fID).Append(",")
                 .Append(fUserID).Append(",").Append(fAction).Append(",")
                 .Append(UserDeptCode).Append(",")
                 ;
            //.Append(fDeptCode).Append(",");
            sbval.ToString();
            DataTable dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetNextNodeInfo_Dept", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);

            sbval.Clear();

            if (msg != "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('" + msg + "');", true);
                return "";
            }
            if (dt.Rows.Count != 0)
            {
                string tmpstr = "";
            //    string tmpstr1 = "";
                foreach (DataRow dr in dt.Rows)
                {
                    nextNodeInfo = dr["NodeName"].ToString().Trim() + "{?+?}"//當前流程
                            + dr["NextSeq"].ToString().Trim() + "_" + dr["NextName"].ToString().Trim() + "{?+?}"//下一站點_下一站點名稱
                            + dr["NextOwnerListName"].ToString().Trim() + "{?+?}"//下一簽核人員
                            + dr["NextOwnerList"].ToString().Trim() + "{?+?}"//下一簽核List
                            + dr["PreSelectNode"].ToString().Trim() + "{?+?}"//預選節點
                            + dr["PreNodeName"].ToString().Trim() + "{?+?}"//預選節點名稱
                            + dr["SelectList"].ToString().Trim() + "{?+?}"//選簽列表人員工號
                            + dr["SelectListShow"].ToString().Trim() + "{?+?}"//選簽列表人員姓名
                            + dr["LastAction"].ToString().Trim() + "{?+?}";//判斷是會簽，轉簽。
                    tmpstr = dr["NextOwnerDeptCode"].ToString().Trim(); //下一站人員deptcode,因為是最後添加的,所以只能放在字串的最後,這裡先用tmpstr轉存
                //    tmpstr1 = hid_OwnerDeptCode.Value.ToString().Trim(); //當前BaseUserdeptcode,因為是最後添加的,所以只能放在字串的最後,這裡先用tmpstr1轉存

                }
                nextNodeInfo += fType + "{?+?}" + fID + "{?+?}" + fDeptCode + "{?+?}" + "formNO=" + fFormNo + ";formDate=" + fFormDate + "{?+?}" + formName + "{?+?}" + sysname + "{?+?}" + tmpstr + "{?+?}" + UserDeptCode;  //UserDeptCode為當前BaseUserdeptcode
                nextNodeInfo.ToString();
            }
        }
        catch (Exception e)
        {
            Response.Write("Enotes_PK0338_GetNextNodeInfo function error:" + e.Message);
        }

        return nextNodeInfo;
    }



    [Ajax.AjaxMethod]
    //取得下一站流程信息(含頁面欄位值傳遞)
    public string GetNextNodeInfoByPara(string fType, string fID, string fUserID, string fAction, string fDeptCode,
        string fFormNo, string fFormDate, string fPara)//f->form
    {

        fDeptCode.ToString();



        StringBuilder sbval = new StringBuilder();
        try
        {
            sbval.Append(formName).Append(",").Append(FormIDCol).Append(",")
                 .Append(fType).Append(",").Append(fID).Append(",")
                 .Append(fUserID).Append(",").Append(fAction).Append(",")
                 .Append(fPara).Append(",");//fPara為頁面預留動態參數,For流程動態選擇下站人員使用
            sbval.ToString();
            DataTable dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetNextNodeInfoByPara", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);

            sbval.Clear();

            if (msg != "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('" + msg + "');", true);
                return "";
            }
            if (dt.Rows.Count!=0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    nextNodeInfo = dr["NodeName"].ToString().Trim() + "{?+?}"//當前流程
                            + dr["NextSeq"].ToString().Trim() + "_" + dr["NextName"].ToString().Trim() + "{?+?}"//下一站點_下一站點名稱
                            + dr["NextOwnerListName"].ToString().Trim() + "{?+?}"//下一簽核人員
                            + dr["NextOwnerList"].ToString().Trim() + "{?+?}"//下一簽核List
                            + dr["PreSelectNode"].ToString().Trim() + "{?+?}"//預選節點
                            + dr["PreNodeName"].ToString().Trim() + "{?+?}"//預選節點名稱
                            + dr["SelectList"].ToString().Trim() + "{?+?}"//選簽列表人員工號
                            + dr["SelectListShow"].ToString().Trim() + "{?+?}"//選簽列表人員姓名
                            + dr["LastAction"].ToString().Trim() + "{?+?}";//判斷是會簽，轉簽

                }
                nextNodeInfo += fType + "{?+?}" + fID + "{?+?}" + fDeptCode + "{?+?}" + "formNO=" + fFormNo + ";formDate=" + fFormDate;
                nextNodeInfo.ToString();
            }
        }
        catch (Exception e)
        {
            Response.Write("Enotes_PK0338_GetNextNodeInfoByPara function error:" + e.Message);
        }
        //}
        return nextNodeInfo;

    }


    //獲取該文件當前站別for 前端js用
    public string GetCurrentNode()
    {

        string nodeSeq = "";
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        try
        {
            DBAccessProc.setConnStr(strCon);
            
                        string url = Request.Url.ToString();
                        string IDforModify = "";
                        if (url.IndexOf("formID") > -1)
                        {
                            string docid = Request.QueryString["formID"].ToString().Trim();
                            if (Is32Formatted(docid))
                            {
                                docid = Request.QueryString["formID"].ToString().Trim();        //如果為數字表示未加密碼
                            }
                            else
                            {
                                docid = base64.base64decode(Request.QueryString["formID"].ToString().Trim());
                            }
                            IDforModify = docid;
                        }
                        else
                        {
                            IDforModify = Session["car_formID"].ToString().Trim();
                        }

                        sbval.Append(IDforModify).Append(",")
                .Append(formName).Append(",");

            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetCurrentNode_1", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("Enotes_PK0338_GetCurrentNode_1 GetCurrentNode Failure:" + msg);
                return "";
            }
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    nodeSeq = dr["NodeSeq"].ToString().Trim();
                }
            }

        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetCurrentNode_1 GetCurrentNode function error,Detail:" + ex.Message);
        }

        return nodeSeq;
    }

    //獲取該文件當前站別 for Save用(防止兩份文件同時打開時串文件)
    public string GetCurrentNode(string fid)
    {

        string nodeSeq = "";
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        try
        {
            DBAccessProc.setConnStr(strCon);
            sbval.Append(fid).Append(",")
                .Append(formName).Append(",");

            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetCurrentNode_1", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("Enotes_PK0338_GetCurrentNode_1 GetCurrentNode Failure:" + msg);
                return "";
            }
            if (dt.Rows.Count!=0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    nodeSeq = dr["NodeSeq"].ToString().Trim();
                }
            }

        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetCurrentNode_1 GetCurrentNode function error,Detail:" + ex.Message);
        }

        return nodeSeq;
    }


    [Ajax.AjaxMethod]
    //獲取動態流程
    public string GetAutoProcess()
    {
        string autoNode = "";
        StringBuilder strval = new StringBuilder();

        strval
            .Append(formName).Append(",")
            .Append(FormIDCol).Append(",")
            .Append(Session["car_formtype"].ToString().Trim()).Append(",")
            .Append(Session["car_formID"].ToString().Trim()).Append(",")
            .Append(UserID).Append(",")
            .Append(hid_OwnerDeptCode.Value.ToString().Trim()).Append(",")
            ;

        System.Data.DataTable dt = new DataTable();
        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_GetAutoProcess_Dept", 0, 1, "", strval.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("GA_CarManagerment_PK0338_GetAutoProcess_Dept Failure:" + msg);
                return "";
            }
            if (dt.Rows.Count!=0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    autoNode += dr["AutoNode"].ToString().Trim() + ";";
                }
            }

        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_GetAutoProcess_Dept function error,Detail:" + ex.Message);
        }

        return autoNode;
    }


    //Flow gridview綁定數據
    public void BindFlow()
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();

        try
        {
            //     string formid = FormNo.Text.ToString();
            sbval.Append(formName).Append(",");
            sbval.Append(Session["car_formID"].ToString().Trim()).Append(",");
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_FlowGridview", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            if ("" != msg)
            {
                Response.Write("Enotes_PK0338_FlowGridview bind Failure:" + msg);
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_FlowGridview bind function error,Detail:" + ex.Message);
        }
        if (dt.Rows.Count!=0)
        {
            DataView view = dt.AsDataView();
            this.Flow_GridView.DataSource = view;    //设置GridView控件的数据源为创建的数据集ds
            Flow_GridView.DataKeyNames = new string[] { "AuditID" };//将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
            this.Flow_GridView.Style.Add("word-break", "keep-all");
            this.Flow_GridView.Style.Add("word-wrap", "normal");
            this.Flow_GridView.DataBind();
        }
    }

    
    //判斷是否是會簽/轉簽狀況。會簽之後不能再‘會簽/轉簽’；轉簽之後不能再會簽。
    public string checkSignType()
    {
        string lastAction = "";
   //     string Action = "";
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        StringBuilder sbrep = new StringBuilder();

        
        try
        {
            UserID = Session["UserID"].ToString().Trim();//获取当前登录用户

            sbval.Append(Session["car_formID"].ToString().Trim()).Append(",")
                .Append(UserID).Append(",")
                ;
       //         .Append(sysid).Append(",")
      //          .Append(tableid).Append(",")
      //          .Append(Session["car_formtype"].ToString().Trim()).Append(",");

            sbrep.Append(sysname + ".dbo.V_EPS_NeedSign");

            dt = DBAccessProc.GetDataTable("Enotes_PK0338_CheckIsCounterSign_1", 0, 1, sbrep.ToString(), sbval.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("Enotes_PK0338_CheckIsCounterSign_1 Failure:" + msg);
            }
            if (dt.Rows.Count!=0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lastAction = dr["LastAction"].ToString().Trim();
         //           Action = dr["Action"].ToString().Trim();
                }

                if (lastAction == "COUNTERSIGN")//會簽，則禁用‘會簽/轉簽’
                {
                    this.td_moreSign.Visible = false;
                    this.td_reject.Visible = false;
                }
                if (lastAction == "FORWARDSIGN")//轉簽，則禁用‘會簽’
                {
                    this.td_moreSign.Visible = false;
                }
            }
            else
            {
                this.td_approve.Visible = false;
                this.td_reject.Visible = false;
                this.td_moreSign.Visible = false;
        //        this.dispatch.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_CheckIsCounterSign function error,Detail:" + ex.Message);
        }
        return lastAction;
    }

    //獲取當前登錄人狀態下的表單ID
    public string GetMyFormID()
    {
        string str = "";

        UserID = Session["UserID"].ToString().Trim();
        //UserID = "PK0338";
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        StringBuilder sbrep = new StringBuilder();
        //       StringBuilder sbrep = new StringBuilder();
        try
        {

            sbval.Append(UserID).Append(",");
           //      .Append(formName).Append(",");

            sbrep.Append(sysname + ".dbo.V_EPS_NeedSign");

        //    sbval.ToString();
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetMyformlist_1", 0, 1, sbrep.ToString(), sbval.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("Enotes_PK0338_GetMyformlist_1 Failure:" + msg);
            }

            if (dt.Rows.Count!=0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    str += dr["FormID"].ToString().Trim() + ";";
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetMyformlist_1 function error,Detail:" + ex.Message);
        }
        return str;
    }



    //根據FormID獲取該表單的所有信息
    public string getFormList()
    {
        System.Data.DataTable dt = new DataTable();
        System.Data.DataTable dt1 = new DataTable();
        StringBuilder sbval = new StringBuilder();

            
        try
        {
            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_GetFormInfo", 0, 1, "", Session["car_formID"].ToString().Trim(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("GA_CarManagerment_PK0338_GetFormInfo Failure:" + msg);
            }

            if (dt.Rows.Count!=0)
            {
                string tmpname = "";
                string tmpDelflag = "";
                string tmpDelby = "";
                string tmpDelon = "";
                string tmpid = "";
                string tmpano = "";
                string tmpaname = "";
                foreach (DataRow dr in dt.Rows)
                {
                    tmpid = dr["FormID"].ToString().Trim();
                    FormNo.Text = dr["FormNo"].ToString().Trim();
                    FactoryArea.SelectedValue = dr["FactoryArea"].ToString().Trim();
                    FeeCode.Text = dr["FeeCode"].ToString().Trim();
                    ManList.Text = dr["ManList"].ToString().Trim();
                    ManNum.Text = dr["ManNum"].ToString().Trim();
                    tmpname = dr["Departuretime"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        Departuretime.Text = Convert.ToDateTime(dr["Departuretime"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                    tmpname = dr["Prereturntime"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        Prereturntime.Text = Convert.ToDateTime(dr["Prereturntime"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                    Destination.Text = dr["Destination"].ToString().Trim();
                    PhoneNo.Text = dr["PhoneNo"].ToString().Trim();
                    Plnum.Text = dr["Plnum"].ToString().Trim();
                    tmpname = dr["Pldate"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        Pldate.Text = Convert.ToDateTime(dr["Pldate"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                    SetCarTypeList();
                    CarType.SelectedValue = dr["CarType"].ToString().Trim();
                    SubCarType.SelectedValue = dr["SubCarType"].ToString().Trim();
                    Purpose.Text = dr["Purpose"].ToString().Trim();
                    IsShare.SelectedValue = dr["IsShare"].ToString().Trim();
                    CarNo.Text = dr["CarNo"].ToString().Trim();

                    setCarnoList();//取得司機姓名清單
                    DriverName.SelectedValue = dr["DriverName"].ToString().Trim();

                    DriverPhoneNo.Text = dr["DriverPhoneNo"].ToString().Trim();
                    DisManNo.Text = dr["DisManNo"].ToString().Trim();
                    DisManName.Text = dr["DisManName"].ToString().Trim();
                    Feedback_Service.SelectedValue = dr["Feedback_Service"].ToString().Trim();
                    Feedback_Clean.SelectedValue = dr["Feedback_Clean"].ToString().Trim();
                    Feedback_Satisfied.SelectedValue = dr["Feedback_Satisfied"].ToString().Trim();
                    tmpname = dr["AcFinishTime"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        AcFinishTime.Text = Convert.ToDateTime(dr["AcFinishTime"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                    tmpname = dr["AcDeparturetime"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        AcDeparturetime.Text = Convert.ToDateTime(dr["AcDeparturetime"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                    tmpname = dr["Acreturntime"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        Acreturntime.Text = Convert.ToDateTime(dr["Acreturntime"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                    Kilometers.Text = dr["Kilometers"].ToString().Trim();
                    OilNum.Text = dr["OilNum"].ToString().Trim();
                    AcManList.Text = dr["AcManList"].ToString().Trim();
                    AcManNum.Text = dr["AcManNum"].ToString().Trim();
                    AcDestination.Text = dr["AcDestination"].ToString().Trim();
                    AcArrive.Text = dr["AcArrive"].ToString().Trim();
                    Routes.Text = dr["Routes"].ToString().Trim();
                    TotalKilometers.Text = dr["TotalKilometers"].ToString().Trim();

                    ApplyMan.Text = dr["ApplyMan"].ToString().Trim();
                    ApplyManNo.Text = dr["ApplyManNo"].ToString().Trim();
                    ApplyDept.Text = dr["ApplyDept"].ToString().Trim();
                    ApplyPhoneNo.Text = dr["ApplyPhoneNo"].ToString().Trim();
                    ApplyArea.Text = dr["ApplyArea"].ToString().Trim();
                    tmpname = dr["ApplyManDeptCode"].ToString().Trim();    //此欄位為20181013添加,所以需要判斷是否有值,若沒有,需要取值
                    if (tmpname != "" && tmpname != null)
                    {
                        ApplyManDeptCode.Text = tmpname;
                    }
                    else
                    {
                        ApplyManDeptCode.Text = GetDeptcodeByNo(ApplyManNo.Text);
                    }
                    


                    LFactoryArea.Text = dr["FactoryArea"].ToString().Trim();
                    LFeeCode.Text = dr["FeeCode"].ToString().Trim();
                    LManList.Text = dr["ManList"].ToString().Trim();
                    LManNum.Text = dr["ManNum"].ToString().Trim();
                    tmpname = dr["Departuretime"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        LDeparturetime.Text = Convert.ToDateTime(dr["Departuretime"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                    tmpname = dr["Prereturntime"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        LPrereturntime.Text = Convert.ToDateTime(dr["Prereturntime"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                    LDestination.Text = dr["Destination"].ToString().Trim();
                    LPhoneNo.Text = dr["PhoneNo"].ToString().Trim();
                    LPlnum.Text = dr["Plnum"].ToString().Trim();
                    tmpname = dr["Pldate"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        LPldate.Text = Convert.ToDateTime(dr["Pldate"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                     
                    tmpname= dr["CarType"].ToString().Trim();
                    if (tmpname == "OfficalCar") {
                        LCarType.Text = "公務車";
                    }
                    if (tmpname == "SelfDrive")
                    {
                        LCarType.Text = "自駕車";
                    }
                    if (tmpname == "SendCar")
                    {
                        LCarType.Text = "派車";
                    }
                    
                    tmpname = dr["SubCarType"].ToString().Trim();
                    if (tmpname == "Zinge")
                    {
                        LSubCarType.Text = "Zinge公務車";
                    }
                    if (tmpname == "Innova")
                    {
                        LSubCarType.Text = "Innova公務車";
                    }
                    if (tmpname == "Toyota")
                    {
                        LSubCarType.Text = "Toyota主管座車";
                    }
                    if (tmpname == "RCK8351")
                    {
                        LSubCarType.Text = "公務車-RCK8351";
                    }
                    if (tmpname == "RCK8352")
                    {
                        LSubCarType.Text = "公務車-RCK8352";
                    }
                    LPurpose.Text = dr["Purpose"].ToString().Trim();
                    LIsShare.Text = dr["IsShare"].ToString().Trim();
                    LCarNo.Text = dr["CarNo"].ToString().Trim();
                    LDriverName.Text = dr["DriverName"].ToString().Trim();
                    LDriverPhoneNo.Text = dr["DriverPhoneNo"].ToString().Trim();
                    tmpname = dr["DisManNo"].ToString().Trim();
                    if (tmpname!="")
                    {
                    LDisMan.Text = dr["DisManName"].ToString().Trim()+"("+dr["DisManNo"].ToString().Trim()+")";
                    }
                    
                    LFeedback_Service.Text = dr["Feedback_Service"].ToString().Trim();
                    LFeedback_Clean.Text = dr["Feedback_Clean"].ToString().Trim();
                    LFeedback_Satisfied.Text = dr["Feedback_Satisfied"].ToString().Trim();
                    tmpname = dr["AcFinishTime"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        LAcFinishTime.Text = Convert.ToDateTime(dr["AcFinishTime"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                    tmpname = dr["AcDeparturetime"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        LAcDeparturetime.Text = Convert.ToDateTime(dr["AcDeparturetime"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                    tmpname = dr["Acreturntime"].ToString().Trim();
                    if (tmpname != "" && tmpname != null && (tmpname.IndexOf("1900") <= -1))
                    {
                        LAcreturntime.Text = Convert.ToDateTime(dr["Acreturntime"]).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                    }
                    LKilometers.Text = dr["Kilometers"].ToString().Trim();
                    LOilNum.Text = dr["OilNum"].ToString().Trim();
                    LAcManList.Text = dr["AcManList"].ToString().Trim();
                    LAcManNum.Text = dr["AcManNum"].ToString().Trim();
                    LAcDestination.Text = dr["AcDestination"].ToString().Trim();
                    LAcArrive.Text = dr["AcArrive"].ToString().Trim();
                    LRoutes.Text = dr["Routes"].ToString().Trim();
                    LTotalKilometers.Text = dr["TotalKilometers"].ToString().Trim();

                    PreRoutes.Text = dr["PreRoutes"].ToString().Trim();
                    LPreRoutes.Text = dr["PreRoutes"].ToString().Trim();
                    PreKilometers.Text = dr["PreKilometers"].ToString().Trim();
                    LPreKilometers.Text = dr["PreKilometers"].ToString().Trim();


                    tmpname = dr["ApplyDate"].ToString().Trim();
                    if (tmpname != "" && tmpname != null)
                    {
                        ApplyDate.Text = Convert.ToDateTime(dr["ApplyDate"]).ToString("yyyy-MM-dd").Trim();
                    }
                   
                    formYear = Convert.ToDateTime(dr["ApplyDate"]).Year.ToString();
                    formMonthDay = string.Format("{0:MMdd}", Convert.ToDateTime(dr["ApplyDate"]));

                    tmpDelflag = dr["DeleteFlag"].ToString().Trim();
                    tmpDelby = dr["DeletedBy"].ToString().Trim();
                    tmpDelon = dr["DeletedOn"].ToString().Trim();
                }

                if (tmpDelflag == "1")
                {
                    Panel_Action.Visible = false;
                    Panel_ForStatus.Visible = true;
                    if (tmpDelby != "")
                    {
                        Label_ForStatus.Text = "此表單已被 " + tmpDelby + " 於 " + tmpDelon + " 作廢!";
                    }
                    else
                    {
                        Label_ForStatus.Text = "此表單已作廢!";
                    }
                }
                else
                {
                    Label_ForStatus.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_GetFormDataListInfo function error,Detail:" + ex.Message);
        }
        return Session["car_formID"].ToString().Trim();
    }


    [Ajax.AjaxMethod]
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
            if (dt.Rows.Count!=0)
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

    [Ajax.AjaxMethod]
    public string GetUserNameDept(string user)
    {
        //string user = this.TextBox1.Text.ToString();
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        string UserName = "";//聲明一個變量接受查詢數據
        string Dept = "";
        try
        {
            //設定連接字串
            DBAccessProc.setConnStr(strCon);
            sbval.Append(user).Append(","); //定義變量用於獲取變更的參數

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetUserNameDept", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            if (dt.Rows.Count!=0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    UserName = dr["Name"].ToString();
                    Dept = dr["Dept"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetUserNameDept error,Detail:" + ex.Message);
            sbrep.Clear();
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("Enotes_PK0338_GetUserNameDept Failure:" + msg);

        }

        sbrep.Clear();
        sbval.Clear();

        return UserName + "|||" + Dept;
    }




    //取得當前文件使用的流程名稱
    public string getFormType(string sysid, string tableid)
    {
        System.Data.DataTable dt = new DataTable();
  //      StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        string Formtype = "";//聲明一個變量接受查詢數據
        try
        {
            //設定連接字串
            DBAccessProc.setConnStr(strCon);
            sbval.Append(sysid).Append(",")
                .Append(tableid).Append(","); //定義變量用於獲取變更的參數

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetFormType", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            if (dt.Rows.Count!=0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    Formtype = dr["FormType"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetFormType error,Detail:" + ex.Message);
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("Enotes_PK0338_GetFormType Failure:" + msg);

        }

        sbval.Clear();

        return Formtype;
    }

    //取得當前文件ID
    public string getFormID(string formno)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        string fid = "";//聲明一個變量接受查詢數據
        try
        {
            //設定連接字串
            DBAccessProc.setConnStr(strCon);
            sbval.Append(formno).Append(",");
            sbrep.Append(sysname + ".dbo.V_EPS_DataAll").Append(",");

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetFormID", 0, 1, sbrep.ToString(), sbval.ToString(), ",", out msg, out effectRow);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    fid = dr["FormID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetFormID error,Detail:" + ex.Message);
            sbval.Clear();
            sbrep.Clear();
        }

        if ("" != msg)
        {
            Response.Write("Enotes_PK0338_GetFormType Failure:" + msg);

        }

        sbval.Clear();

        return fid;
    }

    // 保存附件信息
    public void SaveFile_Click(object sender, EventArgs e)
    {
        string userID = "";
        string filename = "";
        string filepath = "";
        string souc_route = "";
        string dest_route = "";
        string attfieldname = "";
        url = Request.Url.ToString();

        StringBuilder sbval = new StringBuilder();
        if ((Session["UserID"] == null) || (Session["UserID"].ToString().Trim() == ""))
        {
            Response.Redirect("http://10.56.69.77/GA_CarManagement?refer=" + base64.base64encode(url));
        }
        else
        {
            userID = Session["UserID"].ToString().Trim();//获取当前登录用户
        }


        string tmp_id = Session["car_formID"].ToString().Trim();  //取得當前文件ID

        if (tmp_id == "")        //若為新文件,且沒有儲存,則需要先儲存,然後取得ID
        {
            Session.Add("car_formNo", GetformNo("CA"));
            FormNo.Text = Session["car_formNo"].ToString().Trim();
            string UserID = Session["UserID"].ToString().Trim();//获取当前登录用户
            string formNo = Session["car_formNo"].ToString().Trim();
            string applyman = ApplyMan.Text.ToString();
            string applymanno = ApplyManNo.Text.ToString();
            string applydept = ApplyDept.Text.ToString();
            string applydate = ApplyDate.Text.ToString();
            string applyphoneno = ApplyPhoneNo.Text.ToString();
            string applyarea = ApplyArea.Text.ToString();


            Session.Add("car_status", "edit");

            //sql查詢 formid
            System.Data.DataTable dt = new DataTable();
            try
            {
                //將查詢數據以DataTable形式取出
                dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_GetFormidForFirstSave", 0, 1, "", Session["car_formNo"].ToString().Trim(), ",", out msg, out effectRow);
                if (dt.Rows.Count != 0)
                {
                    Session.Add("car_formID", dt.Rows[0][0].ToString());
                    tmp_id = Session["car_formID"].ToString().Trim();
                }
                if ("" != msg)
                {
                    Response.Write("GA_CarManagerment_PK0338_GetFormidForFirstSave Failure:" + msg);
                }
            }
            catch (Exception ex)
            {
                Response.Write("GA_CarManagerment_PK0338_GetFormidForFirstSave error,Detail:" + ex.Message);
            }
        }


        string[] uploadfile = this.frontname.Value.Split(';');
        formYear = Convert.ToDateTime(ApplyDate.Text).Year.ToString();
        formMonthDay = string.Format("{0:MMdd}", Convert.ToDateTime(ApplyDate.Text));
        attfieldname = AttFieldName.Value;
        filepath = "/Files/Archived/" + formYear + "/" + formMonthDay + "/" + tmp_id + "/" + attfieldname + "/";
        string dest = Server.MapPath("~") + filepath;

        string souc_route1 = "";
        for (int i = 0; i < uploadfile.Length; i++)
        {
            if (uploadfile[i] != "" && uploadfile[i] != null)
            {
                filename = Path.GetFileName(uploadfile[i]);

                souc_route = WebSitePathHelper.GetFilePath(uploadfile[i]);
                souc_route1 = souc_route.Replace("\\" + filename, "");
                dest_route = dest + filename;
                DateTime datetime = DateTime.Now;
                string dts = string.Format("{0:yyyyMMddHHmmssffff}", datetime);

                //上傳附件如已經在目標文件夾，就舊文件重命名

                if (DirFile.IsExistFile(dest + filename))
                {
                    //FileOperate.FileMove(dest + filename, dest + dts + filename);
                    filename = "(" + dts + ")" + filename;
                    FileOperate.FileMove(souc_route, dest + filename);
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('該文件已存在，如需再次上傳請更改文件名！')", true);
                }
                else
                {


                    if (!System.IO.Directory.Exists(dest))   //判斷是否已經存在該文件夾,如不存在則創建一個文件夾
                    {
                        System.IO.Directory.CreateDirectory(dest);
                    }
                    FileOperate.FileMove(souc_route, dest_route);
                    
                }

                try
                {
                    sbval.Append(tmp_id).Append("{?+?}")
                        .Append(sysid).Append("{?+?}")
                        .Append(tableid).Append("{?+?}")
                        .Append(filepath).Append("{?+?}")
                        .Append(filename).Append("{?+?}")
                            .Append(userID).Append("{?+?}")
                            .Append(userID).Append("{?+?}")
                            .Append(attfieldname).Append("{?+?}");
                    sbval.ToString();

                    DBAccessProc.ExecuteSQL("Enotes_PK0338_InsertUploadFiles", " ", sbval.ToString(), "{?+?}", out msg, out rowcount, out effectRow);
                    sbval.Clear();

                    if (msg != "")
                    {
                        Response.Write("Enotes_PK0338_InsertUploadFiles Error:" + msg);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    Response.Write("Enotes_PK0338_InsertUploadFiles function error:" + ex.Message);
                }

            }
        }
        if (souc_route1 != "")
        {
            System.IO.Directory.Delete(souc_route1, true);
            souc_route1 = "";
        }
        

        //      ScriptManager.RegisterStartupScript(UpdatePanel4, this.GetType(), "LoadAtt", "LoadAttach();", true); //关闭加载div

        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "LoadAttach()", true);

        //     ScriptManager.RegisterStartupScript(UpdatePanel4, this.GetType(), "HiddenDiv", "CloseDiv();", true); //关闭加载div

        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "CloseDiv()", true);
    }

    //由相對路徑獲取絕對路徑
    private string urlconvertorlocal(string imagesurl1)
    {
        string tmpRootDir = Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
        string imagesurl2 = tmpRootDir + imagesurl1.Replace(@"/", @"\"); //转换成绝对路径
        return imagesurl2;
    }

    [Ajax.AjaxMethod]
    //根據FormID獲取該表單的所有上傳的附件名稱========附檔功能用
    public string getExistFiles(string attfieldname, string formYear, string formMonthDay, string formID)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        string fileList = "";
        //string[] filenameall;
        try
        {
            //filenameall = System.IO.Directory.GetFiles(urlconvertorlocal(@"/Files/Archived/" + formYear + '/' + formMonthDay + '/' + formID + '/' + attfieldname + '/'));

            sbval.Append(formID).Append("{?+?}")
                        .Append(sysid).Append("{?+?}")
                        .Append(tableid).Append("{?+?}")
                        .Append(attfieldname).Append("{?+?}");
            //sbval.ToString();

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetExistFiles", 0, 1, "", sbval.ToString(), "{?+?}", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("Enotes_PK0338_GetExistFiles Failure:" + msg);
                return "";
            }

            if (dt.Rows.Count!=0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    fileList += dr["FileName"].ToString().Trim().Replace("+", "%2b") + ";";

                }
            }
        }
        catch (Exception ex)
        {
            string aaa = ex.Message;
            Response.Write("Enotes_PK0338_GetExistFiles function error,Detail:" + ex.Message);
        }

        return fileList;

    }


    [Ajax.AjaxMethod]
    //根據文件名獲取后綴名========附檔功能用
    public string getFilesExtens(string fileName)
    {
        string fileExtension = Path.GetExtension(fileName).ToUpper();
        return fileExtension;
    }

    //從database中取得沒有權限的panel list,並將其設定為隱藏
    public void SetAuthorization(string currentnode, string ismanager)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        StringBuilder sbrep = new StringBuilder();
        string AR = "";
        string sqlstring = "";

        try
        {
            sbrep.Append(" and IsEdit is NULL ").Append("{?+?}");
            if (ismanager == "Y")
            {
                sqlstring = "Enotes_PK0338_GetAuthorization_ForManager_1";
                sbval.Append(Session["car_formtype"].ToString().Trim()).Append("{?+?}");
            }
            else
            {
                sqlstring = "Enotes_PK0338_GetAuthorizationForNot";
                sbval.Append(Session["car_formtype"].ToString().Trim()).Append("{?+?}")
                        .Append(currentnode).Append("{?+?}");
            }

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable(sqlstring, 0, 1, sbrep.ToString(), sbval.ToString(), "{?+?}", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("Enotes_PK0338_GetAuthorization Failure:" + msg);

            }
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AR = dr["AREngName"].ToString().Trim();

                    ((Panel)this.Form.FindControl("ContentPlaceHolder1").FindControl(AR)).Style.Add("display", "none");

                    if (AR == "Panel_ApplyCtrl_1")
                    {
                        ((Panel)this.Form.FindControl("ContentPlaceHolder1").FindControl("Panel_Att0")).Style.Add("display", "none");
                    }

                }
            }
            else
            {
                ((Panel)this.Form.FindControl("ContentPlaceHolder1").FindControl("Panel_ApplyCtrl")).Style.Add("display", "none");
                ((Panel)this.Form.FindControl("ContentPlaceHolder1").FindControl("Panel_DriverInfo")).Style.Add("display", "none");
                ((Panel)this.Form.FindControl("ContentPlaceHolder1").FindControl("Panel_Feedback")).Style.Add("display", "none");
                ((Panel)this.Form.FindControl("ContentPlaceHolder1").FindControl("Panel_ApplyCtrl_1")).Style.Add("display", "none");
                ((Panel)this.Form.FindControl("ContentPlaceHolder1").FindControl("Panel_Att0")).Style.Add("display", "none");
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetAuthorization function error,Detail:" + ex.Message);
        }

    }

    //從database中取得有權限的panel list,並將其設定為可編輯
    public List<string> GetAuthorization(string currentnode, string ismanager)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        StringBuilder sbrep = new StringBuilder();
      //  string AR = "";
        string sqlstring = "";

        try
        {
            sbrep.Append(" and IsEdit='Y' ").Append("{?+?}");

            if (ismanager == "Y")
            {
                sqlstring = "Enotes_PK0338_GetAuthorization_ForManager_1";
                sbval.Append(Session["car_formtype"].ToString().Trim()).Append("{?+?}");
            }
            else
            {
                sqlstring = "Enotes_PK0338_GetAuthorization";
                sbval.Append(Session["car_formtype"].ToString().Trim()).Append("{?+?}")
                        .Append(currentnode).Append("{?+?}");
            }

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable(sqlstring, 0, 1, sbrep.ToString(), sbval.ToString(), "{?+?}", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("Enotes_PK0338_GetAuthorization Failure:" + msg);

            }
            if (dt.Rows.Count!=0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                   // AR = dr["AREngName"].ToString().Trim();
                    ARList.Add(dr["AREngName"].ToString().Trim());

                }
            }
        }
        catch (Exception ex)
        {
          Response.Write("Enotes_PK0338_GetAuthorization function error,Detail:" + ex.Message);
          sbval.Clear();
          sbrep.Clear();
        }
        sbval.Clear();
        sbrep.Clear();
        return ARList;
    }

    //根據登錄人員工號，獲取此人所在部級部門，在‘輔助對話框頁簽中’，課級單位默認顯示該部級部門下
    public string GetDefaultClassLevel()
    {
        UserID = Session["UserID"].ToString().Trim();//获取当前登录用户
        //UserID = "PK0338";
        string defaultDept = "";
        System.Data.DataTable dt = new DataTable();

        try
        {
            DBAccessProc.setConnStr(strCon);

            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetDefaultSupiorDept", 0, 1, "", UserID, ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("GetDefaultClassLevel Failure:" + msg);
                return "";
            }
            if (dt.Rows.Count!=0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    defaultDept = dr["DeptName"].ToString().Trim();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Approve GetDefaultClassLevel function error,Detail:" + ex.Message);
        }
        return defaultDept;
    }



    //取得當前表單所有的附檔欄位名稱==========附檔功能使用
    [Ajax.AjaxMethod]
    public string GetAttachNameList(string sysid, string tableid)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        string namelist = "";//聲明一個變量接受查詢數據
        try
        {
            DBAccessProc.setConnStr(strCon);//設定連接字串
            sbval.Append(sysid).Append(",")
                .Append(tableid).Append(","); //定義變量用於獲取變更的參數

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetAttachNameList", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            if (dt.Rows.Count!=0)
            {
                namelist = dt.Rows[0]["AttachNameList"].ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetAttachNameList function error,Detail:" + ex.Message);
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("Enotes_PK0338_GetAttachNameList Failure:" + msg);

        }

        sbval.Clear();


        return namelist;

    }


    //設定廠區欄位DropDownList綁定
    protected void setCarnoList()
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        StringBuilder sbrep = new StringBuilder();

        string farea = "";
        farea = FactoryArea.SelectedValue.ToString();

        if (farea != "0") sbrep.Append(" and FactoryArea=@P1{?,?} ");
        else sbrep.Append("{?,?}");

        sbval.Append(farea).Append("{?,?}");

        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_GetDriverName", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectRow);
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_GetDriverName function error,Detail:" + ex.Message);
        }

        if ("" != msg)
        {
            Response.Write("GA_CarManagerment_PK0338_GetDriverName Failure:" + msg);
            return;
        }
        if (dt.Rows.Count!=0)
        {
            DriverName.DataSource = dt.DefaultView;
            DriverName.DataValueField = "DriverName";
            DriverName.DataTextField = "DriverName";
            DriverName.DataBind();
            DriverName.Items.Insert(0, new ListItem("------Please Select------", ""));
            DriverName.SelectedIndex = 0;
        }
    }

    [Ajax.AjaxMethod]
    public string GetDriverInfo(string fareavalue,string drnamevalue)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        StringBuilder sbrep = new StringBuilder();

        if (drnamevalue == "0")
        {
            return "";
        }
        else {

            sbval.Append(fareavalue).Append("{?,?}")
             .Append(drnamevalue).Append("{?,?}");

            sbrep.Append(" and FactoryArea=@P1{?,?} ")
                .Append(" and DriverName=@P2{?,?} ")
                .Append("{?,?}")
                .Append("{?,?}");
        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_GetDriverInfo", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectRow);
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_GetDriverInfo function error,Detail:" + ex.Message);
        }

        if ("" != msg)
        {
            Response.Write("GA_CarManagerment_PK0338_GetDriverInfo Failure:" + msg);
            return "";
        }
        string returnstr1 = "";
        string returnstr2 = "";
        if (dt.Rows.Count!=0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                returnstr1 = dr["CarNo"].ToString(); 
                returnstr2=dr["DriverPhoneNo"].ToString(); 
            }
        }
        return returnstr1 + "|||" + returnstr2;
       }
    }

    [Ajax.AjaxMethod]
    public string GetDriverInfoForArea(string farea,string cartype)
    {
        StringBuilder sbval = new StringBuilder();
        StringBuilder sbrep = new StringBuilder();

        sbval.Append(farea).Append("{?,?}")
            .Append(cartype).Append("{?,?}");

        if (cartype != "")
        {
            sbrep.Append(" and Brand=@P2{?,?} ");
        }
        else {
            sbrep.Append("{?,?}");
        }

        System.Data.DataTable dt = new DataTable();

            try
            {
                DBAccessProc.setConnStr(strCon);
                dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_GetDriverInfoForArea", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectRow);
            }
            catch (Exception ex)
            {
                Response.Write("GA_CarManagerment_PK0338_GetDriverInfoForArea function error,Detail:" + ex.Message);
            }

            if ("" != msg)
            {
                Response.Write("GA_CarManagerment_PK0338_GetDriverInfoForArea Failure:" + msg);
                return "";
            }
            string returnstr1 = "";
            string returnstr2 = "";
            if (dt.Rows.Count!=0)
            {
                /*
                if (farea == "TW")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        returnstr1 = dr["DriverName"].ToString();
                        returnstr2 = dr["DriverPhoneNo"].ToString();
                    }
                    return returnstr1 + "|||" + returnstr2;
                }
                else
                 
                {* */
                    foreach (DataRow dr in dt.Rows)
                    {
                        returnstr1 = returnstr1 + dr["DriverName"].ToString() + "|||";
                    }
                    if (returnstr1 != "")
                    {
                        returnstr1 = returnstr1.Substring(0, returnstr1.Length - 3);
                    }
                    return returnstr1;
             //   }
            }
            else {
                return "";
            }
            
        }


    //刪除副檔功能
    protected void DeleteAttFile(object sender, EventArgs e)
    {
        string userID = "";
        string filepath = "";
        string attfieldname = "";
        url = Request.Url.ToString();

        StringBuilder sbval = new StringBuilder();
        if ((Session["UserID"] == null) || (Session["UserID"].ToString().Trim() == ""))
        {
            Response.Redirect("http://10.56.69.77/GA_CarManagement?refer=" + base64.base64encode(url));
        }
        else
        {
            userID = Session["UserID"].ToString().Trim();//获取当前登录用户
        }


        string tmp_id = Session["car_formID"].ToString().Trim();  //取得當前文件ID

        string[] uploadfile = this.frontname.Value.Split(';');
        formYear = Convert.ToDateTime(ApplyDate.Text).Year.ToString();
        formMonthDay = string.Format("{0:MMdd}", Convert.ToDateTime(ApplyDate.Text));
        attfieldname = AttFieldName.Value;  //隱藏欄位取得當前需要刪除的副檔欄位名稱
        filepath = "../Files/Archived/" + formYear + "/" + formMonthDay + "/" + tmp_id + "/" + attfieldname + "/";
        string dest = Server.MapPath("~") + filepath;

        if (System.IO.Directory.Exists(dest))   //判斷是否已經存在該文件夾,如存在則刪除
        {
            System.IO.Directory.Delete(dest,true);
               
        }

                try
                {
                    sbval.Append(tmp_id).Append("{?+?}")
                        .Append(sysid).Append("{?+?}")
                        .Append(tableid).Append("{?+?}")
                        .Append(attfieldname).Append("{?+?}")
                        .Append(userID).Append("{?+?}");
                    sbval.ToString();

                    DBAccessProc.ExecuteSQL("Enotes_PK0338_DeleteAttachFile", " ", sbval.ToString(), "{?+?}", out msg, out rowcount, out effectRow);
                    sbval.Clear();

                    if (msg != "")
                    {
                        Response.Write("Enotes_PK0338_DeleteAttachFile Error:" + msg);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    Response.Write("Enotes_PK0338_DeleteAttachFile function error:" + ex.Message);
                }

            //}
        //}


        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "LoadAttach()", true);

        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "CloseDelDiv()", true);
    }


    //判斷當前人員是否是平台管理員
    public void checkmanager(string sysid, string userid)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        try
        {
            DBAccessProc.setConnStr(strCon);//設定連接字串
            sbval.Append(sysid).Append(",")
                .Append(userid).Append(","); //定義變量用於獲取變更的參數

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_IsManagerExist_1", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            if (dt.Rows.Count != 0)
            {
                Session.Add("car_IsSysmanager", "Y");
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_IsManagerExist function error,Detail:" + ex.Message);
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("Enotes_PK0338_IsManagerExist Failure:" + msg);

        }
        sbval.Clear();

    }

    //判斷當前人員是否是當前系統管理員
    public void checksysman(string userid)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        try
        {
            DBAccessProc.setConnStr(strCon);//設定連接字串
            sbval.Append(userid).Append(","); //定義變量用於獲取變更的參數
            sbrep.Append(sysname + ".dbo.List_SysManager");

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_CheckSysman", 0, 1, sbrep.ToString(), sbval.ToString(), ",", out msg, out effectRow);
            if (dt.Rows.Count != 0)
            {
                Session.Add("car_IsSysmanager", "Y");
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_CheckSysman function error,Detail:" + ex.Message);
            sbrep.Clear();
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("Enotes_PK0338_CheckSysman Failure:" + msg);

        }

        sbrep.Clear();
        sbval.Clear();

    }

    public static bool Is32Formatted(string input)
    {
        try
        {
            Convert.ToInt32(input);
            return true;
        }
        catch
        {
            return false;
        }
    }


    [Ajax.AjaxMethod]
    public string GetCarTypeForArea(string farea)
    {
        StringBuilder sbval = new StringBuilder();

        sbval.Append(farea).Append("{?,?}");


        System.Data.DataTable dt = new DataTable();

        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_GetCarType", 0, 1, "", sbval.ToString(), "{?,?}", out msg, out effectRow);
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_GetCarType function error,Detail:" + ex.Message);
        }

        if ("" != msg)
        {
            Response.Write("GA_CarManagerment_PK0338_GetCarType Failure:" + msg);
            return "";
        }
        string returnstr = "";
        string returnstr1 = "";
        string returnstr2 = "";
        if (dt.Rows.Count != 0)
        {
           
                foreach (DataRow dr in dt.Rows)
                {
                    returnstr1 = returnstr1 + dr["CarTypeChi"].ToString() + "|||";
                    returnstr2 = returnstr2 + dr["CarTypeEng"].ToString() + "|||";  
                }
                returnstr = returnstr1.Substring(0, returnstr1.Length - 3) + "***" + returnstr2.Substring(0, returnstr2.Length - 3);
                return returnstr;
            
        }
        else
        {
            return "***";
        }

    }

    //設定CarType欄位DropDownList綁定
    protected void SetCarTypeList()
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();

        string farea = "";
        farea = FactoryArea.SelectedValue.ToString();

        sbval.Append(farea).Append("{?,?}");


        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_GetCarType", 0, 1, "", sbval.ToString(), "{?,?}", out msg, out effectRow);
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_GetCarType function error,Detail:" + ex.Message);
        }

        if ("" != msg)
        {
            Response.Write("GA_CarManagerment_PK0338_GetCarType Failure:" + msg);
            return;
        }
        if (dt.Rows.Count != 0)
        {
            CarType.DataSource = dt.DefaultView;
            CarType.DataValueField = "CarTypeEng";
            CarType.DataTextField = "CarTypeChi";
            CarType.DataBind();
            CarType.Items.Insert(0, new ListItem("------Please Select------", ""));
            CarType.SelectedIndex = 0;
        }
    }

    protected void FactoryArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetCarTypeList();
    }

    //根據FormID獲取該表單當前BaseUer人員部門ID
    public string GetNowManDeptCode()
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();

        string nowmandeptcode = "";

        try
        {
            sbval.Append(Session["car_formID"].ToString().Trim()).Append(",")
           //     .Append(sysid).Append(",")
          //      .Append(tableid).Append(",")
                .Append(Session["car_formtype"].ToString().Trim())
                ;


            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetOwnerDeptCodeByFormID_1", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("CarApply_PK0338_GetOwnerDeptCodeByFormID_1 Failure:" + msg);
            }

            if (dt.Rows.Count != 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    nowmandeptcode = dr["OwnerDeptCode"].ToString().Trim();
                    hid_Ownerlist.Value = dr["OwnerList"].ToString().Trim();     //儲存當前簽核者工號,添加於20181016
                } 
            }
            else
            {
                return "";
            }

        }
        catch (Exception ex)
        {
            Response.Write("CarApply_PK0338_GetOwnerDeptCodeByFormID_1 function error,Detail:" + ex.Message);
            return "";
        }
        return nowmandeptcode;
    }

    public string GetDeptcodeByNo(String tmpno)    //根據工號取主文件(不含兼)部門代碼
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();

        string tmpdeptcode = "";

        try
        {
            sbval.Append(tmpno).Append(",")
                ;


            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetDeptCodeByNo", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("CarApply_PK0338_GetDeptCodeByNo Failure:" + msg);
            }

            if (dt.Rows.Count != 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    tmpdeptcode = dr["DeptCode"].ToString().Trim();
                }
            }
            else
            {
                return "";
            }

        }
        catch (Exception ex)
        {
            Response.Write("CarApply_PK0338_GetDeptCodeByNo function error,Detail:" + ex.Message);
            return "";
        }
        return tmpdeptcode;
    }


}