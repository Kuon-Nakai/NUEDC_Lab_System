<%@ WebHandler Language="C#" Class="LocalREST" %>

using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LocalREST : IHttpHandler
{
    public static Action<string> CardSwipeHandler;
    //private object RespData;
    //public void setTestData(object data) => RespData = data;
    public void ProcessRequest(HttpContext context)
    {
        if (context.Request.HttpMethod == "POST")
        {
            // Button callbacks
            var json = new System.IO.StreamReader(context.Request.InputStream).ReadToEnd();
            var obj = JsonConvert.DeserializeObject<IPCReq>(json);
            //var r = RemoteDelegates.Invoke(obj.Req, obj.Param);
            if(obj.Source.Equals("NFC"))
                CardSwipeHandler?.Invoke(obj.Data);
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(new
            {
                status = "success",
                data = new { }
            }));
            context.Response.StatusCode = 200;
        }

        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    //private class AjaxReq
    //{
    //    public string   Req;
    //    public string[] Param;
    //}

    private class IPCReq
    {
        public string Source;
        public string Data;
    }
}