<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        Application["SessionData"] = new Queue<int>();
        // Code that runs on application startup
        System.Threading.Thread UpdThr = new System.Threading.Thread(() =>
        {
            var cd = 600;
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

                if (--cd == 0)
                {
                    cd = 600;
                    // Place data in queue
                    var sessionData = Application["SessionData"] as Queue<int>;
                    if (sessionData.Count > 128)
                        sessionData.Dequeue();
                    sessionData.Enqueue(sessions.Count);

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
