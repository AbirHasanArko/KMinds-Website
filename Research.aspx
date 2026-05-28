<%@ Page Title="Research" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Research.aspx.cs" Inherits="KMinds.Portal.Web.Research" %>

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
                <asp:FileUpload ID="ThumbnailUpload" runat="server" accept="image/*" />
                <div class="upload-icon">📷</div>
                <p>Click or drag thumbnail</p>
                <img class="image-preview" src="" alt="Thumbnail preview" />
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
        <asp:Repeater ID="ResearchRepeater" runat="server" OnItemDataBound="ResearchRepeater_ItemDataBound" OnItemCommand="ResearchRepeater_ItemCommand">
            <ItemTemplate>
                <div class="card">
                  <img class="card-image" src='<%# Eval("ThumbnailUrl") %>' alt='<%# Eval("Title") %>'>
                  <div class="card-body">
                    <div class="card-meta">
                        <span>🔬 By <%# Eval("AuthorName") %></span>
                        <span>· <%# Eval("PublishDate", "{0:MMM dd, yyyy}") %></span>
                        <div style="margin-left:auto;display:flex;gap:0.25rem;">
                            <asp:HyperLink ID="EditLink" runat="server" NavigateUrl='<%# "Research.aspx?edit_id=" + Eval("ResearchId") %>' Text="Edit" CssClass="btn btn-secondary" style="padding:0.2rem 0.5rem;font-size:0.75rem;" Visible="false"></asp:HyperLink>
                            <asp:LinkButton ID="DeleteBtn" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ResearchId") %>' Text="Delete" CssClass="btn btn-secondary" style="padding:0.2rem 0.5rem;font-size:0.75rem;background-color:#ffebee;color:#c62828;border-color:#ffcdd2" Visible="false" OnClientClick="return confirm('Are you sure you want to delete this research item?');"></asp:LinkButton>
                        </div>
                    </div>
                    <h3><%# Eval("Title") %></h3>
                    <p><%# Eval("Summary") %></p>
                    <div style="display:flex;gap:0.5rem;margin-top:0.5rem">
                      <button type="button" class="btn btn-secondary btn-sm" onclick="openDetailsModal('<%# HttpUtility.JavaScriptStringEncode(Eval("Title").ToString()) %>', this.parentNode.nextElementSibling.innerHTML, '<%# HttpUtility.JavaScriptStringEncode("🔬 By " + Eval("AuthorName") + " &middot; " + Eval("PublishDate", "{0:MMM dd, yyyy}")) %>')">View Details</button>
                      <asp:HyperLink runat="server" NavigateUrl='<%# Eval("DownloadLink") %>' CssClass="btn btn-primary btn-sm">Read Paper &rarr;</asp:HyperLink>
                    </div>
                    <div style="display:none;"><%# Eval("FullContent") %></div>
                  </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
      </div>
    </section>
</asp:Content>
