# KMinds Portal — Frontend Documentation

> **Prepared for:** Showcase Interview Prep  
> **Tech Stack:** Pure HTML5, CSS3, Vanilla JavaScript (ES Modules)  
> **No frameworks, no build tools — everything runs as static files.**

---

## 1. Project Overview

KMinds is the official portal for **KUET's Data Science, AI & Machine Learning club**. It lets members sign up, log in, post articles/research/datasets, view events, and allows admins (President, VP, General Secretary, Treasurer) to manage members and verify payments via bKash.

### Key Design Decisions
- **No backend** — all data is stored in `localStorage` for demo purposes. The architecture is designed so a backend (e.g. Node/Express + MongoDB) can be plugged in at Phase 4.
- **No frameworks** — pure vanilla JS with ES Modules (`import`/`export`), proving strong fundamentals.
- **Dynamic Theme System** — dark-mode glassmorphism by default, with a fully integrated light-mode toggle using CSS custom properties.
- **Role-Based Access Control (RBAC)** — sections are shown/hidden client-side based on a role switcher stored in `localStorage`.

---

## 2. File Structure

```
kminds-portal/
├── index.html              ← Landing page (public)
├── login.html              ← Login + bKash payment form
├── signup.html             ← Registration form
├── dashboard.html          ← Member/Admin hub
├── profile.html            ← User profile page
├── events.html             ← Event listing + admin event creation
├── members.html            ← Admin member audit table
├── article-list.html       ← Article creation + feed
├── research-list.html      ← Research submission + feed
├── dataset-list.html       ← Dataset upload + feed
└── assets/
    ├── css/
    │   └── main.css         ← Single stylesheet (design system)
    ├── js/
    │   ├── config.js        ← Constants & configuration
    │   ├── main.js          ← Entry point, bootstraps everything
    │   ├── ui.js            ← UI utilities (toast, role switcher, image preview)
    │   └── validation.js    ← Form validation logic
    └── images/
        ├── hero-banner.png
        ├── event-datathon.png
        ├── event-workshop.png
        ├── article-preview.png
        ├── research-preview.png
        └── dataset-preview.png
```

---

## 3. Page-by-Page Breakdown

### 3.1 `index.html` — Landing Page
**Purpose:** Public-facing homepage to attract new members.

**Sections:**
| Section | What It Shows |
|---------|--------------|
| **Hero** | Full-width banner with background image, gradient title, badge pills (Datathons, Datasets, Research, Workshops), CTA button |
| **Stats Row** | 4 animated stat cards (120+ Members, 35+ Papers, 50+ Datasets, 20+ Events) with counting animation |
| **What We Do** | 4-column feature grid with inline SVG icons |
| **Recent Highlights** | 3-card grid with images, metadata, and descriptions |
| **CTA** | "Ready to Begin?" section with Sign Up / Sign In buttons |

**Interview Talking Point:** The stats use an **animated counter** (`initAnimatedCounters()` in `main.js`) that increments numbers from 0 to the target value using `setInterval`, parsing the numeric portion and preserving suffixes like `+`.

---

### 3.2 `login.html` — Authentication
**Purpose:** Email + password login, plus bKash payment reference submission.

**Key Features:**
- Email is validated against `@stud.kuet.ac.bd` using regex
- Password requires minimum 8 characters
- On successful validation, redirects to `dashboard.html` after 500ms
- Separate bKash form validates reference is alphanumeric, 6-30 chars
- Payment reference is saved to `localStorage` under `kminds_payment_queue`

---

### 3.3 `signup.html` — Registration
**Purpose:** New member registration with profile photo upload.

**Form Fields:** Full Name, Roll Number, Email (pattern-validated), Department (select), Year-Term (select), Profile Photo (drag-and-drop upload area), Password + Confirm Password.

**Validation Chain (in order):**
1. Name and Roll not empty
2. Email matches `@stud.kuet.ac.bd` regex
3. Department is in the valid list
4. Year-Term is valid
5. Password ≥ 8 characters
6. Password === Confirm Password

