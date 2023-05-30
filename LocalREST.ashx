<%@ WebHandler Language="C#" Class="LocalREST" %>

using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LocalREST : IHttpHandler
{
    //private object RespData;
    //public void setTestData(object data) => RespData = data;
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
                status = (r.statusCode >= 200 && r.statusCode < 300) ? "success" : "error",
                data   = r.data
            }));
            context.Response.StatusCode = r.statusCode;
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