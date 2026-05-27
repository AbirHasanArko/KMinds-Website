<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="KMinds.Portal.Web.Dashboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Welcome -->
    <section class="hero" style="padding:2rem 2rem">
      <div class="hero-bg" style="background-image:url('assets/images/hero-banner.png')"></div>
      <div class="hero-content">
        <h1 id="dashboard-welcome">Welcome back, <asp:Literal ID="UserNameLiteral" runat="server" Text="Member"></asp:Literal>!</h1>
        <p>Your KMinds command center — manage content, track events, and collaborate with fellow members.</p>
      </div>
    </section>

    <!-- Quick Stats -->
    <div class="stats-row">
      <div class="stat-card">
        <div class="stat-number" id="dash-articles"><asp:Literal ID="ArticlesCountLiteral" runat="server" Text="0"></asp:Literal></div>
        <div class="stat-label">Your Articles</div>
      </div>
      <div class="stat-card">
        <div class="stat-number" id="dash-research"><asp:Literal ID="ResearchCountLiteral" runat="server" Text="0"></asp:Literal></div>
        <div class="stat-label">Research Items</div>
      </div>
      <div class="stat-card">
        <div class="stat-number" id="dash-datasets"><asp:Literal ID="DatasetsCountLiteral" runat="server" Text="0"></asp:Literal></div>
        <div class="stat-label">Datasets Shared</div>
      </div>
      <div class="stat-card">
        <div class="stat-number" id="dash-events"><asp:Literal ID="EventsCountLiteral" runat="server" Text="0"></asp:Literal></div>
        <div class="stat-label">Upcoming Events</div>
      </div>
    </div>

    <!-- Member Actions -->
    <section id="member-actions" data-role="member">
      <h2>Quick Actions</h2>
      <div class="feature-grid">
        <a href="Articles.aspx" class="feature-item" style="text-decoration:none">
          <div class="feature-icon">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>
          </div>
          <div class="feature-text">
            <h3>Post Article</h3>
            <p>Share insights, tutorials, and competition walkthroughs.</p>
          </div>
        </a>
        <a href="Research.aspx" class="feature-item" style="text-decoration:none">
          <div class="feature-icon" style="background:var(--accent-dim);color:var(--accent)">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="8"/><path d="M21 21l-4.35-4.35"/></svg>
          </div>
          <div class="feature-text">
            <h3>Share Research</h3>
            <p>Discuss papers and contribute to collaborative projects.</p>
          </div>
        </a>
        <a href="Datasets.aspx" class="feature-item" style="text-decoration:none">
          <div class="feature-icon">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><ellipse cx="12" cy="5" rx="9" ry="3"/><path d="M21 12c0 1.66-4 3-9 3s-9-1.34-9-3"/><path d="M3 5v14c0 1.66 4 3 9 3s9-1.34 9-3V5"/></svg>
          </div>
          <div class="feature-text">
            <h3>Upload Dataset</h3>
            <p>Share curated datasets with the community.</p>
          </div>
        </a>
      </div>
    </section>

    <!-- Admin Actions -->
    <asp:PlaceHolder ID="AdminActionsPlaceHolder" runat="server" Visible="false">
        <section id="admin-actions">
          <h2>Admin Panel</h2>
          <div class="feature-grid">
            <a href="Events.aspx" class="feature-item" style="text-decoration:none">
              <div class="feature-icon" style="background:var(--accent-dim);color:var(--accent)">
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="4" width="18" height="18" rx="2"/><path d="M16 2v4M8 2v4M3 10h18"/></svg>
              </div>
              <div class="feature-text">
                <h3>Create Events</h3>
                <p>Schedule events and notify all members automatically.</p>
              </div>
            </a>
            <a href="Members.aspx" class="feature-item" style="text-decoration:none">
              <div class="feature-icon" style="background:var(--danger-dim);color:var(--danger)">
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 00-3-3.87M16 3.13a4 4 0 010 7.75"/></svg>
              </div>
              <div class="feature-text">
                <h3>Manage Members</h3>
                <p>Review payments, verify references, and audit activity.</p>
              </div>
            </a>
          </div>
        </section>
    </asp:PlaceHolder>

    <!-- Roles Info -->
    <section>
      <h2>Role Permissions</h2>
      <p style="margin-bottom:0.75rem">Permissions are enforced per role. Use the role switcher to preview different views.</p>
      <div class="card-grid" style="grid-template-columns:repeat(auto-fill,minmax(220px,1fr))">
        <div class="card" style="border-left:3px solid var(--brand)">
          <div class="card-body">
            <h3>President / VP / GS</h3>
            <p>Full access: post, audit, create events, verify payments, revoke members.</p>
          </div>
        </div>
        <div class="card" style="border-left:3px solid var(--accent)">
          <div class="card-body">
            <h3>Treasurer</h3>
            <p>Post content, manage finances, create events, verify payments.</p>
          </div>
        </div>
        <div class="card" style="border-left:3px solid var(--text-muted)">
          <div class="card-body">
            <h3>Member</h3>
            <p>Post articles, research items, and datasets after verification.</p>
          </div>
        </div>
      </div>
    </section>
</asp:Content>
