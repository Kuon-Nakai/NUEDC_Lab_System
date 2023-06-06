<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        Application["SessionData"] = new Queue<int>();
        Application["TotalQueriesData"] = new Queue<int>();
        Application["DBQueriesData"] = new Queue<int>();
        Application["DBExecData"] = new Queue<int>();
        Application["LendCntData"] = new Queue<int>();
        Application["UserCntData"] = new Queue<int>();
        // Code that runs on application startup
        System.Threading.Thread UpdThr = new System.Threading.Thread(() =>
        {
            var svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
            var cd = 6; // should be 600
            // set to 6 just for ease of displaying effects.
            while (true)
            {
                var stk = new Stack<dynamic>();
                if (Application["SessionCnt"] == null)
                    Application["SessionCnt"] = new List<HttpSessionState>();
                var sessions = Application["SessionCnt"] as List<HttpSessionState>;

                // Actively remove dead sessions from list
                foreach(var s in sessions)
                {
                    if (s["Counted"] == null)
                        stk.Push(s);
                }
                foreach (var s in stk)
                    sessions.Remove(stk.Pop());

                stk.Clear();
                
                if (Application["TotalQueries"] == null)
                    Application["TotalQueries"] = 0;

                if (Application["DBQueries"] == null)
                    Application["DBQueries"] = 0;

                if (Application["DBExec"] == null)
                    Application["DBExec"] = 0;

                if (--cd == 0)
                {
                    // Runs every min
                    cd = 6;
                    // Place data in queues
                    // Session, real time
                    var sessionData = Application["SessionData"] as Queue<int>;
                    if (sessionData.Count > 128)
                        sessionData.Dequeue();
                    sessionData.Enqueue(sessions.Count);

                    // Page query events, accumulative within intervals
                    if (Application["TotalQueriesLst"] == null)
                        Application["TotalQueriesLst"] = 0;
                    var tqd = Application["TotalQueriesData"] as Queue<int>;
                    if (tqd.Count > 128)
                        tqd.Dequeue();
                    tqd.Enqueue((int)Application["TotalQueries"] - (int)Application["TotalQueriesLst"]);
                    Application["TotalQueriesLst"] = (int)Application["TotalQueries"];

                    // DB query events, accumulative within intervals
                    if (Application["DBQueriesLst"] == null)
                        Application["DBQueriesLst"] = 0;
                    var dqd = Application["DBQueriesData"] as Queue<int>;
                    if (dqd.Count > 128)
                        dqd.Dequeue();
                    dqd.Enqueue((int)Application["DBQueries"] - (int)Application["DBQueriesLst"]);
                    Application["DBQueriesLst"] = (int)Application["DBQueries"];

                    // DB execution events (update/delete), accumulative within intervals
                    if (Application["DBExecLst"] == null)
                        Application["DBExecLst"] = 0;
                    var ded = Application["DBExecData"] as Queue<int>;
                    if (ded.Count > 128)
                        ded.Dequeue();
                    ded.Enqueue((int)Application["DBExec"] - (int)Application["DBExecLst"]);
                    Application["DBExecLst"] = (int)Application["DBExec"];

                    // Total user count, accumulative over whole lifetime
                    if (Application["DBExecLst"] == null)
                        Application["DBExecLst"] = 0;
                    var ucd = Application["UserCntData"] as Queue<int>;
                    if (ucd.Count > 128)
                        ucd.Dequeue();
                    ucd.Enqueue(int.Parse(svr.QuerySingle("select count(MemberCode) from members group by com having com=0;").ToString()));

                    // Total user count, accumulative over whole lifetime
                    if (Application["DBExecLst"] == null)
                        Application["DBExecLst"] = 0;
                    var lcd = Application["LendCntData"] as Queue<int>;
                    if (lcd.Count > 128)
                        lcd.Dequeue();
                    lcd.Enqueue(int.Parse(svr.QuerySingle("select count(TransactionCode) from lending group by com having com=1;").ToString()));
                }
                System.Threading.Thread.Sleep(1000);
            }

        });
        UpdThr.Start();
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {

    }

</script>
