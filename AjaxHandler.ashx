<%@ WebHandler Language="C#" Class="AjaxHandler" %>

using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json;

public class AjaxHandler : IHttpHandler
{

    public void ProcessRequest (HttpContext context) {
        if(context.Request.HttpMethod == "POST")
        {
            // Button callbacks
            var json = new System.IO.StreamReader(context.Request.InputStream).ReadToEnd();
            var obj = JsonConvert.DeserializeObject<AjaxReq>(json);
            var r = RemoteDelegates.Invoke(obj.Req, obj.Param);
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(new
            {
                status = (r >= 200 && r < 300) ? "success" : "error"
            }));
            context.Response.StatusCode = r;
        }

        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    private class AjaxReq
    {
        public string   Req;
        public string[] Param;
    }

}