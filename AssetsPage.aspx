<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AssetsPage.aspx.cs" Inherits="AssetsPage" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_TopNavLinks" runat="Server">
    <li class=""><a href="index.aspx">主页</a></li>
    <li><a href="#top" class="smoothscroll" style="color: white">元器件</a></li>
    <li><a href="Events.aspx">活动</a></li>
    <li class=""><a href="#footer" class="smoothscroll">联系我们</a></li>
    <li><a href="Management.aspx" style="visibility: hidden">管理</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <asp:LinkButton Text="Log in" runat="server" CssClass="btn btn--stroke" ForeColor="White" BorderColor="White" ID="Login_Jmp_bt" />
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

                    <h1>NUEDC实验室 元器件查询/借用系统</h1>
                </div>

            </div>
            <!-- end row -->
            <%--                <div class="row">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server"></asp:SiteMapPath>
                </div>--%>

            <div class="row u-add-half-bottom" style="background-color: rgba(255,255,255,0.7);">

                <div class="column">

                    <h3>元器件资产系统统计</h3>

                    <ul class="stats-tabs">
                        <li class="stats-tabs-li">
                            <asp:Label Text="1,234" runat="server" ID="TotalEntries_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">总分类数</em></li>
                        <li>
                            <asp:Label Text="567" runat="server" ID="TotalComp_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">总器件数</em></li>
                        <li>
                            <asp:Label Text="23,456" runat="server" ID="TotalLent_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">总借出</em></li>
                        <li>
                            <asp:Label Text="3,456" runat="server" ID="TotalReturned_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">完成借出</em></li>
                        <li>
                            <asp:Label Text="456" runat="server" ID="TotalQueries_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">查询数</em></li>
                        <li>
                            <asp:Label Text="26" runat="server" ID="TotalReg_lb" CssClass="stats-tabs-li-a" />
                            <em class="stats-tabs-li-a-em">登记次数</em></li>
                    </ul>

                    <div class="row u-add-half-bottom">

                        <div class="column lg-6 tab-12">
                            <div class="row">
                                <h5 style="position: relative; top: -39px; width: 150px">元器件搜索</h5>
                                <asp:TextBox runat="server" TextMode="Search" ID="sear_tb" />
                                <asp:LinkButton runat="server" CssClass="btn btn--primary" ID="Search_bt" OnClick="Search_bt_Click">搜索</asp:LinkButton>
                            </div>
                            <div class="row">
                                <h5 style="position: relative; top: -39px; width: 150px">快捷查询</h5>
                                <asp:LinkButton runat="server" CssClass="btn btn--stroke" ID="AwaitReturn_bt" OnClick="AwaitReturn_bt_Click">待归还</asp:LinkButton>
                                <asp:LinkButton runat="server" CssClass="btn btn--stroke">可续借</asp:LinkButton>
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
                                <asp:LinkButton runat="server" CssClass="btn btn--primary u-fullwidth" ID="LendReg_bt">登记借出</asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="BorrowNotAvailable_pn" Visible="false">
                                <asp:LinkButton runat="server" CssClass="btn u-fullwidth" ID="Lend_NotAva_bt">登记借出</asp:LinkButton>
                            </asp:Panel>

                            <asp:LinkButton runat="server" CssClass="btn btn--stroke u-fullwidth" ID="Locate_bt">元件定位</asp:LinkButton>
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
                                <asp:LinkButton runat="server" CssClass="btn btn--stroke u-fullwidth" ID="Return_bt">归还</asp:LinkButton>
                            </asp:Panel>
                        </div>

                    </div>

                </div>

            </div>
            <!-- end row -->

        </section>
        <!-- end content -->

    </div>
</asp:Content>

