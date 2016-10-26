<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="component_pager.ascx.cs" Inherits="WebServiceForEFGP.Manage.component_pager" %>

<style>
    .pagination > li > a.active {
        z-index: 2;
        color: #23527c;
        background-color: #eee;
        border-color: #ddd;
    }
</style>


<div style="text-align: center;">

    <ul class="pagination">
        <asp:Repeater ID="rpt_data_pager" runat="server">
            <ItemTemplate>
                <li>
                    <a class="<%# (bool)(Eval("isCurrent"))?"active":"" %>"
                        href='<%# String.IsNullOrEmpty(Eval("url").ToString())?"#":Eval("url") %>'><%# Eval("text") %></a>
                </li>

            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>


