using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login_Reg : System.Web.UI.Page
{
    MySqlSvr svr;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
        
    }

    public void Login_bt_Click()
    {
        
    }

    public void Reg_bt_Click()
    {
        Validate("Reg");
        if(Reg_Acc_ReqVal.IsValid & Reg_Name_ReqVal.IsValid & Reg_Psw_ReqVal.IsValid & Reg_Psw_CompVal.IsValid &
            (RegMail_tb.Text.Length==0 || Regex.IsMatch(RegMail_tb.Text, "^[a-zA-Z0-9]+@[a-zA-Z0-9.]+(com|cn|co|net)$"))
            )
        {
            if(svr.Execute($"Insert into members values({RegAcc_tb.Text}, {RegName_tb.Text}, 4, NOW(), NOW(), null, {RegPsw0_tb.Text})",
                (Exception e) =>
                {
                    DynamicControls.CreateAlert($"创建数据记录时发生数据库错误:\n{e.Message}", "error", Alerts_pn);
                }) == 1)
            {
                DynamicControls.CreateAlert("创建数据记录时影响行数异常", "error", Alerts_pn);
            }
        }
    }

    protected void Reg_Acc_YearVal_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = (DateTime.Now.Month > 7 ? DateTime.Now.Year + 1 : DateTime.Now.Year) % 100 > int.Parse(RegAcc_tb.Text.Substring(0, 2));
    }
}