<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Manage.Master" AutoEventWireup="true" CodeBehind="FormOptionSetting.aspx.cs" Inherits="WebServiceForEFGP.Manage.FormOptionSetting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div style="border:1px solid #BBBBBB; padding:15px;margin-top:10px;">

        表單名稱：
        <asp:DropDownList ID="ddl_forms" runat="server" DataTextField="Text" DataValueField="Value" OnSelectedIndexChanged="ddl_forms_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Text="- 請選擇 -" Value=""></asp:ListItem>
        </asp:DropDownList>

        欄位名稱：
        <asp:DropDownList ID="ddl_fields" runat="server" AutoPostBack="true" DataTextField="Text" DataValueField="Value" OnSelectedIndexChanged="ddl_fields_SelectedIndexChanged" >
            <asp:ListItem Text="- 請選擇 -" Value=""></asp:ListItem>
        </asp:DropDownList>        

        
        <asp:HiddenField ID="hid_select_field_type" runat="server" />

    </div>

    <%--result--%>
    <div>


        <div style="margin: 20px 0;">
            <h4>
                <i class="fa fa-bar-chart"></i>&nbsp;欄位屬性
            </h4>
            <ul>
                <li>單選/多選：  <asp:Label ID="lb_field_type" runat="server" Text=""></asp:Label> </li>                
            </ul>
        </div>
        
        

        <div style="margin:20px 0;">

            <h4>
                <i class="fa fa-list"></i>&nbsp;
                欄位選項列表
            </h4>

            <div>
                <h5>新增選項：
                </h5>                

                <label>
                    選項名稱：
                    <asp:TextBox ID="txt_add_text" runat="server"></asp:TextBox>
                </label>               
                

                <label>
                    值：
                    <asp:TextBox ID="txt_add_value" runat="server"></asp:TextBox>
                </label>
                
                
                <label>
                順序：
                    <asp:TextBox ID="txt_add_sort" TextMode="Number" style="width:50px;" runat="server"></asp:TextBox>
                    </label>                
                    
                       <asp:CheckBox ID="chk_add_needKeyIn" runat="server" Text="顯示輸入欄位" />
                

                <asp:Button ID="btn_add_option" runat="server" OnCommand="btn_add_option_Command" Text="新增" />               
                <asp:Button ID="btn_delete_select" runat="server" OnCommand="btn_delete_select_Command" Text="刪除選取項目" />


            </div>

            <hr />

            <table class=" table table-condensed table-hover table-striped">
            <thead>
                <tr>
                    <th>
                        <input type="checkbox" id="chk_select_all" />
                    </th>
                    <th>序號</th>
                    <th>名稱</th>
                    <th>值</th>                   

                    <th>輸入欄</th>
                    <th>順序</th>

                    <th>建立時間</th>
                    <th>
                    </th>
                </tr>
            </thead>
            <tbody id="tb_formOption_list">
                <asp:Repeater ID="rpt_formOption_list" runat="server" OnItemCommand="rpt_formOption_list_ItemCommand"  OnItemDataBound="rpt_formOption_list_ItemDataBound" >
                    <ItemTemplate>

                        <tr>
                            <td>
                                <asp:HiddenField ID="hid_list_id" Value='<%#Eval("id") %>' runat="server" />

                                <asp:CheckBox ID="chb_list_select" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lb_list_index" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <%#Eval("text") %>
                            </td>
                            <td>
                                <%#Eval("value") %>
                            </td>

                            <td>

                                <asp:HiddenField ID="hid_list_needKeyIn" Value='<%#Eval("needKeyIn") %>' runat="server" />

                                <asp:Label ID="lb_list_needKeyIn" runat="server" Text=''></asp:Label>
                            </td>

                            <td>
                                <asp:TextBox ID="txt_list_sort" Text='<%#Eval("sort") %>' Style="width: 50px" TextMode="Number" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <%#  DataBinder.Eval(Container.DataItem, "dateCreated", "{0:yyyy/M/d hh:mm:ss}") %>
                            </td>
                            <td>
                                <asp:Button ID="btn_list_update" runat="server" Text="更新" CommandName="update" CommandArgument='<%#Eval("id") %>' />
                                <asp:Button ID="btn_list_delete" runat="server" Text="刪除" CommandName="delete"  CommandArgument='<%#Eval("id") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        </div>
        
    </div>


    <script>

        (function ($) {

            function formOptionSettingClass() {
                this.init();

            }


            formOptionSettingClass.prototype.init = function () {
                var thisClass = this;

                thisClass.bind_ui();

                thisClass.element_event_bind();
            }

            formOptionSettingClass.prototype.ui = {
                tb_formOption_list: null,
                chk_select_all: null,
                btn_add_option: null,
                btn_delete_select: null,
                txt_add_text: null,
                txt_add_value: null,
                txt_add_sort: null,
                chk_add_needKeyIn: null,
                ddl_forms: null,
                ddl_fields:null

            }

            formOptionSettingClass.prototype.bind_ui = function () {
                var thisClass = this;

                thisClass.ui.ddl_forms = $('[id$="ddl_forms"]');
                thisClass.ui.ddl_fields = $('[id$="ddl_fields"]');
                thisClass.ui.tb_formOption_list = $("#tb_formOption_list");
                thisClass.ui.chk_select_all = $("#chk_select_all");
                thisClass.ui.btn_add_option = $('[id$="btn_add_option"]');
                thisClass.ui.btn_delete_select = $('[id$="btn_delete_select"]');

                thisClass.ui.txt_add_text = $('[id$="txt_add_text"]');
                thisClass.ui.txt_add_value = $('[id$="txt_add_value"]');
                thisClass.ui.txt_add_sort = $('[id$="txt_add_sort"]');
            }
            
            formOptionSettingClass.prototype.element_event_bind = function () {
                var thisClass = this;               

                
                thisClass.ui.tb_formOption_list.on('click', '[name$="btn_list_update"]', function () {
                    var sort = thisClass.ui.tb_formOption_list.find('input[name$="txt_list_sort"]').val();

                    if (sort == "" || !$.isNumeric(sort)) {
                        toastr.error('順序請輸入數字');
                        return false;
                    }

                });

                thisClass.ui.tb_formOption_list.on('click', '[name$="btn_list_delete"]', function () {
                    if (!confirm("確定要刪除此項目?")) {
                        return false;
                    };
                    
                });

                thisClass.ui.btn_add_option.click(function () {
                    var validate_ret = thisClass.add_option_validate();

                    if (!validate_ret.success) {
                        toastr.error(validate_ret.message);
                        return false;
                    }

                });

                thisClass.ui.btn_delete_select.click(function () {
                    //validate 
                    var checkedCount = thisClass.ui.tb_formOption_list.find('input[type="checkbox"]:checked').length;
                    if (checkedCount == 0) {
                        toastr.error('請選擇刪除項目')
                        return false;
                    }

                    if (!confirm('確定要刪除選取項目?')) {

                        return false;
                    }

                });

                thisClass.ui.chk_select_all.change(function () {
                    var checked = $(this).prop("checked");
                    thisClass.ui.tb_formOption_list.find('input[type="checkbox"]').prop('checked', checked);
                });


                return;
            };

            formOptionSettingClass.prototype.add_option_validate = function () {
                var thisClass = this;
                var ret = {
                    success: false,
                    message: ""
                };


                if ($.trim(thisClass.ui.ddl_forms.val()) == '') {
                    ret.message = "請選擇表單種類";
                    return ret;
                }

                if ($.trim(thisClass.ui.ddl_fields.val()) == "") {
                    ret.message = "請選擇欄位種類";
                    return ret;
                }


                if ($.trim(thisClass.ui.txt_add_sort.val()) == "" || !$.isNumeric(thisClass.ui.txt_add_sort.val())) {
                    ret.message = "輸入順序請輸入數字";
                    return ret;
                }

                if ($.trim(thisClass.ui.txt_add_text.val()) == "") {
                    ret.message = "選項文字為必填選項";
                    return ret;
                }


                if ($.trim(thisClass.ui.txt_add_value.val()) == "") {
                    ret.message = "選項值為必填選項";
                    return ret;
                }

                ret.success = true;                
                return ret;

            };

            var page = new formOptionSettingClass();

        })($);


    </script>

</asp:Content>
