import { ROLES, STORAGE_KEYS } from "./config.js";

function getRoleLabel(role) {
  const labels = {
    "member": "Member",
    "treasurer": "Treasurer",
    "general-secretary": "General Secretary",
    "vice-president": "Vice-President",
    "president": "President"
  };
  return labels[role] || role;
}

export function showToast(message, type = "success") {
  let host = document.getElementById("kminds-toast-host");
  if (!host) {
    host = document.createElement("div");
    host.id = "kminds-toast-host";
    host.className = "kminds-toast-host";
    document.body.appendChild(host);
  }

  const toast = document.createElement("div");
  toast.className = `kminds-toast ${type === "error" ? "kminds-toast-error" : ""}`;
  toast.textContent = message;
  host.appendChild(toast);

  requestAnimationFrame(() => toast.classList.add("kminds-toast-visible"));

  setTimeout(() => {
    toast.classList.remove("kminds-toast-visible");
    setTimeout(() => toast.remove(), 200);
  }, 2800);
}

export function getStoredRole() {
  return localStorage.getItem(STORAGE_KEYS.role) || "member";
}

export function applyRoleVisibility(currentRole) {
  const role = ROLES.includes(currentRole) ? currentRole : "member";
  const roleBlocks = document.querySelectorAll("section[data-role], div[data-role]");

  roleBlocks.forEach((block) => {
    const allowedRoles = (block.dataset.role || "")
      .split(/\s+/)
      .filter(Boolean)
      .map((item) => item.toLowerCase());

    const isVisible = allowedRoles.length === 0 || allowedRoles.includes(role);
    block.classList.toggle("is-role-hidden", !isVisible);
    block.setAttribute("aria-hidden", String(!isVisible));
  });

  const roleBadge = document.getElementById("kminds-role-badge");
  if (roleBadge) {
    roleBadge.textContent = `Current role: ${getRoleLabel(role)}`;
  }
}

export function initRoleSwitcher() {
  if (document.getElementById("kminds-role-controls")) {
    return;
  }

  const wrapper = document.createElement("div");
  wrapper.id = "kminds-role-controls";
  wrapper.className = "kminds-role-controls";
  wrapper.innerHTML = `
    <p id="kminds-role-badge">Current role: ${getRoleLabel(getStoredRole())}</p>
    <label for="kminds-role-select" style="display:none">Role preview</label>
    <select id="kminds-role-select" aria-label="Role preview selector">
      <option value="member">Member</option>
      <option value="treasurer">Treasurer</option>
      <option value="general-secretary">General Secretary</option>
      <option value="vice-president">Vice-President</option>
      <option value="president">President</option>
    </select>
  `;

  const header = document.querySelector(".site-header .header-inner");
  if (header) {
    header.appendChild(wrapper);
  } else {
    const fallback = document.querySelector("header");
    if (fallback) fallback.appendChild(wrapper);
  }

  const select = document.getElementById("kminds-role-select");
  const initialRole = getStoredRole();
  select.value = ROLES.includes(initialRole) ? initialRole : "member";

  select.addEventListener("change", () => {
    localStorage.setItem(STORAGE_KEYS.role, select.value);
    applyRoleVisibility(select.value);
    showToast(`Preview role switched to ${getRoleLabel(select.value)}.`);
  });
}

export function initThemeSwitcher() {
  const toggleBtn = document.createElement("button");
  toggleBtn.id = "kminds-theme-toggle";
  toggleBtn.className = "btn btn-secondary btn-sm";
  toggleBtn.setAttribute("aria-label", "Toggle dark/light mode");

  const storedTheme = localStorage.getItem(STORAGE_KEYS.theme);
  const prefersLight = window.matchMedia("(prefers-color-scheme: light)").matches;
  const currentTheme = storedTheme || (prefersLight ? "light" : "dark");
  
  if (currentTheme === "light") {
    document.documentElement.setAttribute("data-theme", "light");
    toggleBtn.innerHTML = "🌙 Dark Mode";
  } else {
    toggleBtn.innerHTML = "☀️ Light Mode";
  }

  toggleBtn.addEventListener("click", () => {
    const isLight = document.documentElement.getAttribute("data-theme") === "light";
    const newTheme = isLight ? "dark" : "light";
    
    if (newTheme === "light") {
      document.documentElement.setAttribute("data-theme", "light");
    } else {
      document.documentElement.removeAttribute("data-theme");
    }
    
    localStorage.setItem(STORAGE_KEYS.theme, newTheme);
    toggleBtn.innerHTML = newTheme === "light" ? "🌙 Dark Mode" : "☀️ Light Mode";
    showToast(`${newTheme === "light" ? "Light" : "Dark"} mode enabled.`);
  });

  const nav = document.querySelector(".site-header nav ul");
  if (nav) {
    const li = document.createElement("li");
    li.style.marginLeft = "0.5rem";
    li.appendChild(toggleBtn);
    nav.appendChild(li);
  }
}

