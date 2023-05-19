using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RemoteDelegates
/// </summary>
public class RemoteDelegates
{
    private static Dictionary<string, Func<string[], int>> funcs = new Dictionary<string, Func<string[], int>>();

    public static void RegisterDelegate(string name, Func<string[], int> func) => funcs[name] = func;
    public static int Invoke(string name, params string[] args) => funcs[name]?.Invoke(args) ?? 404;

}