**On success:** User data is serialized to JSON and stored in `localStorage` under key `kminds_demo_user`.

---

### 3.4 `dashboard.html` — Member Hub
**Purpose:** Central navigation hub after login.

**Sections:**
- **Welcome Hero** — personalized greeting (pulls first name from `localStorage`)
- **Quick Stats** — articles, research, datasets, upcoming events
- **Quick Actions** (`data-role="member"`) — links to Post Article, Share Research, Upload Dataset
- **Admin Panel** (`data-role="president vice-president general-secretary treasurer"`) — links to Create Events, Manage Members. **Hidden for regular members** via RBAC.
- **Role Permissions** — 3-card info grid explaining permission tiers

---

### 3.5 `events.html` — Events
**Sections:**
- **Upcoming Events** — card grid with event images, dates, descriptions, location badges
- **Create Event** (admin-only, `data-role` restricted) — form with title, description, banner image upload, date, time, location

---

### 3.6 `members.html` — Admin Audit Panel
**Purpose:** Payment verification queue for admin roles only.

**Features:**
- **Filter Row** — dropdowns for Role, Department, Year-Term, Status
- **Audit Table** — columns: Member, Email, Role, Dept, Year, bKash Ref, Status, Actions
- **Action Buttons** — ✓ Approve, ✗ Reject, ⊘ Revoke — each updates the status badge in-place and fires a `CustomEvent`
- **Revocation Policy** — info card about unpaid member suspension

The entire audit section has `data-role="president vice-president general-secretary treasurer"` so it's hidden from regular members.

---

### 3.7 `profile.html` — User Profile
- Avatar with dynamic initials (computed from stored name)
- Definition list (`<dl>`) showing Email, Department, Year-Term, Roll, Role
- Payment status section with bKash reference submission form
- Posting privileges info card

All fields are populated from `localStorage` by `populateProfileFromStorage()`.

---

### 3.8 Content Pages (Articles, Research, Datasets)
Each follows the same pattern:
1. **Create/Submit Form** — with title, content/abstract, optional image upload, and a submit button
2. **Feed Section** — card grid with placeholder content cards showing images, metadata, titles, descriptions

---

## 4. CSS Architecture (`main.css`)

### 4.1 Design Tokens (CSS Custom Properties)

```css
:root {
  --bg-base: #0a0e1a;           /* Deep navy background */
  --bg-surface: rgba(15,20,35,0.85);
  --bg-card: rgba(20,28,50,0.65);  /* Card backgrounds */
  --bg-glass: rgba(25,35,60,0.45); /* Glassmorphism panels */
  --brand: #00d4aa;              /* Primary teal/green */
  --accent: #f0a030;             /* Secondary amber */
  --danger: #ff4d6a;             /* Red for errors/reject */
  --success: #2dd4a0;
  --border: rgba(255,255,255,0.06);
  --transition: 250ms cubic-bezier(.4,0,.2,1);
}
```

**Interview Point:** All colors, shadows, radii, and transitions are centralized as CSS custom properties. This means changing the entire theme requires editing only the `:root` block.

### 4.2 Glassmorphism Technique
```css
.site-header {
  background: var(--bg-glass);          /* semi-transparent */
  backdrop-filter: blur(20px) saturate(1.4);  /* frosted glass */
  border: 1px solid var(--border);      /* subtle white border */
}
```
The header, footer, and section cards all use this pattern. The `backdrop-filter` blurs content behind the element, creating a frosted-glass effect.

### 4.3 Gradient Text
```css
h2 {
  background: linear-gradient(135deg, var(--brand), #60e8cc);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}
```
This clips a gradient to the text shape, making headings visually striking.

### 4.4 Layout System
- **Max-width container:** `width: min(var(--max-w), 94vw)` with auto margins — responsive without media queries
- **CSS Grid** for main content (`display: grid; gap: 1.25rem`)
- **Card grids:** `grid-template-columns: repeat(auto-fill, minmax(300px, 1fr))` — auto-responsive columns
- **Flexbox** for header, nav, filter rows, button groups

