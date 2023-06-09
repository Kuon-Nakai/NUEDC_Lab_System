﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AssetsPage.aspx.cs" Inherits="AssetsPage" Async="true" AsyncTimeout="10000" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="css/LightboxStyles.scss" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_TopNavLinks" runat="Server">
    <li class=""><a href="index.aspx">主页</a></li>
    <li><a href="#top" class="smoothscroll" style="color: white">元器件</a></li>
    <li><a href="Events.aspx">活动</a></li>
    <li class=""><a href="#footer" class="smoothscroll">联系我们</a></li>
    <li><a href="Management.aspx" style="visibility: hidden">管理</a></li>
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
                                <asp:Panel runat="server" DefaultButton="Search_bt" CssClass="row">
                                    <asp:TextBox runat="server" TextMode="Search" ID="sear_tb" />
                                    <asp:LinkButton runat="server" CssClass="btn btn--primary" ID="Search_bt" OnClick="Search_bt_Click">搜索</asp:LinkButton>
                                </asp:Panel>
                            </div>
                            <div class="row">
                                <h5 style="position: relative; top: -39px; width: 150px">快捷查询</h5>
                                <asp:LinkButton runat="server" CssClass="btn btn--stroke" ID="AwaitReturn_bt" OnClick="AwaitReturn_bt_Click">待归还</asp:LinkButton>
                                <asp:LinkButton runat="server" CssClass="btn btn--stroke" ID="Borrowed_bt" OnClick="Borrowed_bt_Click">已借出</asp:LinkButton>
                            </div>
                            <div style="max-width: 100%; overflow-x: scroll">
                                <asp:GridView ID="Asset_gv" runat="server" Width="98%" AllowPaging="True" PageSize="15" HorizontalAlign="Left" SelectedRowStyle-BackColor="#FFCC99" SelectedRowStyle-BorderColor="#FF9900" SelectedRowStyle-ForeColor="Red" OnPageIndexChanging="Asset_gv_PageIndexChanging" OnRowCreated="Asset_gv_RowCreated" OnSelectedIndexChanged="Asset_gv_SelectedIndexChanged"></asp:GridView>
                            </div>

                        </div>

                        <div class="column lg-6 tab-12">
                            <div class="row">
                                <h5 style="position: relative; top: -39px; width: 60px">分类</h5>
                                <asp:DropDownList runat="server" AutoPostBack="true" CssClass="Ddl" ID="TypeSel0_ddl" OnSelectedIndexChanged="TypeSel0_ddl_SelectedIndexChanged"></asp:DropDownList>
                                <asp:DropDownList runat="server" AutoPostBack="true" CssClass="Ddl" ID="TypeSel1_ddl" OnSelectedIndexChanged="TypeSel1_ddl_SelectedIndexChanged"></asp:DropDownList>
                                <asp:DropDownList runat="server" AutoPostBack="true" CssClass="Ddl" ID="TypeSel2_ddl" OnSelectedIndexChanged="TypeSel2_ddl_SelectedIndexChanged"></asp:DropDownList>
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
                                    <br />
                                    元件识别码<br />
                                    <br />
                                </div>
                                <div class="column lg-6 tab-12" style="text-align: right">
                                    <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="Borrowable_lb" />
                                    <br />
                                    <br />
                                    <asp:TextBox runat="server" CssClass="u-fullwidth" TextMode="Number" ID="BorrowQtySel_tb" />
                                    <asp:TextBox runat="server" CssClass="u-fullwidth" TextMode="Number" ID="ItemID_tb" />
                                </div>
                            </div>
                            <%--<asp:Panel runat="server" ID="BorrowConfirm_pn">
                                <asp:LinkButton runat="server" CssClass="btn btn--primary u-fullwidth" ID="LendReg_bt" OnClick="LendReg_bt_Click">登记借出</asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="BorrowNotAvailable_pn" Visible="false">
                            </asp:Panel>--%>
                            <asp:LinkButton runat="server" CssClass="btn btn--primary u-fullwidth" ID="Borrow_tb" Text="登记借出" />
                            <asp:LinkButton runat="server" CssClass="btn btn--stroke u-fullwidth" ID="Locate_bt">元件定位</asp:LinkButton>
                            <asp:Panel runat="server" Visible="false" ID="Return_pn">
                                <h4>已借出该元件</h4>
                                <div class="row">
                                    <div class="column">
                                        待归还(件)
                                            <br />
                                        <br />
                                        归还期限
                                            <br />
                                        <br />
                                    </div>
                                    <div class="column">
                                        <asp:Label Text="-1" runat="server" ID="ToReturn_lb" CssClass="u-pull-right" />
                                        <br />
                                        <br />
                                        <asp:Label Text="" runat="server" ID="ReturnDeadline_lb" CssClass="u-pull-right" />
                                        <br />
                                        <br />
                                    </div>
                                </div>
                                <asp:DropDownList ID="ReturnCodeSel_ddl" runat="server" AutoPostBack="true" CssClass="u-pull-right" OnSelectedIndexChanged="ReturnCodeSel_ddl_SelectedIndexChanged"></asp:DropDownList>
                                <asp:LinkButton runat="server" CssClass="btn btn--primary u-fullwidth" ID="Return_bt" OnClick="Return_bt_Click">归还</asp:LinkButton>
                            </asp:Panel>
                        </div>

                    </div>

                </div>

            </div>
            <!-- end row -->
            
        </section>
        <!-- end content -->
        <script type="text/javascript">
            function showBorrowConfirmPopup() {

                const instance = basicLightbox.create(`
		<h2 style="color:white">请确认登录信息和返还日期, 并仔细阅读以下注意事项</h2>
		<p style="color:white">当前登录: <%=userId %></p>
		<p style="color:white">归还期限: <%=DateTime.Today.AddMonths(1).ToLongDateString() %></p>
        <p style="color:white">
            注意事项: <br />
            · 除另有声明外, 借出时长默认为: 一个月  请在规定时间内前往实验室归还元件<br />
            · 时限已到但需要继续使用的, 请在归还期限前联系实验室管理员登记续借<br />
            · 元件在借出期间损坏或丢失的, 请联系实验室管理员<br />
            · 电阻、电容等消耗性元件可在线上自助登记归还, 不需要实验室成员登记<br />
            · 开发板、传感器等较贵重元件需交给实验室成员检查完整性 并由实验室成员登记归还<br />
        </p>
        <hr />
        <asp:LinkButton Text="我已阅读, 确认借出" runat="server" ID="ConfirmBorrow_bt" CssClass="btn btn--primary btn--large" OnClick="LendReg_bt_Click" />
	`);

                //instance.element().insertAdjacentHTML('afterbegin', '<p>Before placeholder</p>')
                //instance.element().insertAdjacentHTML('beforeend', '<p>After placeholder</p>')

                instance.show();

            }
            function showReturnImpossiblePopup() {

                const instance = basicLightbox.create(`
		<h2 style="color:white">请将元件交给实验室成员检查登记</h2>
        <p style="color:white">该元件较贵重, 需由实验室成员登记归还</p>
	`);

                //instance.element().insertAdjacentHTML('afterbegin', '<p>Before placeholder</p>')
                //instance.element().insertAdjacentHTML('beforeend', '<p>After placeholder</p>')

                instance.show();

            }
        </script>
    </div>
</asp:Content>

