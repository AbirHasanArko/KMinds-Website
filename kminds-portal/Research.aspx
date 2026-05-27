<%@ Page Title="Research" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Research.aspx.cs" Inherits="KMinds.Portal.Web.Research" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:PlaceHolder ID="SubmitResearchPlaceHolder" runat="server" Visible="false">
        <section aria-labelledby="submit-research-heading">
          <h2 id="submit-research-heading">Submit Research Item</h2>
          <div class="form-container" style="display:flex;flex-direction:column;gap:1.25rem;">
            <asp:Label ID="ResearchMessage" runat="server" ForeColor="Green" Visible="false"></asp:Label>
            <div class="form-group">
              <label for="ResearchTitleTextBox">Title</label>
              <asp:TextBox ID="ResearchTitleTextBox" runat="server" placeholder="Research paper title" required="true"></asp:TextBox>
            </div>
            <div class="form-group">
              <label for="ResearchAbstractTextBox">Abstract</label>
              <asp:TextBox ID="ResearchAbstractTextBox" runat="server" TextMode="MultiLine" Rows="5" placeholder="Paper abstract or summary..." required="true"></asp:TextBox>
            </div>
            <div class="form-group">
              <label for="ResearchCoverUpload">Cover / Diagram (optional)</label>
              <div class="image-upload-area">
                <asp:FileUpload ID="ResearchCoverUpload" runat="server" accept="image/*" />
                <div class="upload-icon">📊</div>
                <p>Upload an architecture diagram or result figure</p>
              </div>
            </div>
            <div class="form-group">
              <label for="ResearchLinkTextBox">Paper/Resource Link</label>
              <asp:TextBox ID="ResearchLinkTextBox" runat="server" TextMode="Url" placeholder="https://arxiv.org/..." required="true"></asp:TextBox>
            </div>
            <asp:Button ID="ShareResearchButton" runat="server" Text="Share Research" OnClick="ShareResearchButton_Click" CssClass="btn btn-primary" Width="100%" />
          </div>
        </section>
    </asp:PlaceHolder>

    <section aria-labelledby="research-feed-heading">
      <h2 id="research-feed-heading">Research Feed</h2>
      <div class="card-grid" id="research-feed">
        <asp:Repeater ID="ResearchRepeater" runat="server">
            <ItemTemplate>
                <div class="card">
                  <img class="card-image" src='<%# Eval("ThumbnailUrl") %>' alt='<%# Eval("Title") %>'>
                  <div class="card-body">
                    <div class="card-meta"><span>🔬</span><span><%# Eval("AuthorName") %></span><span>·</span><span><%# Eval("PublishDate", "{0:yyyy}") %></span></div>
                    <h3><%# Eval("Title") %></h3>
                    <p><%# Eval("Abstract") %></p>
                    <asp:HyperLink runat="server" NavigateUrl='<%# Eval("DownloadLink") %>' CssClass="btn btn-secondary btn-sm" style="margin-top:0.5rem">Read Paper →</asp:HyperLink>
                  </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
      </div>
    </section>
</asp:Content>
