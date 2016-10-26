<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CostApplyForm.aspx.cs" Inherits="WebServiceForEFGP.Export.CostApplyForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>


    <style type="text/css">
        body {
            font-size: 12px;
            font-family: 微軟正黑體;
        }

        .block_all_container {
            padding: 10px;
            margin: auto 0;
            /*width:780px;*/
        }

        .block_title {
        }

        .block_title .block_title_logo {
            display: inline-block;
            width: 33%;
            height: 70px;
            vertical-align: top;
        }

        .block_title .block_title_main {
            display: inline-block;
            width: 33%;
            text-align: center;
            font-size: 20px;
            height: 70px;
            vertical-align: top;
            line-height: 70px;
        }

        .block_title .block_form_info {
            display: inline-block;
            width: 33%;
            height: 70px;
            vertical-align: bottom;
        }


        .block_header {
            margin: 10px 0;
            padding: 5px;
            background-color: #87286e;
            font-size: 14px;
            color: white;
        }

        .table_form_view {
            border: none;
            border-collapse: collapse;
            width: 100%;
            font-size: 12px;
        }

        .table_form_view .td_form_field {
            width: 100px;
            text-align: right;
            padding: 5px;
            vertical-align: top;
        }

        .table_form_view .td_form_value {
            padding: 5px;
            font-weight: 800;
            /*width: 16%;*/
            vertical-align: top;
        }

        .table_withhold_list {
            width: 100%;
            border: 1px solid #808080;
            border-collapse: collapse;
            margin: 5px 0;
        }

        .table_withhold_list td,
        .table_withhold_list th {
            border: 1px solid #808080;
        }


        .table_process_list {
            width: 100%;
            border: 1px solid #808080;
            border-collapse: collapse;
            margin: 5px 0;
        }

        .table_process_list td,
        .table_process_list th {
            border: 1px solid #808080;
            text-align: center;
        }


        @page {
            size: landscape;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="block_all_container">

            <div class="block_title">
                <div class="block_title_logo">
                    <img src="../Images/logo_140x66.jpg" />
                </div>
                <div class="block_title_main">
                    費用申請暨零用金報銷單

                </div>
                <div class="block_form_info">

                    <table style="border-collapse: collapse; border: none; width: 100%;">
                        <tbody>
                            <tr>
                                <td style="text-align: right; width: 85%;">申請日期：
                                </td>

                                <td>
                                    <asp:Label ID="lb_applyDate" runat="server" Text=""></asp:Label>

                                </td>
                            </tr>

                            <tr>
                                <td style="text-align: right;">表單序號：
                                </td>

                                <td>
                                    <asp:Label ID="lb_formSerialNumber" runat="server" Text=""></asp:Label>

                                </td>
                            </tr>

                        </tbody>

                    </table>
                </div>
            </div>


            <div class="block_issuer_info">
                <table class="table_form_view">
                    <tr>
                        <td class="td_form_field">* 填單人：
                        </td>
                        <td class="td_form_value">
                            <asp:Label ID="lb_issuer_id" runat="server" Text=""></asp:Label>
                            &nbsp;
                            <asp:Label ID="lb_issuer_name" runat="server" Text=""></asp:Label>

                        </td>
                        <td class="td_form_field">* 部門：
                        </td>
                        <td class="td_form_value">
                            <asp:Label ID="lb_issuer_dept_id" runat="server" Text=""></asp:Label>
                            &nbsp;
                            <asp:Label ID="lb_issuer_dept_name" runat="server" Text=""></asp:Label>

                        </td>
                        <td class="td_form_field">* 分機：
                        </td>
                        <td class="td_form_value">
                            <asp:Label ID="lb_issuer_ext_no" runat="server" Text=""></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td class="td_form_field">* 申請人：
                        </td>
                        <td class="td_form_value">
                            <asp:Label ID="lb_applyer_id" runat="server" Text=""></asp:Label>&nbsp; 
                            <asp:Label ID="lb_applyer_name" runat="server" Text=""></asp:Label>


                        </td>
                        <td class="td_form_field">* 部門：
                        </td>
                        <td class="td_form_value">
                            <asp:Label ID="lb_applyer_dept_id" runat="server" Text=""></asp:Label>&nbsp; 
                            <asp:Label ID="lb_applyer_dept_name" runat="server" Text=""></asp:Label>

                        </td>
                        <td class="td_form_field">* 分機：
                        </td>
                        <td class="td_form_value">
                            <asp:Label ID="lb_applyer_ext_no" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td class="td_form_field">* 申請類型：
                        </td>
                        <td class="td_form_value">
                            <asp:Label ID="lb_applyCategory" runat="server" Text=""></asp:Label>

                        </td>
                        <td class="td_form_field">直營門市：
                        </td>
                        <td class="td_form_value" colspan="3">
                            <asp:Label ID="lb_retailId" runat="server" Text=""></asp:Label>&nbsp;
                            <asp:Label ID="lb_retailName" runat="server" Text=""></asp:Label>



                        </td>
                    </tr>
                </table>
            </div>

            <div class="block_header">
                付款資訊
            </div>

            <div class="block_payee_info">
                <table class="table_form_view">
                    <tbody>
                        <tr>
                            <td class="td_form_field">* 收款人：
                            </td>
                            <td class="td_form_value">
                                <asp:Label ID="lb_payeeId" runat="server" Text=""></asp:Label>
                                &nbsp;
                                <asp:Label ID="lb_payeeName" runat="server" Text=""></asp:Label>

                            </td>


                            <td class="td_form_field">* 請款主旨：
                            </td>

                            <td class="td_form_value" colspan="3">
                                <asp:Label ID="lb_paymentTitle" runat="server" Text=""></asp:Label>

                            </td>
                        </tr>

                        <tr runat="server" id="tr_payee_info_line_payment_type">

                            <td class="td_form_field">* 付款方式：
                            </td>
                            <td class="td_form_value">
                                <asp:Label ID="lb_paymentType" runat="server" Text=""></asp:Label>

                            </td>

                            <td class="td_form_field">* 付款條件：
                            </td>

                            <td class="td_form_value">
                                <asp:Label ID="lb_paymentCondition" runat="server" Text=""></asp:Label>
                            </td>

                            <td class="td_form_field"><%--* 付款群組：--%>
                            </td>

                            <td class="td_form_value">
                                <asp:Label ID="lb_paymentGroup" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>

                        <tr runat="server" id="tr_payee_info_line_cert_type">
                            <td class="td_form_field">* 憑證種類：
                            </td>
                            <td class="td_form_value">
                                <asp:Label ID="lb_paymentlbCertificate" runat="server" Text=""></asp:Label>
                            </td>

                            <td class="td_form_field">* 幣別：
                            </td>

                            <td class="td_form_value">
                                <asp:Label ID="lb_currency" runat="server" Text=""></asp:Label>
                            </td>

                            <td class="td_form_field">* 匯率：
                            </td>

                            <td class="td_form_value">
                                <asp:Label ID="lb_currencyRate" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>

                        <tr runat="server" id="tr_payee_info_line_is_citizen">
                            <td class="td_form_field">* 是否為本國人：
                            </td>
                            <td class="td_form_value" colspan="5">
                                <asp:Label ID="lb_isCitizen" runat="server" Text=""></asp:Label>

                            </td>

                        </tr>

                        <tr runat="server" id="tr_payee_info_line_form_serial_number">
                            <td class="td_form_field">* 單號：
                            </td>
                            <td class="td_form_value" colspan="5">

                                <asp:Label ID="lb_formNo" runat="server" Text=""></asp:Label>

                            </td>

                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="block_header">
                <asp:Label ID="lb_payee_item" runat="server" Text="請款項目 (取得憑證 / 不扣繳)"></asp:Label>
            </div>



            <div class="list">
                <table class="table_withhold_list" id="table_notwithhold_list">
                    <thead>
                        <tr>
                            <th>序號</th>
                            <th>費用支出項目<br />
                                費用摘要</th>
                            <%--<th>費用摘要</th>--%>
                            <th>費用歸屬部門</th>
                            <%--<th>請款金額</th>--%>
                            <th>憑證類別<br />
                                賣方統編</th>
                            <%--<th>憑證日期</th>--%>
                            <th>憑證號碼<br />
                                憑證日期</th>
                            <%--<th>賣方統編</th>--%>
                            <th>憑證金額<br />
                                請款金額</th>
                            <th>憑證未稅<br />
                                請款未稅</th>
                            <%--<th>稅率</th>--%>
                            <th>稅額</th>
                            <%--<th>會計科目</th>--%>
                            <th>科目代碼<br />
                                會計科目</th>
                            <%--<th>電信業務註記</th>--%>
                            <th>電信會計<br />用途別</th>



                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rpt_notwithhold" runat="server" OnItemDataBound="rpt_notwithhold_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="lb_sort" runat="server" Text=""></asp:Label>

                                    </td>
                                    <td style="text-align: left;"><%#Eval("gd_paymentItemName") %><br />
                                        <%#Eval("gd_Description") %></td>
                                    <%--費用支出項目<br />費用摘要--%>

                                    <%--2016/05/27 Stephen Add 將費用歸屬部門的長度斷行,改用Label呈現--%>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lb_gd_belongDepartment" runat="server" Text='<%#Eval("gd_belongDepartment") %>'></asp:Label>
                                    </td>
                                    <%--費用歸屬部門--%>
                                    <%--<td style="text-align:right;"><%#Eval("gd_statementAmount") %></td>--%><%--請款金額--%>
                                    <td style="text-align: left;"><%#Eval("gd_paymentCertificateCategory") %><br />
                                        <%#Eval("gd_sellerTaxId") %></td>
                                    <%--憑證類別<br />賣方統編--%>

                                    <td style="text-align: left;"><%#Eval("gd_paymentCertificateNo") %><br />
                                        <%#Eval("gd_datePaymentCertificate") %></td>
                                    <%--憑證號碼--%>

                                    <td style="text-align: right;"><%#Eval("gd_paymentCertificateAmount") %><br />
                                        <%#Eval("gd_statementAmount") %></td>
                                    <%--憑證金額--%>
                                    <td style="text-align: right;"><%#Eval("gd_paymentCertificateWithoutTax") %><br />
                                        <%#Eval("gd_statementAmountWithoutTax") %></td>

                                    <%--稅率--%>
                                    <%--<td style="text-align: right;"><%#Eval("gd_taxRate") %></td>--%>

                                    <%--稅額--%>
                                    <td  style="text-align: right;">
                                        <%#Eval("gd_taxAmount") %></td>
                                                                       
                                    <%--會科代碼--%>
                                    <td style="text-align: left;">
                                        <%#Eval("gd_accountItemNo") %><br />
                                        <%#Eval("gd_accountItem") %>
                                    </td>                                    

                                    <%--電信會計用途別--%>
                                    <td style="text-align: left;">
                                        <%#Eval("gd_telSalesMemoName") %><br />
                                        <%#Eval("gd_paymentMemoName") %>
                                    </td>
                                    
                                </tr>
                            </ItemTemplate>


                        </asp:Repeater>

                        <tr>
                            <td colspan="21" style="text-align: right;">小計 &nbsp;
                                請款金額：
                                <asp:Label ID="lbNotWithhold_sum_statementAmount" runat="server" Text=""></asp:Label>
                                元&nbsp;
                                憑證金額：
                                <asp:Label ID="lbNotWithhold_sum_certificateAmount" runat="server" Text=""></asp:Label>
                                元&nbsp;
                                稅額：
                                <asp:Label ID="lbNotWithhold_sum_taxAmount" runat="server" Text=""></asp:Label>
                                元
                            </td>
                        </tr>




                    </tbody>
                </table>

                <table id="table_withhold_list" class="table_withhold_list">
                    <thead>
                        <tr>
                            <th>序號</th>
                            <th>費用支出項目<br />
                                費用摘要</th>
                            <%--<th>費用摘要</th>--%>
                            <th>費用歸屬部門</th>
                            <th>匯率<br />
                                幣別</th>
                            <th>原幣金額<br />
                                扣繳方式</th>
                            <th>台幣金額</th>
                            <%--<th>所得稅率</th>--%>
                            <th>扣繳稅額<br />
                                稅率</th>
                            <th>二代健保<br />
                                稅率</th>
                            <%--<th>二代健保</th>--%>
                            <th>付款金額</th>
                            <th>會科代碼<br />
                                會計科目</th>
                            <%--<th>會科代碼</th>--%>
                            <%--<th>電信業務註記</th>--%>
                            <th>電信會計<br />用途別</th>
                            <%--  <th>電信業務代碼</th>--%>
                            <%--<th>費用用途註記</th>--%>
                            <%--<th>費用用途代碼</th>--%>
                            <%--<th>格式代碼</th>--%>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rpt_withhold" runat="server" OnItemDataBound="rpt_notwithhold_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="lb_sort" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="text-align: left;"><%#Eval("gd_paymentItemName") %><br />
                                        <%#Eval("gd_Description") %></td>
                                    <%--費用支出項目--%>
                                    <%--<td style="text-align:left;"><%#Eval("gd_Description") %></td>--%><%--費用摘要--%>
                                    <%--<td style="text-align:left;"><%#Eval("gd_belongDepartment") %></td>--%><%--費用歸屬部門--%>
                                    <%--2016/05/27 Stephen Add 將費用歸屬部門的長度斷行,改用Label呈現--%>
                                    <td style="text-align: left;">
                                        <asp:Label ID="lb_gd_belongDepartment" runat="server" Text='<%#Eval("gd_belongDepartment") %>'></asp:Label>
                                    </td>
                                    <%--費用歸屬部門--%>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lb_gd_currencyRate" runat="server" Text='<%#Eval("gd_originCurrency") %>'></asp:Label><br />
                                        <%#Eval("gd_originCurrency") %></td>
                                    <%--幣別--%>
                                    <td style="text-align: right;"><%#Eval("gd_originCurrencyAmount") %><br />
                                        <asp:Label ID="lb_gd_withholdType" runat="server" Text='<%#Eval("gd_withholdType") %>'></asp:Label></td>
                                    <%--原幣金額--%>
                                    <td style="text-align: right;"><%#Eval("gd_NTDAmount") %></td>
                                    <%--扣繳稅額 && 稅率--%>
                                    <%--<td style="text-align:right;"><%#Eval("gd_incomeTaxRate") %></td>--%><%--所得稅率--%>
                                    <td style="text-align: right;"><%#Eval("gd_taxAmount") %><br />
                                        <%#Eval("gd_incomeTaxRate") %></td>
                                    <%--二代健保 && 二代健保稅率 --%>
                                    <td style="text-align: right;">
                                        <%#Eval("gd_secondNHIAmount") %><br /><%#Eval("gd_secondNHITaxRate") %>
                                    </td>

                                    <%--付款金額--%>                                    
                                    <td style="text-align: right;"><%#Eval("gd_paymentAmount") %></td>

                                    <%--會計科目--%>
                                    <td style="text-align: left;"><%#Eval("gd_accountItemNo") %><br />
                                        <%#Eval("gd_accountItem") %></td>                                    
                                    <%--<td style="text-align:left;"><%#Eval("gd_accountItemNo") %></td>--%><%--會科代碼--%>
                                    <td style="text-align: left;"><%#Eval("gd_telSalesMemoName") %><br />
                                        <%#Eval("gd_paymentMemoName") %></td>
                                    <%--電信業務註記--%>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </tbody>
                </table>
            </div>

            <div class="block_header">
                簽核歷程
            </div>

            <div class="process_list_container">


                <table class="table_process_list">
                    <thead>
                        <tr>
                            <th>部門</th>
                            <th>人員</th>
                            <th>完成時間</th>
                            <th>簽核關卡</th>
                            <th>簽核意見</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rpt_process_list" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#Eval("organizationUnitName") %>
                                    </td>
                                    <td>
                                        <%#Eval("userName") %>
                                    </td>
                                    <td>
                                        <%#  DataBinder.Eval(Container.DataItem, "completedTime", "{0:yyyy/M/d hh:mm:ss}") %>
                                    </td>
                                    <td>
                                        <%#Eval("workItemName") %>
                                    </td>
                                    <td>
                                        <%#Eval("executiveComment") %>
                                    </td>

                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>

            </div>

        </div>
        <asp:Literal ID="lt_alert" runat="server"></asp:Literal>

    </form>
    <script src="/Scripts/jquery-1.10.2.min.js"></script>
    <script>


        if ($("#table_notwithhold_list").find('tbody tr').length == 1) {
            $("#table_notwithhold_list").hide();
        } else {
            $("#table_notwithhold_list").show();
        }

        if ($("#table_withhold_list").find('tbody tr').length == 0) {
            $("#table_withhold_list").hide();
        } else {
            $("#table_withhold_list").show();
        }


        if ($('[id$="lb_formSerialNumber"]').length > 0) {
            window.print();
        }


    </script>
</body>
</html>
