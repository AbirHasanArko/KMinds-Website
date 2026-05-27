<%@ Page Title="Events" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Events.aspx.cs" Inherits="KMinds.Portal.Web.Events" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Upcoming Events -->
    <section aria-labelledby="event-list-heading">
      <h2 id="event-list-heading">Upcoming Events</h2>
      <div class="card-grid" id="event-feed">
        <asp:Repeater ID="EventsRepeater" runat="server">
            <ItemTemplate>
                <div class="card">
                  <img class="card-image" src='<%# Eval("ImageUrl") %>' alt='<%# Eval("Title") %>'>
                  <div class="card-body">
                    <div class="card-meta">
                      <span>📅</span><span><%# Eval("EventDate", "{0:MMM dd, yyyy}") %></span><span>·</span><span><%# Eval("EventDate", "{0:hh:mm tt}") %></span>
                    </div>
                    <h3><%# Eval("Title") %></h3>
                    <p><%# Eval("Description") %></p>
                    <span class="badge" style="margin-top:0.5rem">📍 <%# Eval("Location") %></span>
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
                <asp:FileUpload ID="EventBannerUpload" runat="server" accept="image/*" />
                <div class="upload-icon">🎨</div>
                <p>Upload an event banner or poster</p>
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
