<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HomePage.aspx.cs" Inherits="HomePage" Title="NUEDC实验室" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title title="NUEDC System"></title>
    <link href="css/Master.css" rel="stylesheet" type="text/css" />
    <link href="css/Style.css" rel="stylesheet" type="text/css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
</head>

<body>
    <div id="page" class="s-pagewrap">
        <form id="form1" runat="server">
            <asp:ScriptManager runat="server" ID="Smg"></asp:ScriptManager>
            <div style="text-align:center; background-color:#1C1C24"> 
                <asp:Label runat="server" Text="NUEDC实验室管理系统"></asp:Label>
                <p></p>
                <div style="text-align:left" class="relwrapper">
                
                    <asp:Panel runat="server" Width="600px">
                        <div id="Asset_dv">
                            <asp:Button runat="server" Text="资产查询/借还" ID="Asset_bt" Width="100px"></asp:Button>
                            <asp:Button runat="server" Text=" " Width="100px" ID="Asset_fx"></asp:Button>
                        </div>
                    
                        <p></p>
                        <asp:Button runat="server" Text="仪器使用预约"></asp:Button>
                        <p></p>
                        <asp:Button runat="server" Text="成员概览" CssClass="btn--stroke s-intro__content-btn smoothscroll" ForeColor="White"></asp:Button>
                        <p></p>
                        <asp:Button runat="server" Text="安全控制台"></asp:Button>
                        <p></p>
                        <asp:Button runat="server" Text="系统管理"></asp:Button>
                    </asp:Panel>
                    <asp:Panel runat="server" Width="300px">
                        <asp:Label runat="server" Text="访客您好，请登录"></asp:Label>
                        <p></p>
                        <asp:Panel runat="server" Width="275px" ID="Login_pn">
                            <asp:Label ID="Label1" runat="server" Text="账号"></asp:Label>
                            <asp:TextBox ID="Username_tb" runat="server"></asp:TextBox>
                            <p></p>
                            <asp:Label ID="Label2" runat="server" Text="密码"></asp:Label>
                            <asp:TextBox ID="Password_tb" runat="server" TextMode="Password"></asp:TextBox>
                            <p></p>
                            <asp:Button ID="Login_bt" runat="server" Text="登录" Width="100px"  />
                            <asp:LinkButton ID="Reg_bt" runat="server" Width="100px">注册</asp:LinkButton>
                        </asp:Panel>
                    </asp:Panel>
                </div>

                <div id="bricks" class="row bricks">
                    <div class="column lg-12 masonry">
                        <div class="bricks-wrapper" style="position: relative; height: 698.625px;">

                            <div class="grid-sizer"></div>

                            <article class="brick brick--double entry" style="position: absolute; left: 0%; top: 0px;">
                                <a href="AssetsPage.aspx" class="entry__link">
                                    <div class="entry__thumb">
                                        <img src="images/mumumumumumumu.jpg" alt="">
                                    </div>
                                    <div class="entry__info">
                                        <div class="entry__cat">Component Borrowing Registeration</div>
                                        <h4 class="entry__title">元件借出/归还登记</h4>
                                    </div>
                                </a>
                            </article> <!-- entry -->

                            <article class="brick brick--double entry" style="position: absolute; left: 49.9995%; top: 0px;">
                                <a href="EquipmentPage.aspx" class="entry__link">
                                    <div class="entry__thumb">
                                        <img src="images/Lyco.jpg" alt="">
                                    </div>
                                    <div class="entry__info">
                                        <div class="entry__cat">Equipment Use Appointment</div>
                                        <h4 class="entry__title">仪器使用预约</h4>
                                    </div>
                                </a>
                            </article> <!-- entry -->

                            <article class="brick entry" style="position: absolute; left: 0%; top: 465.75px;">
                                <a href="#modal-03" class="entry__link">
                                    <div class="entry__thumb">
                                        <img src="images/bocchi-glitch.gif" alt="">
                                    </div>
                                    <div class="entry__info">
                                        <div class="entry__cat">Members Overview</div>
                                        <h4 class="entry__title">成员概览</h4>
                                    </div>
                                </a>
                            </article> <!-- entry -->

                            <article class="brick entry" style="position: absolute; left: 24.9997%; top: 465.75px;">
                                <a href="#modal-04" class="entry__link">
                                    <div class="entry__thumb">
                                        <img src="images/folio/lamp@2x.jpg" alt="">
                                    </div>
                                    <div class="entry__info">
                                        <div class="entry__cat">E-Commerce</div>
                                        <h4 class="entry__title">Lamp</h4>
                                    </div>
                                </a>
                            </article> <!-- entry -->

                            <article class="brick entry" style="position: absolute; left: 49.9995%; top: 465.75px;">
                                <a href="#modal-05" class="entry__link">
                                    <div class="entry__thumb">
                                        <img src="images/folio/tropical@2x.jpg" alt="">
                                    </div>
                                    <div class="entry__info">
                                        <div class="entry__cat">Frontend Design</div>
                                        <h4 class="entry__title">Tropical</h4>
                                    </div>
                                </a>
                            </article> <!-- entry -->

                            <article class="brick entry" style="position: absolute; left: 74.9992%; top: 465.75px;">
                                <a href="#modal-06" class="entry__link">
                                    <div class="entry__thumb">
                                        <img src="images/folio/woodcraft@2x.jpg" alt="">
                                    </div>
                                    <div class="entry__info">
                                        <div class="entry__cat">E-Commerce</div>
                                        <h4 class="entry__title">Woodcraft</h4>
                                    </div>
                                </a>
                            </article> <!-- entry -->

                        </div> <!-- end bricks-wrapper -->
                    </div> <!-- end masonry -->
                </div>

    <%--            <asp:Panel ID="WrongPassword_pn" runat="server">
                    <div id="BgDim_pn"></div>
                </asp:Panel>--%>
                <svg xmlns="http://www.w3.org/2000/svg" width="200", height="200">
                    <rect x="10" y="10" height="45" width="100" fill="blue" onclick="bt_click('test', 1, 0)"></rect>
                </svg>
            </div>
        </form>
    </div>
    
</body>

<script type="text/javascript">
    function alrt(msg) {
        alert(msg);
    }
    function bt_click(id, ev, param) {
        $.ajax({
            url: 'HomePage.aspx',
            type: 'POST',
            data: {
                'id': id,
                'evnt': ev,
                'param': param
            },
            //success: function (r) { alert("Call successful!"); },
            //error: function (jqxhr, txt, err) { alert(err+txt); },
            complete: function (jqxhr, txt) { console.log('Response: ' + jqxhr.status); }
        });
    }
</script>

</html>
