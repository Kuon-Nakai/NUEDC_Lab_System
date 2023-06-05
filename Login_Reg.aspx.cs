using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

#pragma warning disable IDE0074 // Use compound assignment

public partial class Login_Reg : System.Web.UI.Page
{
    private DynamicControls dc = new DynamicControls();
    MySqlSvr svr;

    #region Cross boundary variables...
    public string Username;
    public string PopupL1 = "请在读卡器刷校园卡...";
    public string PopupL2 = "";
    #endregion

    private string CardCache;
    private string VerifyCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.MaintainScrollPositionOnPostBack = true;

        CardLogin.Attributes["href"] = "#";
        //RestorePsw_bt.Attributes["href"] = "#";

        MasterPage.col = .5f;
        Validate();
        if (svr == null)
        {
            svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
        }
        if (IsPostBack) return;
        if (Session["UserID"] != null)
        {
            // User already logged in
            Username = svr.QuerySingle($"select MemberName from members where MemberCode='{Session["UserID"]}'") as string ?? "Unavailable";
            DataBind();
            Logout_pn.Visible = true;
        }
        //RemoteDelegates.RegisterDelegate("Login_bt_Click", Login_bt_Click);
        //DynamicControls.CreateAlert("请提供登录信息", "error", Alerts_pn);
    }

    protected void Login_bt_Click(object sender, EventArgs e)
    {
        //Login_Acc_ReqVal.Validate();
        //Login_Psw_ReqVal.Validate();
        Page.Validate("Login");
        if (!Login_Acc_ReqVal.IsValid || !Login_Psw_ReqVal.IsValid || LoginAcc_tb.Text.Trim().Length == 0 || LoginPsw_tb.Text.Trim().Length == 0)
        {
            // Fields invalid
            dc.CreateAlert("请提供登录信息", "error", Alerts_pn);
            return;
        }
        if ((string)svr.QuerySingle($"select password from members where MemberCode={LoginAcc_tb.Text};") == LoginPsw_tb.Text)
        {
            // Auth pass
            Session["UserID"] = LoginAcc_tb.Text;
            Session["UserPerm"] = svr.QuerySingle($"select PermissionLevel from members where MemberCode={LoginAcc_tb.Text};");
            try
            {
                dc.CreateAlert($"登录成功, 欢迎 {svr.QuerySingle($"select MemberName from members where MemberCode={Session["LoginAcc"]}")}", "success", Alerts_pn);
            }
            catch (Exception) { }
            if ((((Stack<string>)Session["jmpStack"])?.Count ?? 0) != 0)
            {
                Response.Redirect(((Stack<string>)Session["jmpStack"]).Pop());
            }
            return;
        }
        // Auth fail
        if (svr.QuerySingle($"Select * from members where MemberCode={LoginAcc_tb.Text};").GetType().Name == "DBNull")
        {
            // Account does not exist
            dc.CreateAlert("账号不存在, 请注册", "notice", Alerts_pn);
            Page.SetFocus(RegAcc_tb);
            return;
        }
        // Account exists
        dc.CreateAlert("密码错误, 请重试", "notice", Alerts_pn);
    }

    protected void Reg_bt_Click(object sender, EventArgs ea)
    {
        Validate("Reg");
        if (Reg_Acc_ReqVal.IsValid & Reg_Name_ReqVal.IsValid & Reg_Psw_ReqVal.IsValid & Reg_Psw_CompVal.IsValid &
            (RegMail_tb.Text.Length == 0 || Regex.IsMatch(RegMail_tb.Text, "^[a-zA-Z0-9]+@[a-zA-Z0-9.]+(com|cn|co|net)$")))
        {
            if (svr.Execute($"Insert into members values({RegAcc_tb.Text}, '{RegName_tb.Text}', 4, NOW(), NOW(), null, '{RegPsw0_tb.Text}')",
                (Exception e) =>
                {
                    dc.CreateAlert($"创建数据记录时发生数据库错误:\n{e.Message}", "error", Alerts_pn);
                    return;
                }) != 1)
            {
                dc.CreateAlert("创建数据记录时影响行数异常", "error", Alerts_pn);
                return;
            }
            dc.CreateAlert("账号创建成功!", "success", Alerts_pn);
        }
        else
        {
            dc.CreateAlert("注册信息有误", "info", Alerts_pn);
        }
    }

    protected void Reg_Acc_YearVal_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = (DateTime.Now.Month > 7 ? DateTime.Now.Year + 1 : DateTime.Now.Year) % 100 > int.Parse(RegAcc_tb.Text.Substring(0, 2));
    }
    protected void Login_Acc_tb_TextChanged(object sender, EventArgs e) { }
    protected void Login_Psw_tb_TextChanged(object sender, EventArgs e) { }

    protected void Logout_bt_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect(Request.RawUrl);
    }

    protected void CardLogin_Click(object sender, EventArgs e)
    {
        RemoteDelegates.CardSwipeHandler = (string data) =>
        {
            PopupL1 = "请检查登录信息";
            PopupL2 = $"卡号: {data}\n姓名: {svr.QuerySingle($"select MemberName from members where MemberCode={Session["LoginAcc"]}")}";
            ViewState["CardCache"] = data;
            DataBind();
            FindControl("ConfirmCard_bt").Visible = true;
        };
        PopupL2 = "已向服务器发送请求, 等待读卡...";
    }
    protected void ConfirmCard_bt_Click(object sender, EventArgs e)
    {
        FindControl("ConfirmCard_bt").Visible = false;
        Session["UserID"] = ViewState["CardCache"];
        var perm = svr.QuerySingle($"select PermissionLevel from members where MemberCode={LoginAcc_tb.Text};");
        if( perm == null )
        {
            Session["UserID"] = null;
            PopupL1 = "错误";
            PopupL2 = "账号不存在";
            // todo: move to 
        }
        Session["UserPerm"] = perm;
        ViewState["CardCache"] = null;
        if ((((Stack<string>)Session["jmpStack"])?.Count ?? 0) != 0)
        {
            Response.Redirect(((Stack<string>)Session["jmpStack"]).Pop());
        }
        return;
    }

    protected void ConfirmRst_bt_Click(object sender, EventArgs e)
    {
        // Assert: Verif code is valid
        if (!(ViewState["VerifyCode"] as string).Equals(Rst_VerifCode_tb.Text))
        {
            dc.CreateAlert("验证码错误!", "notice", Alerts_pn);
            return;
        }

        // Assert: New passwords match
        if (!Rst_NewPsw_tb.Text.Equals(Rst_RepPsw_tb.Text))
        {
            dc.CreateAlert("两次输入的密码不一致!", "notice", Alerts_pn);
            return;
        }

        // Operation: Update password
        svr.SafeOpen();
        using (var transact = svr.cn.BeginTransaction())
        {
            if (svr.Execute($"update members set password='{Rst_NewPsw_tb.Text}' where MemberCode={Rst_Acc_tb.Text};", transact) != 1)
            {
                transact.Rollback();
                dc.CreateAlert("修改密码时数据异常", "error", Alerts_pn);
                return;
            }
            transact.Commit();
            dc.CreateAlert("密码已更新", "success", Alerts_pn);
            Restore2_pn.Visible = false;
            RestorePsw_bt.Visible = true;
        }
    }

    protected void StartRst_bt_Click(object sender, EventArgs e)
    {
        if (svr.QuerySingle("select MemberCode from members where MemberCode=" + Rst_Acc_tb.Text) == null)
        {
            dc.CreateAlert("账号不存在", "notice", Alerts_pn);
            return;
        }
        PopupL1 = svr.QuerySingle($"select Email from members where MemberCode={Rst_Acc_tb.Text}").ToString();
        DataBind();
        if(PopupL1.Length == 0)
        {
            if(svr.QuerySingle($"select MemberCode from members where MemberCode={Rst_Acc_tb.Text}") == null)
            {
                dc.CreateAlert("账号不存在", "notice", Alerts_pn);
            }
            else
            {
                dc.CreateAlert("该账号未绑定邮箱, 请联系系统管理员协助重置密码", "notice", Alerts_pn);
            }
            Rst_NewPsw_tb.Enabled = false;
            Rst_RepPsw_tb.Enabled = false;
            Rst_VerifCode_tb.Enabled = false;
            ConfirmRst_bt.Enabled = false;
            ConfirmRst_bt.CssClass = "btn btn--large";
            return;
        }
        ConfirmRst_bt.CssClass = "btn btn--primary btn--large";
        ViewState["VerifyCode"] = MailSvr.SendVerificationMail(PopupL1,
            "[NUEDC实验室]密码重置验证", 
            "NUEDC实验室综合服务平台收到了一个密码重置请求, 验证码:\n\n" +
            "{0}\n\n" +
            "如非本人操作, 请忽略该邮件。\n" +
            "本邮件由NUEDC综合服务平台自动发送\n" +
            $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}");
        dc.CreateAlert("验证码已发送", "success", Alerts_pn);
        Restore1_pn.Visible = false;
        Restore2_pn.Visible = true;
    }

    protected void RestorePsw_bt_Click1(object sender, EventArgs e)
    {
        RestorePsw_bt.Visible = false;
        Restore1_pn.Visible = true;
        Restore2_pn.Visible = false;
    }
}