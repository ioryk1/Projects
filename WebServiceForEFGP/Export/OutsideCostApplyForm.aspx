<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutsideCostApplyForm.aspx.cs" Inherits="WebServiceForEFGP.Export.OutsideCostApplyForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Content/font-awesome.min.css" rel="stylesheet" />
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

        .table_gd_statement_list {
            width: 100%;
            border: 1px solid #808080;
            border-collapse: collapse;
            margin: 5px 0;
        }

        .table_gd_statement_list td,
        .table_gd_statement_list th {
            border: 1px solid #808080;
            padding:5px;
        }

        .table_gd_statement_list td.currency {
            text-align:right;
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
        .table_sum_list {
            width:100%;
            border-collapse:collapse;
        }

        .table_sum_list th,
        .table_sum_list td {
            width:25%;
            padding:10px;
        }
        .table_sum_list td.table_sum_field {
            text-align:right;
        }

        .table_sum_list td.table_sum_value {
            text-align:right;
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
                    公出費用報銷單

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
                </tbody>
            </table>
            </div>
            


            <div class="block_header">
                請款憑證
            </div>

            <div class="list">
                <table class="table_gd_statement_list">
                    <thead>
                        <tr>
                            <th>
                                項次
                            </th>
                            <th>
                                <div>部門代碼</div>
                                <div>部門名稱</div>
                            </th>
                            <th>
                                <div>交通方式</div>                                
                            </th>
                            <th>
                                <div>工具</div>                                
                            </th>
                            <th>
                                <div>地點_起</div>                                
                                <div>地點_迄</div>                                
                            </th>
                            <th>
                                <div>公出事由</div>                                                                
                            </th>

                            <th>
                                <div>憑證號碼</div>  
                                <div>憑證日期</div>  
                            </th>

                              <th>
                                <div>憑證金額</div>  
                                <div>請款金額</div>  
                            </th>

                            <th>
                                <div>憑證未稅金額</div>  
                                <div>請款未稅金額</div>  
                            </th>

                             <th>
                                <div>稅額</div>                                  
                            </th>
                            
                             <th>
                                 <div>代碼科目</div>                                  
                                 <div>會計科目</div>                                  
                            </th>

                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rpt_statement_list" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: center;"><%#Eval("gd_no") %>
                                    </td>
                                    <td>
                                        <div><%#Eval("gd_BelongDepartmentId") %></div>
                                        <div><%#Eval("gd_BelongDepartmentName") %></div>
                                    </td>
                                    <td style="text-align: center;">
                                        <div><%#Eval("gd_TransferType") %></div>
                                    </td>
                                    <td style="text-align: center;">
                                        <div><%#Eval("gd_PublicTransportation") %></div>
                                    </td>
                                    <td style="text-align: center;">
                                        <div><%#Eval("gd_PubicLocationStart") %></div>
                                        <div><%#Eval("gd_PubicLocationEnd") %></div>
                                    </td>
                                    <td style="text-align: center;">
                                        <div><%#Eval("gd_PublicBizDescription") %></div>
                                    </td>

                                    <td style="text-align: center;">
                                        <div><%#Eval("gd_PaymentCertificateNo") %></div>
                                        <div><%#Eval("gd_DatePaymentCertificate_txt") %></div>
                                    </td>

                                    <td class="currency">
                                        <div><%#Eval("gd_PaymentCertificatePrice") %></div>
                                        <div><%#Eval("gd_StatementAmount") %></div>
                                    </td>

                                    <td class="currency">
                                        <div><%#Eval("gd_PaymentCertificateWithoutTaxPrice") %></div>
                                        <div><%#Eval("gd_StatementAmountWithoutTax") %></div>
                                    </td>

                                    <td class="currency">
                                        <div><%#Eval("gd_TaxPrice") %></div>
                                    </td>

                                    <td>
                                        <div><%#Eval("gd_AccountItemNo") %></div>
                                        <div><%#Eval("gd_AccountItemName") %></div>
                                    </td>
                                </tr>
                            </ItemTemplate>

                        </asp:Repeater>
                        
                        
                    </tbody>
                </table>
            </div>

            <div id="block_private_trans_header"  runat="server" class="block_header">
                私車公用 - 明細
            </div>

            <div id="block_private_trans_content"  runat="server"  class="list">
                <table class="table_gd_statement_list">
                    <thead>
                        <tr>
                            <th>
                                項次
                            </th>
                            <th>
                                <div>公出日期</div>
                                
                            </th>
                            <th>
                                <div>公出事由</div>                                
                            </th>
                            <th>
                                <div>地點(起)</div>                                
                            </th>
                            <th>
                                <div>地點(訖)</div>                                                                
                            </th>
                            <th>
                                <div>汽/機車</div>                                                                
                            </th>

                            <th>
                                <div>里程數</div>                                  
                            </th>

                            <th>
                                <div>油資補助金額</div>                                  
                            </th>

                            <th>
                                <div>eTag</div>                                  
                            </th>

                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rpt_privateTrans_list" runat="server">
                            <ItemTemplate>
                                <tr>
                            <td style="text-align:center;">
                                <%#Eval("gd_no") %>
                            </td>
                            <td style="text-align:center;">
                                <div><%#Eval("gd_DateBizTrip") %></div>
                                
                            </td>
                            <td style="text-align:center;">
                                <div><%#Eval("gd_BizTripDescription") %></div>                                
                            </td>
                            <td style="text-align:center;">
                                <div><%#Eval("gd_BizTripLocationStart") %></div>                                
                            </td>
                            <td style="text-align:center;">
                                <div><%#Eval("gd_BizTripLocationEnd") %></div>                                                                
                            </td>
                            <td style="text-align:center;">
                                <div><%#Eval("gd_PrivateTransportation") %></div>                                                                
                            </td>

                            <td class="currency">
                                <div><%#Eval("gd_EarnMileage") %></div>                                  
                            </td>

                            <td class="currency">
                                <div><%#Eval("gd_OilTransportationSubsidyPrice") %></div>                                  
                            </td>

                            <td class="currency">
                                <div><%#Eval("gd_EtagPrice") %></div>                                  
                            </td>

                        </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        

                        <tr>
                            <td  colspan="7" style="text-align:right;" >
                                小計
                            </td>
                            

                            <td class="currency">
                                <div>
                                    <asp:Label ID="lb_txtTotalAmount_oilTransportationSubsidyPrice" runat="server" Text=""></asp:Label></div>                                  
                            </td>

                            <td class="currency">
                                <div><asp:Label ID="lb_txtTotalAmount_etagPrice" runat="server" Text=""></asp:Label></div>                                  
                            </td>

                        </tr>
                    </tbody>
                </table>

            </div>



            <div class="block_header">
                補助金額計算
            </div>

            <div class="list">
                <table class="table_sum_list">
                    <thead>
                        <tr>
                            <th colspan="2">私車公用</th>
                            
                            <th colspan="2">大眾交通工具</th>
                            
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="table_sum_field">
                                <span runat="server" id="block_earnMileage_check" visible="false" >
                                    <i class="fa fa-check-circle"></i>
                                </span>
                                依里程數計算補助：</td>
                            <td class="table_sum_value">
                                <asp:Label ID="lb_txtPrivateTrans_earnMileageSubsidyAmount" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span>
                            </td>
                            <td class="table_sum_field">憑證金額小計：</td>
                            <td class="table_sum_value">
                                <asp:Label ID="lb_txtPublicTrans_sumPaymentCertificateAmount" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="table_sum_field">
                                <span runat="server" id="block_subsidyLimit_check" visible="false" >
                                    <i class="fa fa-check-circle"></i>
                                </span>
                                私車公用補助上限：</td>
                            <td class="table_sum_value"><asp:Label ID="lb_txtPrivateTrans_subsidyLimitAmount" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                            <td class="table_sum_field">憑證未稅金額小計：</td>
                            <td class="table_sum_value"><asp:Label ID="lb_txtPublicTrans_sumPaymentCertificateWithoutTaxAmount" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                        </tr>

                        <tr>
                            <td class="table_sum_field">憑證金額小計：</td>
                            <td class="table_sum_value"><asp:Label ID="lb_txtPrivateTrans_sumPaymentCertificatePrice" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                            <td class="table_sum_field">稅額小計：</td>
                            <td class="table_sum_value"><asp:Label ID="lb_txtPublicTrans_sumTaxAmount" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                        </tr>
                         <tr>
                            <td class="table_sum_field">請款總額：</td>
                            <td class="table_sum_value"><asp:Label ID="lb_txtPrivateTrans_sumStatementAmount" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                            <td class="table_sum_field">請款金額小計：</td>
                            <td class="table_sum_value"><asp:Label ID="lb_txtPublicTrans_sumStatementAmount" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                        </tr>

                          <tr>
                            <td class="table_sum_field">未稅請款金額小計：</td>
                            <td class="table_sum_value"><asp:Label ID="lb_txtPrivateTrans_sumStatementWithoutAmount" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                            <td class="table_sum_field">未稅請款金額小計：</td>
                            <td class="table_sum_value"><asp:Label ID="lb_txtPublicTrans_sumStatementWithoutTaxAmount" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                        </tr>

                          <tr>
                            <td class="table_sum_field">稅額小計：</td>
                            <td class="table_sum_value"><asp:Label ID="lb_txtPrivateTrans_sumTaxAmount" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                            <td class="table_sum_field"></td>
                            <td class="table_sum_value"></td>
                        </tr>

                        <tr>
                            <td class="table_sum_field">e-tag 小計：</td>
                            <td class="table_sum_value"><asp:Label ID="lb_txtPrivateTrans_sumETagPrice" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                            <td class="table_sum_field"></td>
                            <td class="table_sum_value"></td>
                        </tr>

                        <tr style="border-top:1px solid #808080;">
                            <td class="table_sum_field">本次請款總額：</td>
                            <td class="table_sum_value">                                
                                <asp:Label ID="lb_txtSumTotalPrice" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                            <td class="table_sum_field"></td>
                            <td class="table_sum_value"></td>
                        </tr>

                        <tr>
                            <td class="table_sum_field">稅額總計：</td>
                            <td class="table_sum_value">
                                <asp:Label ID="lb_txtSumTaxAmount" runat="server" Text=""></asp:Label>&nbsp;<span class="unit">元</span></td>
                            <td class="table_sum_field"></td>
                            <td class="table_sum_value"></td>
                        </tr>


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


            <asp:Literal ID="lt_alert" runat="server"></asp:Literal>
        </div>
    </form>
    <script src="/Scripts/jquery-1.10.2.min.js"></script>
    <script>
        
        if ($('[id$="lb_formSerialNumber"]').text().length > 0) {
            window.print();
        }
    </script>
</body>
</html>



