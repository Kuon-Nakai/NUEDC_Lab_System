﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    //MySqlSvr svr;

    //public RemoteDelegates.AjaxResult Popup_poll_Handler(string[] param)
    //{
    //    return new RemoteDelegates.AjaxResult
    //    {
    //        statusCode = 200,
    //        data = new
    //        {
    //            ModalID = 1
    //        }
    //    };
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        // Active session counter using LINQ syntax
        var sessions = Application["SessionCnt"] as List<HttpSessionState>;
        if ((from s in sessions
            where s.SessionID == Session.SessionID
            select new { }).Count() == 0)
        { 
            // No existing record of the current session ID
            sessions.Add(Session);
            Session["Counted"] = true;
        }

        // Query counter
        if (Application["TotalQueries"] == null)
            Application["TotalQueries"] = 0;
        MasterPage.col = 0f;
        //Session.Clear(); // for testing
        if (Session["UserID"] != null)
        {
            if (Session["UserPerm"] == null)
            {
                Session["UserID"] = null;
                return;
            }
            Login_Jmp_bt.Text = "已登录";
            if ((int)Session["UserPerm"] < 3)
            {
                AssetsManage_lk.Visible = true;
                EventsManage_lk.Visible = true;
                Security_lk.Visible = true;
            }
        }
        //RemoteDelegates.RegisterDelegate("Popup_poll", Popup_poll_Handler);
    }
    protected void Login_Jmp_bt_Click(object sender, EventArgs e)
    {
        if (Session["jmpStack"] == null) Session["jmpStack"] = new Stack<string>();
        ((Stack<string>)Session["jmpStack"]).Push("index.aspx");
        Response.Redirect("Login_Reg.aspx");
    }
}