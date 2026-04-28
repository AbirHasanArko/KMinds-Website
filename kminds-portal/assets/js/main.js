import { bindBkashReferenceValidation, bindContentFormValidation, bindLoginValidation, bindMemberTableFilters, bindSignupValidation } from "./validation.js";
import { applyRoleVisibility, getStoredRole, initRoleSwitcher, showToast } from "./ui.js";

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

      if (action === "approve") {
        statusCell.textContent = "Approved";
        button.closest("tr").dataset.status = "approved";
      } else if (action === "reject") {
        statusCell.textContent = "Rejected";
        button.closest("tr").dataset.status = "rejected";
      } else if (action === "revoke") {
        statusCell.textContent = "Revoked";
        button.closest("tr").dataset.status = "revoked";
      }

      const event = new CustomEvent("kminds:memberAction", {
        detail: {
          action,
          member
        }
      });
      window.dispatchEvent(event);
    });
  });
}

function initRoleExperience() {
  initRoleSwitcher();
  applyRoleVisibility(getStoredRole());
}

document.addEventListener("DOMContentLoaded", () => {
  initRoleExperience();
  bindSignupValidation();
  bindLoginValidation();
  bindBkashReferenceValidation();
  bindContentFormValidation();
  bindMemberTableFilters();
  initDemoActionButtons();

  window.addEventListener("kminds:memberAction", (event) => {
    const member = event.detail?.member || "Member";
    const action = event.detail?.action || "updated";
    showToast(`${member} marked as ${action}.`);
  });
});
