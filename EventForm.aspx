<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EventForm.aspx.cs" Inherits="EventForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- styles
            ----------------------------------------------- -->
    <div id="styles" class="s-styles">

        <div class="row">

            <div class="column lg-12 intro">

                <h1>NUEDC实验室 报名信息采集</h1>
            </div>

        </div>
        <!-- end row -->
        <%--                <div class="row">
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server"></asp:SiteMapPath>
                </div>--%>

        <div class="row" style="width:100%">

            <div class="column">

                <h3>活动信息收集表</h3>

                <div class="row u-add-half-bottom">
                    <div class="column" style="width: 100%">
                        <asp:Panel runat="server" ID="Form_pn" Width="70%" />

                        <hr />
                        <asp:LinkButton Text="提交并确认报名" runat="server" ID="FormSubmit_bt" OnClick="FormSubmit_bt_Click" CssClass="btn btn--primary u-fullwidth" />

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
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_TopNavLinks" runat="Server">
    <li><a href="index.aspx">主页</a></li>
    <li><a href="AssetsPage.aspx">元器件</a></li>
    <li><a href="#" style="color:white">活动</a></li>
    <li class=""><a href="#footer" class="smoothscroll">联系我们</a></li>
    <li><a href="Management.aspx" style="visibility: hidden">管理</a></li>
</asp:Content>