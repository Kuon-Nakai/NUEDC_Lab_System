<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_TopNavLinks" runat="Server">
    <li class=""><a href="#intro" class="smoothscroll" style="color:white">主页</a></li>
    <li><a href="AssetPage.aspx">元器件</a></li>
    <li><a href="Events.aspx">活动</a></li>
    <li class=""><a href="#footer" class="smoothscroll">联系我们</a></li>
    <li><a href="Management.aspx" style="visibility: hidden">管理</a></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <asp:LinkButton Text="Log in" runat="server" CssClass="btn btn--stroke" ForeColor="White" BorderColor="White" ID="Login_Jmp_bt" OnClick="Login_Jmp_bt_Click" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <asp:Panel ID="Alerts_pn" runat="server">
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section id="intro" class="s-intro target-section">

        <div class="s-intro__bg"></div>

        <div class="row s-intro__content">

            <div class="s-intro__content-bg"></div>

            <div class="column lg-12 s-intro__content-inner">

                <h1 class="s-intro__content-title">DHU NUEDC实验室
                    <br />
                    综合服务管理平台
                </h1>
                <h5 style="color: white">DHU NUEDC Innovation Lab
                    <br />
                    General Service & Management Platform
                    <br />
                </h5>

                <div class="s-intro__content-buttons">
                    <a href="#services" class="btn btn--stroke s-intro__content-btn smoothscroll">服务浏览</a>
                </div>

            </div>
            <!-- s-intro__content-inner -->

        </div>
        <!-- s-intro__content -->

        <div class="s-intro__scroll-down">
            <a href="#about" class="smoothscroll">
                <span>Scroll Down</span>
            </a>
        </div>
        <!-- s-intro__scroll-down -->

    </section>
    <!-- end s-intro -->
    <section id="services" class="s-folio target-section">
        <h2 style="color: white; margin-left: 150px">服务</h2>

        <div id="bricks" class="row bricks">
            <div class="column lg-12 masonry">
                <div class="bricks-wrapper">

                    <div class="grid-sizer"></div>

                    <article class="brick brick--double entry">
                        <asp:HyperLink NavigateUrl="#modal-01" runat="server" CssClass="entry__link">
                            <div class="entry__thumb">
                                <img src="images/mumumumumumumu.jpg" alt="">
                            </div>
                            <div class="entry__info">
                                <div class="entry__cat">Components Management</div>
                                <h4 class="entry__title">元器件查询/借用管理系统</h4>
                            </div>
                        </asp:HyperLink>
                    </article>
                    <!-- entry -->

                    <article class="brick brick--double entry">
                        <a href="#modal-02" class="entry__link">
                            <div class="entry__thumb">
                                <img src="images/Lyco.jpg" alt="">
                            </div>
                            <div class="entry__info">
                                <div class="entry__cat">Event Signup System</div>
                                <h4 class="entry__title">活动报名管理系统</h4>
                            </div>
                        </a>
                    </article>
                    <!-- entry -->

                    <article class="brick entry">
                        <a href="#modal-03" class="entry__link">
                            <div class="entry__thumb">
                                <img src="images/bocchi-glitch.gif" style="width: 300%" alt="">
                            </div>
                            <div class="entry__info">
                                <div class="entry__cat">Security Monitor</div>
                                <h4 class="entry__title">安全管理系统</h4>
                            </div>
                        </a>
                    </article>
                    <!-- entry -->

                    <article class="brick entry">
                        <a href="#modal-04" class="entry__link">
                            <div class="entry__thumb">
                                <img src="images/folio/lamp@2x.jpg" alt="">
                            </div>
                            <div class="entry__info">
                                <div class="entry__cat">E-Commerce</div>
                                <h4 class="entry__title">Lamp</h4>
                            </div>
                        </a>
                    </article>
                    <!-- entry -->

                    <article class="brick entry">
                        <a href="#modal-05" class="entry__link">
                            <div class="entry__thumb">
                                <img src="images/folio/tropical@2x.jpg" alt="">
                            </div>
                            <div class="entry__info">
                                <div class="entry__cat">Frontend Design</div>
                                <h4 class="entry__title">Tropical</h4>
                            </div>
                        </a>
                    </article>
                    <!-- entry -->

                    <article class="brick entry">
                        <a href="#modal-06" class="entry__link">
                            <div class="entry__thumb">
                                <img src="images/folio/woodcraft@2x.jpg" alt="">
                            </div>
                            <div class="entry__info">
                                <div class="entry__cat">E-Commerce</div>
                                <h4 class="entry__title">Woodcraft</h4>
                            </div>
                        </a>
                    </article>
                    <!-- entry -->

                </div>
                <!-- end bricks-wrapper -->
            </div>
            <!-- end masonry -->
        </div>
        <!-- end bricks -->

        <div id="modal-01" hidden>
            <asp:HiddenField ID="modal1_hf" runat="server" />
            <div class="modal-popup">
                <%--<img src="images/mumumumumumumu.jpg" alt="">--%>

                <div class="modal-popup__desc">
                    <h4>服务简介</h4>
                    <h3>元器件查询/借用管理系统</h3>
                    <hr />
                    <p>
                        借助此系统, 可自行查询已登记的元器件, 物联快速定位, 并在线登记借出。
                        <br />
                        <br />
                        返还元器件请交还给实验室成员并协助登记。<br />
                    </p>
                    <a href="AssetsPage.aspx" class="btn btn--primary u-fullwidth">前往</a>
                    <asp:HyperLink NavigateUrl="AssetsManage.aspx" runat="server" Visible="false" CssClass="btn btn--stroke u-fullwidth" ID="AssetsManage_lk">管理</asp:HyperLink>
                    <ul class="modal-popup__cat">
                        <li>借出操作需要登录</li>
                    </ul>
                </div>
            </div>
        </div>
        <!-- end modal -->

        <div id="modal-02" hidden>
            <div class="modal-popup">
                <div class="modal-popup__desc">
                    <h4>服务简介</h4>
                    <h3>活动报名管理系统</h3>
                    <hr />
                    <p>
                        在此系统可以进行活动报名, 填报信息, 与在线文档相比更加安全高效。
                        <br />
                        <br />
                        信息填报中如有特殊要求, 请联系活动负责人。<br />
                    </p>
                    <a href="Events.aspx" class="btn btn--primary u-fullwidth">前往</a>
                    <asp:HyperLink NavigateUrl="EventsManage.aspx" runat="server" Visible="false" CssClass="btn btn--stroke u-fullwidth" ID="EventsManage_lk">管理</asp:HyperLink>
                    <ul class="modal-popup__cat">
                        <li>使用快速填报功能需要登录</li>
                    </ul>
                </div>
            </div>
        </div>
        <!-- end modal -->

        <div id="modal-03" hidden>
            <div class="modal-popup">
                <div class="modal-popup__desc">
                    <h4>服务简介</h4>
                    <h3>安全管理系统</h3>
                    <hr />
                    <p>
                        实时监测实验室物联传感网络及其各元件状态。
                        <br />
                        <br />
                        该系统部分功能要求2FA验证才能执行。<br />
                    </p>
                    <asp:HyperLink NavigateUrl="Security.aspx" runat="server" Visible="false" CssClass="btn btn--stroke u-fullwidth" ID="Security_lk">管理</asp:HyperLink>
                    <ul class="modal-popup__cat">
                        <li>使用该系统需要实验室管理员权限</li>
                    </ul>
                </div>
            </div>
        </div>
        <!-- end modal -->

        <div id="modal-04" hidden>
            <div class="modal-popup">
                <img src="images/folio/gallery/g-lamp.jpg" alt="">

                <div class="modal-popup__desc">
                    <h5>The Lamp</h5>
                    <p>Dolores velit qui quos nobis. Aliquam delectus voluptas quos possimus non voluptatem voluptas voluptas. Est doloribus eligendi porro doloribus voluptatum.</p>
                    <ul class="modal-popup__cat">
                        <li>E-Commerce</li>
                    </ul>
                </div>

                <a href="https://www.behance.net/" class="modal-popup__details">Project link</a>
            </div>
        </div>
        <!-- end modal -->

        <div id="modal-05" hidden>
            <div class="modal-popup">

                <img src="images/folio/gallery/g-tropical.jpg" alt="">

                <div class="modal-popup__desc">
                    <h5>Tropical</h5>
                    <p>Proin gravida nibh vel velit auctor aliquet. Aenean sollicitudin, lorem quis bibendum auctor, nisi elit consequat ipsum, nec sagittis sem nibh id elit.</p>
                    <ul class="modal-popup__cat">
                        <li>Frontend Design</li>
                    </ul>
                </div>

                <a href="https://www.behance.net/" class="modal-popup__details">Project link</a>
            </div>
        </div>
        <!-- end modal -->

        <div id="modal-06" hidden>
            <div class="modal-popup">
                <img src="images/folio/gallery/g-woodcraft.jpg" alt="">

                <div class="modal-popup__desc">
                    <h5>Woodcraft</h5>
                    <p>Quisquam vel libero consequuntur autem voluptas. Qui aut vero. Omnis fugit mollitia cupiditate voluptas. Aenean sollicitudin, lorem quis bibendum auctor.</p>
                    <ul class="modal-popup__cat">
                        <li>E-Commerce</li>
                        <li>Product Design</li>
                    </ul>
                </div>

                <a href="https://www.behance.net/" class="modal-popup__details">Project link</a>
            </div>
        </div>
        <!-- end modal -->

    </section>

    <section id="about" class="s-intro target-section">
        <div style="margin-left: 50px; color:white">
            <h1 style="color:white">关于NUEDC实验室</h1>
            <div class="row u-pull-left">

                <div class="lg-4 mob-12 column">
                    <p>
                        Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. 
                    </p>
                </div>

                <div class="lg-4 mob-12 column">
                    <p>
                        Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. 
                    </p>
                </div>

            </div>
        </div>
        <img src="images/pcb2.png" style="position:absolute; width:900px; right:0px; top:-1px" />
    </section>

</asp:Content>