/**
 * Image preview: attach change listeners to all file inputs inside .image-upload-area
 */
export function initImagePreviews() {
  const uploadAreas = document.querySelectorAll(".image-upload-area");

  uploadAreas.forEach((area) => {
    const fileInput = area.querySelector("input[type='file']");
    const preview = area.querySelector(".image-preview");
    if (!fileInput || !preview) return;

    fileInput.addEventListener("change", () => {
      const file = fileInput.files[0];
      if (!file) {
        preview.classList.remove("visible");
        preview.src = "";
        return;
      }

      if (!file.type.startsWith("image/")) {
        showToast("Please select a valid image file.", "error");
        fileInput.value = "";
        return;
      }

      if (file.size > 10 * 1024 * 1024) {
        showToast("Image size must be under 10 MB.", "error");
        fileInput.value = "";
        return;
      }

      const reader = new FileReader();
      reader.onload = (e) => {
        preview.src = e.target.result;
        preview.classList.add("visible");
        // Hide the placeholder text
        const icon = area.querySelector(".upload-icon");
        const text = area.querySelector("p");
        if (icon) icon.style.display = "none";
        if (text) text.style.display = "none";
      };
      reader.readAsDataURL(file);
    });

    // Drag and drop support
    area.addEventListener("dragover", (e) => {
      e.preventDefault();
      area.style.borderColor = "var(--brand)";
      area.style.background = "var(--brand-dim)";
    });

    area.addEventListener("dragleave", () => {
      area.style.borderColor = "";
      area.style.background = "";
    });

    area.addEventListener("drop", (e) => {
      e.preventDefault();
      area.style.borderColor = "";
      area.style.background = "";
      if (e.dataTransfer.files.length > 0) {
        fileInput.files = e.dataTransfer.files;
        fileInput.dispatchEvent(new Event("change"));
      }
    });
  });
}

/**
 * Populate the profile page from localStorage data
 */
export function populateProfileFromStorage() {
  const userRaw = localStorage.getItem(STORAGE_KEYS.user);
  if (!userRaw) return;

  try {
    const user = JSON.parse(userRaw);

    const fields = {
      "profile-email": user.email,
      "profile-dept": user.department,
      "profile-year-term": user.yearTerm,
      "profile-roll": user.roll,
    };

    for (const [id, value] of Object.entries(fields)) {
      const el = document.getElementById(id);
      if (el && value) el.textContent = value;
    }

    // Update name in heading
    const heading = document.getElementById("account-heading");
    if (heading && user.name) heading.textContent = user.name;

    // Update avatar initials
    const initialsEl = document.getElementById("avatar-initials");
    if (initialsEl && user.name) {
      const parts = user.name.trim().split(/\s+/);
      const initials = parts.length >= 2
        ? (parts[0][0] + parts[parts.length - 1][0]).toUpperCase()
        : parts[0].substring(0, 2).toUpperCase();
      initialsEl.textContent = initials;
    }

    // Update role from switcher
    const roleEl = document.getElementById("profile-role");
    if (roleEl) {
      roleEl.textContent = getRoleLabel(getStoredRole());
    }
  } catch (e) {
    // silently fail
  }
}

/**
 * Populate the dashboard welcome message from localStorage
 */
export function populateDashboardWelcome() {
  const welcome = document.getElementById("dashboard-welcome");
  if (!welcome) return;

  const userRaw = localStorage.getItem(STORAGE_KEYS.user);
  if (userRaw) {
    try {
      const user = JSON.parse(userRaw);
      if (user.name) {
        welcome.textContent = `Welcome back, ${user.name.split(" ")[0]}!`;
      }
    } catch (e) {
      // fallback
    }
  }
}
