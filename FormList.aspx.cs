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
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.Linq.SqlClient; //SqlMethods

public partial class FormList : System.Web.UI.Page
{
    string strCon = "";
    string msg = "";
    int effectRow = 0;
    string UserID = "";
    string SerialNumberString = "";
    string CASTATUS = "";
    string uid;
    string upw;
    string sysname = "GA_GoodsRequest";
    //string resultsTrue = "";
    //string CAstaffNo = "";
    //string staffNo = "";
    BaseDataContext bc = new BaseDataContext();
    HR_LoginDataContext login = new HR_LoginDataContext();
    HRDataContext db = new HRDataContext();
    MyLibs.encrypt.Base64 base64 = new MyLibs.encrypt.Base64();

    protected void Page_Load(object sender, EventArgs e)
    {
        strCon = ConfigurationManager.ConnectionStrings["BASEConnectionString"].ConnectionString;
        Ajax.Utility.RegisterTypeForAjax(typeof(FormList));
        if (!IsPostBack)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("http://10.56.69.77/FitiGroup/");
            }
            else
            {
                UserID = Session["UserID"].ToString().Trim();//获取当前登录用户
            }

            // UserID = "P1266";
            BindData();
        }
    }
    //[Ajax.AjaxMethod]
    //protected int getFormListCountByUserID()
    //{
    //    int formCount = 0;
    //    //當前登錄人的角色
    //    UserID = Session["UserID"].ToString();
    //    DataTable dt = new DataTable();
    //    StringBuilder sbval = new StringBuilder();
    //    sbval.Append(UserID).Append(",");
    //    try
    //    {
    //        DBAccessProc.setConnStr(strCon);
    //        dt = DBAccessProc.GetDataTable("ENotes_PK0338_NeedMyActionQty", 0, 1, ",", sbval.ToString(), ",", out msg, out effectRow);

    //        if ("" != msg)
    //        {
    //            Response.Write("ENotes_PK0338_NeedMyActionQty Failure:" + msg);
    //            return 0;
    //        }
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            formCount = Convert.ToInt32(dr["FormCount"].ToString());
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write("ENotes_PK0338_NeedMyActionQty function error,Detail:" + ex.Message);
    //    }

    //    return formCount;
    //}
   
    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {//每行編號
        if (e.Row.RowIndex != -1)
        {
            int id = e.Row.RowIndex + 1;
            e.Row.Cells[0].Text = id.ToString();
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dv = (DataRowView)e.Row.DataItem;

            string S1 = dv["FormNo"].ToString().Trim();
            string S2 = dv["FormTypeName"].ToString().Trim();
            string S3 = dv["Register_Name"].ToString().Trim();
            string S4 = dv["ApplyDept"].ToString().Trim();
            string S5 = dv["NodeSeq"].ToString().Trim();
            string S6 = dv["Form_Status"].ToString().Trim();
            string S7 = dv["ApplyDate"].ToString().Trim();
            //在頁面中開啟 URL Link
            //var form = from a in bc.V_EPS_FormList where a.FormNo == formno && a.UrlLink != null select new { a.FormID, a.UrlLink, a.FormUrl, a.FormType }; //form基本資料
            var pw = from a in login.EmployeeBase where a.UserID == Session["UserID"] select a.PASSWORD;
            HyperLink lbl_id = (HyperLink)e.Row.FindControl("lbl_id"); //取gridview存入SQL值(HyperLink)
            Label FormID = (Label)e.Row.FindControl("FormID");
            Label UrlLink = (Label)e.Row.FindControl("UrlLink");
            Label FormUrl = (Label)e.Row.FindControl("FormUrl");
            Label FormType = (Label)e.Row.FindControl("FormType");

            //if (form.Count() > 0)
            //{
            //string formid = form.First().FormID.ToString().Trim();//判斷formid，若formid=0為Xpages
            if (int.Parse(FormID.Text) > 0)
            {
                lbl_id.NavigateUrl = UrlLink.Text + "&uid=" + base64.base64encode(Session["UserID"].ToString());
                //用戶ID
            }
            else
            {
                lbl_id.NavigateUrl = UrlLink.Text + "login&username=" + Session["UserID"] + "&password=" + pw.First().ToString() + "&RedirectTo=" + FormUrl.Text + "&documentId=" + FormType.Text + "&action=editDocument";
            }
            ////当鼠标停留时更改背景色
            //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#c0d3da'");
            ////当鼠标移开时还原背景色
            //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");


        }
    }
    protected void logout(object sender, EventArgs e)
    {
        Session.Abandon();  //清空 Sesion
        Response.Redirect("http://10.56.69.77/FitiGroup/");
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

    //GridView綁定數據
    protected void BindData()
    {

        DataTable dt = new DataTable();
        dt = getResult();
        DataView view = dt.AsDataView();

        //设置GridView控件的数据源为创建的数据集ds
        GridView.DataSource = view;
        //将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
        GridView.DataKeyNames = new string[] { "FormNo" };
        //表頭內容不換行
        GridView.Style.Add("word-break", "keep-all");
        GridView.Style.Add("word-wrap", "normal");
        //====================Fromat===============
        GridView.Font.Size = 11;
        GridView.GridLines = GridLines.Both;	//設定格線
        GridView.PageSize = 15;    //10行
        GridView.PagerSettings.Position = PagerPosition.Bottom; //分頁位置
        GridView.Font.Name = "微軟正黑體";		//設定字型大小
        GridView.AlternatingRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#e6e6e6");
        GridView.PagerStyle.HorizontalAlign = HorizontalAlign.Center; //分頁對齊
        GridView.AllowPaging = true;
        GridView.PagerStyle.Font.Size = 12;
        //====================Fromat===============
        ////绑定数据库表中数据
        GridView.DataBind();
    }

    //獲取數據，用於綁定GridView
    protected DataTable getResult()
    {
        UserID = Session["UserID"].ToString().Trim();//获取当前登录用户
        //UserID = "P1266";
        //select選擇框，選擇后跳轉頁面
        string FormType = "";//1->表單單據類別
        if (Request.QueryString.Count != 0)
        {
            FormType = Request.QueryString["FormType"].ToString();
        }

        DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        string txtFormID = "";
        string beginDate1 = "";
        string beginDate2 = "";

        if (beginDate1 != "" && beginDate2 != "")
        {
            if (Convert.ToDateTime(beginDate1) > Convert.ToDateTime(beginDate2))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('起始日期不能大於終止日期');", true);
            }
        }
        sbrep.Append(sysname + ".dbo.V_EPS_NeedSign");
        if (txtFormID != "") sbrep.Append(" and  FormNo=@P2, ");
        else sbrep.Append(",");
        if (beginDate1 != "") sbrep.Append(" and  ApplyDate>=@P3, ");
        else sbrep.Append(",");
        if (beginDate2 != "") sbrep.Append(" and  ApplyDate<=@P4, ");
        else sbrep.Append(",");
        if (FormType != "") sbrep.Append(" and  FormType=@P5, ");
        else sbrep.Append(",");

        sbval.Append(UserID).Append(",");
            //.Append(txtFormID).Append(",")
            //.Append(beginDate1).Append(",")
            //.Append(beginDate2).Append(",")
            //.Append(FormType).Append(",")
        
        sbrep.ToString();
        sbval.ToString();
        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("ENotes_P1266_NeedMyAction", 0, 1, sbrep.ToString(), sbval.ToString(), ",", out msg, out effectRow);
            if ("" != msg)
            {
                Response.Write("ENotes_P1266_NeedMyAction Failure:" + msg);
                return null;
            }
            //dt = DBAccessProc.GetDataTable("ENotes_PK0338_NeedMyAction", 0, 1, sbrep.ToString(), sbval.ToString(), ",", out msg, out effectRow);
            //if ("" != msg)
            //{
            //    Response.Write("ENotes_PK0338_NeedMyAction Failure:" + msg);
            //    return null;
            //}
        }
        catch (Exception ex)
        {
            Response.Write("ENotes_PK0338_NeedMyAction function error,Detail:" + ex.Message);
        }
        return dt;
    }

    //查詢
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = getResult();
        GridView.DataSource = dt;
        GridView.DataKeyNames = new string[] { "FormNo" };
        GridView.DataBind();
    }
}