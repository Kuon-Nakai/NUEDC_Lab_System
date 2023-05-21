<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Security.aspx.cs" Inherits="Security" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_TopNavLinks" Runat="Server">
    <li class=""><a href="index.aspx">主页</a></li>
    <li><a href="AssetPage.aspx">元器件</a></li>
    <li><a href="Events.aspx">活动</a></li>
    <li class=""><a href="#footer" class="smoothscroll">联系我们</a></li>
    <li><a href="Management.aspx">管理</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <asp:LinkButton Text="Log in" runat="server" CssClass="btn btn--stroke" ForeColor="White" BorderColor="White" ID="Login_Jmp_bt" OnClick="Login_Jmp_bt_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <asp:Panel ID="Alerts_pn" runat="server"></asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <div id="styles" class="s-styles" style="padding-top: 76px">

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
                                <asp:LinkButton Text="0" runat="server" ID="LinkButton1" CssClass="stats-tabs-li-a" OnClick="LinkButton1_Click" />
                                <input type="button" name="idk" value="test" onshow="alert('test');" />
                                <em class="stats-tabs-li-a-em">会话数</em></li>
                            <li>
                                <asp:LinkButton Text="0" runat="server" ID="LinkButton2" CssClass="stats-tabs-li-a" />
                                <em class="stats-tabs-li-a-em">数据库查询</em></li>
                            <li>
                                <asp:LinkButton Text="0" runat="server" ID="LinkButton3" CssClass="stats-tabs-li-a" />
                                <em class="stats-tabs-li-a-em">数据库更新</em></li>
                            <li>
                                <asp:LinkButton Text="0" runat="server" ID="LinkButton4" CssClass="stats-tabs-li-a" />
                                <em class="stats-tabs-li-a-em">总注册用户</em></li>
                            <li>
                                <asp:LinkButton Text="0" runat="server" ID="LinkButton5" CssClass="stats-tabs-li-a" />
                                <em class="stats-tabs-li-a-em">查询数</em></li>
                            <li>
                                <asp:LinkButton Text="0" runat="server" ID="LinkButton6" CssClass="stats-tabs-li-a" />
                                <em class="stats-tabs-li-a-em">登记次数</em></li>
                        </ul>

                        <div class="row u-add-half-bottom" style="max-height:700px" id="ChartArea"></div>

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
                                            data: series
                                        }
                                    ]
                                }
                                chart.setOption(option);
                            }
                        </script>

                        <div class="row u-add-half-bottom">

                            <div class="column lg-6 tab-12">
                                <div class="row">
                                    <h5 style="position: relative; top: -39px; width: 150px">元器件搜索</h5>
                                    <asp:TextBox runat="server" TextMode="Search" />
                                    <a class="btn btn--primary" href="#0" onclick="bt_click('Search_bt_Click', 0, null);">搜索</a>
                                </div>
                                <div class="row">
                                    <h5 style="position: relative; top: -39px; width: 150px">快捷查询</h5>
                                    <a class="btn btn--stroke" href="#0">待归还</a>
                                    <a class="btn btn--stroke" href="#0">可续借</a>
                                </div>
                                <div style="max-width: 100%; overflow-x: scroll">
                                    <asp:GridView ID="Asset_gv" runat="server" Width="98%" AllowPaging="True" PageSize="15" AutoGenerateSelectButton="True" HorizontalAlign="Left" SelectedRowStyle-BackColor="#FFCC99" SelectedRowStyle-BorderColor="#FF9900" SelectedRowStyle-ForeColor="Red"></asp:GridView>
                                </div>

                            </div>

                            <div class="column lg-6 tab-12">
                                <div class="row">
                                    <h5 style="position: relative; top: -39px; width: 60px">分类</h5>
                                    <asp:DropDownList runat="server" AutoPostBack="true" CssClass="Ddl" ID="TypeSel0_ddl"></asp:DropDownList>
                                    <asp:DropDownList runat="server" AutoPostBack="true" CssClass="Ddl" ID="TypeSel1_ddl"></asp:DropDownList>
                                    <asp:DropDownList runat="server" AutoPostBack="true" CssClass="Ddl" ID="TypeSel2_ddl"></asp:DropDownList>
                                </div>

                                <h4>元器件信息<br />
                                </h4>
                                <div style="position: relative; left: 160px; top: -50px">Component Information</div>
                                <div class="row u-add-half-bottom">
                                    <div class="column lg-6 tab-12">
                                        元件名称<br />
                                        <br />
                                        元件类型<br />
                                        <br />
                                        值<br />
                                        <br />
                                        位置<br />
                                        <br />
                                        属性<br />
                                        <br />
                                        <asp:Panel runat="server" ID="Datasheet_pn0">
                                            参考文档
                                        </asp:Panel>
                                    </div>
                                    <div class="column lg-6 tab-12" style="text-align: right; margin-bottom: 0px">
                                        <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="AssetName_lb" />
                                        <br />
                                        <br />
                                        <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="AssetClass_lb" />
                                        <br />
                                        <br />
                                        <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="PrimValue_lb" />
                                        <br />
                                        <br />
                                        <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="Location_lb" />
                                        <br />
                                        <br />
                                        <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="Property_lb" />
                                        <br />
                                        <br />
                                        <asp:Panel runat="server" ID="Datasheet_pn1">
                                            <asp:HyperLink runat="server" ID="Datasheet_lk" Text="Not available"></asp:HyperLink>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="row u-add-half-bottom" style="margin-top: 0px">
                                    <div class="column lg-6 tab-12">
                                        可借出数量<br />
                                        <br />
                                        申请借出(件)<br />
                                        <br />
                                    </div>
                                    <div class="column lg-6 tab-12" style="text-align: right">
                                        <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="Borrowable_lb" />
                                        <br />
                                        <br />
                                        <asp:TextBox runat="server" CssClass="u-fullwidth" TextMode="Number" ID="BorrowQtySel_tb" AutoPostBack="True" />
                                    </div>
                                </div>
                                <asp:Panel runat="server" ID="BorrowConfirm_pn">
                                    <a class="btn btn--primary u-fullwidth" href="#0">登记借出</a>
                                </asp:Panel>
                                <asp:Panel runat="server" ID="BorrowNotAvailable_pn" Visible="false">
                                    <a class="btn u-fullwidth" href="#0">登记借出</a>
                                </asp:Panel>

                                <a class="btn btn--stroke u-fullwidth" href="#0">元件定位</a>
                                <asp:Panel runat="server" Visible="false">
                                    <div class="row">
                                        <div class="column">
                                            待归还(件)
                                            <br />
                                            <br />
                                            借出期限
                                            <br />
                                            <br />
                                        </div>
                                        <div class="column">
                                            <asp:Label Text="-1" runat="server" ID="Label1" />
                                            <br />
                                            <br />
                                            <asp:Label Text="1919/8/10" runat="server" ID="Label2" />
                                            <br />
                                            <br />
                                        </div>
                                    </div>
                                    <a class="btn btn--stroke u-fullwidth" href="#0">归还</a>
                                </asp:Panel>
                            </div>

                        </div>

                    </div>

                </div>
                <!-- end row -->

                </section> 
            <!-- end content -->

            </div>
            <!-- end styles -->
</asp:Content>

