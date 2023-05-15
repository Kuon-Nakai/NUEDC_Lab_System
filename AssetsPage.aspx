<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssetsPage.aspx.cs" Inherits="AssetsPage" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>NUEDC实验室 - 元器件管理系统</title>
    
    <script src="https://ajax.microsoft.com/ajax/4.0/1/MicrosoftAjax.js" type="text/javascript"></script>

    <script>
        document.documentElement.classList.remove('no-js');
        document.documentElement.classList.add('js');

    </script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- CSS
    ================================================== -->
    <link rel="stylesheet" href="css/vendor.css" />
    <link rel="stylesheet" href="css/Style.css" />
    <link rel="stylesheet" href="css/Master.css" />

    <style type="text/css" media="screen">
        .s-styles {
            padding-top: var(--vspace-5);
            padding-bottom: var(--vspace-2);
            position: relative;
        }

            .s-styles .intro h1 {
                margin-top: 0;
            }
    </style>
    <!-- favicons
    ================================================== -->
    <link rel="apple-touch-icon" sizes="180x180" href="apple-touch-icon.png" />
    <link rel="icon" type="image/png" sizes="32x32" href="favicon-32x32.png" />
    <link rel="icon" type="image/png" sizes="16x16" href="favicon-16x16.png" />
    <link rel="manifest" href="site.webmanifest" />
