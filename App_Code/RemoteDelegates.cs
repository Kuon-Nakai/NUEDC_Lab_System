using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

/// <summary>
/// Summary description for RemoteDelegates
/// </summary>
public class RemoteDelegates
{
    private static Dictionary<string, Func<string[], AjaxResult>> funcs = new Dictionary<string, Func<string[], AjaxResult>>();

    public static void RegisterDelegate(string name, Func<string[], AjaxResult> func) => funcs[name] = func;
    public static AjaxResult Invoke(string name, params string[] args)
    {
        if(!funcs.ContainsKey(name)) return new AjaxResult { statusCode = 404, data = new object() };
        return funcs[name](args);
    }

    public class AjaxResult
    {
        public int statusCode;
        public object data;
    }
}