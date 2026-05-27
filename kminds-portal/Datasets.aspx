<%@ Page Title="Datasets" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Datasets.aspx.cs" Inherits="KMinds.Portal.Web.Datasets" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:PlaceHolder ID="UploadDatasetPlaceHolder" runat="server" Visible="false">
        <section aria-labelledby="upload-dataset-heading">
          <h2 id="upload-dataset-heading">Share Dataset</h2>
          <div class="form-container" style="display:flex;flex-direction:column;gap:1.25rem;">
            <asp:Label ID="DatasetMessage" runat="server" ForeColor="Green" Visible="false"></asp:Label>
            <div class="form-group">
              <label for="DatasetTitleTextBox">Dataset Title</label>
              <asp:TextBox ID="DatasetTitleTextBox" runat="server" placeholder="e.g. Bangla Sentiment Dataset" required="true"></asp:TextBox>
            </div>
            <div class="form-group">
              <label for="DatasetDescTextBox">Description</label>
              <asp:TextBox ID="DatasetDescTextBox" runat="server" TextMode="MultiLine" Rows="4" placeholder="Describe the dataset, its source, and use cases..." required="true"></asp:TextBox>
            </div>
            <div class="form-group">
              <label for="DatasetPreviewUpload">Preview Image (optional)</label>
              <div class="image-upload-area">
                <asp:FileUpload ID="DatasetPreviewUpload" runat="server" accept="image/*" />
                <div class="upload-icon">📈</div>
                <p>Upload a data visualization or sample preview</p>
              </div>
            </div>
            <div class="form-group">
              <label for="DatasetFileUpload">Dataset File</label>
              <asp:FileUpload ID="DatasetFileUpload" runat="server" required="true" />
            </div>
            <asp:Button ID="UploadDatasetButton" runat="server" Text="Upload Dataset" OnClick="UploadDatasetButton_Click" CssClass="btn btn-primary" Width="100%" />
          </div>
        </section>
    </asp:PlaceHolder>

    <section aria-labelledby="dataset-feed-heading">
      <h2 id="dataset-feed-heading">Shared Datasets</h2>
      <div class="card-grid" id="dataset-feed">
        <asp:Repeater ID="DatasetsRepeater" runat="server">
            <ItemTemplate>
                <div class="card">
                  <img class="card-image" src='<%# Eval("ThumbnailUrl") %>' alt='<%# Eval("Title") %>'>
                  <div class="card-body">
                    <div class="card-meta"><span>📊</span><span><%# Eval("Domain") %></span><span>·</span><span><%# Eval("Size") %></span></div>
                    <h3><%# Eval("Title") %></h3>
                    <p><%# Eval("Description") %></p>
                    <asp:HyperLink runat="server" NavigateUrl='<%# Eval("DownloadLink") %>' CssClass="btn btn-secondary btn-sm" style="margin-top:0.5rem">Download →</asp:HyperLink>
                  </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
      </div>
    </section>
</asp:Content>
