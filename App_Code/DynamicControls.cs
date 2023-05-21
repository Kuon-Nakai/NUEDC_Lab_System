using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// 用于动态生成页面元素
/// </summary>
public class DynamicControls
{
    /// <summary>
    /// 生成提醒信息框
    /// </summary>
    /// <param name="message">提示信息</param>
    /// <param name="alertType">提示类型 error, success, info和notice有效</param>
    /// <returns>创建的Panel对象</returns>
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
    /// <summary>
    /// 在页面上生成提醒信息框
    /// </summary>
    /// <param name="message">提示信息</param>
    /// <param name="alertType">提示类型 error, success, info和notice有效</param>
    /// <param name="parent">提示信息框的父级对象 一般是右上角的Alert_pn</param>
    /// <returns>创建的Panel对象</returns>
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
    /// <summary>
    /// 动态创建HyperLink对象
    /// </summary>
    /// <param name="message">文字</param>
    /// <param name="parent">提示信息框的父级对象</param>
    /// <returns>创建的HyperLink对象</returns>
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
    /// <summary>
    /// 动态创建任意HTML原生对象
    /// </summary>
    /// <param name="tag">对象名称</param>
    /// <param name="parent">提示信息框的父级对象</param>
    /// <returns>创建的HyperLink对象</returns>
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