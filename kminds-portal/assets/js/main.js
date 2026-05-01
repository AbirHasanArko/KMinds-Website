import { bindBkashReferenceValidation, bindContentFormValidation, bindLoginValidation, bindMemberTableFilters, bindSignupValidation } from "./validation.js";
import { applyRoleVisibility, getStoredRole, initRoleSwitcher, initThemeSwitcher, showToast, initImagePreviews, populateProfileFromStorage, populateDashboardWelcome } from "./ui.js";

function initDemoActionButtons() {
  const actionButtons = document.querySelectorAll("button[data-action]");
  actionButtons.forEach((button) => {
    button.addEventListener("click", () => {
      const member = button.dataset.member || "member";
      const action = button.dataset.action || "update";
      const statusCell = button.closest("tr")?.querySelector("[data-cell='payment-status']");
      if (!statusCell) {
        return;
      }

      const statusMap = {
        approve: { text: "Approved", class: "status-approved" },
        reject: { text: "Rejected", class: "status-rejected" },
        revoke: { text: "Revoked", class: "status-revoked" }
      };

      const info = statusMap[action];
      if (info) {
        statusCell.innerHTML = `<span class="status ${info.class}">${info.text}</span>`;
        button.closest("tr").dataset.status = action === "approve" ? "approved" : action === "reject" ? "rejected" : "revoked";
      }

      const event = new CustomEvent("kminds:memberAction", {
        detail: { action, member }
      });
      window.dispatchEvent(event);
    });
  });
}

function initRoleExperience() {
  initRoleSwitcher();
  applyRoleVisibility(getStoredRole());
}

function initAnimatedCounters() {
  const counters = document.querySelectorAll(".stat-number");
  counters.forEach((el) => {
    const raw = el.textContent.trim();
    const match = raw.match(/^(\d+)/);
    if (!match) return;
    const target = parseInt(match[1], 10);
    const suffix = raw.replace(/^\d+/, "");
    let current = 0;
    const step = Math.max(1, Math.floor(target / 40));
    const interval = setInterval(() => {
      current += step;
      if (current >= target) {
        current = target;
        clearInterval(interval);
      }
      el.textContent = current + suffix;
    }, 30);
  });
}

document.addEventListener("DOMContentLoaded", () => {
  initThemeSwitcher();
  initRoleExperience();
  bindSignupValidation();
  bindLoginValidation();
  bindBkashReferenceValidation();
  bindContentFormValidation();
  bindMemberTableFilters();
  initDemoActionButtons();
  initImagePreviews();
  populateProfileFromStorage();
  populateDashboardWelcome();
  initAnimatedCounters();

  window.addEventListener("kminds:memberAction", (event) => {
    const member = event.detail?.member || "Member";
    const action = event.detail?.action || "updated";
    showToast(`${member} marked as ${action}.`);
  });
});
