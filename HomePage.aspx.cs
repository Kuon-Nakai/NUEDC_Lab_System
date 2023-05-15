using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class HomePage : System.Web.UI.Page
{
    MySqlConnection cn;
    
    private void Alert(string identifier, object txt)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), identifier, $"alrt('{txt.ToString()}')", true);
        
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        AjaxController.ResetFuncs();
        AjaxController.AddFunction("login-bt-click_0", new AjaxController.HttpReq((object _) => Login_bt_Click(sender, e)));
        AjaxController.AddFunction("test_0", new AjaxController.HttpReq((_) => HttpStatusCode.OK));
        AjaxController.AddFunction("test_1", new AjaxController.HttpReq((_) => HttpStatusCode.BadRequest));
    }


    
    protected HttpStatusCode Login_bt_Click(object sender, EventArgs e)
    {
        cn = new MySqlConnection("host=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
        cn.Open();
        try
        {
            if (Password_tb.Text == (string)new MySqlCommand($"Select {Username_tb.Text} From member", cn).ExecuteScalar())
            //if(false)
            {
                // Auth pass
                cn.Close();
            }
            else
            {
                // Auth fail
                Alert("PswWrong", "密码错误或用户不存在!");
                cn.Close();
            }
        }catch(MySqlException ex) 
        {
            Alert("SQLExc", $"Server-side SQL Exception: {ex.Message}");
        }
        return HttpStatusCode.OK;
    }
}