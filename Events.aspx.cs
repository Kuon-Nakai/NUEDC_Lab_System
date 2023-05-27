using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Events : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage.col = .5f;
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

    protected void Asset_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataSet ds = new DataSet();
        ds = (DataSet)this.ViewState["ds"];
        Asset_gv.DataSource = ds;
        Asset_gv.PageIndex = e.NewPageIndex;
        Asset_gv.DataBind();
    }
}