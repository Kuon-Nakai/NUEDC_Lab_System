using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

/// <summary>
/// Summary description for MailSvr
/// </summary>
public class MailSvr
{
    private readonly static MailAddress sendAddr = new MailAddress("dhu_nuedc_system@163.com");
    private readonly static NetworkCredential cd = new NetworkCredential("dhu_nuedc_system@163.com", "IUHNACMOJJNLAAEF");
    private readonly static SmtpClient client = new SmtpClient("smtp.163.com")
    {
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        Credentials = cd
    };
    public static string SendVerificationMail(string target,string subject, string body)
    {
        var code = new Random().Next(0, 9999).ToString();
        var msg = new MailMessage(from: sendAddr, to: new MailAddress(target))
        {
            Subject = subject,
            SubjectEncoding = System.Text.Encoding.UTF8,
            Body = string.Format(body, code),
            BodyEncoding = System.Text.Encoding.UTF8
        };
        client.Send(msg);
        return code;
    }
    public static void SendMail(string target, string subject, string body) => client.Send(new MailMessage(from: sendAddr, to: new MailAddress(target))
    {
        Subject = subject,
        SubjectEncoding = System.Text.Encoding.UTF8,
        Body = body,
        BodyEncoding = System.Text.Encoding.UTF8
    });
    public MailSvr()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}