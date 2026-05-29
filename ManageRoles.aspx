<%@ Page Title="Manage Roles" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ManageRoles.aspx.cs" Inherits="KMinds.Portal.Web.ManageRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .kminds-table { width: 100%; border-collapse: collapse; margin-top: 1rem; }
        .kminds-table th, .kminds-table td { padding: 1rem; border-bottom: 1px solid var(--border); text-align: left; }
        .kminds-table th { background-color: var(--surface); font-weight: 600; }
        .form-control { padding: 0.5rem; border: 1px solid var(--border); border-radius: 4px; background: var(--surface); color: var(--text); }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="manage-roles-section" style="padding: 2rem;">
        <div class="container">
            <h1 class="page-title">Manage User Roles</h1>
            <p class="subtitle">Assign roles to members. Only Presidents and Admins can view this page.</p>

            <asp:Label ID="StatusMessage" runat="server" Visible="false"></asp:Label>

            <div style="overflow-x: auto; margin-top: 2rem;">
                <asp:GridView ID="UsersGridView" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="UserId" OnRowCommand="UsersGridView_RowCommand" 
                    CssClass="kminds-table" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="FullName" HeaderText="Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Department" HeaderText="Department" />
                        
                        <asp:TemplateField HeaderText="Role">
                            <ItemTemplate>
                                <asp:DropDownList ID="RoleDropDown" runat="server" CssClass="form-control" style="width: auto;">
                                    <asp:ListItem Text="Member" Value="member" />
                                    <asp:ListItem Text="Treasurer" Value="treasurer" />
                                    <asp:ListItem Text="General Secretary" Value="general-secretary" />
                                    <asp:ListItem Text="Vice-President" Value="vice-president" />
                                    <asp:ListItem Text="President" Value="president" />
                                    <asp:ListItem Text="Admin" Value="Admin" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="UpdateRoleBtn" runat="server" CommandName="UpdateRole" 
                                    CommandArgument='<%# Container.DataItemIndex %>' 
                                    Text="Update Role" CssClass="btn btn-primary btn-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </section>
</asp:Content>
