<%@ Page Title="Articles" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Articles.aspx.cs" Inherits="KMinds.Portal.Web.Articles" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Create Article -->
    <asp:PlaceHolder ID="CreateArticlePlaceHolder" runat="server" Visible="false">
        <section aria-labelledby="new-article-heading">
          <h2 id="new-article-heading">Create Article</h2>
          <div class="form-container" style="display:flex;flex-direction:column;gap:1.25rem;">
            <asp:Label ID="ArticleMessage" runat="server" ForeColor="Green" Visible="false"></asp:Label>
            <div class="form-group">
              <label for="ArticleTitleTextBox">Title</label>
              <asp:TextBox ID="ArticleTitleTextBox" runat="server" placeholder="Article title" required="true"></asp:TextBox>
            </div>
            <div class="form-group">
              <label for="ArticleSummaryTextBox">Summary</label>
              <asp:TextBox ID="ArticleSummaryTextBox" runat="server" TextMode="MultiLine" Rows="3" placeholder="Brief summary of the article..." required="true"></asp:TextBox>
            </div>
            <div class="form-group">
              <label for="ArticleCoverUpload">Cover Image (optional)</label>
              <div class="image-upload-area" id="article-upload-area">
                <asp:FileUpload ID="ArticleCoverUpload" runat="server" accept="image/*" />
                <div class="upload-icon">🖼️</div>
                <p>Click or drag to upload cover image</p>
              </div>
            </div>
            <div class="form-group">
              <label for="ArticleContentTextBox">Content</label>
              <asp:TextBox ID="ArticleContentTextBox" runat="server" TextMode="MultiLine" Rows="8" placeholder="Write your article content here..." required="true"></asp:TextBox>
            </div>
            <asp:Button ID="PublishArticleButton" runat="server" Text="Publish Article" OnClick="PublishArticleButton_Click" CssClass="btn btn-primary" Width="100%" />
          </div>
        </section>
    </asp:PlaceHolder>

    <!-- Article Feed -->
    <section aria-labelledby="article-feed-heading">
      <h2 id="article-feed-heading">Article Feed</h2>
      <div class="card-grid" id="article-feed">
        <asp:Repeater ID="ArticlesRepeater" runat="server">
            <ItemTemplate>
                <div class="card">
                  <img class="card-image" src='<%# Eval("ThumbnailUrl") %>' alt='<%# Eval("Title") %>'>
                  <div class="card-body">
                    <div class="card-meta"><span>📝</span><span>By <%# Eval("AuthorName") %></span><span>·</span><span><%# Eval("PublishDate", "{0:MMM dd, yyyy}") %></span></div>
                    <h3><%# Eval("Title") %></h3>
                    <p><%# Eval("Content") %></p>
                  </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
      </div>
    </section>
</asp:Content>