</head>
<body id="top" class="ss-show">


    <!-- preloader
    ================================================== -->
    <div id="preloader" style="display: none;">
        <div id="loader"></div>
    </div>

    <input type="hidden" id="scrollPosition" value="0" />

    <form id="form1" runat="server" onsubmit="saveScrollPosition();">

        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <!-- page wrap
    ================================================== -->
        <div id="page" class="s-pagewrap">


            <!-- # site header 
        ================================================== -->
            <header class="s-header sticky offset scrolling">

                <div class="row s-header__inner">

                    <div class="s-header__block">
                        <div class="s-header__logo">
                            <a class="logo" href="index.html">
                                <img src="images/logo.svg" alt="Homepage">
                            </a>
                        </div>

                        <a class="s-header__menu-toggle" href="#0"><span>Menu</span></a>
                    </div>
                    <!-- end s-header__block -->

                    <nav class="s-header__nav">

                        <ul class="s-header__menu-links">
                            <li class=""><a href="#intro" class="smoothscroll">主页</a></li>
                            <li><a href="index.html#about">元器件</a></li>
                            <li><a href="index.html#services">活动</a></li>
                            <li class=""><a href="#footer" class="smoothscroll">联系我们</a></li>
                            <li><a href="index.html#folio" style="visibility:hidden">管理</a></li>
                        </ul>
                        <!-- s-header__menu-links -->

                        <!-- s-header__social -->

                    </nav>
                    <!-- end s-header__nav -->
                </div>
                <div style="margin-top: 10px; position: sticky" class="column u-pull-right">
                    <asp:Panel runat="server" ID="Alerts_pn" Width="70%">
                        <asp:Panel runat="server" ID="Alert_DBQueryEmpty_pn" Visible="false" HorizontalAlign="Center"></asp:Panel>
                    </asp:Panel>
                </div>
                <!-- end s-header__inner -->

            </header>
            <!-- end s-header -->

            <!-- styles
            ----------------------------------------------- -->
            <div id="styles" class="s-styles">

                <div class="row">

                    <div class="column lg-12 intro">

                        <h1>NUEDC实验室 元器件查询/借用系统</h1>
                    </div>

                </div>
                <!-- end row -->

                <div class="row u-add-half-bottom">

                    <div class="column">

                        <h3>元器件资产系统统计</h3>

                        <ul class="stats-tabs">
                            <li class="stats-tabs-li"><asp:Label Text="1,234" runat="server" ID="TotalEntries_lb" CssClass="stats-tabs-li-a" /> <em class="stats-tabs-li-a-em">总分类数</em></li>
                            <li><asp:Label Text="567" runat="server" ID="TotalComp_lb" CssClass="stats-tabs-li-a" /> <em class="stats-tabs-li-a-em">总器件数</em></li>
                            <li><asp:Label Text="23,456" runat="server" ID="TotalLent_lb" CssClass="stats-tabs-li-a" /> <em class="stats-tabs-li-a-em">总借出</em></li>
                            <li><asp:Label Text="3,456" runat="server" ID="TotalReturned_lb" CssClass="stats-tabs-li-a" /> <em class="stats-tabs-li-a-em">完成借出</em></li>
                            <li><asp:Label Text="456" runat="server" ID="TotalQueries_lb" CssClass="stats-tabs-li-a" /> <em class="stats-tabs-li-a-em">查询数</em></li>
                            <li><asp:Label Text="26" runat="server" ID="TotalReg_lb" CssClass="stats-tabs-li-a" /> <em class="stats-tabs-li-a-em">登记次数</em></li>
                        </ul>
                        
                        <div class="row u-add-half-bottom">

                            <div class="column lg-6 tab-12">
                                <div class="row">
                                    <h5 style="position:relative; top:-39px; width:150px">元器件搜索</h5>
                                    <asp:TextBox runat="server" TextMode="Search" />
                                    <a class="btn btn--primary" href="#0" onclick="bt_click('Search_bt_Click', 0, null);">搜索</a>
                                </div>
                                <div class="row">
                                    <h5 style="position:relative; top:-39px; width:150px">快捷查询</h5>
                                    <a class="btn btn--stroke" href="#0">待归还</a>
                                    <a class="btn btn--stroke" href="#0">可续借</a>
                                </div>
                                <div style="max-width:100%; overflow-x:scroll">
                                    <asp:GridView ID="Asset_gv" runat="server" Width="98%" AllowPaging="True" PageSize="15" AutoGenerateSelectButton="True" HorizontalAlign="Left" SelectedRowStyle-BackColor="#FFCC99" SelectedRowStyle-BorderColor="#FF9900" SelectedRowStyle-ForeColor="Red"></asp:GridView>
                                </div>
                                
                            </div>

                            <div class="column lg-6 tab-12">
                                <div class="row">
                                    <h5 style="position:relative; top:-39px; width:60px">分类</h5>
                                    <asp:DropDownList runat="server" AutoPostBack="true" CssClass="Ddl" ID="TypeSel0_ddl"> </asp:DropDownList>
                                    <asp:DropDownList runat="server" AutoPostBack="true" CssClass="Ddl" ID="TypeSel1_ddl"> </asp:DropDownList>
                                    <asp:DropDownList runat="server" AutoPostBack="true" CssClass="Ddl" ID="TypeSel2_ddl"> </asp:DropDownList>
                                </div>
                                
                                <h4>元器件信息<br/></h4>
                                <div style="position:relative; left:160px; top:-50px">Component Information</div>
                                <div class="row u-add-half-bottom">
                                    <div class="column lg-6 tab-12">
                                        元件名称<br/> <br />
                                        元件类型<br/> <br />
                                        值<br/> <br />
                                        位置<br/> <br />
                                        属性<br/>
                                        <asp:Panel runat="server" ID="Datasheet_pn0">
                                            参考文档
                                        </asp:Panel>
                                    </div>
                                    <div class="column lg-6 tab-12" style="text-align:right">
                                        <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="AssetName_lb" /> <br /> <br />
                                        <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="AssetClass_lb" /> <br /> <br />
                                        <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="PrimValue_lb" /> <br /> <br />
                                        <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="Location_lb"/> <br /> <br />
                                        <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="Property_lb"/> <br />
                                        <asp:Panel runat="server" ID="Datasheet_pn1">
                                            <asp:HyperLink runat="server" ID="Datasheet_lk" Text="Not available"></asp:HyperLink>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="row u-add-half-bottom">
                                     <div class="column lg-6 tab-12">
                                         可借出数量<br/> <br />
                                         申请借出(件)<br/> <br />
                                     </div>
                                     <div class="column lg-6 tab-12" style="text-align:right">
                                         <asp:Label Text="Unknown" Font-Bold="true" runat="server" ID="Borrowable_lb"/> <br /> <br />
                                        <asp:TextBox runat="server" CssClass="u-fullwidth" TextMode="Number" ID="BorrowQtySel_tb" OnTextChanged="BorrowQtySel_tb_TextChanged"  />
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
                                            待归还(件) <br /> <br />
                                            借出期限 <br /> <br />
                                        </div>
                                        <div class="column">
                                            <asp:Label Text="-1" runat="server" ID="QtyToReturn_lb" /> <br /> <br />
                                            <asp:Label Text="1919/8/10" runat="server" ID="Label1" /> <br /> <br />
                                        </div>
                                    </div>
                                    <a class="btn btn--stroke u-fullwidth" href="#0">归还</a>
                                </asp:Panel>
                            </div>

                        </div>

                        <h3 class="u-add-bottom">Buttons</h3>

                        <p>
                            <a class="btn btn--primary u-fullwidth" href="#0">Primary Button</a>
                            <a class="btn u-fullwidth" href="#0">Default Button</a>
                            <a class="btn btn--stroke u-fullwidth" href="#0">Stroke Button</a>
                        </p>

                        <h3 class="u-add-bottom">Code</h3>

                        <pre class=" language-css"><code class=" language-css">
    <span class="token selector">code</span> <span class="token punctuation">{</span>
    <span class="token property">font-size</span><span class="token punctuation">:</span> 1.4rem<span class="token punctuation">;</span>
    <span class="token property">margin</span><span class="token punctuation">:</span> 0 .2rem<span class="token punctuation">;</span>
    <span class="token property">padding</span><span class="token punctuation">:</span> .2rem .6rem<span class="token punctuation">;</span>
    <span class="token property">white-space</span><span class="token punctuation">:</span> nowrap<span class="token punctuation">;</span>
    <span class="token property">background</span><span class="token punctuation">:</span> #F1F1F1<span class="token punctuation">;</span>
    <span class="token property">border</span><span class="token punctuation">:</span> 1px solid #E1E1E1<span class="token punctuation">;</span>	
    <span class="token property">border-radius</span><span class="token punctuation">:</span> 3px<span class="token punctuation">;</span>
    <span class="token punctuation">}</span>
