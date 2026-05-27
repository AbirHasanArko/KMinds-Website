<%@ Page Title="Sign Up" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="KMinds.Portal.Web.Register" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <section style="max-width:560px;margin:1.5rem auto" aria-labelledby="signup-heading">
      <div style="text-align:center;margin-bottom:1.5rem">
        <div class="avatar" style="margin:0 auto 1rem;width:64px;height:64px;font-size:1.3rem">
          <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#fff" stroke-width="2"><path d="M16 21v-2a4 4 0 00-4-4H6a4 4 0 00-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M22 21v-2a4 4 0 00-3-3.87M16 3.13a4 4 0 010 7.75"/></svg>
        </div>
        <h2 id="signup-heading" style="background:linear-gradient(135deg,var(--hero-title-start),var(--brand));-webkit-background-clip:text;-webkit-text-fill-color:transparent;background-clip:text;font-size:1.6rem">Join KMinds</h2>
        <p>Use your KUET student email (@stud.kuet.ac.bd)</p>
      </div>

      <div class="form-container" style="display:flex;flex-direction:column;gap:1.25rem;">
        <asp:Label ID="ErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>

        <div class="form-row">
          <div class="form-group">
            <label for="FullNameTextBox">Full Name</label>
            <asp:TextBox ID="FullNameTextBox" runat="server" placeholder="Your full name" required="true"></asp:TextBox>
          </div>
          <div class="form-group">
            <label for="RollTextBox">Roll Number</label>
            <asp:TextBox ID="RollTextBox" runat="server" placeholder="e.g. 2105001" required="true"></asp:TextBox>
          </div>
        </div>

        <div class="form-group">
          <label for="EmailTextBox">Email (@stud.kuet.ac.bd)</label>
          <asp:TextBox ID="EmailTextBox" runat="server" TextMode="Email" placeholder="yourid@stud.kuet.ac.bd" required="true" pattern="^[^@\s]+@stud\.kuet\.ac\.bd$"></asp:TextBox>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label for="DepartmentDropDown">Department</label>
            <asp:DropDownList ID="DepartmentDropDown" runat="server" required="true" CssClass="form-control">
              <asp:ListItem Value="" Text="Select Dept" disabled="true" Selected="True"></asp:ListItem>
              <asp:ListItem Value="EEE" Text="EEE"></asp:ListItem>
              <asp:ListItem Value="CSE" Text="CSE"></asp:ListItem>
              <asp:ListItem Value="ECE" Text="ECE"></asp:ListItem>
              <asp:ListItem Value="BME" Text="BME"></asp:ListItem>
              <asp:ListItem Value="MSE" Text="MSE"></asp:ListItem>
              <asp:ListItem Value="MTE" Text="MTE"></asp:ListItem>
              <asp:ListItem Value="ME" Text="ME"></asp:ListItem>
              <asp:ListItem Value="IEM" Text="IEM"></asp:ListItem>
              <asp:ListItem Value="TE" Text="TE"></asp:ListItem>
              <asp:ListItem Value="LE" Text="LE"></asp:ListItem>
              <asp:ListItem Value="ESE" Text="ESE"></asp:ListItem>
              <asp:ListItem Value="ChE" Text="ChE"></asp:ListItem>
              <asp:ListItem Value="CE" Text="CE"></asp:ListItem>
              <asp:ListItem Value="URP" Text="URP"></asp:ListItem>
              <asp:ListItem Value="BECM" Text="BECM"></asp:ListItem>
              <asp:ListItem Value="ARCH" Text="ARCH"></asp:ListItem>
            </asp:DropDownList>
          </div>
          <div class="form-group">
            <label for="YearTermDropDown">Year-Term</label>
            <asp:DropDownList ID="YearTermDropDown" runat="server" required="true" CssClass="form-control">
              <asp:ListItem Value="" Text="Select" disabled="true" Selected="True"></asp:ListItem>
              <asp:ListItem Value="1-1" Text="1-1"></asp:ListItem>
              <asp:ListItem Value="1-2" Text="1-2"></asp:ListItem>
              <asp:ListItem Value="2-1" Text="2-1"></asp:ListItem>
              <asp:ListItem Value="2-2" Text="2-2"></asp:ListItem>
              <asp:ListItem Value="3-1" Text="3-1"></asp:ListItem>
              <asp:ListItem Value="3-2" Text="3-2"></asp:ListItem>
              <asp:ListItem Value="4-1" Text="4-1"></asp:ListItem>
              <asp:ListItem Value="4-2" Text="4-2"></asp:ListItem>
            </asp:DropDownList>
          </div>
        </div>

        <div class="form-group">
          <label for="ProfileImageUpload">Profile Photo (optional)</label>
          <div class="image-upload-area" id="profile-upload-area">
            <asp:FileUpload ID="ProfileImageUpload" runat="server" accept="image/*" />
            <div class="upload-icon">📷</div>
            <p>Click or drag to upload profile photo</p>
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label for="PasswordTextBox">Password</label>
            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" required="true" placeholder="Min 8 characters"></asp:TextBox>
          </div>
          <div class="form-group">
            <label for="ConfirmPasswordTextBox">Confirm Password</label>
            <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" TextMode="Password" required="true" placeholder="Re-enter password"></asp:TextBox>
          </div>
        </div>

        <asp:Button ID="RegisterButton" runat="server" Text="Create Account" OnClick="RegisterButton_Click" CssClass="btn btn-primary" Width="100%" />
      </div>

      <p style="text-align:center;margin-top:1rem">
        Already have an account? <a href="Login.aspx">Sign in</a>
      </p>
    </section>
</asp:Content>
