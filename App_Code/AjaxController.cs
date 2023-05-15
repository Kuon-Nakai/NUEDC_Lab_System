using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

/// <summary>
/// Summary description for AjaxController
/// </summary>
public class AjaxController : ApiController
{
    private static Dictionary<string, HttpReq> funcs = new Dictionary<string, HttpReq>();
    private static Dictionary<string, ClientPollHandler> polls = new Dictionary<string, ClientPollHandler>();
    public delegate HttpStatusCode HttpReq(object param);
    public delegate IHttpActionResult ClientPollHandler(object param);
    public static void ResetFuncs()
    {
        funcs.Clear();
    }
    public static void AddFunction(string k, HttpReq v)
    {
        funcs.Add(k, v);
    }

    [HttpPost]
    public IHttpActionResult HttpHandler(string id, int evnt, object param)
    {
        if (id.StartsWith("?")) //client polling for data
        {
            //return polls.ContainsKey(id) ? polls[id].Invoke(param) : StatusCode(HttpStatusCode.BadRequest);

            var r = Request.CreateResponse(HttpStatusCode.OK, new { value = "114514" });
            return (IHttpActionResult)r;
        }
        return (funcs.ContainsKey($"{id}_{evnt}") ? StatusCode(funcs[$"{id}_{evnt}"].Invoke(param)) : StatusCode(HttpStatusCode.BadRequest));
    }
    public AjaxController()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}