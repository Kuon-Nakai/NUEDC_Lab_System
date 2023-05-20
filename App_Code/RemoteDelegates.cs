using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

/// <summary>
/// 客户端Ajax POST请求回调函数管理
/// </summary>
public class RemoteDelegates
{
    private static Dictionary<string, Func<string[], AjaxResult>> funcs = new Dictionary<string, Func<string[], AjaxResult>>();
    /// <summary>
    /// 注册/更新一个回调函数
    /// </summary>
    /// <param name="name">客户端调用时的名称</param>
    /// <param name="func">委托函数</param>
    public static void RegisterDelegate(string name, Func<string[], AjaxResult> func) => funcs[name] = func;
    /// <summary>
    /// 调用回调函数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static AjaxResult Invoke(string name, params string[] args)
    {
        if(!funcs.ContainsKey(name)) return new AjaxResult { statusCode = 404, data = new object() };
        return funcs[name](args);
    }
    /// <summary>
    /// Ajax POST回调函数返回类型
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// HTTP请求结果 如200 OK, 400 Bad Request, 404 Not Found...
        /// </summary>
        public int statusCode;
        /// <summary>
        /// 返回给客户端的处理结果 任意类型
        /// </summary>
        public object data;
    }
}