</code></pre>

                    </div>

                </div>
                <!-- end row -->

                <div class="row u-add-half-bottom">

                    <div class="column lg-6 tab-12">

                        <h1>H1 Heading Doloremque dolor voluptas est sequi omnis.</h1>
                        <p>
                            Doloremque dolor voluptas est sequi omnis. Pariatur ut aut. Sed enim tempora qui veniam qui cum vel. 
                        Voluptas odit at vitae minima. In assumenda ut. Voluptatem totam impedit accusantium reiciendis excepturi aut qui accusamus praesentium.
                        </p>

                        <h2>H2 Heading Doloremque dolor voluptas est sequi omnis.</h2>
                        <p>
                            Doloremque dolor voluptas est sequi omnis. Pariatur ut aut. Sed enim tempora qui veniam qui cum vel. 
                        Voluptas odit at vitae minima. In assumenda ut. Voluptatem totam impedit accusantium reiciendis excepturi aut qui accusamus praesentium.
                        </p>

                        <h3>H3 Heading Doloremque dolor voluptas est sequi omnis.</h3>
                        <p>
                            Doloremque dolor voluptas est sequi omnis. Pariatur ut aut. Sed enim tempora qui veniam qui cum vel. 
                        Voluptas odit at vitae minima. In assumenda ut. Voluptatem totam impedit accusantium reiciendis excepturi aut qui accusamus praesentium.
                        </p>


                    </div>

                    <div class="column lg-6 tab-12">
                        <h4>H4 Heading Doloremque dolor voluptas est sequi omnis.</h4>
                        <p>
                            Doloremque dolor voluptas est sequi omnis. Pariatur ut aut. Sed enim tempora qui veniam qui cum vel. 
                        Voluptas odit at vitae minima. In assumenda ut. Voluptatem totam impedit accusantium reiciendis excepturi aut qui accusamus praesentium.
                        </p>

                        <h5>H5 Heading Doloremque dolor voluptas est sequi omnis.</h5>
                        <p>
                            Doloremque dolor voluptas est sequi omnis. Pariatur ut aut. Sed enim tempora qui veniam qui cum vel. 
                        Voluptas odit at vitae minima. In assumenda ut. Voluptatem totam impedit accusantium reiciendis excepturi aut qui accusamus praesentium.
                        </p>

                        <h6>H6 Heading Doloremque dolor voluptas est sequi omnis.</h6>
                        <p>
                            Doloremque dolor voluptas est sequi omnis. Pariatur ut aut. Sed enim tempora qui veniam qui cum vel. 
                        Voluptas odit at vitae minima. In assumenda ut. Voluptatem totam impedit accusantium reiciendis excepturi aut qui accusamus praesentium.
                        </p>


                    </div>

                </div>
                <!-- end row -->

                <div class="row u-add-half-bottom">

                    <div class="column lg-6 tab-12">

                        <h3 class="u-add-bottom">Responsive Image</h3>

                        <figure>
                            <img src="images/wheel-500.jpg" srcset="images/wheel-1000.jpg 1000w, 
                            images/wheel-500.jpg 500w"
                                sizes="(max-width: 1000px) 100vw, 1000px" alt="">

                            <figcaption>Here is some random picture.
                            </figcaption>
                        </figure>

                    </div>

                    <div class="column lg-6 tab-12">

                        <h3 class="u-add-bottom">Responsive video</h3>

                        <div class="video-container">
                            <iframe src="https://player.vimeo.com/video/14592941?color=00a650&amp;title=0&amp;byline=0&amp;portrait=0" width="500" height="281" frameborder="0" webkitallowfullscreen="" mozallowfullscreen="" allowfullscreen=""></iframe>
                        </div>

                    </div>

                </div>
                <!-- end row -->

                <div class="row u-add-bottom">

                    <div class="column lg-12">

                        <h3>Tables</h3>
                        <p>Be sure to use properly formed table markup with <code>&lt;thead&gt;</code> and <code>&lt;tbody&gt;</code> when building a <code>table</code>.</p>

                        <div class="table-responsive">

                            <table>
                                <thead>
                                    <tr>
                                        <th>Name</th>
                                        <th>Age</th>
                                        <th>Sex</th>
                                        <th>Location</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>William J. Seymour</td>
                                        <td>34</td>
                                        <td>Male</td>
                                        <td>Azusa Street</td>
                                    </tr>
                                    <tr>
                                        <td>Jennie Evans Moore</td>
                                        <td>30</td>
                                        <td>Female</td>
                                        <td>Azusa Street</td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>

                    </div>

                </div>
                <!-- end row -->

                <div class="row">

                    <div class="column lg-12">
                        <h3>Pagination</h3>

                        <nav class="pgn">
                            <ul>
                                <li>
                                    <a class="pgn__prev" href="#0">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                            <path d="M12.707 17.293L8.414 13H18v-2H8.414l4.293-4.293-1.414-1.414L4.586 12l6.707 6.707z"></path></svg>
                                    </a>
                                </li>
                                <li><a class="pgn__num" href="#0">1</a></li>
                                <li><span class="pgn__num current">2</span></li>
                                <li><a class="pgn__num" href="#0">3</a></li>
                                <li><a class="pgn__num" href="#0">4</a></li>
                                <li><a class="pgn__num" href="#0">5</a></li>
                                <li><span class="pgn__num dots">…</span></li>
                                <li><a class="pgn__num" href="#0">8</a></li>
                                <li>
                                    <a class="pgn__next" href="#0">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                            <path d="M11.293 17.293l1.414 1.414L19.414 12l-6.707-6.707-1.414 1.414L15.586 11H6v2h9.586z"></path></svg>
                                    </a>
                                </li>
                            </ul>
                        </nav>

                    </div>

                </div>
                <!-- end row -->

                <div class="row">

                    <div class="column lg-6 tab-12">

                        <h3 class="u-add-bottom">Form Styles</h3>

                        <form>
                            <div>
                                <label for="sampleInput">Your email</label>
                                <input class="u-fullwidth" type="email" placeholder="test@mailbox.com" id="sampleInput">
                            </div>
                            <div>
                                <label for="sampleRecipientInput">Reason for contacting</label>
                                <div class="ss-custom-select">
                                    <select class="u-fullwidth" id="sampleRecipientInput">
                                        <option value="Option 1">Questions</option>
                                        <option value="Option 2">Report</option>
                                        <option value="Option 3">Others</option>
                                    </select>
                                </div>
                            </div>

                            <label for="exampleMessage">Message</label>
                            <textarea class="u-fullwidth" placeholder="Your message" id="exampleMessage"></textarea>

                            <label class="u-add-bottom">
                                <input type="checkbox">
                                <span class="label-text">Send a copy to yourself</span>
                            </label>

                            <input class="btn--primary u-fullwidth" type="submit" value="Submit">
                        </form>

                    </div>

                    <div class="column lg-6 tab-12">

                        <h3>Alert Boxes</h3>

                        <br>

                        <div class="alert-box alert-box--error hideit" style="display: none;">
                            <p>Error Message. Your Message Goes Here.</p>
                            <span class="alert-box__close"></span>
                        </div>
                        <!-- end error -->

                        <div class="alert-box alert-box--success hideit" style="display: none;">
                            <p>Success Message. Your Message Goes Here.</p>
                            <span class="alert-box__close"></span>
                        </div>
                        <!-- end success -->

                        <div class="alert-box alert-box--info hideit" style="display: none;">
                            <p>Info Message. Your Message Goes Here.</p>
                            <span class="alert-box__close"></span>
                        </div>
                        <!-- end info -->

                        <div class="alert-box alert-box--notice hideit" style="display: none;">
                            <p>Notice Message. Your Message Goes Here.</p>
                            <span class="alert-box__close"></span>
                        </div>
                        <!-- end notice -->

                    </div>

                </div>
                <!-- end row -->

                <div class="row">

                    <div class="lg-12 column">
                        <h3>Grid Columns</h3>
                    </div>

                </div>
                <!-- Row End-->

                <!--<h4>1/3 Columns</h4>  -->

                <div class="row">

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

                    <div class="lg-4 mob-12 column">
                        <p>
                            Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. 
                       
                        </p>
                    </div>

                </div>

                <!--<h4>1/4 Columns</h4>  -->

                <div class="row">

                    <div class="lg-3 tab-6 mob-12 column">
                        <p>
                            Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. 
                       
                        </p>
                    </div>

                    <div class="lg-3 tab-6 mob-12 column">
                        <p>
                            Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. 
                       
                        </p>
                    </div>

                    <div class="lg-3 tab-6 mob-12 column">
                        <p>
                            Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. 
                       
                        </p>
                    </div>

                    <div class="lg-3 tab-6 mob-12 column">
                        <p>
                            Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. 
                       
                        </p>
                    </div>

                </div>

                <!--<h4>1/2 Columns</h4>  -->

                <div class="row">

                    <div class="lg-6 mob-12 column">
                        <p>
                            Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. 
                       
                        </p>
                    </div>

                    <div class="lg-6 mob-12 column">
                        <p>
                            Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. 
                       
                        </p>
                    </div>

                </div>

                <!--<h4>2/3 Columns</h4>  -->

                <div class="row">

                    <div class="lg-8 tab-7 mob-12 column">
                        <p>
                            Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor.
                       
                        </p>
                    </div>

                    <div class="lg-4 tab-5 mob-12 column">
                        <p>
                            Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. 
                       
                        </p>
                    </div>

                </div>

                <!--<h4>3/4 Columns</h4>  -->

                <div class="row">

                    <div class="lg-3 tab-5 mob-12 column">
                        <p>
                            Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at. 
                       
                        </p>
                    </div>

                    <div class="lg-9 tab-7 mob-12 column">
                        <p>
                            Cras aliquet. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor.Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor. Integer faucibus, eros ac molestie placerat, enim tellus varius lacus,
                        nec dictum nunc tortor id urna. Suspendisse dapibus ullamcorper pede. Vivamus ligula ipsum,
                        faucibus at, tincidunt eget, porttitor non, dolor.
                       
                        </p>
                    </div>

                </div>

            </div>
            <!-- end styles -->

            </section> 
            <!-- end content -->


            <!-- # site-footer
        ================================================== -->
            <footer id="footer" class="s-footer target-section">

                <div class="row section-header" data-num="04">
                    <h3 class="column lg-12 section-header__pretitle text-pretitle">GET IN TOUCH</h3>
                    <div class="column lg-6 stack-on-1100 section-header__primary">
                        <h2 class="title text-display-1">Have an idea or an epic project in mind? Talk to us. 
                    Let's work together and make something great. 
                    Drop us a line at <a href="mailto:#0" title="">hello@mueller.com</a>
                        </h2>
                    </div>
                    <div class="column lg-6 stack-on-1100 section-header__secondary">

                        <div class="contact-block">
                            <h6>Where To Find Us</h6>
                            <p>
                                1600 Amphitheatre Parkway
                                <br>
                                Mountain View, California
                                <br>
                                94043  US
                       
                            </p>
                        </div>

                        <div class="contact-block">
                            <h6>Contact Infos</h6>
                            <ul class="contact-list">
                                <li><a href="tel:197-543-2345">+197 543 2345</a></li>
                                <li><a href="tel:197-123-9876">+197 123 9876</a></li>
                            </ul>
                        </div>

                    </div>
                </div>
                <!-- end section-header -->

                <div class="row list-block block-lg-one-half block-tab-whole block-stack-on-1000 s-footer__btns">
                    <div class="column list-block__item">
                        <div class="s-footer__contact-btn">
                            <a href="mailto:#0" class="btn btn--primary u-fullwidth">Let's Talk 
                        </a>
                        </div>
                    </div>
                    <div class="column list-block__item">
                        <div class="subscribe-form s-footer__subscribe">
                            <h6>Subscribe</h6>
                            <form id="mc-form" class="mc-form" novalidate="true">
                                <input type="email" name="EMAIL" id="mce-EMAIL" class="u-fullwidth text-center" placeholder="Your Email Address" title="The domain portion of the email address is invalid (the portion after the @)." pattern="^([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x22([^\x0d\x22\x5c\x80-\xff]|\x5c[\x00-\x7f])*\x22)(\x2e([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x22([^\x0d\x22\x5c\x80-\xff]|\x5c[\x00-\x7f])*\x22))*\x40([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x5b([^\x0d\x5b-\x5d\x80-\xff]|\x5c[\x00-\x7f])*\x5d)(\x2e([^\x00-\x20\x22\x28\x29\x2c\x2e\x3a-\x3c\x3e\x40\x5b-\x5d\x7f-\xff]+|\x5b([^\x0d\x5b-\x5d\x80-\xff]|\x5c[\x00-\x7f])*\x5d))*(\.\w{2,})+$" required="">
                                <input type="submit" name="subscribe" value="Subscribe" class="btn btn--primary u-fullwidth">
                                <!-- <div style="position: absolute; left: -5000px;" aria-hidden="true"><input type="text" name="b_cdb7b577e41181934ed6a6a44_9a91cfe7b3" tabindex="-1" value=""></div> -->
                                <div class="mc-status"></div>
                            </form>
                        </div>
                    </div>
                </div>
                <!-- end s-footer__btns -->

                <div class="row s-footer__bottom">

                    <div class="column lg-6 tab-12 s-footer__bottom-left">
                        <ul class="s-footer__social">
                            <li>
                                <a href="">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" style="fill: rgba(0, 0, 0, 1); transform: ; -ms-filter: ">
                                        <path d="M20,3H4C3.447,3,3,3.448,3,4v16c0,0.552,0.447,1,1,1h8.615v-6.96h-2.338v-2.725h2.338v-2c0-2.325,1.42-3.592,3.5-3.592 c0.699-0.002,1.399,0.034,2.095,0.107v2.42h-1.435c-1.128,0-1.348,0.538-1.348,1.325v1.735h2.697l-0.35,2.725h-2.348V21H20 c0.553,0,1-0.448,1-1V4C21,3.448,20.553,3,20,3z"></path></svg>
                                    <span class="u-screen-reader-text">Facebook</span>
                                </a>
                            </li>
                            <li>
                                <a href="">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" style="fill: rgba(0, 0, 0, 1); transform: ; -ms-filter: ">
                                        <path d="M19.633,7.997c0.013,0.175,0.013,0.349,0.013,0.523c0,5.325-4.053,11.461-11.46,11.461c-2.282,0-4.402-0.661-6.186-1.809 c0.324,0.037,0.636,0.05,0.973,0.05c1.883,0,3.616-0.636,5.001-1.721c-1.771-0.037-3.255-1.197-3.767-2.793 c0.249,0.037,0.499,0.062,0.761,0.062c0.361,0,0.724-0.05,1.061-0.137c-1.847-0.374-3.23-1.995-3.23-3.953v-0.05 c0.537,0.299,1.16,0.486,1.82,0.511C3.534,9.419,2.823,8.184,2.823,6.787c0-0.748,0.199-1.434,0.548-2.032 c1.983,2.443,4.964,4.04,8.306,4.215c-0.062-0.3-0.1-0.611-0.1-0.923c0-2.22,1.796-4.028,4.028-4.028 c1.16,0,2.207,0.486,2.943,1.272c0.91-0.175,1.782-0.512,2.556-0.973c-0.299,0.935-0.936,1.721-1.771,2.22 c0.811-0.088,1.597-0.312,2.319-0.624C21.104,6.712,20.419,7.423,19.633,7.997z"></path></svg>
                                    <span class="u-screen-reader-text">Twitter</span>
                                </a>
                            </li>
                            <li>
                                <a href="">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" style="fill: rgba(0, 0, 0, 1); transform: ; -ms-filter: ">
                                        <path d="M11.999,7.377c-2.554,0-4.623,2.07-4.623,4.623c0,2.554,2.069,4.624,4.623,4.624c2.552,0,4.623-2.07,4.623-4.624 C16.622,9.447,14.551,7.377,11.999,7.377L11.999,7.377z M11.999,15.004c-1.659,0-3.004-1.345-3.004-3.003 c0-1.659,1.345-3.003,3.004-3.003s3.002,1.344,3.002,3.003C15.001,13.659,13.658,15.004,11.999,15.004L11.999,15.004z"></path><circle cx="16.806" cy="7.207" r="1.078"></circle><path d="M20.533,6.111c-0.469-1.209-1.424-2.165-2.633-2.632c-0.699-0.263-1.438-0.404-2.186-0.42 c-0.963-0.042-1.268-0.054-3.71-0.054s-2.755,0-3.71,0.054C7.548,3.074,6.809,3.215,6.11,3.479C4.9,3.946,3.945,4.902,3.477,6.111 c-0.263,0.7-0.404,1.438-0.419,2.186c-0.043,0.962-0.056,1.267-0.056,3.71c0,2.442,0,2.753,0.056,3.71 c0.015,0.748,0.156,1.486,0.419,2.187c0.469,1.208,1.424,2.164,2.634,2.632c0.696,0.272,1.435,0.426,2.185,0.45 c0.963,0.042,1.268,0.055,3.71,0.055s2.755,0,3.71-0.055c0.747-0.015,1.486-0.157,2.186-0.419c1.209-0.469,2.164-1.424,2.633-2.633 c0.263-0.7,0.404-1.438,0.419-2.186c0.043-0.962,0.056-1.267,0.056-3.71s0-2.753-0.056-3.71C20.941,7.57,20.801,6.819,20.533,6.111z M19.315,15.643c-0.007,0.576-0.111,1.147-0.311,1.688c-0.305,0.787-0.926,1.409-1.712,1.711c-0.535,0.199-1.099,0.303-1.67,0.311 c-0.95,0.044-1.218,0.055-3.654,0.055c-2.438,0-2.687,0-3.655-0.055c-0.569-0.007-1.135-0.112-1.669-0.311 c-0.789-0.301-1.414-0.923-1.719-1.711c-0.196-0.534-0.302-1.099-0.311-1.669c-0.043-0.95-0.053-1.218-0.053-3.654 c0-2.437,0-2.686,0.053-3.655c0.007-0.576,0.111-1.146,0.311-1.687c0.305-0.789,0.93-1.41,1.719-1.712 c0.534-0.198,1.1-0.303,1.669-0.311c0.951-0.043,1.218-0.055,3.655-0.055c2.437,0,2.687,0,3.654,0.055 c0.571,0.007,1.135,0.112,1.67,0.311c0.786,0.303,1.407,0.925,1.712,1.712c0.196,0.534,0.302,1.099,0.311,1.669 c0.043,0.951,0.054,1.218,0.054,3.655c0,2.436,0,2.698-0.043,3.654H19.315z"></path></svg>
                                    <span class="u-screen-reader-text">Instagram</span>
                                </a>
                            </li>
                            <li>
                                <a href="">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" style="fill: rgba(0, 0, 0, 1); transform: ; msfilter: ;">
                                        <path d="M20.66 6.98a9.932 9.932 0 0 0-3.641-3.64C15.486 2.447 13.813 2 12 2s-3.486.447-5.02 1.34c-1.533.893-2.747 2.107-3.64 3.64S2 10.187 2 12s.446 3.487 1.34 5.02a9.924 9.924 0 0 0 3.641 3.64C8.514 21.553 10.187 22 12 22s3.486-.447 5.02-1.34a9.932 9.932 0 0 0 3.641-3.64C21.554 15.487 22 13.813 22 12s-.446-3.487-1.34-5.02zM12 3.66c2 0 3.772.64 5.32 1.919-.92 1.174-2.286 2.14-4.1 2.9-1.002-1.813-2.088-3.327-3.261-4.54A7.715 7.715 0 0 1 12 3.66zM5.51 6.8a8.116 8.116 0 0 1 2.711-2.22c1.212 1.201 2.325 2.7 3.34 4.5-2 .6-4.114.9-6.341.9-.573 0-1.006-.013-1.3-.04A8.549 8.549 0 0 1 5.51 6.8zM3.66 12c0-.054.003-.12.01-.2.007-.08.01-.146.01-.2.254.014.641.02 1.161.02 2.666 0 5.146-.367 7.439-1.1.187.373.381.793.58 1.26-1.32.293-2.674 1.006-4.061 2.14S6.4 16.247 5.76 17.5c-1.4-1.587-2.1-3.42-2.1-5.5zM12 20.34c-1.894 0-3.594-.587-5.101-1.759.601-1.187 1.524-2.322 2.771-3.401 1.246-1.08 2.483-1.753 3.71-2.02a29.441 29.441 0 0 1 1.56 6.62 8.166 8.166 0 0 1-2.94.56zm7.08-3.96a8.351 8.351 0 0 1-2.58 2.621c-.24-2.08-.7-4.107-1.379-6.081.932-.066 1.765-.1 2.5-.1.799 0 1.686.034 2.659.1a8.098 8.098 0 0 1-1.2 3.46zm-1.24-5c-1.16 0-2.233.047-3.22.14a27.053 27.053 0 0 0-.68-1.62c2.066-.906 3.532-2.006 4.399-3.3 1.2 1.414 1.854 3.027 1.96 4.84-.812-.04-1.632-.06-2.459-.06z"></path></svg>
                                    <span class="u-screen-reader-text">Dribbble</span>
                                </a>
                            </li>
                        </ul>
                    </div>

                    <div class="column lg-6 tab-12 s-footer__bottom-right">
                        <div class="ss-copyright">
                            <span>© Copyright Mueller 2022</span>
                            <span>Design by <a href="https://www.styleshout.com/">StyleShout</a></span>
                        </div>
                    </div>

                </div>
                <!-- s-footer__bottom -->

                <div class="ss-go-top link-is-visible">
                    <a class="smoothscroll" title="Back to Top" href="#top">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" style="fill: rgba(0, 0, 0, 1); transform: ; msfilter: ;">
                            <path d="M6 4h12v2H6zm5 10v6h2v-6h5l-6-6-6 6z"></path></svg>
                    </a>
                </div>
                <!-- end ss-go-top -->

            </footer>
            <!-- end footer -->


            <!-- Java Script
    ================================================== -->
            <script src="js/plugins.js"></script>
            <script src="js/main.js"></script>
            <script type="text/javascript">
                var r_i = true;
                function pollForValue(id, para, func) {
                    $.ajax({
                        url: 'AssetsPage.aspx',
                        method: 'POST',
                        dataType: 'json',
                        data: {
                            id: id,
                            evnt: 0,
                            param: para
                        },
                        //success: func,
                        error: function (xhr, status, err) {
                            if (r_i) {
                                r_i = false;
                                pollForValue(id, para, func);
                            }
                            console.log("Failed to poll for value: " + id);
                            console.log(xhr);
                            console.log(status);
                            console.log(err);
                        },
                        success: function (r1, r2, r3) {
                            console.log(r1);
                            console.log(r2);
                            console.log(r3);
                        }
                    });
                }
                window.onload = function () {
                    pollForValue("?totalEntries", null, function (r) { document.getElementById("TotalEntries_lb").innerText = JSON.parse(r.responseText).value; })

                }

                function saveScrollPosition() {
                    document.getElementById("scrollPosition").value = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
                }

                function restoreScrollPosition() {
                    var scrollPosition = document.getElementById("scrollPosition").value;
                    window.scrollTo(0, scrollPosition);
                }

                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function () {
                    restoreScrollPosition();
                });

            </script>
        </div>
    </form>
    <script type="text/javascript">
        function bt_click(id, ev, param) {
            $.ajax({
                url: 'AssetsPage.aspx',
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
</body>
</html>
