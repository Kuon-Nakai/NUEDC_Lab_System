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
    public string Username;
    protected void Page_Load(object sender, EventArgs e)
    {
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

    public void Reg_bt_Click()
    {
        Validate("Reg");
        if (Reg_Acc_ReqVal.IsValid & Reg_Name_ReqVal.IsValid & Reg_Psw_ReqVal.IsValid & Reg_Psw_CompVal.IsValid &
            (RegMail_tb.Text.Length == 0 || Regex.IsMatch(RegMail_tb.Text, "^[a-zA-Z0-9]+@[a-zA-Z0-9.]+(com|cn|co|net)$")))
        {
            if (svr.Execute($"Insert into members values({RegAcc_tb.Text}, {RegName_tb.Text}, 4, NOW(), NOW(), null, {RegPsw0_tb.Text})",
                (Exception e) =>
                {
                    dc.CreateAlert($"创建数据记录时发生数据库错误:\n{e.Message}", "error", Alerts_pn);
                }) == 1)
            {
                dc.CreateAlert("创建数据记录时影响行数异常", "error", Alerts_pn);
            }
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
}