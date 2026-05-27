<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="KMinds.Portal.Web.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <section style="max-width:480px;margin:2rem auto">
      <div style="text-align:center;margin-bottom:1.5rem">
        <div class="avatar" style="margin:0 auto 1rem;width:64px;height:64px;font-size:1.3rem">
          <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#fff" stroke-width="2"><path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
        </div>
        <h2 id="login-heading" style="background:linear-gradient(135deg,var(--hero-title-start),var(--brand));-webkit-background-clip:text;-webkit-text-fill-color:transparent;background-clip:text;font-size:1.6rem">Welcome Back</h2>
        <p>Sign in with your KUET student email</p>
      </div>

      <div class="form-container" style="display:flex;flex-direction:column;gap:1.25rem;">
        <asp:Label ID="ErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        
        <div class="form-group">
          <label for="EmailTextBox">Institutional Email</label>
          <asp:TextBox ID="EmailTextBox" runat="server" TextMode="Email" placeholder="example@stud.kuet.ac.bd" required="true"></asp:TextBox>
        </div>
        <div class="form-group">
          <label for="PasswordTextBox">Password</label>
          <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" required="true"></asp:TextBox>
        </div>
        <asp:Button ID="LoginButton" runat="server" Text="Sign In" OnClick="LoginButton_Click" CssClass="btn btn-primary" Width="100%" />
      </div>

      <p style="text-align:center;margin-top:1.25rem">
        New member? <a href="Register.aspx">Create an account</a>
      </p>
    </section>

    <section style="max-width:480px;margin:0 auto" aria-labelledby="payment-proof-heading">
      <h2 id="payment-proof-heading">Payment Verification</h2>
      <p style="margin-bottom:1rem">New members must submit a bKash transaction reference to verify boarding fee payment after signing up.</p>
      <div class="form-container" style="display:flex;flex-direction:column;gap:1.25rem;">
        <div class="form-group">
          <label for="BkashRefTextBox">bKash Transaction Reference</label>
          <asp:TextBox ID="BkashRefTextBox" runat="server" placeholder="e.g. TXN12345AB" required="true"></asp:TextBox>
        </div>
        <asp:Button ID="VerifyPaymentButton" runat="server" Text="Submit Reference" CssClass="btn btn-secondary" Width="100%" />
      </div>
    </section>
</asp:Content>
