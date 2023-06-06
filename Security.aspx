<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Security.aspx.cs" Inherits="Security" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_TopNavLinks" runat="Server">
    <li class=""><a href="index.aspx">主页</a></li>
    <li><a href="AssetsPage.aspx">元器件</a></li>
    <li><a href="Events.aspx">活动</a></li>
    <li class=""><a href="#footer" class="smoothscroll">联系我们</a></li>
    <li><a href="#" style="color: white">管理</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <asp:LinkButton Text="Log in" runat="server" CssClass="btn btn--stroke" ForeColor="White" BorderColor="White" ID="Login_Jmp_bt" OnClick="Login_Jmp_bt_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <asp:Panel ID="Alerts_pn" runat="server"></asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="styles" class="s-styles" style="padding-top: 76px">
        <section id="content">
            <div style="background-image: url(images/pcb1.jpg); width: 100%; height: 500px; background-size: cover; background-position: center; padding-top: 0"></div>
            <br />
            <div class="row">

                <div class="column lg-12 intro" style="background-color: rgba(255,255,255,0.7); z-index: 3">

                    <h1>实验室安全/网站统计仪表盘</h1>
                </div>

            </div>

            <div class="row u-add-half-bottom" style="background-color: rgba(255,255,255,0.7);">

                <div class="column">

                    <h3>实验室物联网络状态</h3>

                    <ul class="stats-tabs">
                        <li class="stats-tabs-li">
                            <asp:LinkButton Text="0" runat="server" ID="TotalEntries_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">在线节点</em></li>
                        <li>
                            <asp:LinkButton Text="0" runat="server" ID="TotalComp_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">总节点</em></li>
                        <li>
                            <asp:LinkButton Text="0kbps" runat="server" ID="TotalLent_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">数据接收</em></li>
                        <li>
                            <asp:LinkButton Text="0%" runat="server" ID="TotalReturned_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">组件电量</em></li>
                        <li>
                            <asp:LinkButton Text="0Wh" runat="server" ID="TotalQueries_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">系统能耗</em></li>
                        <li>
                            <asp:LinkButton Text="0" runat="server" ID="TotalReg_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">警告</em></li>
                        <li>
                            <asp:LinkButton Text="0" runat="server" ID="LinkButton7" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">异常</em></li>
                    </ul>

                    <h3>网站使用统计</h3>

                    <ul class="stats-tabs">
                        <li class="stats-tabs-li">
                            <%--<asp:LinkButton Text="0" runat="server" ID="SessionCnt_bt" CssClass="stats-tabs-li-a" OnClientClick="loadChart('ChartArea', '会话数', <%=SessionCnt %>, )" />--%>
                            <a class="stats-tabs-li-a" onclick="loadChart('ChartArea', '会话数', <%=SessionResult %>,<%=series %> );"><%=(Application["SessionCnt"] as List<HttpSessionState>)?.Count ?? 0 %></a>
                            <%--<input type="button" name="idk" value="test" onshow="alert('test');" />--%>
                            <em class="stats-tabs-li-a-em">会话数</em></li>
                        <li>
                            <a class="stats-tabs-li-a" onclick="loadChart('ChartArea', '数据库查询/行', <%=DBQueriesResult %>,<%=series %> );"><%=(Application["DBQueriesData"] as Queue<int>)?.Last() %></a>
                            <em class="stats-tabs-li-a-em">数据库查询/行</em></li>
                        <li>
                            <a class="stats-tabs-li-a" onclick="loadChart('ChartArea', '数据库操作', <%=DBExecResult %>,<%=series %> );"><%=(Application["DBExecData"] as Queue<int>)?.Last() %></a>
                            <em class="stats-tabs-li-a-em">数据库操作</em></li>
                        <li>
                            <a class="stats-tabs-li-a" onclick="loadChart('ChartArea', '总注册用户', <%=UserCntResult %>,<%=series %> );"><%=(Application["UserCntData"] as Queue<int>)?.Last() %></a>
                            <em class="stats-tabs-li-a-em">总注册用户</em></li>
                        <li>
                            <a class="stats-tabs-li-a" onclick="loadChart('ChartArea', '查询数', <%=TotalQueriesResult %>,<%=series %> );"><%=(Application["TotalQueriesData"] as Queue<int>)?.Last() %></a>
                            <em class="stats-tabs-li-a-em">查询数</em></li>
                        <li>
                            <a class="stats-tabs-li-a" onclick="loadChart('ChartArea', '登记次数', <%=LendCntResult %>,<%=series %> );"><%=(Application["LendCntData"] as Queue<int>)?.Last() %></a>
                            <em class="stats-tabs-li-a-em">登记次数</em></li>
                    </ul>

                    <div class="row u-add-half-bottom" style="height: 700px; width: 100%" id="ChartArea"></div>

                    <script src="Scripts/ECharts/echarts-all.js"></script>
                    <script type="text/javascript">
                        function loadChart(area, title, x, series) {
                            var chart = echarts.init(document.getElementById(area));
                            var option = {
                                title: {
                                    text: title,
                                    x: 'center'
                                },
                                tooltip: {},
                                legend: {
                                    orient: 'vertical',
                                    x: 'left',
                                    data: [title]
                                },
                                xAxis: {
                                    data: x
                                },
                                yAxis: {},
                                toolbox: {
                                    show: true,
                                    feature: {
                                        mark: { show: true },
                                        dataZoom: { show: true },
                                        dataView: {
                                            show: true,
                                            readOnly: true
                                        },
                                        restore: { show: true },
                                        magicType: {
                                            show: true,
                                            type: ['bar', 'line']
                                        },
                                        saveAsImage: { show: true }
                                    }
                                },
                                series: [
                                    {
                                        name: title,
                                        type: 'line',
                                        data: x
                                    }
                                ]
                            }
                            chart.setOption(option);
                        }
                    </script>
                </div>
            </div>
        </section>
        <!-- end content -->

    </div>
    <!-- end styles -->
</asp:Content>

