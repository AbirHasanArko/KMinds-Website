<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="KMinds.Portal.Web._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Hero -->
    <section class="hero">
      <div class="hero-bg" style="background-image: url('assets/images/hero-banner.png')"></div>
      <div class="hero-content">
        <h1>Unlock the Power of Data</h1>
        <p>KMinds is KUET's club for Data Science, Artificial Intelligence, and Machine Learning. Compete, research, and grow together.</p>
        <div class="badge-row">
          <span class="badge">🏆 Datathons</span>
          <span class="badge badge--accent">📊 Datasets</span>
          <span class="badge">🔬 Research</span>
          <span class="badge badge--accent">🎓 Workshops</span>
        </div>
        <div style="margin-top:1.5rem">
          <a href="Register.aspx" class="btn btn-primary">Join KMinds →</a>
        </div>
      </div>
    </section>

    <!-- Stats -->
    <div class="stats-row">
      <div class="stat-card">
        <div class="stat-number" id="stat-members">120+</div>
        <div class="stat-label">Active Members</div>
      </div>
      <div class="stat-card">
        <div class="stat-number">35+</div>
        <div class="stat-label">Research Papers</div>
      </div>
      <div class="stat-card">
        <div class="stat-number">50+</div>
        <div class="stat-label">Datasets Shared</div>
      </div>
      <div class="stat-card">
        <div class="stat-number">20+</div>
        <div class="stat-label">Events Hosted</div>
      </div>
    </div>

    <!-- What We Do -->
    <section>
      <h2>What We Do</h2>
      <div class="feature-grid">
        <div class="feature-item">
          <div class="feature-icon">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5"/></svg>
          </div>
          <div class="feature-text">
            <h3>Datathons & Competitions</h3>
            <p>Participate in Kaggle-style challenges and inter-university competitions.</p>
          </div>
        </div>
        <div class="feature-item">
          <div class="feature-icon">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M4 7V4h16v3M9 20h6M12 4v16"/></svg>
          </div>
          <div class="feature-text">
            <h3>Dataset Collection</h3>
            <p>Curate and share open datasets for real-world Bengali NLP, CV, and more.</p>
          </div>
        </div>
        <div class="feature-item">
          <div class="feature-icon">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="8"/><path d="M21 21l-4.35-4.35"/></svg>
          </div>
          <div class="feature-text">
            <h3>Research Discussions</h3>
            <p>Weekly paper reading sessions and collaborative research projects.</p>
          </div>
        </div>
        <div class="feature-item">
          <div class="feature-icon">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="4" width="18" height="18" rx="2"/><path d="M16 2v4M8 2v4M3 10h18"/></svg>
          </div>
          <div class="feature-text">
            <h3>Workshops & Events</h3>
            <p>Hands-on workshops on ML frameworks, cloud computing, and data engineering.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Recent Activity Preview -->
    <section>
      <h2>Recent Highlights</h2>
      <div class="card-grid">
        <div class="card">
          <img class="card-image" src="assets/images/event-datathon.png" alt="KMinds Datathon 2026">
          <div class="card-body">
            <div class="card-meta">
              <span>🏆</span><span>Event</span><span>·</span><span>May 15, 2026</span>
            </div>
            <h3>KMinds Datathon 2026</h3>
            <p>Annual inter-university data science competition with real-world problem sets.</p>
          </div>
        </div>
        <div class="card">
          <img class="card-image" src="assets/images/article-preview.png" alt="Intro to Data Preprocessing">
          <div class="card-body">
            <div class="card-meta">
              <span>📝</span><span>Article</span><span>·</span><span>By Member</span>
            </div>
            <h3>Intro to Data Preprocessing</h3>
            <p>A beginner-friendly guide to cleaning, transforming, and preparing datasets.</p>
          </div>
        </div>
        <div class="card">
          <img class="card-image" src="assets/images/research-preview.png" alt="ML for Medical Imaging">
          <div class="card-body">
            <div class="card-meta">
              <span>🔬</span><span>Research</span><span>·</span><span>Research Wing</span>
            </div>
            <h3>ML for Medical Image Analysis</h3>
            <p>Exploring deep learning approaches for automated medical image classification.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section style="text-align:center;padding:2.5rem 1.5rem">
      <h2>Ready to Begin?</h2>
      <p style="max-width:500px;margin:0 auto 1.25rem">Create an account with your KUET student email and complete payment verification to join.</p>
      <div style="display:flex;gap:0.75rem;justify-content:center;flex-wrap:wrap">
        <a href="Register.aspx" class="btn btn-primary">Create Account</a>
        <a href="Login.aspx" class="btn btn-secondary">Sign In</a>
      </div>
    </section>
</asp:Content>
