<%@ Page Title="Events" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Events.aspx.cs" Inherits="KMinds.Portal.Web.Events" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Upcoming Events -->
    <section aria-labelledby="event-list-heading">
      <h2 id="event-list-heading">Upcoming Events</h2>
      <div class="card-grid" id="event-feed">
        <asp:Repeater ID="EventsRepeater" runat="server" OnItemDataBound="EventsRepeater_ItemDataBound" OnItemCommand="EventsRepeater_ItemCommand">
            <ItemTemplate>
                <div class="card">
                  <img class="card-image" src='<%# Eval("ImageUrl") %>' alt='<%# Eval("Title") %>'>
                  <div class="card-body">
                    <div class="card-meta">
                      <span>📅</span><span><%# Eval("EventDate", "{0:MMM dd, yyyy}") %></span><span>·</span><span><%# Eval("EventDate", "{0:hh:mm tt}") %></span>
                      <div style="margin-left:auto;display:flex;gap:0.25rem;">
                          <asp:HyperLink ID="EditLink" runat="server" NavigateUrl='<%# "Events.aspx?edit_id=" + Eval("EventId") %>' Text="Edit" CssClass="btn btn-secondary" style="padding:0.2rem 0.5rem;font-size:0.75rem;" Visible="false"></asp:HyperLink>
                          <asp:LinkButton ID="DeleteBtn" runat="server" CommandName="Delete" CommandArgument='<%# Eval("EventId") %>' Text="Delete" CssClass="btn btn-secondary" style="padding:0.2rem 0.5rem;font-size:0.75rem;background-color:#ffebee;color:#c62828;border-color:#ffcdd2" Visible="false" OnClientClick="return confirm('Are you sure you want to delete this event?');"></asp:LinkButton>
                      </div>
                    </div>
                    <h3><%# Eval("Title") %></h3>
                    <p><%# Eval("Summary") %></p>
                    <div style="display:flex;align-items:center;justify-content:space-between;margin-top:0.75rem">
                      <span class="badge">📍 <%# Eval("Location") %></span>
                      <button type="button" class="btn btn-primary btn-sm" onclick="openDetailsModal('<%# HttpUtility.JavaScriptStringEncode(Eval("Title").ToString()) %>', this.parentNode.nextElementSibling.innerHTML, '<%# HttpUtility.JavaScriptStringEncode("📅 " + Eval("EventDate", "{0:MMM dd, yyyy} at {0:hh:mm tt}") + " &middot; 📍 " + Eval("Location")) %>')">Event Details &rarr;</button>
                    </div>
                    <div style="display:none;"><%# Eval("FullContent") %></div>
                  </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
      </div>
    </section>

    <!-- Create Event (Admin) -->
    <asp:PlaceHolder ID="AdminEventCreatePlaceHolder" runat="server" Visible="false">
        <section id="admin-event-create" aria-labelledby="event-create-heading">
          <h2 id="event-create-heading">Create Event</h2>
          <p style="margin-bottom:1rem">Create events and notify all members by email.</p>
          <div class="form-container" style="display:flex;flex-direction:column;gap:1.25rem;">
            <asp:Label ID="EventMessage" runat="server" ForeColor="Green" Visible="false"></asp:Label>
            <div class="form-group">
              <label for="EventTitleTextBox">Event Title</label>
              <asp:TextBox ID="EventTitleTextBox" runat="server" placeholder="Event name" required="true"></asp:TextBox>
            </div>
            <div class="form-group">
              <label for="EventDescTextBox">Description</label>
              <asp:TextBox ID="EventDescTextBox" runat="server" TextMode="MultiLine" Rows="4" placeholder="Describe the event..." required="true"></asp:TextBox>
            </div>
            <div class="form-group">
              <label for="EventBannerUpload">Event Banner (optional)</label>
              <div class="image-upload-area">
                <asp:FileUpload ID="EventImageUpload" runat="server" accept="image/*" />
                <div class="upload-icon">📷</div>
                <p>Click or drag event poster</p>
                <img class="image-preview" src="" alt="Event preview" />
              </div>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label for="EventDateTextBox">Date</label>
                <asp:TextBox ID="EventDateTextBox" runat="server" TextMode="Date" required="true"></asp:TextBox>
              </div>
              <div class="form-group">
                <label for="EventTimeTextBox">Time</label>
                <asp:TextBox ID="EventTimeTextBox" runat="server" TextMode="Time" required="true"></asp:TextBox>
              </div>
            </div>
            <div class="form-group">
              <label for="EventLocationTextBox">Location</label>
              <asp:TextBox ID="EventLocationTextBox" runat="server" placeholder="e.g. KUET CSE Lab or Online" required="true"></asp:TextBox>
            </div>
            <asp:Button ID="CreateEventButton" runat="server" Text="Create Event & Notify" OnClick="CreateEventButton_Click" CssClass="btn btn-primary" Width="100%" />
          </div>
        </section>
    </asp:PlaceHolder>
</asp:Content>
