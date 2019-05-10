using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FOX.DAL;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using FOX.COMMON;

public partial class FormReSend : System.Web.UI.Page
{
    MyLibs.encrypt.Base64 base64 = new MyLibs.encrypt.Base64();
    string strCon = "";
    string msg = "";
    int effectRow = 0;
    int rowCount = 0;

    public string CASTATUS = "";//CA狀態
    public string SerialNumberString = "";//CA序號
    public string CAINFO = "";//CA全部信息

    string formNo = "";
    string formDate = "";
    //Dosign參數
    string userID = "";
    string deptCode="";
    string formInfo="";
    string cano="";
    string formTable = "FormCarApply";
    string formIDCol = "FORMID";
    string sysid = "006";
    string tableid = "01";
    string formType = "";
    string formID = "";
    string action = "FORWARDSIGN";
    string comment = "";
    string setNextList = "";//代理人工號
    string setSelectSeq = "";
    string setSelectList = "";
    string notifyList ="";
    string notifyMsg = "";
    string delimiter = ";";
    string factoryarea = "";
    string beginDate1 = "";
    string beginDate2 = "";
    string txtFormID = "";
    string txt_companyname = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        
        strCon = ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString();
        GetInitValue();
        if (!IsPostBack)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("http://10.56.69.77/GA_CarManagement");
            }
            else
            {
                userID = Session["UserID"].ToString().Trim();//获取当前登录用户
            }

            BindData();

        }
    }

    //Tip+DropDownList綁定多個字段
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

            e.Row.Cells[7].Attributes.Add("onmouseout", "this.style.backgroundColor=this.oldcolor;Hide();");

            e.Row.Cells[3].Width = 200;
            e.Row.Cells[3].Attributes.Add("style", "word-break :break-all ; word-wrap:break-word;white-space: normal; ");

            HyperLink lbl_id = (HyperLink)e.Row.FindControl("lbl_id");
            Label FormUrl = (Label)e.Row.FindControl("FormUrl");
            Label FormType = (Label)e.Row.FindControl("FormType");
            Label FormID = (Label)e.Row.FindControl("FormID");
            Label FormNo = (Label)e.Row.FindControl("FormNo");
            lbl_id.NavigateUrl = FormUrl.Text + "?formType=" + base64.base64encode(FormType.Text) + "&formID=" + base64.base64encode(FormID.Text) + "&formno=" + base64.base64encode(FormNo.Text);
            
            //2.DropDownList綁定多個字段值
            string ownerList = ((Label)e.Row.FindControl("txtOwnerList")).Text;//获取每行控件ID为label1的文本值
            DropDownList ddlAgent = e.Row.FindControl("ddlAgent") as DropDownList;
            DataTable dt=ddlAgentList(ownerList);
            if (dt != null)
            {
                for (int i = 5; i < 8; i++)
                {//從第5個字段<firstAgentNoName>開始
                    if (dt.Rows[0][i].ToString() != "")
                    {
                        ddlAgent.Items.Add(new ListItem(dt.Rows[0][i].ToString()));
                    }
                }
                ddlAgent.DataBind();
                ddlAgent.Items.Insert(0, new ListItem("-----請選擇-----", "0"));
            }
        }
    }

    //表單重送
    protected void GridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "BtnReSend")
        {
            string tmp_no = "";
            string strno = "";

            GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;

            HyperLink txtFormNo = (HyperLink)row.FindControl("lbl_id");   //表單編號 
            tmp_no = txtFormNo.Text.ToString();

            if (tmp_no != "")
            {
                string[] nocollection = tmp_no.Split('-');
                strno = nocollection[0];
            }

            //if (strno == "SV")
            //{
            //    formTable = "FormSupplierVisit";
            //}
           
            string agentNoName = ((DropDownList)row.FindControl("ddlAgent")).SelectedItem.Text.ToString();//代理人姓名(工號)
            if (agentNoName != "-----請選擇-----")
            {
                userID = userID = Session["UserID"].ToString().Trim();
                formNo = (string)e.CommandArgument;//表單信息
                formID = ((Label)row.FindControl("txtFormID")).Text.ToString().Trim();
                formType = ((Label)row.FindControl("txtFormType")).Text.ToString().Trim();
                comment = ((TextBox)row.FindControl("txtComment")).Text.ToString().Trim();//意見
                formDate = ((Label)row.FindControl("txtFormDate")).Text.ToString().Trim();
            //    cano = Session["CAString"] == null ? "" : Session["CAString"].ToString().Trim();
                cano = "";
                formInfo = "formNO=" + formNo + ";ApplyDate=" + formDate;
                int left = agentNoName.IndexOf('(');
                int right = agentNoName.IndexOf(')');
                setNextList = agentNoName.Substring(left + 1, right - left - 1);//提取代理人;substring第二個參數是長度
                setSelectList = ((Label)row.FindControl("txtOwnerList")).Text.ToString().Trim();//本人
                DoSign(formTable,formIDCol,formType,formID,userID,deptCode,action
                    ,comment,setNextList,setSelectSeq,setSelectList,notifyList
                    ,notifyMsg,cano,formInfo,delimiter);
            }
            else 
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(),"","alert('未選擇代理人，或沒有代理人，無法重送！');",true);
            }
            
        }
    }

    //翻頁時進行處理
    protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView.PageIndex = e.NewPageIndex;//獲取當前分頁索引值
        BindData();
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
            GridView.DataSource = view;
            //将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
            GridView.DataKeyNames = new string[] { "FormID" };
            //表頭內容不換行
            GridView.Style.Add("word-break", "keep-all");
            GridView.Style.Add("word-wrap", "normal");
            //绑定数据库表中数据
            GridView.DataBind();
        }
    }

    //獲取數據，用於綁定GridView
    protected DataTable getResult()
    {
        userID = Session["UserID"].ToString().Trim();//获取当前登录用户

        DataTable dt = new DataTable();
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


        sbval.Append(userID).Append("{?,?}")
            .Append(txtFormID).Append("{?,?}")
            .Append(factoryarea).Append("{?,?}");

        sbval.Append(beginDate1).Append("{?,?}")
            .Append(beginDate2).Append("{?,?}");

        sbrep.ToString();
        sbval.ToString();
        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_FormReSendList", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectRow);
            if ("" != msg)
            {
                Response.Write("GA_CarManagerment_PK0338_FormReSendList Failure:" + msg);
                return null;
            }
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_FormReSendList function error,Detail:" + ex.Message);
        }
        return dt;
    }

    //查詢
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


    //前臺DropDownList的綁定DataSource
    protected DataTable ddlAgentList(string ownerList)
    {
        DataTable dt = new DataTable();
        
        DBAccessProc.setConnStr(strCon);
        dt = DBAccessProc.GetDataTable("Enotes_PK0338_FormReSendGetAgentID", 0, 1, "", ownerList, ",", out msg, out effectRow);

        return dt;   
    }

    protected void DoSign(string FormTable,string FormIDCol,string FormType,string FormID
        ,string UserID,string DeptCode,string Action,string Comment,string SetNextList,
        string SetSelectSeq,string SetSelectList,string NotifyList,string NofityMsg,
        string CaNo, string FormInfo, string Delimiter) 
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        try
        {
            sbval.Append(FormTable).Append("{?+?}").Append(FormIDCol).Append("{?+?}")
                 .Append(FormType).Append("{?+?}").Append(FormID).Append("{?+?}")
                 .Append(UserID).Append("{?+?}").Append(DeptCode).Append("{?+?}")
                 .Append(Action).Append("{?+?}").Append(Comment).Append("{?+?}")
                 .Append(SetNextList).Append("{?+?}").Append(SetSelectSeq).Append("{?+?}")
                 .Append(SetSelectList).Append("{?+?}").Append(NotifyList).Append("{?+?}")
                 .Append(NofityMsg).Append("{?+?}").Append(CaNo).Append("{?+?}")
                 .Append(FormInfo).Append("{?+?}").Append(";").Append("{?+?}");

            sbval.ToString();
            DBAccessProc.setConnStr(strCon);
            DBAccessProc.ExecuteSQL("Enotes_PK0338_DoSign", "", sbval.ToString(), "{?+?}", out msg, out rowCount, out effectRow);
            sbval.Clear();

            if (msg != "")
            {
               
                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('重送失敗!')" + msg + ";", true); 
                return;
            }
            
            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('重送成功!');window.location.href=window.location.href;", true); 
        }
        catch (Exception e)
        {
            Response.Write("DoSign function error:" + e.Message);
        }
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
      //      beginDate2 = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-dd").Trim();
        }
    }
         
}