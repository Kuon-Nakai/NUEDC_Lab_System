using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Events : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] != null)
        {
            Login_Jmp_bt.Text = "已登录";
        }

    }

    protected void Login_Jmp_bt_Click(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            if (Session["jmpStack"] == null) Session["jmpStack"] = new Stack<string>();
            ((Stack<string>)Session["jmpStack"]).Push("Events.aspx");
            Response.Redirect("Login_Reg.aspx");
        }
    }
}