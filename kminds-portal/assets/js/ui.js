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
    setTimeout(() => toast.remove(), 180);
  }, 2400);
}

export function getStoredRole() {
  return localStorage.getItem(STORAGE_KEYS.role) || "member";
}

export function applyRoleVisibility(currentRole) {
  const role = ROLES.includes(currentRole) ? currentRole : "member";
  const roleBlocks = document.querySelectorAll("[data-role]");

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

  const wrapper = document.createElement("section");
  wrapper.id = "kminds-role-controls";
  wrapper.className = "kminds-role-controls";
  wrapper.innerHTML = `
    <p id="kminds-role-badge">Current role: ${getRoleLabel(getStoredRole())}</p>
    <label for="kminds-role-select">Role preview</label>
    <select id="kminds-role-select" aria-label="Role preview selector">
      <option value="member">Member</option>
      <option value="treasurer">Treasurer</option>
      <option value="general-secretary">General Secretary</option>
      <option value="vice-president">Vice-President</option>
      <option value="president">President</option>
    </select>
  `;

  const header = document.querySelector("header");
  if (header) {
    header.appendChild(wrapper);
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
