using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using FOX.DAL;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text;
using FOX.COMMON;

public partial class V_FormEnd : System.Web.UI.Page
{
    MyLibs.encrypt.Base64 base64 = new MyLibs.encrypt.Base64();
    string strCon = "";
    string msg = "";
    int effectRow = 0;
    int rowcount = 0;
    string UserID = "";

    string txtFormID = "";
    string sysid = "006";
    string tableName = "GA_CarManagerment.dbo.FormCarApply";

    string factoryarea = "";
    string beginDate1 = "";
    string beginDate2 = "";
    string sysname = "GA_CarManagerment";


    protected void Page_Load(object sender, EventArgs e)
    {
        strCon = ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString();
        GetInitValue();

        if (!IsPostBack)
        {
            Session.Add("car_IsSysmanager", "N");

            if (Session["UserID"] == null)
            {
                Response.Redirect("http://10.56.69.77/GA_CarManagement");
            }
            else
            {
                UserID = Session["UserID"].ToString().Trim();//获取当前登录用户

                checkmanager(sysid, UserID);
                checksysman(UserID);

                string ismag = Session["car_IsSysmanager"].ToString().Trim();

                if (ismag == "Y")
                {
                    Panel_Text.Visible = false;
                    Panel_Content.Visible = true;
                    DataTable dt = new DataTable();
                    dt = getResult();
                    if (dt != null)
                    {
                        GridView.DataSource = dt;
                        GridView.DataKeyNames = new string[] { "FormID" };
                        GridView.DataBind();
                    }

                }
                else
                {
                    Panel_Text.Visible = true;
                    Panel_Content.Visible = false;
                }

                
            }

            DBAccessProc.setConnStr(strCon);

            BindData();

        }
    }

    

