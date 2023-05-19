using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for DynamicControls
/// </summary>
public class DynamicControls
{
    public static Panel CreateAlert(string message, string alertType)
    {
        var p = new HtmlGenericControl("p")
        {
            InnerText = message
        };
        var span = new HtmlGenericControl("span")
        {
            Attributes =
            {
                ["class"] = "alert-box__close"
            }
        };
        var div = new HtmlGenericControl("div")
        {
            Attributes =
            {
                ["class"] = $"alert-box alert-box--{alertType} u-fullwidth",
                ["style"] = "margin-top:10px;"
            }
        };
        div.Controls.Add(p);
        div.Controls.Add(span);
        var panel = new Panel()
        {
            Attributes =
            {
                ["runat"] = "server"
            },
            Visible = true,
            HorizontalAlign = HorizontalAlign.Center
        };
        panel.Controls.Add(div);
        return panel;
    }
    public static Panel CreateAlert(string message, string alertType, Control parent)
    {
        var p = new HtmlGenericControl("p")
        {
            InnerText = message
        };
        var span = new HtmlGenericControl("span")
        {
            Attributes =
            {
                ["class"] = "alert-box__close"
            }
        };
        var div = new HtmlGenericControl("div")
        {
            Attributes =
            {
                ["class"] = $"alert-box alert-box--{alertType} u-fullwidth",
                ["style"] = "margin-top:10px;"
            }
        };
        div.Controls.Add(p);
        div.Controls.Add(span);
        var panel = new Panel()
        {
            Attributes =
            {
                ["runat"] = "server"
            },
            Visible = true,
            HorizontalAlign = HorizontalAlign.Center
        };
        panel.Controls.Add(div);
        parent.Controls.Add(panel);
        
        return panel;
    }
    public static HyperLink CreateHyperLink(string message, string link, Control parent)
    {
        var r = new HyperLink()
        {
            Text = message,
            NavigateUrl = link
        };
        parent.Controls.Add(r);
        return r;
    }
    public static HtmlGenericControl CreateHTMLElement(string tag, Control parent)
    {
        var r = new HtmlGenericControl(tag);
        parent.Controls.Add(r);
        return r;
    }

    public DynamicControls()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}