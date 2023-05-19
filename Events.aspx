<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Events.aspx.cs" Inherits="Events" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- styles
            ----------------------------------------------- -->
    <div id="styles" class="s-styles">

        <div class="row">

            <div class="column lg-12 intro">

                <h1>NUEDC实验室 活动报名</h1>
            </div>

        </div>
        <!-- end row -->
        <%--                <div class="row">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server"></asp:SiteMapPath>
                </div>--%>

        <div class="row u-add-half-bottom">

            <div class="column">

                <h3>活动信息统计</h3>

                <ul class="stats-tabs">
                    <li class="stats-tabs-li">
                        <asp:Label Text="1,234" runat="server" ID="TotalEntries_lb" CssClass="stats-tabs-li-a" />
                        <em class="stats-tabs-li-a-em">总活动数</em></li>
                    <li>
                        <asp:Label Text="567" runat="server" ID="TotalOpen_lb" CssClass="stats-tabs-li-a" />
                        <em class="stats-tabs-li-a-em">开放报名</em></li>
                    <li>
                        <asp:Label Text="23,456" runat="server" ID="TotalLent_lb" CssClass="stats-tabs-li-a" />
                        <em class="stats-tabs-li-a-em">已结束</em></li>
                    <li>
                        <asp:Label Text="456" runat="server" ID="TotalQueries_lb" CssClass="stats-tabs-li-a" />
                        <em class="stats-tabs-li-a-em">成功报名</em></li>
                </ul>

                <div class="row u-add-half-bottom">

                    <div class="column">
                        <div class="row">
                            <h5 style="position: relative; top: -39px; width: 150px">活动搜索</h5>
                            <asp:TextBox runat="server" TextMode="Search" />
                            <asp:HyperLink ID="Search_bt" runat="server" CssClass="btn btn--primary">搜索</asp:HyperLink>
                        </div>
                        <div style="max-width: 100%; overflow-x: scroll">
                            <asp:GridView ID="Asset_gv" runat="server" Width="98%" AllowPaging="True" PageSize="15" AutoGenerateSelectButton="True" HorizontalAlign="Left" SelectedRowStyle-BackColor="#FFCC99" SelectedRowStyle-BorderColor="#FF9900" SelectedRowStyle-ForeColor="Red"></asp:GridView>
                        </div>

                    </div>

                </div>

                <div class="row u-add-bottom">
                    <div class="column">

                        <h4>活动信息<br />
                        </h4>
                        <div style="position: relative; left: 160px; top: -50px">Event Information</div>
                        <div class="row u-add-half-bottom">
                            <div class="column lg-6 tab-12">
                                活动名称<br />
                                <br />
                                活动时间<br />
                                <br />
                                地点<br />
                                <br />
                                报名时间<br />
                                <br />
                                备注<br />
                                <br />
                                人数/组数限制
                            </div>
                            <div class="column lg-6 tab-12" style="text-align: right; margin-bottom: 0px">
                                <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="EventName_lb" />
                                <br />
                                <br />
                                <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="EventTime_lb" />
                                <br />
                                <br />
                                <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="Location_lb" />
                                <br />
                                <br />
                                <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="SignupTime_lb" />
                                <br />
                                <br />
                                <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="Notes_lb" />
                                <br />
                                <br />
                                <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="MaxCount_lb" />
                            </div>
                        </div>
                        <asp:HyperLink runat="server" CssClass="btn btn--primary u-fullwidth" Visible="false" ID="Signup_bt">报名参加</asp:HyperLink>
                        <asp:HyperLink runat="server" CssClass="btn u-fullwidth" Visible="false" ID="NoSignup_bt">报名参加</asp:HyperLink>
                        <asp:HyperLink runat="server" CssClass="btn btn--stroke u-fullwidth" Visible="false" ID="EditForm_bt">修改信息</asp:HyperLink>
                        <asp:HyperLink runat="server" CssClass="btn btn--stroke u-fullwidth" Visible="false" ID="Cancel_bt">取消报名</asp:HyperLink>
                        <br />
                        <hr />
                        <h3>活动介绍</h3>
                        <div style="position: relative; left: 160px; top: -50px">Event Description</div>
                        <asp:Label runat="server" Text="No desc..." ID="EventDesc_lb"></asp:Label>
                        <br />
                        <hr />
                    </div>
                </div>

            </div>

        </div>

    </div>
    <!-- end styles -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <asp:Panel ID="Alerts_pn" runat="server"></asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <asp:Button Text="Log in" runat="server" CssClass="s-header__social-a" ID="Login_Jmp_bt" BackColor="Black" ForeColor="White" OnClick="Login_Jmp_bt_Click" />
</asp:Content>