    //每行編號
    //Tip
    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //每行編號
        if (e.Row.RowIndex != -1)
        {
            int id = e.Row.RowIndex + 1;
            e.Row.Cells[0].Text = id.ToString();
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dv = (DataRowView)e.Row.DataItem;


            string S1 = dv["FactoryArea"].ToString().Trim();  //
            string S2 = dv["FeeCode"].ToString().Trim(); //
            string S3 = dv["SDate"].ToString().Trim(); //
            string S4 = dv["OutTime"].ToString().Trim(); //
            string S5 = dv["ManList"].ToString().Trim();    //
            string S6 = dv["ManNum"].ToString().Trim() + " 人"; //
            string S7 = dv["PhoneNo"].ToString().Trim();   //           
            string S8 = dv["Destination"].ToString().Trim(); //
            S8 = S8.Replace("\r\n", "; ");
            string S9 = dv["Purpose"].ToString().Trim();   //
            S9 = S9.Replace("\r\n", "; ");

            //如果要使鼠标移到每行的每个单元格都显示就把e.Row.Cells[0].Attributes.Add换成e.Row.Attributes.Add
            e.Row.Cells[1].Attributes.Add("onmouseover", "this.oldcolor=this.style.backgroundColor;this.style.backgroundColor='#C8F7FF';this.style.cursor='hand';");
            e.Row.Cells[1].Attributes.Add("onmousemove", "Show('" + S1 + "','" + S2 + "','" + S3 + "','" + S4 + "','" + S5 + "','" + S6 + "','" + S7 + "','" + S8 + "','" + S9 + "');");

            e.Row.Cells[1].Attributes.Add("onmouseout", "this.style.backgroundColor=this.oldcolor;Hide();");

            e.Row.Cells[5].Width = 200;
            e.Row.Cells[6].Width = 120;
            e.Row.Cells[7].Width = 100;
            e.Row.Cells[5].Attributes.Add("style", "word-break :break-all ; word-wrap:break-word;white-space: normal; ");

            HyperLink lbl_id = (HyperLink)e.Row.FindControl("lbl_id");
            Label FormUrl = (Label)e.Row.FindControl("FormUrl");
            Label FormType = (Label)e.Row.FindControl("FormType");
            Label FormID = (Label)e.Row.FindControl("FormID");
            Label FormNo = (Label)e.Row.FindControl("FormNo");
            lbl_id.NavigateUrl = FormUrl.Text + "?formType=" + base64.base64encode(FormType.Text) + "&formID=" + base64.base64encode(FormID.Text) + "&formno=" + base64.base64encode(FormNo.Text);

        }
    }

    //表單作廢
    protected void GridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string formID = (string)e.CommandArgument;//取得ID值
        string tmp_no = "";
        string strno = "";

        if (e.CommandName == "updateDel")
        {
            System.Data.DataTable dt = new DataTable();
            StringBuilder sbval = new StringBuilder();
            StringBuilder sbrep = new StringBuilder();
            try
            {
                GridViewRow row = ((Control)e.CommandSource).BindingContainer as GridViewRow;
                HyperLink txtFormNo = (HyperLink)row.FindControl("lbl_id");   //表單編號 
                tmp_no = txtFormNo.Text.ToString();

                if (tmp_no != "")
                {
                    string[] nocollection = tmp_no.Split('-');
                    strno = nocollection[0];
                }

                DBAccessProc.setConnStr(strCon);
                sbval.Append(formID).Append(",");
                sbrep.Append(tableName).Append(",");
                DBAccessProc.ExecuteSQL("ENotes_PK0338_Delete", sbrep.ToString(), sbval.ToString(), ",", out msg, out rowcount, out effectRow);
                BindData();
            }
            catch (Exception ex)
            {
                Response.Write("ENotes_PK0338_Delete Form function error,Detail:" + ex.Message);
                sbval.Clear();
            }

            if ("" != msg)
            {
                Response.Write("ENotes_PK0338_Delete Form Failure:" + msg);
                return;
            }
        }
    }

    //翻頁時進行處理
    protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //GridView.PageIndex = e.NewPageIndex;//獲取當前分頁索引值
        //BindData();

        // 得到該控制項
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (e.NewPageIndex == -3)
        {
            //點擊了Go按鈕
            TextBox txtNewPageIndex = null;

            //GridView較DataGrid提供了更多的API，獲取分頁塊可以使用BottomPagerRow 或者TopPagerRow，當然還增加了HeaderRow和FooterRow
            GridViewRow pagerRow = theGrid.BottomPagerRow;

            if (pagerRow != null)
            {
                //得到text控制項
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (txtNewPageIndex != null)
            {
                //得到索引
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        {
            //點擊了其他的按鈕
            newPageIndex = e.NewPageIndex;
        }
        //防止新索引溢出
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;

        //得到新的值
        theGrid.PageIndex = newPageIndex;//獲取當前分頁索引值

        //重新綁定
        BindData();
    }

    //查询
    protected void btnQuery_Click(object sender, EventArgs e)
    {

        string a1 = DateType.SelectedValue.ToString();
        string a2 = txt_applyDate.Text.ToString();
        string a3 = txt_applyDate2.Text.ToString();
        if (a1 != "" && (a2 == "" && a3 == ""))
        {
            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇日期範圍!');", true);
            return;
        }
        if (a2 != "" && a3 != "")
        {
            if (a1 == "")
            {
                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇日期類別!');", true);
                return;
            }
            if (Convert.ToDateTime(a2) > Convert.ToDateTime(a3))
            {
                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('起始日期不能大於終止日期!');", true);
                return;
            }
        }

        DataTable dt = new DataTable();
        dt = getResult();
        if (dt != null)
        {
            GridView.DataSource = dt;
            GridView.DataKeyNames = new string[] { "FormID" };
            GridView.DataBind();
        }
    }


    //GridView綁定數據
    protected void BindData()
    {
        DataTable dt = new DataTable();
        dt = getResult();
        if (dt != null)
        {
            DataView view = dt.AsDataView();
            //设置GridView控件的数据源为创建的数据集ds
            this.GridView.DataSource = view;
            //将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
            this.GridView.DataKeyNames = new string[] { "FormID" };
            //表頭內容不換行
            GridView.Style.Add("word-break", "keep-all");
            GridView.Style.Add("word-wrap", "normal");
            //绑定数据库表中数据
            this.GridView.DataBind();
        }
    }
        
    //獲取數據，用於綁定GridView
    protected DataTable getResult()
    {
        UserID = Session["UserID"].ToString().Trim();
        //string FormType = "";
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();

        txtFormID = txt_Flowformid.Text.ToString();
        string datetype = DateType.SelectedValue.ToString();
        beginDate1 = txt_applyDate.Text.ToString().Trim();
        beginDate2 = txt_applyDate2.Text.ToString().Trim();

        factoryarea = FactoryArea.SelectedValue.ToString().Trim();

        if (txtFormID != "") sbrep.Append(" and FormNo=@P2{?,?} ");
        else sbrep.Append("{?,?}");
        if (factoryarea != "") sbrep.Append(" and FactoryArea=@P3{?,?} ");
        else sbrep.Append("{?,?}");



        if (datetype == "用車日期" && (beginDate1 != "" || beginDate2 != ""))
        {

            if (beginDate1 != "") sbrep.Append(" and (SDate>=@P4){?,?} ");
            else sbrep.Append("{?,?}");
            if (beginDate2 != "") sbrep.Append(" and (SDate<=@P5){?,?} ");
            else sbrep.Append("{?,?}");

        }
        else
        {
            if (beginDate1 != "") sbrep.Append(" and ApplyDate>=@P4{?,?} ");
            else sbrep.Append("{?,?}");
            if (beginDate2 != "") sbrep.Append(" and ApplyDate<=@P5{?,?} ");
            else sbrep.Append("{?,?}");
        }


        sbval.Append(UserID).Append("{?,?}")
            .Append(txtFormID).Append("{?,?}")
            .Append(factoryarea).Append("{?,?}");

        sbval.Append(beginDate1).Append("{?,?}")
            .Append(beginDate2).Append("{?,?}");

        sbrep.ToString();
        sbval.ToString();

        try
        {
            //EPS_PK2764_GetFormInFlow
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_FinishedForManager", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("GA_CarManagerment_PK0338_FinishedForManager Failure:" + msg);
                return null;
            }
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_FinishedForManager error,Detail:" + ex.Message);
        }
        return dt;
    }

    //取基本數據值
    protected void GetInitValue()
    {
        //OwnerInfo = this.txt_TheOwnerInFlow.Text.ToString().Trim();//根據‘簽核人’進行查詢時，有區別于其他欄位 
        txtFormID = this.txt_Flowformid.Text.ToString().Trim();
        beginDate1 = this.txt_applyDate.Text.ToString().Trim();
        beginDate2 = "";
        if (beginDate1 == "")   //為避免資料量過多時,頁面負載過重,default顯示12個月以內的文件
        {
            beginDate1 = Convert.ToDateTime(DateTime.Today.AddMonths(-12)).ToString("yyyy-MM-dd").Trim();
        //    beginDate2 = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-dd").Trim();
        }
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


}