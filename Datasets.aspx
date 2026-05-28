<%@ Page Title="Datasets" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Datasets.aspx.cs" Inherits="KMinds.Portal.Web.Datasets" %>

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
                <asp:FileUpload ID="ThumbnailUpload" runat="server" accept="image/*" />
                <div class="upload-icon">📷</div>
                <p>Click or drag thumbnail</p>
                <img class="image-preview" src="" alt="Thumbnail preview" />
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
        <asp:Repeater ID="DatasetsRepeater" runat="server" OnItemDataBound="DatasetsRepeater_ItemDataBound" OnItemCommand="DatasetsRepeater_ItemCommand">
            <ItemTemplate>
                <div class="card">
                  <img class="card-image" src='<%# Eval("ThumbnailUrl") %>' alt='<%# Eval("Title") %>'>
                  <div class="card-body">
                    <div class="card-meta">
                        <span>📊 <%# Eval("Domain") %></span>
                        <span>· <%# Eval("Size") %></span>
                        <div style="margin-left:auto;display:flex;gap:0.25rem;">
                            <asp:HyperLink ID="EditLink" runat="server" NavigateUrl='<%# "Datasets.aspx?edit_id=" + Eval("DatasetId") %>' Text="Edit" CssClass="btn btn-secondary" style="padding:0.2rem 0.5rem;font-size:0.75rem;" Visible="false"></asp:HyperLink>
                            <asp:LinkButton ID="DeleteBtn" runat="server" CommandName="Delete" CommandArgument='<%# Eval("DatasetId") %>' Text="Delete" CssClass="btn btn-secondary" style="padding:0.2rem 0.5rem;font-size:0.75rem;background-color:#ffebee;color:#c62828;border-color:#ffcdd2" Visible="false" OnClientClick="return confirm('Are you sure you want to delete this dataset?');"></asp:LinkButton>
                        </div>
                    </div>
                    <h3><%# Eval("Title") %></h3>
                    <p><%# Eval("Summary") %></p>
                    <div style="display:flex;gap:0.5rem;margin-top:0.5rem">
                      <button type="button" class="btn btn-secondary btn-sm" onclick="openDetailsModal('<%# HttpUtility.JavaScriptStringEncode(Eval("Title").ToString()) %>', this.parentNode.nextElementSibling.innerHTML, '<%# HttpUtility.JavaScriptStringEncode("📊 " + Eval("Domain") + " &middot; " + Eval("Size")) %>')">View Details</button>
                      <asp:HyperLink runat="server" NavigateUrl='<%# Eval("DownloadLink") %>' CssClass="btn btn-primary btn-sm">Download &rarr;</asp:HyperLink>
                    </div>
                    <div style="display:none;"><%# Eval("FullContent") %></div>
                  </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
      </div>
    </section>
</asp:Content>
