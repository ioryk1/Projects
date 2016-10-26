<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Manage.Master" AutoEventWireup="true" CodeBehind="PaymentItemSetting.aspx.cs" Inherits="WebServiceForEFGP.Manage.PaymentItemSetting" %>

<%@ Register Src="~/Manage/component_pager.ascx" TagPrefix="uc1" TagName="component_pager" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <style type="text/css">
      .table_paymentItemSetting_form {
          border:1.5px solid #808080;
      }
      .table_paymentItemSetting_form .td_field {
          width:150px;
          background-color:#ECECEC;
          text-align:right;
          font-weight:800;

      }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:MultiView ID="mv_paymentItemSetting" runat="server">
        <asp:View ID="vw_paymentItemSetting_list" runat="server">
            

        <div style="border: 1px solid #ccc; padding: 15px;">
            <fieldset class="clearfix form-inline">
                <legend>新增費用支費項目</legend>

                <div class="form-group">
                    <label>類型：</label>

                    <asp:DropDownList ID="ddl_paymentItemSetting_type"
                        DataTextField="Text" DataValueField="Value"
                        CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </div>

                <div class=" form-group">
                    <label>品項：</label>
                    <asp:TextBox ID="txt_paymentItemSetting_name" CssClass="form-control" runat="server"></asp:TextBox>
                </div>



                <div class=" form-group">
                    <label>會計科目：</label>
                    <%--<input type="text" id="txt_paymentItemSetting_accountItem" class="form-control" />--%>
                    <asp:DropDownList ID="ddl_paymentItemSetting_accountItem" runat="server" DataTextField="Text" DataValueField="Value"></asp:DropDownList>


                    <asp:HiddenField ID="hid_paymentItemSetting_accountItem" runat="server" />

                </div>

                <div class=" form-group">
                    <label>說明：</label>
                    <asp:TextBox ID="txt_paymentItemSetting_memo" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <br />
                <br />



                <div class="form-group form-group-sm">
                    <label>
                        稅率:
                    </label>

                    <div class="input-group">
                        <asp:TextBox ID="txt_paymentItemSetting_taxRate" CssClass="form-control input-group-sm input-sm" runat="server"></asp:TextBox>
                        <div class=" input-group-addon">%</div>
                    </div>

                </div>

                <div class="form-group">
                    <label>
                        外國人所得稅率：
                    </label>

                    <div class="input-group">
                        <asp:TextBox ID="txt_paymentItemSetting_foreignRate" CssClass="form-control input-group-sm input-sm" runat="server"></asp:TextBox>
                        <div class=" input-group-addon">%</div>
                    </div>

                </div>

                <br />
                <br />

                <div class="form-group">
                    <label>
                        本國人所得稅率：
                    </label>

                    <div class="input-group">
                        <asp:TextBox ID="txt_paymentItemSetting_localRate" CssClass="form-control input-group-sm input-sm" runat="server"></asp:TextBox>
                        <div class=" input-group-addon">%</div>
                    </div>

                </div>

                <div class="form-group">
                    <label>
                        二代健保稅率：
                    </label>

                    <div class="input-group">
                        <asp:TextBox ID="txt_paymentItemSetting_secondNHITaxRate" CssClass="form-control input-group-sm input-sm" runat="server"></asp:TextBox>
                        <div class=" input-group-addon">%</div>
                    </div>

                </div>
                <br />
                <br />


                <div class="form-group">
                    <label>
                        代碼：
                    </label>

                    <asp:TextBox CssClass="form-control" ID="txt_paymentItemSetting_code" runat="server"></asp:TextBox>

                </div>

                <div class="form-group">
                    <label>
                        此項目要會簽至經費所屬部門：
                    </label>

                    <asp:CheckBox ID="chk_paymentItemSetting_needCountersign" runat="server" />

                </div>
                <br />
                <br />


                <asp:Button CssClass="btn btn-default" ID="btn_paymentItemSetting_add" OnClick="btn_paymentItemSetting_add_Click" runat="server" Text="新增" />



                <%--<button class=" btn btn-default" type="button">
                    新增
                </button>--%>
            </fieldset>
        </div>

        <fieldset>
            <legend>列表</legend>

            搜尋條件<br />


            類型：

            <asp:DropDownList ID="ddl_paymentItemSetting_filter_cashApply"
                DataTextField="Text" DataValueField="Value"
                runat="server">
            </asp:DropDownList>

            關鍵字：
            <asp:TextBox ID="txt_paymentItemSetting_filter_keyword" runat="server" placeholder="項目名稱或備註" ></asp:TextBox>


            <asp:Button ID="btn_search" runat="server" OnClick="btn_search_Click" Text="搜尋" />

            <table class="table table-hover table-condensed table-striped">
                <thead>
                    <tr>
                        <th>
                            <input style="display: none;" type="checkbox" id="chk_paymentItemSetting_list_select_all" />
                        </th>

                        <th>類型
                        </th>

                        <th>品項
                        </th>

                        <th style="width: 300px;">會計科目
                        </th>



                        <th style="width: 90px">稅率
                        </th>

                        <th style="width: 90px">外國人所得稅率
                        </th>

                        <th style="width: 90px">本國人所得稅率
                        </th>

                        <th style="width: 90px">二代健保稅率
                        </th>
                        

                        <th></th>
                    </tr>
                </thead>

                <tbody id="tb_paymentItemSetting_list">

                    <asp:Repeater ID="rpt_paymentItemSetting" runat="server" OnItemDataBound="rpt_paymentItemSetting_ItemDataBound"
                        OnItemCommand="rpt_paymentItemSetting_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:CheckBox Visible="false" ID="chk_paymentItemSetting_list_select" runat="server" />

                                </td>
                                <td>
                                    <asp:HiddenField ID="hid_paymentItemSetting_list_cashApply" Value='<%#Eval("applyTypeKey") %>' runat="server" />
                                    <asp:DropDownList ID="ddl_paymentItemSetting_list_cashApply" DataTextField="Text" DataValueField="Value" CssClass="form-control input-sm" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_paymentItemSetting_list_name" Text='<%#Eval("itemName") %>'
                                        CssClass="form-control input-group-sm input-sm" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:HiddenField ID="hid_paymentItemSetting_list_acocounNo" runat="server" Value='<%#Eval("accountNo") %>' />
                                    <asp:DropDownList ID="ddl_paymentItemSetting_list_accountNo" DataTextField="Text" DataValueField="Value" CssClass="form-control" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <div class="input-group  input-group-sm">
                                        <asp:TextBox ID="txt_paymentItemSetting_list_taxRate"
                                            Text='<%#Eval("taxRate") %>'
                                            CssClass="form-control input-group-sm input-sm" runat="server"></asp:TextBox>
                                        <div class=" input-group-addon">%</div>
                                    </div>
                                </td>
                                <td>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="txt_paymentItemSetting_list_foreignRate"
                                            Text='<%#Eval("foreignIncomeTaxRate") %>'
                                            CssClass="form-control input-group-sm input-sm" runat="server"></asp:TextBox>
                                        <div class=" input-group-addon">%</div>
                                    </div>
                                </td>
                                <td>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="txt_paymentItemSetting_list_localRate"
                                            Text='<%#Eval("localIncomeTaxRate") %>' CssClass="form-control input-group-sm input-sm" runat="server"></asp:TextBox>
                                        <div class=" input-group-addon">%</div>
                                    </div>
                                </td>
                                <td>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="txt_paymentItemSetting_list_secondNHITaxRate"
                                            Text='<%#Eval("secondNHITaxRate") %>'
                                            CssClass="form-control input-group-sm input-sm" runat="server"></asp:TextBox>
                                        <div class=" input-group-addon">%</div>
                                    </div>
                                </td>
                                

                                <td>
                                    <%--<button class="btn btn-default">修改</button>
                        <button class="btn btn-default">刪除</button>--%>

                                    <asp:Button CssClass="btn btn-default btn-sm" ID="btn_paymentItemSetting_list_edit" runat="server" Text="編輯" CommandName="edit" CommandArgument='<%#Eval("id") %>' />
                                    <asp:Button CssClass="btn btn-default btn-sm" ID="btn_paymentItemSetting_list_update" runat="server" Text="直接修改" CommandName="update" CommandArgument='<%#Eval("id") %>' />
                                    <asp:Button CssClass="btn btn-default btn-sm" ID="btn_paymentItemSetting_list_delete" runat="server" Text="刪除" CommandName="delete" CommandArgument='<%#Eval("id") %>' />
                                </td>


                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </tbody>

            </table>
            <uc1:component_pager runat="server" ID="component_pager" />
            


        </fieldset>
        </asp:View>
        <asp:View ID="vw_paymentItemSetting_edit" runat="server">
            <div style="padding:15px;">
                <table class="table_paymentItemSetting_form table table-bordered">
                <tbody>
                    <tr>
                        <td class="td_field">
                            類型
                        </td>
                        <td class="td_value">
                            <asp:DropDownList
                                 DataValueField="Value"                                
                                 DataTextField="Text"
                                
                                 ID="ddl_paymentItemSetting_form_applyTypeKey" runat="server">


                            </asp:DropDownList>
                        </td>

                    </tr>

                    <tr>
                        <td class="td_field">
                            品項
                        </td>
                        <td class="td_value">
                            <asp:TextBox ID="txt_paymentItemSetting_form_itemName" runat="server"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>
                        <td class="td_field">
                            會計科目
                        </td>
                        <td class="td_value">
                            <asp:DropDownList 
                                 DataValueField="Value"
                                 DataTextField="Text"
                                ID="ddl_paymentItemSetting_form_accountNo" runat="server"></asp:DropDownList>
                        </td>
                        

                    </tr>

                    <tr>
                        <td class="td_field">
                            稅率
                        </td>
                        <td class="td_value">
                            <asp:TextBox ID="txt_paymentItemSetting_form_taxRate" runat="server"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>
                        <td class="td_field">
                            外國人所得稅率
                        </td>
                        <td class="td_value">
                            <asp:TextBox ID="txt_paymentItemSetting_form_foreignIncomeTaxRate" runat="server"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>
                        <td class="td_field">
                            本國人所得稅率
                        </td>
                        <td class="td_value">
                            <asp:TextBox ID="txt_paymentItemSetting_form_localIncomeTaxRate" runat="server"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>
                        <td class="td_field">
                            二代健保稅率
                        </td>
                        <td class="td_value">
                            <asp:TextBox ID="txt_paymentItemSetting_form_secondNHITaxRate" runat="server"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>
                        <td class="td_field">
                            備註
                        </td>
                        <td class="td_value">
                            <asp:TextBox TextMode="MultiLine" style="width:400px;height:100px;"
                                ID="txt_paymentItemSetting_form_memo" runat="server"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>
                        <td class="td_field">
                            項目代碼
                        </td>
                        <td class="td_value">
                            <asp:TextBox ID="txt_paymentItemSetting_form_code" runat="server"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>
                        <td class="td_field">
                            此項目要會簽至經費所屬部門
                        </td>
                        <td class="td_value">
                            <asp:CheckBox ID="chk_paymentItemSetting_form_needCountersign" runat="server" />
                        </td>

                    </tr>
                    
                    <tr>
                        <td class="td_field">
                            
                        </td>
                        <td class="td_value">

                            <asp:Button ID="btn_paymentItemSetting_form_confirm" OnClick="btn_paymentItemSetting_form_confirm_Click" runat="server" Text="確定" CssClass="btn btn-default" />
                            <asp:Button ID="btn_paymentItemSetting_form_cancel" OnClick="btn_paymentItemSetting_form_cancel_Click" runat="server" Text="取消" CssClass="btn btn-default" />

                        </td>

                    </tr>

                </tbody>
            </table>
            </div>
            

        </asp:View>
    </asp:MultiView>

    <div style="padding: 15px;">


    </div>


    <asp:Literal ID="lt_paymentItemSetting_msg" runat="server"></asp:Literal>


    <script type="text/javascript">
        function AccountItemSetting() {
            this.init();
        }



        AccountItemSetting.prototype.init = function () {
            var thisClass = this;

            $("#chk_paymentItemSetting_list_select_all").change(function () {
                var checked = $(this).prop('checked');
                $("#tb_paymentItemSetting_list").find('input[type="checkbox"]').prop('checked', checked);
            });

            $('[id$="ddl_paymentItemSetting_accountItem"]').select2();

            $('[id$="btn_paymentItemSetting_add"]').click(function () {
                /* validate */
                var addRet = thisClass.addAccountItemRateSettingValidate();
                if (!addRet.success) {
                    alert(addRet.msg);
                    return false;
                }
            });

            $("#tb_paymentItemSetting_list").on('click', '[name$="btn_paymentItemSetting_list_update"]', function () {
                var listValidateRet = thisClass.updateAccountItemRateSettingValidate($(this));
                if (!listValidateRet.success) {
                    alert(listValidateRet.msg);
                    return false;
                }
            });

            $("#tb_paymentItemSetting_list").on('click', '[name$="btn_paymentItemSetting_list_delete"]', function () {
                if (!confirm("確定要刪除此項目?")) {
                    return false;
                }
            });

            $("#tb_paymentItemSetting_list").find('[name$="ddl_paymentItemSetting_list_accountNo"]').select2();

            $('[id$="btn_paymentItemSetting_form_confirm"]').click(function () {
                //validate 
                var validate_ret = thisClass.updateAccountItemRateSettingValidateEidtMode();

                if (!validate_ret.success) {
                    alert(validate_ret.msg);
                    return false;                     
                }
            });


            

            $('.pagination').on('click', 'a', function () {

                if ($(this).attr("href") == "#") {
                    return false;
                } else {
                    var cashApply = $('[id$="ddl_paymentItemSetting_filter_cashApply"]').val();
                    var keyword = $.trim($('[id$="txt_paymentItemSetting_filter_keyword"]').val());

                    var urlparamArr = [];

                    if (cashApply != "") {
                        urlparamArr.push("cashApply=" + encodeURIComponent(cashApply));
                    }

                    if (keyword != "") {
                        urlparamArr.push("keyword=" + encodeURIComponent(keyword));
                    }

                    if (urlparamArr.length == 0) {
                        return true;
                    } else {
                        location.href = $(this).attr("href") + "&" + urlparamArr.join("&");
                        return false;
                    }
                }
            });



            return;
        }

        AccountItemSetting.prototype.updateAccountItemRateSettingValidateEidtMode = function () {
            var thisClass = this;
            var ret = {
                success: false,
                msg: ""
            };

            var msgArr = [];

            if ($('[id$="ddl_paymentItemSetting_form_applyTypeKey"]').val() == "") {
                msgArr.push("請選擇類型");
            }

            if ($('[id$="txt_paymentItemSetting_form_itemName"]').val() == "") {
                msgArr.push("請輸入品項名稱");
            }

            if ($('[id$="ddl_paymentItemSetting_form_accountNo"]').val() == "") {
                msgArr.push("請選擇會計科目");
            }

            if ($('[id$="txt_paymentItemSetting_form_memo"]').val() == "") {
                msgArr.push("請輸入備註");
            }

            if ($('[id$="txt_paymentItemSetting_form_code"]').val() == "") {
                msgArr.push("請選擇項目代碼");
            }

            var taxRate = $.trim($('[id$="txt_paymentItemSetting_form_taxRate"]').val());            
            var foreignRate = $.trim($('[id$="txt_paymentItemSetting_form_foreignIncomeTaxRate"]').val());
            var localRate = $.trim($('[id$="txt_paymentItemSetting_form_localIncomeTaxRate"]').val());
            var secondNHITaxRate = $.trim($('[id$="txt_paymentItemSetting_form_secondNHITaxRate"]').val());


            if (!thisClass.isNativeNum(taxRate)) {
                msgArr.push("稅率請輸入 0 ~ 100 的數值");
                
            }

            if (!thisClass.isNativeNum(foreignRate)) {
                msgArr.push("外國人所得稅率請輸入 0 ~ 100 的數值");
                
            }

            if (!thisClass.isNativeNum(localRate)) {
                msgArr.push("本國人所得稅率請輸入 0 ~ 100 的數值");
                
            }

            if (!thisClass.isNativeNum(secondNHITaxRate)) {
                msgArr.push("二代健保稅率請輸入 0 ~ 100 的數值");                
            }



            ret.success = msgArr.length == 0;
            ret.msg = msgArr.join("\n");
            return ret;
        }

        AccountItemSetting.prototype.updateAccountItemRateSettingValidate = function ($ele) {
            var thisClass = this,
                ret = {
                    success: false,
                    msg: ""
                };


            ret.success = true;
            return ret;
        };

        AccountItemSetting.prototype.addAccountItemRateSettingValidate = function () {
            var thisClass = this,
                ret = {
                    success: false,
                    msg: ""
                };

            var type = $('[id$="ddl_paymentItemSetting_type"]').val();
            var name = $('[id$="txt_paymentItemSetting_name"]').val();
            var accountItem = $('[id$="ddl_paymentItemSetting_accountItem"]').val();
            var memo = $('[id$="txt_paymentItemSetting_memo"]').val();
            var taxRate = $.trim($('[id$="txt_paymentItemSetting_taxRate"]').val());
            var foreignRate = $.trim($('[id$="txt_paymentItemSetting_foreignRate"]').val());
            var localRate = $.trim($('[id$="txt_paymentItemSetting_localRate"]').val());
            var secondNHITaxRate = $.trim($('[id$="txt_paymentItemSetting_secondNHITaxRate"]').val());

            if (type == "") {
                ret.msg = "請選擇類型";
                return ret;
            }

            if ($.trim(name) == "") {
                ret.msg = "請選擇品項";
                return ret;
            }

            if (accountItem == "") {
                ret.msg = "請選擇會計科目";
                return ret;
            }

            /*
            if ($.trim(memo) == "") {
                ret.msg = "請輸入說明";
                return ret;
            }*/

            if (!thisClass.isNativeNum(taxRate)) {
                ret.msg = "稅率請輸入 0 ~ 100 的數值";
                return ret;
            }

            if (!thisClass.isNativeNum(foreignRate)) {
                ret.msg = "外國人所得稅率請輸入 0 ~ 100 的數值";
                return ret;
            }

            if (!thisClass.isNativeNum(localRate)) {
                ret.msg = "本國人所得稅率請輸入 0 ~ 100 的數值";
                return ret;
            }

            if (!thisClass.isNativeNum(secondNHITaxRate)) {
                ret.msg = "二代健保稅率請輸入 0 ~ 100 的數值";
                return ret;
            }

            ret.success = true;
            return ret;
        }

        AccountItemSetting.prototype.isNativeNum = function (str) {
            if (str == "") {
                return false;
            }
            if (!$.isNumeric(str)) {
                return false;
            }
            if (parseFloat(str) > 100 || parseFloat(str) < 0) {
                return false;
            }
            return true;
        }

        var a = new AccountItemSetting();

    </script>


</asp:Content>
