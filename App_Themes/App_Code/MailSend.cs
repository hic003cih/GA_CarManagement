using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;

/// <summary>
/// 呼叫郵件寄送方法(mailfrom,fromname,sendto,ccto,subject,body)
/// </summary>
public class MailSend
{
    public static void SendMailMemo(string mailfrom, string fromName, string sendto, string ccto, string mailsubject, string mailbody, string senderr)
    {
        try
        {
            MailMessage message = new MailMessage();
            if (mailfrom == "" || mailfrom == null)
            {
                mailfrom = "eNotesWeb@foxsemicon.com.tw";
                fromName = "eNotesAdmin";
            }
            message.From = new MailAddress(mailfrom + "@foxsemicon.com.tw", fromName, System.Text.Encoding.UTF8); //mail from 的email及名稱
            if (sendto == "" ||sendto ==null)
            {
                message.To.Add("monica.wl.ou@foxsemicon.com");
                mailsubject = "Web System 找不到收件者,請查看系統!!";
            }
            else
            {
                message.To.Add(sendto);
            }
            if (ccto != "" & ccto != null)
            {
                message.CC.Add(ccto);
            }
            else {
                ccto = "";
            }
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;//E-mail編碼        
            message.Subject = mailsubject;//E-mail主旨
            message.Priority = MailPriority.Normal; //優先權
            message.BodyEncoding = System.Text.Encoding.UTF8;   //用於設定 body HTML 格式 
            message.IsBodyHtml = true;
            var bodymsg = new System.Net.Mail.MailMessage();
            bodymsg.Body = mailbody;        //將body 字串填入
            message.Body = bodymsg.Body;    //取得 body 字串
            SmtpClient smtpClient = new SmtpClient("10.56.69.21", 25);//設定E-mail Server和port
            smtpClient.Send(message);
        }
        catch
        {
            MailAddress from_err = new MailAddress("eNotesWeb@foxsemicon.com", "SystemAdmin", System.Text.Encoding.UTF8);
            MailAddress to_err = new MailAddress(senderr);
            MailMessage message_ex = new MailMessage(from_err, to_err);
            message_ex.IsBodyHtml = true;
            message_ex.BodyEncoding = System.Text.Encoding.UTF8;
            message_ex.Subject = "WEB SYSTEM - 郵件發送異常，請查看！！ ";
            message_ex.Body = mailbody;
            SmtpClient smtpClient_ex = new SmtpClient("10.56.69.21", 25);
            smtpClient_ex.Send(message_ex);
        }
    }
}