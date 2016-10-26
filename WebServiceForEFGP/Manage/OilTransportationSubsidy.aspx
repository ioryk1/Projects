<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Manage.Master" AutoEventWireup="true" CodeBehind="OilTransportationSubsidy.aspx.cs" Inherits="WebServiceForEFGP.Manage.OilTransportationSubsidy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding: 15px;">
        <div style="border: 1px solid #ccc; padding: 15px;">
            <fieldset class="clearfix form-inline">
                <legend>新增油資補助</legend>


                <div class="form-group">
                    <label>年：</label>

                    <asp:DropDownList ID="ddl_oilTransSubsidy_year"
                        DataTextField="Text" DataValueField="Value"
                        CssClass="form-control" runat="server">
                    <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                    <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                    <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                    </asp:DropDownList>

                    <label>月：</label>
                    <asp:DropDownList ID="ddl_oilTransSubsidy_month"
                        DataTextField="Text" DataValueField="Value"
                        CssClass="form-control" runat="server">
                    </asp:DropDownList>



                </div>

                <br />
                <br />
                <div class="form-group">
                    <label>汽車 補助金額：</label>
                    <div class="input-group">
                        <asp:TextBox ID="txt_oilTransSubsidy_car_price" CssClass="form-control input-group-sm input-sm  text-right" runat="server"></asp:TextBox>
                        <div class=" input-group-addon">NT </div>
                    </div>
                </div>
                <br />
                <br />
                <div class="form-group">
                    <label>機車 補助金額：</label>
                    <div class="input-group">
                        <asp:TextBox ID="txt_oilTransSubsidy_motorcycle_price" CssClass="form-control input-group-sm input-sm  text-right" runat="server"></asp:TextBox>
                        <div class=" input-group-addon">NT </div>
                    </div>
                </div>

                <asp:Button CssClass="btn btn-default btn-sm" ID="btn_oilTransSubsidy_add" OnClick="btn_oilTransSubsidy_add_Click" runat="server" Text="新增" />



            </fieldset>

        </div>
        <fieldset>
                <legend>列表</legend>

                搜尋條件：<br />

                年：

                <asp:DropDownList AutoPostBack="true" ID="ddl_oilTransSubsidy_search_year"
                     OnSelectedIndexChanged="ddl_oilTransSubsidy_search_year_SelectedIndexChanged"
                                         
                 DataTextField="Text" DataValueField="Value" runat="server">
                    <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                    <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                    <asp:ListItem Text="2018" Value="2018"></asp:ListItem>

                </asp:DropDownList>                
                


                <table class=" table table-condensed table-hover table-striped" >                      
                    <thead>
                        <tr>
                            <th style="width:100px;">年/月</th>


                            <th style="width:30%">汽車<br />元/公里
                            </th>

                            <th style="width:30%">機車<br />元/公里</th>
                            
                            <th></th>
                        </tr>
                    </thead>
                    <tbody id="tb_oilTransportationSubsidy_list">
                        <asp:Repeater ID="rpt_oilTransportationSubsidy_list" runat="server" OnItemCommand="rpt_oilTransportationSubsidy_list_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <asp:HiddenField ID="hid_car_id" runat="server" Value='<%#Eval("car_id") %>' />
                                    <asp:HiddenField ID="hid_motorcycle_id" Value='<%#Eval("motorcycle_id") %>' runat="server" />

                                    <td><%#Eval("ym_str") %></td>
                                    <td>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="txt_oilTransportationSubsidy_list_car"
                                                Text='<%#Eval("car_price") %>' CssClass="form-control input-group-sm input-sm text-right" runat="server"></asp:TextBox>
                                            <div class=" input-group-addon">NT</div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="input-group input-group-sm">
                                            <asp:TextBox ID="txt_oilTransportationSubsidy_list_motorcycle"
                                                Text='<%#Eval("motorcycle_price") %>' CssClass="form-control input-group-sm input-sm text-right" runat="server"></asp:TextBox>
                                            <div class=" input-group-addon">NT</div>
                                        </div>
                                    </td>

                                    <td>

                                        <asp:Button ID="btn_oilTransportationSubsidy_list_update" CssClass="btn btn-default btn-sm" runat="server" CommandName="update" Text="修改" />
                                        <asp:Button ID="btn_oilTransportationSubsidy_list_delete" CssClass="btn btn-default btn-sm" runat="server" CommandName="delete" Text="刪除" />
                                    </td>

                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                    </tbody>

                </table>


            </fieldset>
    </div>
    
    <asp:Literal ID="lt_msg" runat="server"></asp:Literal>
    <script type="text/javascript">
        (function () {
            function OilTransportationSubsidy() {
                this.init();
            }
            

            OilTransportationSubsidy.prototype.init = function () {
                var thisClass = this;

                thisClass.eventHandler();

            }

            OilTransportationSubsidy.prototype.eventHandler = function () {

                $('[id$="btn_oilTransSubsidy_add"]').click(function () {
                    //validate


                    var year = $('[id$="ddl_oilTransSubsidy_year"]').val();
                    var month = $('[id$="ddl_oilTransSubsidy_month"]').val();
                    var car_price = $('[id$="txt_oilTransSubsidy_car_price"]').val();
                    var motorcycle_price = $('[id$="txt_oilTransSubsidy_motorcycle_price"]').val();

                    if (!$.isNumeric(year)) {
                        alert("請選擇年分");
                        return false;
                    }
                    if (!$.isNumeric(month)) {
                        alert("請選擇月分");
                        return false;
                    }
                    if (!$.isNumeric(car_price)) {
                        alert("汽車補助金額格式錯誤");
                        return false;
                    }
                    if (!$.isNumeric(motorcycle_price)) {
                        alert("機車補助金額格式錯誤");
                        return false;
                    }



                });

                $('[id$="tb_oilTransportationSubsidy_list"]').on('click', '[name$="btn_oilTransportationSubsidy_list_update"]', function () {
                    //validate
                    $tr = $(this).closest('tr');
                    var price_car =  $tr.find('[name$="txt_oilTransportationSubsidy_list_car"]').val();
                    var price_motorcycle = $tr.find('[name$="txt_oilTransportationSubsidy_list_motorcycle"]').val();

                    if (!$.isNumeric(price_car)) {
                        alert("汽車補助請輸入正確的金額");
                        return false;
                    }

                    if (!$.isNumeric(price_motorcycle)) {
                        alert("機車補助請輸入正確的金額");
                        return false;
                    }
                    

                });


                $('[id$="tb_oilTransportationSubsidy_list"]').on('click', '[name$="btn_oilTransportationSubsidy_list_delete"]', function () {
                    if (!confirm("確定要刪除此資料?")) {
                        return false;
                    }
                });                
            }

            OilTransportationSubsidy.prototype.custMethods = function () {
                return {

                }
            }

            var o = new OilTransportationSubsidy();
        })($);


    </script>
</asp:Content>
