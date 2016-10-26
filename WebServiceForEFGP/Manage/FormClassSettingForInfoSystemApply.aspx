<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Manage.Master" AutoEventWireup="true" CodeBehind="FormClassSettingForInfoSystemApply.aspx.cs" Inherits="WebServiceForEFGP.Manage.FormClassSettingForInfoSystemApply" %>
<%@ Register Src="~/Manage/component_pager.ascx" TagPrefix="uc1" TagName="component_pager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
    .checkboxlist label
    {
        margin-right:10px;
    }
    </style>
    <asp:MultiView ID="MultiView" runat="server" ActiveViewIndex="0">
        <asp:View ID="View_list" runat="server">
            <div style="padding: 15px;">
                <div style="border: 1px solid #ccc; padding: 15px;">
                    <fieldset class="clearfix form-inline">
                        <legend>新增資訊需求類別設定</legend>
                        
                        <div class="form-group">
                            <label>處理人員：</label>
                            <select style="width: 250px;" id="ddl_formClassSettingForInfoSystemApply_personnelID" class="form-control"></select>
                            <asp:HiddenField ID="hid_formClassSettingForInfoSystemApply_personnelID" runat="server" />
                        </div>
                        
                        <div class="form-group form-group-sm">
                            <label>類別名稱：</label>
                            <asp:TextBox style="width: 350px;" ID="txt_formClassSettingForInfoSystemApply_className" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        
                        <div class="form-group form-group-sm">
                            <label>主類別：</label>
                            <asp:DropDownList DataTextField="Text" DataValueField="Value" CssClass="form-control" ID="ddl_formClassSettingForInfoSystemApply_parentID" AutoPostBack="true" OnSelectedIndexChanged="ddl_formClassSettingForInfoSystemApply_Change" runat="server"></asp:DropDownList>
                        </div>
                        
                        <div class="form-group form-group-sm">
                            <label>部門名稱：</label>
                            <asp:DropDownList DataTextField="Text" DataValueField="Value" style="width: 250px;" ID="ddl_departmentID" CssClass="form-control" runat="server"></asp:DropDownList>
                            <asp:HiddenField ID="hid_departmentID" runat="server" />
                        </div>
                        <div class="form-group form-group-sm" style="display:none;">
                            <asp:DropDownList DataTextField="Text" DataValueField="Value" style="width: 250px; display:none;" ID="hid_ddl_departmentID" CssClass="form-control" runat="server"></asp:DropDownList>
                        </div>
                        <br />
                        <div class="form-group form-group-sm">
                            <label>核決層級：</label>
                            <asp:DropDownList DataTextField="Text" DataValueField="Value" CssClass="form-control" ID="ddl_formClassSettingForInfoSystemApply_dicisionProcessLevel" runat="server">
                                <asp:ListItem Text="- 請選擇 -" Value=""></asp:ListItem>
                                <asp:ListItem Text="事業部" Value="3"></asp:ListItem>
                                <asp:ListItem Text="處級" Value="2"></asp:ListItem>
                                <asp:ListItem Text="部門" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        
                        <div class="form-group form-group-sm">
                            <label>適用全部門：</label>
                            <asp:DropDownList DataTextField="Text" DataValueField="Value" CssClass="form-control" ID="ddl_formClassSettingForInfoSystemApply_isforAll" runat="server">
                                <asp:ListItem Text="- 請選擇 -" Value=""></asp:ListItem>
                                <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                <asp:ListItem Text="否" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        
                        <div class="form-group form-group-sm">
                            <label>權限單位：</label>
                            <asp:DropDownList DataTextField="Text" DataValueField="Value" style="width: 250px;" CssClass="form-control" ID="ddl_PermissionDepartmentID" runat="server"></asp:DropDownList>
                        </div>
                        
                        <div class="form-group">
                            <label>加簽人員：</label>
                            <select style="width: 250px;" id="ddl_formClassSettingForInfoSystemApply_ActivityPersonnelID" class="form-control"></select>
                            <asp:HiddenField ID="hid_formClassSettingForInfoSystemApply_ActivityPersonnelID" runat="server" />
                        </div>
                        
                        <asp:Button ID="btn_formClassSettingForInfoSystemApply_add" OnClick="btn_formClassSettingForInfoSystemApply_add_Click" runat="server" Text="新增" CssClass="btn btn-default btn-sm" />
                    </fieldset>
                </div>

                <fieldset>
                    <legend>列表</legend>
                    <h4>搜尋條件：</h4>
                    
                    <div class="form-group form-group-sm">
                        <label>類別名稱：</label>
                        <asp:TextBox style="width: 250px;" CssClass="form-control" ID="txt_formClassSettingForInfoSystemApply_className_search_keyword" runat="server"></asp:TextBox>
                        <label>主類別：</label>
                        <asp:DropDownList style="width: 250px;" ID="ddl_formClassSettingForInfoSystemApply_mainClass" runat="server" CssClass="form-control" ></asp:DropDownList>
                        <asp:Button ID="btn_formClassSettingForInfoSystemApply_className_search" OnClick="btn_formClassSettingForInfoSystemApply_className_search_Click" CssClass="btn btn-default btn-sm" data-loading-text="搜尋中" runat="server" Text="搜尋" />
                    </div>
                    
                    <table class="table table-condensed table-hover table-striped">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th style="display:none">處理人員</th>
                                <th>處理人員</th>
                                <th>類別名稱</th>
                                <th>主類別</th>
                                <th style="display:none;">部門名稱</th>
                                <th>部門名稱</th>
                                <th>核決層級</th>
                                <th>權限單位</th>
                                <th style="display:none">加簽人員</th>
                                <th>加簽人員</th>
                                <th></th><%--修改,刪除,進階--%>
                            </tr>
                        </thead>
                        <tbody id="tb_formClassSettingForInfoSystemApply_list">
                            <asp:Repeater ID="rpt_formClassSettingForInfoSystemApply_list" runat="server" OnItemDataBound="rpt_formClassSettingForInfoSystemApply_list_ItemDataBound" OnItemCommand="rpt_formClassSettingForInfoSystemApply_list_ItemCommand" >
                                <ItemTemplate>
                                    <tr>
                                        <td class="td_dataID"><%#Eval("Id") %></td>
                                        <td class="td_dataPersonnelID" style="display:none;"><%#Eval("personnel") %></td>
                                        <td>
                                            <select style="width: 200px;" data-personnel-id='<%#Eval("Id")%>' id="ddl_formClassSettingForInfoSystemApply_personnelID_<%#Eval("Id") %>" class="form-control"></select>
                                            <asp:HiddenField ID="hidd_rptPersonnelID" runat="server" Value='<%#Eval("personnelID") %>'/>
                                        </td>
                                        <td><asp:TextBox ID="txt_formClassSettingForInfoSystemApply_list_name" runat="server" Text='<%#Eval("name") %>' CssClass="form-control input-sm"></asp:TextBox></td>
                                        <td><%#Eval("partent_name") %></td>
                                        <td class="td_departmentID" style="display:none;"><%#Eval("department") %></td>
                                        <td>
                                            <asp:DropDownList ID="ddl_formClassSettingForInfoSystemApply_list_departmentID" DataTextField="Text" DataValueField="Value" CssClass="form-control" runat="server" Width="200px"></asp:DropDownList>
                                            <asp:HiddenField ID="hid_formClassSettingForInfoSystemApply_list_departmentID" runat="server" Value='<%#Eval("departmentID") %>'/>
                                        </td>
                                        <td><%#Eval("dicisionProcessLevel") %></td>
                                        <td style="width: 150px;"><%#Eval("Permission_department") %></td>
                                        <td class="td_ActivityPersonnelID" style="display:none;"><%#Eval("addActivityPersonnel") %></td><%--ActivityPersonnelID--%>
                                        <td>
                                            <select style="width: 200px;" data-signature-id='<%#Eval("Id")%>' id="ddl_formClassSettingForInfoSystemApply_ActivityPersonnelID_<%#Eval("Id") %>" class="form-control"></select>
                                            <asp:HiddenField ID="hidd_rptActivityPersonnelID" runat="server" Value='<%#Eval("addActivityPersonnelID") %>'/>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_formClassSettingForInfoSystemApply_list_update" runat="server" Text="修改" 
                                                CssClass="btn btn-default btn-sm" CommandName="update" CommandArgument='<%#Eval("Id") %>' />
                                            <asp:Button ID="btn_formClassSettingForInfoSystemApply_list_delete" runat="server" Text="刪除" 
                                                CssClass="btn btn-default btn-sm" CommandName="delete" CommandArgument='<%#Eval("Id") %>' />
                                            <asp:Button ID="btn_formClassSettingForInfoSystemApply_list_advanced" runat="server" Text="進階"
                                                 CssClass="btn btn-default btn-sm" CommandName="advanced" CommandArgument='<%#Eval("Id") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>

                    <uc1:component_pager runat="server" ID="component_pager" />

                </fieldset>
            </div>
        </asp:View>
        <asp:View ID="View_advanced" runat="server">

            <div style="padding: 15px;">
                <div style="border: 1px solid #ccc; padding: 15px;">
                    <fieldset class="clearfix form-inline">
                        <legend>進階資訊需求類別設定</legend>
                        
                        <table id="tb_advanced_list" class="table table-bordered">
                            <tr>
                                <td class="text-right">
                                    <label>ID：</label>
                                </td>
                                <td>
                                    <div class="form-group"><asp:Label ID="lblAdvancedID" runat="server" Text=""></asp:Label></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right"><label>處理人員：</label></td>
                                <td class="td_advancedPersonnelID">
                                    <select style="width: 250px;" id="ddl_ViewAdvanced_PersonnelID" class="form-control"></select>
                                    <asp:HiddenField ID="hid_ViewAdvanced_PersonnelID" runat="server" Value='<%#Eval("personnelID") %>'/>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right"><label>類別名稱：</label></td>
                                <td>
                                    <asp:TextBox style="width: 250px;" ID="txt_ViewAdvanced_ClassName" class="form-control" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right"><label>主類別：</label></td>
                                <td>
                                    <asp:DropDownList DataTextField="Text" DataValueField="Value" CssClass="form-control" ID="ddl_ViewAdvanced_PartentID" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right"><label>部門名稱：</label></td>
                                <td>
                                    <asp:DropDownList DataTextField="Text" DataValueField="Value" style="width: 250px;" CssClass="form-control" ID="ddl_ViewAdvanced_DepartmentID" runat="server"></asp:DropDownList>
                                    <asp:HiddenField ID="hid_ViewAdvanced_departmentID" runat="server" Value='<%#Eval("departmentID") %>'/>
                                </td>
                                <td style="display:none;">
                                    <asp:DropDownList DataTextField="Text" DataValueField="Value" style="width: 250px; display:none;" ID="hid_ddl_ViewAdvanced_DepartmentID" CssClass="form-control" runat="server" ></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right"><label>核決層級：</label></td>
                                <td>
                                    <asp:DropDownList DataTextField="Text" DataValueField="Value" CssClass="form-control" ID="ddl_ViewAdvanced_DicisionProcessLevel" runat="server">
                                        <asp:ListItem Text="- 請選擇 -" Value=""></asp:ListItem>
                                        <asp:ListItem Text="事業部" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="處級" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="部門" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right"><label>適用全部門：</label></td>
                                <td>
                                    <asp:DropDownList DataTextField="Text" DataValueField="Value" CssClass="form-control" ID="ddl_ViewAdvanced_IsforAll" runat="server">
                                        <asp:ListItem Text="- 請選擇 -" Value=""></asp:ListItem>
                                        <asp:ListItem Text="是" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="否" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right"><label>權限單位：</label></td>
                                <td>
                                    <asp:DropDownList DataTextField="Text" DataValueField="Value" style="width: 250px;" CssClass="form-control" ID="ddl_ViewAdvanced_PermissionDepartmentID" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right"><label>加簽人員：</label></td>
                                <td class="td_advancedActivityPersonnelID">
                                    <select style="width: 250px;" id="ddl_ViewAdvanced_ActivityPersonnelID" class="form-control"></select>
                                    <asp:HiddenField ID="hid_ViewAdvanced_ActivityPersonnelID" runat="server" Value='<%#Eval("addActivityPersonnelID") %>'/>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right"><label>顯示額外欄位：</label></td>
                                <td>
                                    <asp:CheckBoxList ID="ckl_DisplayOtherField" runat="server" RepeatDirection="Horizontal" CssClass="checkboxlist"></asp:CheckBoxList>
                                    <asp:Label ID="lbl_cklValues" runat="server" Text="" style="display:none;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="text-right"><label>範本下載：</label></td>
                                <td>
                                    <asp:CheckBoxList ID="chk_TemplateDownload" runat="server" RepeatDirection="Horizontal" CssClass="checkboxlist"></asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td class="text-left">
                                    <div class="form-group form-group-sm">
                                        <asp:Button ID="btn_View_advancedBack" runat="server" CssClass="btn btn-default" Text="返回" OnClick="btn_View_advancedBack_Click"/>
                                        <asp:Button ID="btn_View_advancedSave" runat="server" CssClass="btn btn-default" Text="儲存" OnClick="btn_View_advancedSave_Click"/>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </div>

            <script type="text/javascript">
                //$('[id$="btn_View_advancedSave"]').click(function () {

                //});
            </script>
        </asp:View>
    </asp:MultiView>

    <%--此段與人員查詢有關係(顯示ID,Name,分機,部門ID,部門名稱)--%>
    <div class="hide">
        <div id="template_personnel_info">
            <div style="width:50px;display:inline-block;vertical-align:top;">
                <i class="fa fa-user fa-2x"></i>
            </div>

            <div style="display:inline-block;width:200px;vertical-align:top;font-size:11pt;">
                <strong>
                    (<span data-field="id"></span>)-
                    <span data-field="name"></span>
                    <span style="font-size:0">分機:
                        <span data-field="extNo"></span>
                    </span>
                </strong>
                <br />
                <span data-field="department" style="font-size: 10pt; color: #808080;"></span>
                <span data-field="departmentId" style="font-size: 10pt; color: #808080;"></span>
            </div>                                    
        </div>
    </div>

    <asp:Literal ID="lt_akert_msg" runat="server"></asp:Literal>

    <script type="text/javascript">

        function FormClassSettingForInfoSystemApply() {
            this.init();
        }

        FormClassSettingForInfoSystemApply.prototype.init = function () {
            var thisClass = this;
            var mesg = ["處理人員", "加簽人員"];

            $.ajaxSetup({
                contentType: "application/json",
                dataType: "json"
            });

            //bind ui element
            thisClass.ui.template_personnel_info = $("#template_personnel_info");

            //element binding
            $("#ddl_formClassSettingForInfoSystemApply_personnelID").select2(thisClass.seletctObj(mesg[0]));//處理人員
            //element binding
            $("#ddl_formClassSettingForInfoSystemApply_ActivityPersonnelID").select2(thisClass.seletctObj(mesg[1]));//加簽人員
            //element binding
            $('[id$="ddl_departmentID"]').select2();//所屬部門
            //element binding
            $("#tb_formClassSettingForInfoSystemApply_list").find('[name$="ddl_formClassSettingForInfoSystemApply_list_departmentID"]').select2();//所屬部門(tb)

            //動態產生處理人員
            var rpCount = $('#tb_formClassSettingForInfoSystemApply_list >tr').length;

            if (rpCount > 0) {
                for (i = 0; i < rpCount; i++) {
                    var ID = $('#tb_formClassSettingForInfoSystemApply_list >tr').find('.td_dataID')[i].textContent;//每列TD欄位(td_dataID),ID
                    var Personnel = $('#tb_formClassSettingForInfoSystemApply_list >tr').find('.td_dataPersonnelID')[i].textContent.split("(");//每列TD欄位(td_dataPersonnelID),處理人員
                    var ActivityPersonnel = $('#tb_formClassSettingForInfoSystemApply_list >tr').find('.td_ActivityPersonnelID')[i].textContent.split("(");//每列TD欄位(td_ActivityPersonnelID),加簽人員

                    if (Personnel != "") {
                        var PersonnelID = Personnel[1].substring(Personnel[1].length - 1, 0);
                        var Personnelname = Personnel[0];

                        $("#ddl_formClassSettingForInfoSystemApply_personnelID_" + ID).append('<option value="' + PersonnelID + '">' + Personnelname + '</option>');//預帶處理人員
                    }
                    
                    $("#ddl_formClassSettingForInfoSystemApply_personnelID_" + ID).select2(thisClass.seletctObj(mesg[0]));//處理人員控制項

                    if (ActivityPersonnel !="") {
                        var ActivityPersonnelID = ActivityPersonnel[1].substring(ActivityPersonnel[1].length - 1, 0);
                        var ActivityPersonnelname = ActivityPersonnel[0];

                        $("#ddl_formClassSettingForInfoSystemApply_ActivityPersonnelID_" + ID).append('<option value="' + ActivityPersonnelID + '">' + ActivityPersonnelname + '</option>');//預帶加簽人員    
                    }

                    $("#ddl_formClassSettingForInfoSystemApply_ActivityPersonnelID_" + ID).select2(thisClass.seletctObj(mesg[1]));//加簽人員控制項

                }
            }

            //將rpt動態處理人員點選的值,放到隱藏欄位
            $("select[data-personnel-id]").change(function () {
                if ($(this).val() == null) {
                    $(this).closest('tr').find('[name$="hidd_rptPersonnelID"]').val("");
                    $(this).closest('tr').find('[name$="hid_formClassSettingForInfoSystemApply_list_departmentID"]').val("");
                    $(this).closest('tr').find('[name$="ddl_formClassSettingForInfoSystemApply_list_departmentID"]').select2('val', "");
                    $(this).closest('tr').find('[name$="ddl_formClassSettingForInfoSystemApply_list_departmentID"]').prop("disabled", false);
                } else {
                    $(this).closest('tr').find('[name$="hidd_rptPersonnelID"]').val($(this).val());

                    $('[id$="hid_ddl_departmentID"]').val($(this).val());
                    var departmentName = $('[id$="hid_ddl_departmentID"] option:selected').text();
                    
                    $(this).closest('tr').find('[name$="ddl_formClassSettingForInfoSystemApply_list_departmentID"] option:contains(' + departmentName + ')').attr('selected', true);
                    $(this).closest('tr').find('[name$="ddl_formClassSettingForInfoSystemApply_list_departmentID"]').select2('val', $(this).closest('tr').find('[name$="ddl_formClassSettingForInfoSystemApply_list_departmentID"]').val());
                    $(this).closest('tr').find('[name$="ddl_formClassSettingForInfoSystemApply_list_departmentID"]').prop("disabled", true);

                    $(this).closest('tr').find('[name$="hid_formClassSettingForInfoSystemApply_list_departmentID"]').val($(this).closest('tr').find('[name$="ddl_formClassSettingForInfoSystemApply_list_departmentID"]').val());
                }
            });

            //將rpt動態加簽人員點選的值,放到隱藏欄位
            $("select[data-signature-id]").change(function () {
                if ($(this).val() == null) {
                    $(this).closest('tr').find('[name$="hidd_rptActivityPersonnelID"]').val("");
                } else {
                    $(this).closest('tr').find('[name$="hidd_rptActivityPersonnelID"]').val($(this).val());
                }
            });

            //是否全單位適用判斷
            if ($.trim($('[id$="ddl_formClassSettingForInfoSystemApply_isforAll"]').val()) == "1") {
                $('[id$="ddl_PermissionDepartmentID"]').val("");
                $('[id$="ddl_PermissionDepartmentID"]').prop("disabled", true);
            } else {
                $('[id$="ddl_PermissionDepartmentID"]').prop("disabled", false);
            }

            $('[id$="btn_formClassSettingForInfoSystemApply_add"]').click(function () {
                
                var personnelID = $("#ddl_formClassSettingForInfoSystemApply_personnelID").val();

                //if (!personnelID || personnelID == "") {
                //    alert("請選擇處理人員")
                //    return false;
                //}

                if ($.trim($('[id$="txt_formClassSettingForInfoSystemApply_className"]').val()) == "") {
                    alert("請輸入類別名稱")
                    return false;
                }

                if ($.trim($('[id$="ddl_formClassSettingForInfoSystemApply_parentID"]').val()) == "") {
                    alert("請選擇主類別")
                    return false;
                }

                if ($.trim($('[id$="ddl_departmentID"]').val()) == "A0100001" || $.trim($('[id$="ddl_departmentID"]').val()) == "A0110000" || $.trim($('[id$="ddl_departmentID"]').val()) == "A0140000") {
                    alert("請選擇正確的處理人員")
                    return false;
                }

                if ($.trim($('[id$="ddl_formClassSettingForInfoSystemApply_dicisionProcessLevel"]').val()) == "") {
                    alert("請選擇核決層級")
                    return false;
                }

                if ($.trim($('[id$="ddl_formClassSettingForInfoSystemApply_isforAll"]').val()) == "") {
                    alert("請選擇是否適用全部門")
                    return false;
                } else if ($.trim($('[id$="ddl_formClassSettingForInfoSystemApply_isforAll"]').val()) == "0") {
                    if ($.trim($('[id$="ddl_PermissionDepartmentID"]').val()) == "") {
                        alert("請選擇權限單位")
                        return false;
                    }
                }

                //set value to hidden field                
                $('[id$="hid_formClassSettingForInfoSystemApply_personnelID"]').val(personnelID);
            });

            $('[id$="btn_formClassSettingForInfoSystemApply_className_search"]').click(function () {
                $(this).button('loading')

                //此處做法用來解決分頁查詢問題,使用前端get方式
                var keyword  = $('[id$="txt_formClassSettingForInfoSystemApply_className_search_keyword"]').val();
                var mainClass = $('[id$="ddl_formClassSettingForInfoSystemApply_mainClass"]').val();

                location.href = "/Manage/FormClassSettingForInfoSystemApply.aspx?pg=1" + "&k=" + encodeURIComponent(keyword) + "&c=" + mainClass;

                return false;
            });
            //修改
            $('#tb_formClassSettingForInfoSystemApply_list').on('click', '[name$="btn_formClassSettingForInfoSystemApply_list_update"]', function () {
                       
            });
            //刪除
            $('#tb_formClassSettingForInfoSystemApply_list').on('click', '[name$="btn_formClassSettingForInfoSystemApply_list_delete"]', function () {
                if (!confirm('確定要刪除此資料?')) {
                    return false;
                }
            });
            //進階按鈕
            $('#tb_formClassSettingForInfoSystemApply_list').on('click', '[name$="btn_formClassSettingForInfoSystemApply_list_advanced"]', function () {
                var Personnel = $(this).closest('tr').find('.td_dataPersonnelID').text().trim()//處理人員
                var ActivityPersonnel = $(this).closest('tr').find('.td_ActivityPersonnelID').text().trim()//加簽人員

                $(this).closest('tr').find('[name$="hidd_rptPersonnelID"]').val(Personnel);
                $(this).closest('tr').find('[name$="hidd_rptActivityPersonnelID"]').val(ActivityPersonnel);

                //return false;
            });

            //當新增時所需要帶值及隱藏
            $('[id$="ddl_formClassSettingForInfoSystemApply_personnelID"]').change(function () {
                if ($.trim($('[id$="ddl_formClassSettingForInfoSystemApply_personnelID"]').val()) != "") {
                    $('[id$="hid_ddl_departmentID"]').val($('[id$="ddl_formClassSettingForInfoSystemApply_personnelID"]').val());
                    $('[id$="hid_ddl_departmentID"]').prop("disabled", true);
                    
                    var departmentName = $('[id$="hid_ddl_departmentID"] option:selected').text();

                    $('[id$="ddl_departmentID"] option:contains(' + departmentName + ')').attr('selected', true);
                    $('[id$="ddl_departmentID"]').prop("disabled", true);

                    $('[id$="ddl_departmentID"]').select2('val', $('[id$="ddl_departmentID"]').val());
                    $('[id$="hid_departmentID"]').val($('[id$="ddl_departmentID"]').val());
                } else {
                    $('[id$="hid_ddl_departmentID"]').val("");
                    $('[id$="hid_ddl_departmentID"]').prop("disabled", false);
                    $('[id$="ddl_departmentID"]').val("");
                    $('[id$="ddl_departmentID"]').select2('val', "");
                    $('[id$="ddl_departmentID"]').prop("disabled", false);
                }

                //$('[id$="ddl_formClassSettingForInfoSystemApply_personnelID"]').select2.defaults.set("id", "H0960100");
                //$('[id$="ddl_formClassSettingForInfoSystemApply_personnelID"]').select2("val", [{ "id": "H0960100" }]);
                //$('[id$="ddl_formClassSettingForInfoSystemApply_personnelID"]').select2.defaults.set("val", 'H0960100');
                //$('[id$="ddl_formClassSettingForInfoSystemApply_personnelID"]').select2('val', "H0960100");
                //search
                //$('[id$="ddl_formClassSettingForInfoSystemApply_personnelID"]').select2('search', "H0960100");
                //select2('val', someID, true);
                //$('[id$="ddl_formClassSettingForInfoSystemApply_personnelID"]').select2('val', "H0960100", true);

                //
                //$("#ddl_formClassSettingForInfoSystemApply_personnelID").select2('search', "H0960100");

                //initSelection: function(element, callback) {
                //    callback({ id: element.val(), text: element.attr('data-init-text') });
                //}
            });

            //新增時點選處理單位
            $('[id$="ddl_departmentID"]').change(function () {
                if ($.trim($('[id$="ddl_departmentID"]').val()) != "") {
                    $('[id$="hid_departmentID"]').val($('[id$="ddl_departmentID"]').val());
                } else {
                    $('[id$="hid_departmentID"]').val("");
                }
            });

            //是否全部門適用,控制權限單位
            $('[id$="ddl_formClassSettingForInfoSystemApply_isforAll"]').change(function () {
                if ($.trim($('[id$="ddl_formClassSettingForInfoSystemApply_isforAll"]').val()) == "" || $.trim($('[id$="ddl_formClassSettingForInfoSystemApply_isforAll"]').val()) == "0") {
                    $('[id$="ddl_PermissionDepartmentID"]').prop("disabled", false);
                } else if ($.trim($('[id$="ddl_formClassSettingForInfoSystemApply_isforAll"]').val()) == "1") {
                    $('[id$="ddl_PermissionDepartmentID"]').val("");
                    $('[id$="ddl_PermissionDepartmentID"]').prop("disabled", true);
                }
            });

            //加簽人員
            $('[id$="ddl_formClassSettingForInfoSystemApply_ActivityPersonnelID"]').change(function () {
                if ($.trim($('[id$="ddl_formClassSettingForInfoSystemApply_ActivityPersonnelID"]').val()) != "") {
                    $('[id$="hid_formClassSettingForInfoSystemApply_ActivityPersonnelID"]').val($('[id$="ddl_formClassSettingForInfoSystemApply_ActivityPersonnelID"]').val());
                }
            });

            //進階頁面
            var rpvCount = $('#tb_advanced_list tr').length;

            if (rpvCount > 0) {
                var Personnel = $('#tb_advanced_list tr td').find('[name$="hid_ViewAdvanced_PersonnelID"]').val().split("(");//每列TD欄位(td_advancedPersonnelID),處理人員
                var ActivityPersonnel = $('#tb_advanced_list tr td').find('[name$="hid_ViewAdvanced_ActivityPersonnelID"]').val().split("(");//每列TD欄位(td_advancedActivityPersonnelID),加簽人員


                if (Personnel != "") {
                    var PersonnelID = Personnel[1].substring(Personnel[1].length - 1, 0);
                    var Personnelname = Personnel[0];

                    $("#ddl_ViewAdvanced_PersonnelID").append('<option value="' + PersonnelID + '">' + Personnelname + '</option>');//預帶處理人員
                    $('#tb_advanced_list tr td').find('[name$="hid_ViewAdvanced_PersonnelID"]').val(PersonnelID);
                }

                $("#ddl_ViewAdvanced_PersonnelID").select2(thisClass.seletctObj(mesg[0]));//處理人員控制項
                
                if (ActivityPersonnel != "") {
                    var ActivityPersonnelID = ActivityPersonnel[1].substring(ActivityPersonnel[1].length - 1, 0);
                    var ActivityPersonnelname = ActivityPersonnel[0];

                    $("#ddl_ViewAdvanced_ActivityPersonnelID").append('<option value="' + ActivityPersonnelID + '">' + ActivityPersonnelname + '</option>');//預帶加簽人員
                    $('#tb_advanced_list tr td').find('[name$="hid_ViewAdvanced_ActivityPersonnelID"]').val(ActivityPersonnelID);
                }

                $("#ddl_ViewAdvanced_ActivityPersonnelID").select2(thisClass.seletctObj(mesg[1]));//加簽人員控制項
            }

            //處理人員
            $("#ddl_ViewAdvanced_PersonnelID").select2(thisClass.seletctObj(mesg[0]));

            $('[id$="ddl_ViewAdvanced_PersonnelID"]').change(function () {
                if ($.trim($('[id$="ddl_ViewAdvanced_PersonnelID"]').val()) != "") {
                    $('[id$="hid_ViewAdvanced_PersonnelID"]').val($('[id$="ddl_ViewAdvanced_PersonnelID"]').val());
                    $('[id$="hid_ddl_ViewAdvanced_DepartmentID"]').val($('[id$="ddl_ViewAdvanced_PersonnelID"]').val());

                    var departmentName = $('[id$="hid_ddl_ViewAdvanced_DepartmentID"] option:selected').text();

                    $('[id$="ddl_ViewAdvanced_DepartmentID"] option:contains(' + departmentName + ')').attr('selected', true);
                    $('[id$="ddl_ViewAdvanced_DepartmentID"]').select2('val', $('[id$="ddl_ViewAdvanced_DepartmentID"]').val());
                    $("#ddl_ViewAdvanced_DepartmentID").prop("disabled", true);
                } else {
                    $('[id$="hid_ViewAdvanced_PersonnelID"]').val("");
                    $('[id$="hid_ViewAdvanced_departmentID"]').val("");
                    $('[id$="hid_ddl_ViewAdvanced_DepartmentID"]').val("");
                    $('[id$="hid_ddl_ViewAdvanced_DepartmentID"]').prop("disabled", false);
                    $('[id$="ddl_ViewAdvanced_DepartmentID"]').val("");
                    $('[id$="ddl_ViewAdvanced_DepartmentID"]').select2('val', "");
                    $('[id$="ddl_ViewAdvanced_DepartmentID"]').prop("disabled", false);
                }
            });

            //所屬單位
            $('[id$="ddl_ViewAdvanced_DepartmentID"]').select2();//所屬部門

            //加簽人員
            $("#ddl_ViewAdvanced_ActivityPersonnelID").select2(thisClass.seletctObj(mesg[1]));

            $('[id$="ddl_ViewAdvanced_ActivityPersonnelID"]').change(function () {
                //if ($.trim($('[id$="ddl_ViewAdvanced_ActivityPersonnelID"]').val()) != "") {
                //    $('[id$="hid_ViewAdvanced_ActivityPersonnelID"]').val($('[id$="ddl_ViewAdvanced_ActivityPersonnelID"]').val());
                //}
                $('[id$="hid_ViewAdvanced_ActivityPersonnelID"]').val($('[id$="ddl_ViewAdvanced_ActivityPersonnelID"]').val());
            });

            ////進階部門單位
            if ($.trim($('[id$="ddl_ViewAdvanced_PersonnelID"]').val()) != "") {
                $('[id$="ddl_ViewAdvanced_DepartmentID"]').prop("disabled", true);
            } else {
                $('[id$="ddl_ViewAdvanced_DepartmentID"]').prop("disabled", false);
            }

            //當進階修改時所需要帶值及隱藏
            $('[id$="ddl_ViewAdvanced_PersonnelID"]').change(function () {
                if ($.trim($('[id$="ddl_ViewAdvanced_PersonnelID"]').val()) != "") {
                    $('[id$="hid_ddl_ViewAdvanced_DepartmentID"]').val($('[id$="ddl_ViewAdvanced_PersonnelID"]').val());
                    $('[id$="hid_ddl_ViewAdvanced_DepartmentID"]').prop("disabled", true);

                    var departmentName = $('[id$="hid_ddl_ViewAdvanced_DepartmentID"] option:selected').text();
                    $('[id$="ddl_ViewAdvanced_DepartmentID"] option:contains(' + departmentName + ')').attr('selected', true);
                    $('[id$="ddl_ViewAdvanced_DepartmentID"]').prop("disabled", true);

                    $('[id$="hid_ViewAdvanced_departmentID"]').val($('[id$="ddl_ViewAdvanced_DepartmentID"]').val());
                } else {
                    $('[id$="hid_ddl_ViewAdvanced_DepartmentID"]').val("");
                    $('[id$="hid_ddl_ViewAdvanced_DepartmentID"]').prop("disabled", false);
                    $('[id$="ddl_ViewAdvanced_DepartmentID"]').val("");
                    $('[id$="ddl_ViewAdvanced_DepartmentID"]').prop("disabled", false);
                }
            });

            //是否全單位適用判斷
            if ($.trim($('[id$="ddl_ViewAdvanced_IsforAll"]').val()) == "1") {
                $('[id$="ddl_ViewAdvanced_PermissionDepartmentID"]').val("");
                $('[id$="ddl_ViewAdvanced_PermissionDepartmentID"]').prop("disabled", true);
            } else {
                $('[id$="ddl_ViewAdvanced_PermissionDepartmentID"]').prop("disabled", false);
            }

            //是否全部門適用,控制權限單位
            $('[id$="ddl_ViewAdvanced_IsforAll"]').change(function () {
                if ($.trim($('[id$="ddl_ViewAdvanced_IsforAll"]').val()) == "" || $.trim($('[id$="ddl_ViewAdvanced_IsforAll"]').val()) == "0") {
                    $('[id$="ddl_ViewAdvanced_PermissionDepartmentID"]').prop("disabled", false);
                } else if ($.trim($('[id$="ddl_ViewAdvanced_IsforAll"]').val()) == "1") {
                    $('[id$="ddl_ViewAdvanced_PermissionDepartmentID"]').val("");
                    $('[id$="ddl_ViewAdvanced_PermissionDepartmentID"]').prop("disabled", true);
                }
            });
        }

        FormClassSettingForInfoSystemApply.prototype.args = {
            pageSize: 30
        };

        FormClassSettingForInfoSystemApply.prototype.ui = {
            template_personnel_info: null,
        };

        FormClassSettingForInfoSystemApply.prototype.seletctObj = function (str) {
            var thisClass = this;

            return {
                language: "zh-TW",
                //placeholder: '請輸入處理人員',
                placeholder: '請輸入' + str,
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

        //處理人員查詢
        FormClassSettingForInfoSystemApply.prototype.formatRepo = function (repo) {
            var thisClass = this;

            if (repo.loading) { return repo.text; }
            var $tmp = thisClass.ui.template_personnel_info.clone();
            $tmp.attr("id", "emp_" + repo.id);
            $tmp.find('[data-field]').each(function () {
                $(this).text(repo[$(this).attr("data-field")]);
            });

            var ret = $tmp;

            return ret;
        };

        FormClassSettingForInfoSystemApply.prototype.formatRepoSelection = function (repo) {
            if (repo.name) {
                 return (repo.name + (repo.id ? "(" + repo.id + ")" : ""));
            } else {
                 return repo.text + (repo.id ? "(" + repo.id + ")" : "");
            }
        }

        var frmCsf = new FormClassSettingForInfoSystemApply();

    </script>
</asp:Content>