### 4.5 Responsive Design (`@media max-width: 768px`)
- Header stacks vertically
- Form rows collapse to single column
- **Table transforms to card layout** — `thead` is hidden, each `<td>` becomes a block with a pseudo-element label:
```css
td::before {
  content: attr(data-label);  /* reads from data-label attribute */
}
```
This is a well-known responsive table technique that avoids JavaScript.

### 4.6 Animations
Three `@keyframes` defined:
- **`fadeUp`** — elements slide up 16px while fading in (used on sections, cards)
- **`fadeDown`** — header slides down 12px while fading in
- **`pulse`** — opacity oscillation (available for loading states)

Staggered delays on sections: `section:nth-child(2) { animation-delay: 80ms }` etc.

---

## 5. JavaScript Architecture

### 5.1 Module System
Uses native **ES Modules** (`type="module"` on script tags). Four files:

```
config.js  ← Constants (imported by validation.js and ui.js)
ui.js      ← UI components (imported by main.js)
validation.js ← Form handlers (imported by main.js)
main.js    ← Entry point (imports and orchestrates everything)
```

### 5.2 `config.js` — Constants

| Export | Purpose |
|--------|---------|
| `ROLES` | Array: `["member", "treasurer", "general-secretary", "vice-president", "president"]` |
| `ADMIN_ROLES` | Set of 4 admin role strings |
| `VALID_DEPARTMENTS` | Array of 16 KUET department codes |
| `VALID_YEAR_TERMS` | Array: `["1-1", "1-2", ..., "4-2"]` |
| `STORAGE_KEYS` | Object with localStorage key names, including `kminds_theme` |
| `KUET_STUDENT_EMAIL_REGEX` | `/^[^@\s]+@stud\.kuet\.ac\.bd$/i` |

### 5.3 `main.js` — Entry Point

On `DOMContentLoaded`, initializes everything:

```javascript
document.addEventListener("DOMContentLoaded", () => {
  initRoleExperience();       // Role switcher + RBAC
  bindSignupValidation();     // Signup form
  bindLoginValidation();      // Login form
  bindBkashReferenceValidation(); // bKash forms
  bindContentFormValidation();    // Article/Research/Dataset forms
  bindMemberTableFilters();   // Member table dropdowns
  initDemoActionButtons();    // Approve/Reject/Revoke buttons
  initImagePreviews();        // Drag-and-drop image uploads
  populateProfileFromStorage(); // Fill profile page
  populateDashboardWelcome(); // Personalize dashboard greeting
  initAnimatedCounters();     // Stat number animations
});
```

**Interview Point:** Each function uses **guard clauses** — it checks if the relevant DOM elements exist before proceeding. This means `main.js` is loaded on every page but only the relevant features activate. No errors on pages that don't have certain elements.

#### Key Feature: Demo Action Buttons
```javascript
function initDemoActionButtons() {
  // Finds all buttons with data-action attribute
  // On click: updates the status badge in the same row
  // Fires a CustomEvent "kminds:memberAction"
}
```
The `CustomEvent` pattern decouples the action from the notification:
```javascript
window.addEventListener("kminds:memberAction", (event) => {
  showToast(`${event.detail.member} marked as ${event.detail.action}.`);
});
```

#### Animated Counters
```javascript
function initAnimatedCounters() {
  // Parses "120+" → target=120, suffix="+"
  // Uses setInterval with step = target/40
  // Increments every 30ms until target reached
}
```

### 5.4 `ui.js` — UI Utilities

#### Toast Notifications
```javascript
export function showToast(message, type = "success") {
  // Creates a toast-host container (fixed bottom-right)
  // Appends a toast div with the message
  // Uses requestAnimationFrame for smooth fade-in
  // Auto-removes after 2800ms with fade-out
}
```
**Interview Point:** `requestAnimationFrame` ensures the CSS transition triggers properly by waiting one frame after DOM insertion before adding the visible class.

