<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Manage.Master" AutoEventWireup="true" CodeBehind="AdmEmployeeTransportationSetting.aspx.cs" Inherits="WebServiceForEFGP.Manage.AdmEmployeeTransportationSetting" %>

<%@ Register Src="~/Manage/component_pager.ascx" TagPrefix="uc1" TagName="component_pager" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding: 15px;">
        <div style="border: 1px solid #ccc; padding: 15px;">
            <fieldset class="clearfix form-inline">
                <legend>新增私車公用設定</legend>

                <div class="form-group">
                    <label>員工編號：</label>

                    <select style="width: 250px;" id="ddl_admEmployeeTransportationSetting_emp_list" class="form-control">
                    </select>

                    <asp:HiddenField ID="hid_admEmployeeTransportationSetting_emp" runat="server" />
                </div>

                <div class="form-group form-group-sm">
                    <label>交通工具：</label>
                    <asp:DropDownList DataTextField="Text" DataValueField="Value" CssClass="form-control" ID="ddl_admEmployeeTransportationSetting_trans_type" runat="server">
                        <asp:ListItem Text="- 請選擇 -" Value=""></asp:ListItem>
                        <asp:ListItem Text="汽車" Value="car"></asp:ListItem>
                        <asp:ListItem Text="機車" Value="motorcycle"></asp:ListItem>
                    </asp:DropDownList>
                </div>


                <div class="form-group form-group-sm">
                    <label>車牌號碼：</label>                    
                    <asp:TextBox ID="txt_admEmployeeTransportationSetting_transId" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <asp:Button ID="btn_admEmployeeTransportationSetting_add" OnClick="btn_admEmployeeTransportationSetting_add_Click"
                     runat="server" Text="新增" CssClass="btn btn-default btn-sm" />
                
            </fieldset>




        </div>


        <fieldset>
            <legend>列表</legend>

            <h4>搜尋條件：
            </h4>


            <div class="form-group">
                <label>員工：</label>
                <asp:TextBox style="display:inline;width:200px;" CssClass="form-control input-group-sm" ID="txt_admEmployeeTransportationSetting_emp_search_keyword" runat="server"></asp:TextBox>

                <%--<select id="ddl_admEmployeeTransportationSetting_emp_search_list" class="form-control" style="width: 200px; display: inline">
                    
                </select>

                <asp:HiddenField ID="hid_admEmployeeTransportationSetting_emp_search_list" runat="server" />--%>

                <asp:Button ID="btn_admEmployeeTransportationSetting_emp_search" OnClick="btn_admEmployeeTransportationSetting_emp_search_Click" CssClass="btn btn-default btn-sm" data-loading-text="搜尋中" runat="server" Text="搜尋" />

            </div>




            <table class="table table-condensed table-hover table-striped">
                <thead>
                    <tr>
                        <th>員工資料</th>
                        <th>交通方式</th>
                        <th>車牌號碼</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody id="tb_admEmployeeTransportationSetting_list">
                    <asp:Repeater ID="rpt_admEmployeeTransportationSetting_list" runat="server" OnItemCommand="rpt_admEmployeeTransportationSetting_list_ItemCommand" >
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <i class="fa fa-user"></i>                                    
                                    <%#Eval("userName") %>-<%#Eval("EmpNo") %>
                                    <br />
                                    <%#Eval("organizationUnitName") %>                                
                                </td>
                                <td>
                                    <i class="fa <%#Eval("icon") %>"></i>
                                                                         
                                    <asp:Label ID="lb_admEmployeeTransportationSetting_list_trnasType_enum" runat="server" Text='<%#Eval("TransTypeEnum") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="lb_admEmployeeTransportationSetting_list_trnasId" runat="server" Text='<%#Eval("TransId") %>' CssClass="form-control input-sm"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btn_admEmployeeTransportationSetting_list_update" runat="server" Text="修改" 
                                        CssClass="btn btn-default btn-sm" CommandName="update" CommandArgument='<%#Eval("Id") %>' />
                                    <asp:Button ID="btn_admEmployeeTransportationSetting_list_delete" runat="server" Text="刪除" 
                                        CssClass="btn btn-default btn-sm" CommandName="delete" CommandArgument='<%#Eval("Id") %>' />

                                </td>

                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <uc1:component_pager runat="server" ID="component_pager" />
        </fieldset>
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

    <asp:Literal ID="lt_akert_msg" runat="server"></asp:Literal>

    <script type="text/javascript">
        function AdmEmployeeTransportationSetting() {
            this.init();
        }
        AdmEmployeeTransportationSetting.prototype.init = function () {
            var thisClass = this;

            $.ajaxSetup({
                contentType: "application/json",
                dataType: "json"
            });

            


            //bind ui element
            thisClass.ui.template_emp_info = $("#template_emp_info");

            //element binding
            $("#ddl_admEmployeeTransportationSetting_emp_list").select2(thisClass.seletctObj());

            /*
            $("#ddl_admEmployeeTransportationSetting_emp_search_list").select2(thisClass.seletctObj());
            */

            /*
            if ($('[id$="hid_admEmployeeTransportationSetting_emp_search_list"]').val() != "") {
                $('#ddl_admEmployeeTransportationSetting_emp_search_list').val($('[id$="hid_admEmployeeTransportationSetting_emp_search_list"]').val());
                $('#ddl_admEmployeeTransportationSetting_emp_search_list').trigger('change.select2');
            }
            */

            $('[id$="btn_admEmployeeTransportationSetting_add"]').click(function () {
                
                var empNo = $("#ddl_admEmployeeTransportationSetting_emp_list").val();

                if (!empNo  || empNo=="") {
                    alert("請選擇員工")
                    return false;
                }

                if ($('[id$="ddl_admEmployeeTransportationSetting_trans_type"]').val() == "") {
                    alert("請選擇交通工具");
                    return false;
                }

                //set value to hidden field                
                $('[id$="hid_admEmployeeTransportationSetting_emp"]').val(empNo);

            });

            
            $('[id$="btn_admEmployeeTransportationSetting_emp_search"]').click(function () {
                //$('[id$="hid_admEmployeeTransportationSetting_emp_search_list"]').val($('#ddl_admEmployeeTransportationSetting_emp_search_list').val());                

                $(this).button('loading')
            });
            



            $('#tb_admEmployeeTransportationSetting_list').on('click', '[name$="btn_admEmployeeTransportationSetting_list_update"]', function () {
                //validate               

            });

            $('#tb_admEmployeeTransportationSetting_list').on('click', '[name$="btn_admEmployeeTransportationSetting_list_delete"]', function () {

                if (!confirm('確定要刪除此資料?')) {
                    return false;
                }

            });


        }

        AdmEmployeeTransportationSetting.prototype.args = {
            pageSize: 30

        };

        AdmEmployeeTransportationSetting.prototype.ui = {
            template_emp_info: null
        };

        AdmEmployeeTransportationSetting.prototype.seletctObj = function () {
            var thisClass = this;

            return {
                language: "zh-TW",
                placeholder: '請輸入員工編號',
                allowClear: true,
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
                templateResult: function (repo) {
                    return thisClass.formatRepo(repo);
                },   // omitted for brevity, see the source of this page
                templateSelection: thisClass.formatRepoSelection // omitted for brevity, see the source of this page
            };
        }
      
        AdmEmployeeTransportationSetting.prototype.formatRepo = function (repo) {
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

        AdmEmployeeTransportationSetting.prototype.formatRepoSelection = function (repo) {
            

             if (repo.name) {
                 return (repo.name + (repo.id ? "(" + repo.id + ")" : ""));
            } else {
                 return repo.text + (repo.id ? "(" + repo.id + ")" : "");
            }
        }

        var admEmp = new AdmEmployeeTransportationSetting();



    </script>
</asp:Content>
