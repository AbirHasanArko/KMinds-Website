<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="KMinds.Portal.Web.Profile" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <section aria-labelledby="account-heading">
      <div class="profile-header">
        <div class="avatar" id="profile-avatar">
          <span id="avatar-initials"><asp:Literal ID="InitialsLiteral" runat="server"></asp:Literal></span>
        </div>
        <div class="profile-info">
          <h2 id="account-heading" style="background:linear-gradient(135deg,var(--hero-title-start),var(--brand));-webkit-background-clip:text;-webkit-text-fill-color:transparent;background-clip:text;font-size:1.5rem"><asp:Literal ID="FullNameLiteral" runat="server"></asp:Literal></h2>
          <p style="display:flex;align-items:center;gap:0.5rem">
            <asp:Label ID="StatusBadge" runat="server" CssClass="status status-pending"></asp:Label>
            <span><asp:Literal ID="RoleLiteral" runat="server"></asp:Literal></span>
          </p>
        </div>
      </div>
      <dl id="profile-details">
        <dt>Email</dt>
        <dd><asp:Literal ID="EmailLiteral" runat="server"></asp:Literal></dd>
        <dt>Department</dt>
        <dd><asp:Literal ID="DeptLiteral" runat="server"></asp:Literal></dd>
        <dt>Year-Term</dt>
        <dd><asp:Literal ID="YearTermLiteral" runat="server"></asp:Literal></dd>
        <dt>Roll</dt>
        <dd><asp:Literal ID="RollLiteral" runat="server"></asp:Literal></dd>
        <dt>Role</dt>
        <dd><asp:Literal ID="RoleDetailLiteral" runat="server"></asp:Literal></dd>
      </dl>
    </section>

    <section aria-labelledby="payment-status-heading">
      <h2 id="payment-status-heading">Payment Status</h2>
      <p style="margin-bottom:1rem">Status: <asp:Label ID="PaymentStatusText" runat="server" CssClass="status status-pending"></asp:Label></p>
      <div class="form-container" style="display:flex;flex-direction:column;gap:1.25rem;">
        <asp:Label ID="PaymentMessage" runat="server" ForeColor="Green" Visible="false"></asp:Label>
        <div class="form-group">
          <label for="BkashRefTextBox">bKash Transaction Reference</label>
          <asp:TextBox ID="BkashRefTextBox" runat="server" placeholder="e.g. TXN12345AB" required="true"></asp:TextBox>
        </div>
        <asp:Button ID="UpdatePaymentRefButton" runat="server" Text="Submit / Update Reference" OnClick="UpdatePaymentRefButton_Click" CssClass="btn btn-primary" Width="100%" />
      </div>
    </section>

    <section aria-labelledby="posting-privileges-heading">
      <h2 id="posting-privileges-heading">Posting Privileges</h2>
      <div class="feature-grid" style="grid-template-columns:1fr">
        <div class="feature-item">
          <div class="feature-icon">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M22 11.08V12a10 10 0 11-5.93-9.14"/><polyline points="22 4 12 14.01 9 11.01"/></svg>
          </div>
          <div class="feature-text">
            <h3>Active Members</h3>
            <p>All verified members can post articles, research items, and datasets. If payment is pending or revoked, content visibility is suspended.</p>
          </div>
        </div>
      </div>
    </section>
</asp:Content>