#### Role-Based Access Control (RBAC)
```javascript
export function applyRoleVisibility(currentRole) {
  // Finds all elements with data-role attribute
  // Parses space-separated allowed roles
  // Toggles .is-role-hidden class + aria-hidden attribute
}
```
HTML sections declare who can see them:
```html
<section data-role="president vice-president general-secretary treasurer">
```
If the current role isn't in that list, the section gets `display: none`.

#### Role Switcher
`initRoleSwitcher()` dynamically creates a `<select>` dropdown in the header. On change:
1. Saves selected role to `localStorage`
2. Calls `applyRoleVisibility()` to show/hide sections
3. Shows a toast notification

#### Theme Switcher
`initThemeSwitcher()` adds a toggle button to the main navigation:
1. Reads initial theme preference from `localStorage` or OS settings (`window.matchMedia`).
2. Applies a `[data-theme="light"]` attribute to the `<html>` root for light mode.
3. Swaps CSS variables for colors and gradients in `main.css`.
4. Saves user preference to `localStorage`.

#### Image Preview
`initImagePreviews()` attaches to all `.image-upload-area` elements:
- **File validation** — checks `file.type.startsWith("image/")` and size < 10MB
- **FileReader** — reads file as DataURL and sets it as `<img>` src
- **Drag & Drop** — handles `dragover`, `dragleave`, `drop` events with visual feedback

#### Profile Population
`populateProfileFromStorage()` reads `kminds_demo_user` from localStorage and fills:
- Email, Department, Year-Term, Roll fields
- Name heading
- Avatar initials (computed from first + last name initial)
- Role from the role switcher

### 5.5 `validation.js` — Form Validation

#### Helper Functions
- `isEmpty(value)` — trims and checks for empty strings
- `setFieldError(input, message)` — uses `setCustomValidity()` + `reportValidity()` (native browser validation UI)
- `clearFieldError(input)` — resets custom validity

#### Signup Validation (`bindSignupValidation`)
Validates in sequence: name/roll → email regex → department → year-term → password length → password match. On success, saves user to `localStorage`.

#### Login Validation (`bindLoginValidation`)
Validates email format and password length. On success, shows toast and redirects to `dashboard.html` after 500ms timeout.

#### bKash Reference Validation (`bindBkashReferenceValidation`)
Finds all forms containing `input[name='bkash_reference']`. Validates the reference is alphanumeric, 6-30 characters. Saves to a payment queue array in `localStorage`.

#### Content Form Validation (`bindContentFormValidation`)
Generic handler for article/research/dataset forms. Checks all text/URL/textarea inputs are non-empty. Shows "Backend submission will be added in Phase 4" toast.

#### Member Table Filters (`bindMemberTableFilters`)
Attaches `change` listeners to 4 filter dropdowns. On change, iterates all `<tbody tr>` elements and compares their `data-*` attributes against selected filter values. Rows that don't match are set to `hidden`.

---

## 6. Data Flow Diagram

```
┌─────────────┐     localStorage      ┌──────────────┐
│  signup.html │ ──── user data ─────→ │ profile.html │
│  (form)      │     kminds_demo_user  │ (populated)  │
└─────────────┘                        └──────────────┘
                                              │
┌─────────────┐     localStorage      ┌───────┴──────┐
│  login.html  │ ── payment queue ──→  │ members.html │
│  (bKash form)│   kminds_payment_queue│ (audit table)│
└─────────────┘                        └──────────────┘

┌─────────────┐     localStorage      ┌──────────────┐
│ Role Switcher│ ── kminds_demo_role → │  All Pages   │
│ (in header)  │                       │ (RBAC filter)│
└─────────────┘                        └──────────────┘
```

---

## 7. Accessibility Features

| Feature | Implementation |
|---------|---------------|
| **Semantic HTML** | `<header>`, `<main>`, `<footer>`, `<nav>`, `<section>` |
| **ARIA labels** | `aria-label` on all `<nav>` elements |
| **ARIA current** | `aria-current="page"` on active nav links |
| **ARIA labelledby** | Sections linked to their headings via `aria-labelledby` |
| **ARIA hidden** | Role-hidden sections get `aria-hidden="true"` |
| **Form labels** | Every `<input>` has an associated `<label>` with matching `for`/`id` |
| **Alt text** | All `<img>` tags have descriptive `alt` attributes |

