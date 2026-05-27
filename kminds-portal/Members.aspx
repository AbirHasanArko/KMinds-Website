<%@ Page Title="Members & Payment Audit" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Members.aspx.cs" Inherits="KMinds.Portal.Web.Members" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <section id="admin-audit" aria-labelledby="audit-heading">
      <h2 id="audit-heading">Audit & Verification</h2>
      <p style="margin-bottom:1rem">Review member payment references and manage membership status.</p>

      <!-- Filters -->
      <div class="filter-row" style="display:flex;gap:1rem;flex-wrap:wrap;margin-bottom:1.5rem">
        <div class="form-group">
          <label for="FilterRoleDropDown">Role</label>
          <asp:DropDownList ID="FilterRoleDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed" CssClass="form-control">
            <asp:ListItem Value="">All Roles</asp:ListItem>
            <asp:ListItem Value="member">Member</asp:ListItem>
            <asp:ListItem Value="treasurer">Treasurer</asp:ListItem>
            <asp:ListItem Value="general-secretary">General Secretary</asp:ListItem>
            <asp:ListItem Value="vice-president">Vice-President</asp:ListItem>
            <asp:ListItem Value="president">President</asp:ListItem>
          </asp:DropDownList>
        </div>
        <div class="form-group">
          <label for="FilterDeptDropDown">Department</label>
          <asp:DropDownList ID="FilterDeptDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed" CssClass="form-control">
            <asp:ListItem Value="">All Depts</asp:ListItem>
            <asp:ListItem Value="EEE">EEE</asp:ListItem>
            <asp:ListItem Value="CSE">CSE</asp:ListItem>
            <asp:ListItem Value="ECE">ECE</asp:ListItem>
            <asp:ListItem Value="BME">BME</asp:ListItem>
            <asp:ListItem Value="MSE">MSE</asp:ListItem>
            <asp:ListItem Value="MTE">MTE</asp:ListItem>
            <asp:ListItem Value="ME">ME</asp:ListItem>
            <asp:ListItem Value="IEM">IEM</asp:ListItem>
            <asp:ListItem Value="TE">TE</asp:ListItem>
            <asp:ListItem Value="LE">LE</asp:ListItem>
            <asp:ListItem Value="ESE">ESE</asp:ListItem>
            <asp:ListItem Value="ChE">ChE</asp:ListItem>
            <asp:ListItem Value="CE">CE</asp:ListItem>
            <asp:ListItem Value="URP">URP</asp:ListItem>
            <asp:ListItem Value="BECM">BECM</asp:ListItem>
            <asp:ListItem Value="ARCH">ARCH</asp:ListItem>
          </asp:DropDownList>
        </div>
        <div class="form-group">
          <label for="FilterStatusDropDown">Status</label>
          <asp:DropDownList ID="FilterStatusDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed" CssClass="form-control">
            <asp:ListItem Value="">All</asp:ListItem>
            <asp:ListItem Value="pending">Pending</asp:ListItem>
            <asp:ListItem Value="approved">Approved</asp:ListItem>
            <asp:ListItem Value="rejected">Rejected</asp:ListItem>
          </asp:DropDownList>
        </div>
      </div>

      <!-- Table -->
      <div class="table-wrap">
        <table id="member-audit-table">
          <caption>Payment Verification Queue</caption>
          <thead>
            <tr>
              <th scope="col">Member</th>
              <th scope="col">Email</th>
              <th scope="col">Role</th>
              <th scope="col">Dept</th>
              <th scope="col">bKash Ref</th>
              <th scope="col">Status</th>
              <th scope="col">Actions</th>
            </tr>
          </thead>
          <tbody>
            <asp:Repeater ID="MembersRepeater" runat="server" OnItemCommand="MembersRepeater_ItemCommand">
                <ItemTemplate>
                    <tr>
                      <td data-label="Member"><%# Eval("FullName") %></td>
                      <td data-label="Email"><%# Eval("Email") %></td>
                      <td data-label="Role"><%# Eval("Role") %></td>
                      <td data-label="Dept"><%# Eval("Department") %></td>
                      <td data-label="bKash Ref"><%# Eval("PaymentRef") %></td>
                      <td data-label="Status"><span class='status status-<%# Eval("PaymentStatus").ToString().ToLower() %>'><%# Eval("PaymentStatus") %></span></td>
                      <td data-label="Actions">
                        <div class="btn-group">
                          <asp:Button runat="server" CommandName="Approve" CommandArgument='<%# Eval("UserId") %>' CssClass="btn btn-success btn-sm" Text="✓" />
                          <asp:Button runat="server" CommandName="Reject" CommandArgument='<%# Eval("UserId") %>' CssClass="btn btn-danger btn-sm" Text="✗" />
                        </div>
                      </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
          </tbody>
        </table>
      </div>
    </section>
</asp:Content>
