<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Manage.Master" AutoEventWireup="true" CodeBehind="FormNotifySetting.aspx.cs" Inherits="WebServiceForEFGP.Manage.FormNotifySetting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="border: 1px solid #BBBBBB; padding: 15px; margin-top: 10px;">
        表單名稱：
        <asp:DropDownList ID="ddl_forms" runat="server" DataTextField="Text" DataValueField="Value" OnSelectedIndexChanged="ddl_forms_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Text="- 請選擇 -" Value=""></asp:ListItem>
        </asp:DropDownList>

        欄位名稱：
        <asp:DropDownList ID="ddl_fields" runat="server" AutoPostBack="true" DataTextField="Text" DataValueField="Value" OnSelectedIndexChanged="ddl_fields_SelectedIndexChanged" >
            <asp:ListItem Text="- 請選擇 -" Value=""></asp:ListItem>
        </asp:DropDownList>        
        <asp:HiddenField ID="hid_current_select_emp" runat="server" />

        <asp:HiddenField ID="hid_current_select_emp_id_arr" runat="server" />

        <asp:HiddenField ID="hid_select_field_type" runat="server" />
    </div>

     <div style="margin:20px 0;">
          <h4>
                <i class="fa fa-list"></i>&nbsp;
                通知人員姓名：
            </h4>

            <div id="block_select_container" runat="server" visible="false"> 
                <h5>新增通知員工：
                </h5>                 
                <select id="ddl_emp_list"  multiple="multiple" style="width:75%;">
                    

                </select>

                <asp:Button ID="btn_save" runat="server"  CssClass="btn btn-default" Text="儲存" OnClick="btn_save_Click" />
                



            </div>
<div class="hide">

        <div id="template_emp_info">
            <div style="width:50px;display:inline-block;vertical-align:top;">
                <i class="fa fa-user fa-2x"></i>
            </div>            
            <div style="display:inline-block;width:200px;vertical-align:top;font-size:11pt;">
                <strong>(<span data-field="id"></span>)
                -
                <span data-field="name"></span>

                    
                    <span style="font-size:0">
                        分機:
                        <span data-field="extNo"></span>
                    </span>
                </strong>
                <br />

                <span data-field="department" style="font-size: 10pt; color: #808080;"></span>

                <span data-field="departmentId" style="font-size: 10pt; color: #808080;">
                </span>
                                    
                                
            </div>                                    
        </div>
    </div>

    </div>
    <asp:Literal ID="lt_msg" runat="server"></asp:Literal>

    <script type="text/javascript">
        (function () {
            function formNotifySettingClass() {
                this.init();
            }

            formNotifySettingClass.prototype.args = {
                pageSize: 30,
                currentEmpList: [],
                currentEmpIdArr: []
            };

            formNotifySettingClass.prototype.init = function () {
                var thisClass = this;

                thisClass.bind_ui();
                thisClass.element_bind_ui();

                return;
            };

            formNotifySettingClass.prototype.ui = {
                ddl_emp_list: null,
                template_emp_info: null,
                hid_current_select_emp: null,
                btn_save: null,
                hid_current_select_emp_id_arr:null
            };

            formNotifySettingClass.prototype.bind_ui = function () {
                var thisClass = this;
                thisClass.ui.ddl_emp_list = $('#ddl_emp_list');
                thisClass.ui.template_emp_info = $("#template_emp_info");
                thisClass.ui.hid_current_select_emp = $('[id$="hid_current_select_emp"]');
                thisClass.ui.btn_save = $('[id$="btn_save"]');
                thisClass.ui.hid_current_select_emp_id_arr = $('[id$="hid_current_select_emp_id_arr"]');
                
                return;
            }

            formNotifySettingClass.prototype.element_bind_ui = function () {
                var thisClass = this;

                $.ajaxSetup({
                    contentType: "application/json",
                    dataType: "json"
                });

                if ($.trim(thisClass.ui.hid_current_select_emp.val()) != "") {
                    thisClass.args.currentEmpList = JSON.parse(thisClass.ui.hid_current_select_emp.val());

                    //init select2

                    for (var i = 0  ; i < thisClass.args.currentEmpList.length  ; i++) {
                        var e = thisClass.args.currentEmpList[i];

                        thisClass.ui.ddl_emp_list.append($("<option/>", {
                            value: e.id,
                            text: e.name,
                            selected: true
                        }));
                    }
                    thisClass.args.currentEmpIdArr = thisClass.ui.hid_current_select_emp_id_arr.val().split(";");
                }

                thisClass.ui.btn_save.click(function () {

                    if (!confirm("確定要儲存此結果?")) {
                        return false;
                    }                    

                    var newEmpArr = [];
                    var currentSelectIDArr = thisClass.ui.ddl_emp_list.val();

                    if (!$.isArray(currentSelectIDArr)) {
                        currentSelectIDArr = [currentSelectIDArr];
                    }

                    ////把空白濾掉
                    for (var i = 0 ; i < currentSelectIDArr.length ; i++) {
                        if ($.trim(currentSelectIDArr[i]) != "") {
                            newEmpArr.push(currentSelectIDArr[i]);
                        }
                    }
                    thisClass.ui.hid_current_select_emp_id_arr.val(newEmpArr.join(";"));

                    console.log(thisClass.ui.hid_current_select_emp_id_arr.val());                    

                });

                thisClass.ui.ddl_emp_list.select2({
                    language: "zh-TW",
                    ajax: {
                        url: "/Employee/getEmployeeInfoListResult",
                        method: "POST",
                        contentType: "application/json",
                        dataType: 'json',
                        delay: 200,
                        data: function (params) {
                            return JSON.stringify({
                                orderField: "",
                                keyword: params.term, // search term
                                pageIndex: params.page || 1,
                                pageSize: thisClass.args.pageSize,
                                desc: false
                            });
                        },
                        processResults: function (data, params) {
                            // parse the results into the format expected by Select2
                            // since we are using custom formatting functions we do not need to
                            // alter the remote JSON data, except to indicate that infinite
                            // scrolling can be used
                            params.page = params.page || 1;
                            console.log(data);

                            return {
                                results: data.list,
                                pagination: {
                                    more: (params.page * thisClass.args.pageSize) < data.count
                                }
                            };
                        },
                        cache: true
                    },
                    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                    minimumInputLength: 2,
                    templateResult: function(repo){
                        return thisClass.formatRepo(repo);
                    },   // omitted for brevity, see the source of this page
                    templateSelection: thisClass.formatRepoSelection // omitted for brevity, see the source of this page
                });


                return;
            }

            

            formNotifySettingClass.prototype.formatRepo = function (repo) {
                var thisClass = this;

                
                if (repo.loading) { return repo.text; }                
                var $tmp = thisClass.ui.template_emp_info.clone();
                $tmp.attr("id", "emp_" + repo.id);
                $tmp.find('[data-field]').each(function () {
                    $(this).text(repo[$(this).attr("data-field")]);
                });

                var ret = $tmp;

                return ret;
            };

            formNotifySettingClass.prototype.formatRepoSelection = function (repo) {
                if (repo.name) {
                    return (repo.name + "(" + repo.id + ")")
                } else {

                    return repo.text + "(" + repo.id + ")";
                }                
            }
            


            var page = new formNotifySettingClass();

        })($);
    </script>

    
</asp:Content>