---

## 8. SEO Features

- Unique `<title>` per page (e.g., "KMinds | Dashboard")
- `<meta name="description">` on every page
- Single `<h1>` per page
- Semantic heading hierarchy (h1 → h2 → h3)
- `lang="en"` on `<html>`

---

## 9. Common Interview Questions & Answers

### Q: "Why no framework like React?"
**A:** This is a frontend prototype (Phase 1-3). Using vanilla JS proves strong fundamentals — DOM manipulation, ES Modules, event delegation, and the Observer pattern (CustomEvent). A framework would be adopted when component complexity justifies it in Phase 4.

### Q: "How does the role-based access work?"
**A:** Each protected `<section>` has a `data-role` attribute listing allowed roles. On page load, `applyRoleVisibility()` reads the current role from `localStorage`, compares it against each section's allowed roles, and toggles a `.is-role-hidden` CSS class (`display: none !important`). It also sets `aria-hidden` for screen readers.

### Q: "How would you connect this to a real backend?"
**A:** The forms already use `action="#"` and `method="post"` attributes. The validation functions call `event.preventDefault()` and could be modified to use `fetch()` with the validated `FormData`. The `localStorage` calls would be replaced with API responses. The RBAC would move server-side with JWT tokens.

### Q: "Explain the glassmorphism effect."
**A:** It's achieved with three CSS properties: a semi-transparent `background` (using `rgba`), `backdrop-filter: blur(20px)` which blurs content behind the element, and a subtle `border` with low-opacity white. The `saturate(1.4)` boost makes colors behind the glass more vibrant.

### Q: "How does the responsive table work without JS?"
**A:** At `768px`, the table's `display` switches to `block` for all elements. `<thead>` is visually hidden with `position: absolute; left: -9999px`. Each `<td>` uses a CSS pseudo-element (`::before`) that reads from the `data-label` attribute to display inline labels, effectively turning each row into a stacked card.

### Q: "How does the Dark/Light mode toggle work without duplicate CSS?"
**A:** The entire design system is built on CSS custom properties (variables) defined in `:root`. When the user switches to light mode, JavaScript applies a `[data-theme="light"]` attribute to the `<html>` element. The CSS then simply redefines those specific variables under `[data-theme="light"]`, automatically updating the entire UI without needing separate stylesheets.

### Q: "Why ES Modules instead of bundling?"
**A:** Modern browsers natively support `type="module"` scripts. This gives us `import`/`export` without Webpack/Vite overhead. Each file has a single responsibility (config, UI, validation, orchestration), making the code maintainable and testable.

### Q: "How does the toast notification work?"
**A:** `showToast()` creates a `<div>` element, appends it to a fixed-position host container, then uses `requestAnimationFrame` to add a CSS class that triggers a `transform + opacity` transition. After 2800ms, the class is removed and after the 200ms exit animation completes, the DOM element is removed. This ensures no memory leaks.

---

## 10. Technical Vocabulary Cheat Sheet

| Term | Where It's Used |
|------|----------------|
| **CSS Custom Properties** | `:root` design tokens |
| **Glassmorphism** | Header, footer, section cards |
| **Gradient text clipping** | All `<h2>` headings, logo |
| **CSS Grid** | Main layout, card grids, stat rows, definition lists |
| **Flexbox** | Header, nav, badge rows, button groups |
| **ES Modules** | `import`/`export` across 4 JS files |
| **Custom Events** | `kminds:memberAction` for member actions |
| **Guard clauses** | Every init function checks if DOM elements exist |
| **setCustomValidity** | Native browser form validation API |
| **FileReader API** | Image preview with `readAsDataURL` |
| **Drag and Drop API** | Image upload areas |
| **localStorage** | Persistent client-side data storage |
| **requestAnimationFrame** | Smooth toast animations |
| **Responsive breakpoints** | Single breakpoint at 768px |
| **data-* attributes** | Role filtering, table labels, member actions |
