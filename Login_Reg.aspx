<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login_Reg.aspx.cs" Inherits="_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_TopNavLinks" runat="Server">
    <li class=""><a href="HomePage.aspx" class="smoothscroll">主页</a></li>
    <li><a href="AssetsPage.aspx">元器件</a></li>
    <li><a href="index.html#services">活动</a></li>
    <li class=""><a href="#footer" class="smoothscroll">联系我们</a></li>
    <li><a href="index.html#folio" style="visibility: hidden">管理</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <asp:LinkButton Text="Log in" runat="server" CssClass="btn btn--stroke" ForeColor="White" BorderColor="White" ID="Login_Jmp_bt"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <asp:Panel ID="Alerts_pn" runat="server"></asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="styles" class="s-styles">

        <div class="row">

            <%--<div class="column lg-12 intro">--%>

            <h1 style="width: 200px">登录</h1>

            <h3 class="text-left" style="top: 50px; height: fit-content; position: relative">Log in</h3>

            <hr />
            <%--</div>--%>
        </div>
        <!-- end row -->



        <div class="row u-add-half-bottom">

            <div class="column">

                <div class="row u-add-half-bottom">


                    <div class="column lg-6 tab-12">
                        <h3>账号密码登录</h3>
                        <h5 class="text-left">账号(学号)</h5>
                        <asp:TextBox runat="server" CssClass="u-fullwidth" ID="LoginAcc_tb" CausesValidation="true" OnTextChanged="Login_Acc_tb_TextChanged" />
                        <h5 class="text-left">密码</h5>
                        <asp:TextBox runat="server" CssClass="u-fullwidth" ID="LoginPsw_tb" TextMode="Password" CausesValidation="True" OnTextChanged="Login_Psw_tb_TextChanged" />
                        <asp:RequiredFieldValidator ID="Login_Acc_ReqVal" runat="server" ErrorMessage="需要填写账号" ValidationGroup="Login" Visible="false" ControlToValidate="LoginAcc_tb" EnableClientScript="true"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="Login_Psw_ReqVal" runat="server" ErrorMessage="需要填写密码" ValidationGroup="Login" Visible="false" ControlToValidate="LoginPsw_tb" EnableClientScript="true" ValidateRequestMode="Enabled" InitialValue='""'></asp:RequiredFieldValidator>
                        <asp:ValidationSummary ID="Login_ValSummary" runat="server" ValidationGroup="Login" />
                        <%--<a class="btn btn--primary u-fullwidth" href="#0" onclick="callSvr('Login_bt_Click', null)">登录</a>--%>
                        <asp:LinkButton ID="Login_bt" runat="server" Text="登录" CssClass="btn btn--primary u-fullwidth" OnClick="Login_bt_Click" />
                        <%--<a class="btn btn--stroke u-fullwidth" href="#0">忘记密码</a>--%>
                        <asp:LinkButton ID="RestorePsw_bt" runat="server" Text="忘记密码" CssClass="btn btn--stroke u-fullwidth" />
                    </div>

                    <div class="column lg-6 tab-12">
                        <h3>账号注册</h3>
                        <h5 class="text-left">账号(学号)</h5>
                        <asp:TextBox runat="server" CssClass="u-fullwidth" ID="RegAcc_tb" />
                        <h5 class="text-left">姓名</h5>
                        <asp:TextBox runat="server" CssClass="u-fullwidth" ID="RegName_tb" />
                        <h5 class="text-left">密码</h5>
                        <asp:TextBox runat="server" CssClass="u-fullwidth" ID="RegPsw0_tb" TextMode="Password" />
                        <h5 class="text-left">重复密码</h5>
                        <asp:TextBox runat="server" CssClass="u-fullwidth" ID="RegPsw1_tb" TextMode="Password" />
                        <h5 class="text-left">邮箱</h5>
                        用于找回密码和发送通知, 非必填, 默认学校分配的教育邮箱
                                <asp:TextBox runat="server" CssClass="u-fullwidth" ID="RegMail_tb" TextMode="Email" />
                        <asp:RequiredFieldValidator ID="Reg_Acc_ReqVal" runat="server" ErrorMessage="账号为必填项" ControlToValidate="RegAcc_tb" Visible="false" ValidationGroup="Reg"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="Reg_Acc_YearVal" runat="server" ErrorMessage="入学年份不河里捏..." ControlToValidate="RegAcc_tb" Visible="false" ValidationGroup="Reg" OnServerValidate="Reg_Acc_YearVal_ServerValidate"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="Reg_Name_ReqVal" runat="server" ErrorMessage="姓名为必填项" ControlToValidate="RegName_tb" Visible="false" ValidationGroup="Reg"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="Reg_Psw_ReqVal" runat="server" ErrorMessage="密码为必填项" ControlToValidate="RegPsw0_tb" Visible="false" ValidationGroup="Reg"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="Reg_Psw_CompVal" runat="server" ErrorMessage="密码不一致" ControlToValidate="RegPsw1_tb" ControlToCompare="RegPsw0_tb" Visible="false" ValidationGroup="Reg"></asp:CompareValidator>
                        <asp:ValidationSummary ID="Reg_ValidationSummary" runat="server" ForeColor="Red" ValidationGroup="Reg" />
                        <asp:HyperLink ID="Reg_bt" runat="server" CssClass="btn btn--primary u-fullwidth">注册</asp:HyperLink>
                    </div>

                </div>

            </div>

        </div>
        <!-- end row -->

    </div>
</asp:Content>

