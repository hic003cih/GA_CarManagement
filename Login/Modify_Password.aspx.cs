using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Base_Data;
using System.Configuration;

public partial class Modify_Password : System.Web.UI.Page
{
    BaseDataContext dc = new BaseDataContext(ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
            string UserID = Request["UserID"];
            string UserPS = Request["UserPS"];
            //使用LINQ 查詢舊密碼
            var ck_PS = from ck in dc.BRM_USER_INFO
                        where ck.UserID == UserID
                        where ck.password == OldPS.Text
                        select ck;
            if (ck_PS.ToArray().Count() > 0) //密碼正確
            {
                if (!string.IsNullOrEmpty(NewPS.Text) & !string.IsNullOrEmpty(ReNewPS.Text))//檢查Textbox(NewPS and ReNewPS)是否為空
                {
                    if (NewPS.Text == ReNewPS.Text)
                    {
                        //lblMsg.Text = NewPS.Text + ReNewPS.Text;
                        //LINQ update 語法
                        BRM_USER_INFO cust = dc.BRM_USER_INFO.First(c => c.UserID == UserID);
                        cust.password = NewPS.Text;
                        dc.SubmitChanges();
                        //lblMsg.Text = "密碼修改成功";
                        //Response.Write("<script>alert('修改成功');window.location.href='Login.aspx'</script>");
                        ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('修改成功');window.location='Login.aspx'</script>");
                    }
                    else
                    {
                        //lblMsg.Text = "輸入密碼不一致！" + NewPS.Text;
                        //lblMsg.ForeColor = Color.Red;
                        //Response.Write("<script>alert('輸入密碼不一致！')</script>");
                        ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('輸入密碼不一致！');</script>");
                        NewPS.Focus();
                    }
                }
                else
                {
                    //lblMsg.Text = "密碼不得為空！";
                    //密碼不得為空！lblMsg.ForeColor = Color.Red;
                    //Response.Write("<script>alert('密碼不得為空！')</script>");
                    ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('密碼不得為空！');</script>");
                    NewPS.Focus();
                }
            }
            else
            {
                //lblMsg.Text = "密碼錯誤！";
                //lblMsg.ForeColor = Color.Red;
                //Response.Write("<script>alert('密碼錯誤！')</script>");
                ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('密碼錯誤！');</script>");
                OldPS.Focus();
            }
        
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
         Response.Redirect("Login.aspx");
    }
}