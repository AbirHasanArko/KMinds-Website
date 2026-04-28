import {
  KUET_STUDENT_EMAIL_REGEX,
  STORAGE_KEYS,
  VALID_DEPARTMENTS,
  VALID_YEAR_TERMS
} from "./config.js";
import { showToast } from "./ui.js";

function isEmpty(value) {
  return !String(value || "").trim();
}

function setFieldError(input, message) {
  input.setCustomValidity(message);
  input.reportValidity();
}

function clearFieldError(input) {
  input.setCustomValidity("");
}

function savePendingPayment(email, reference) {
  const existing = JSON.parse(localStorage.getItem(STORAGE_KEYS.paymentQueue) || "[]");
  const entry = {
    id: Date.now(),
    email,
    bkashReference: reference,
    status: "pending"
  };
  existing.push(entry);
  localStorage.setItem(STORAGE_KEYS.paymentQueue, JSON.stringify(existing));
}

export function bindSignupValidation() {
  const fullNameInput = document.getElementById("full-name");
  const form = fullNameInput ? fullNameInput.closest("form") : null;
  if (!form) {
    return;
  }

  form.addEventListener("submit", (event) => {
    event.preventDefault();

    const email = form.querySelector("#email");
    const password = form.querySelector("#password");
    const confirmPassword = form.querySelector("#confirm-password");
    const department = form.querySelector("#department");
    const yearTerm = form.querySelector("#year-term");
    const fullName = form.querySelector("#full-name");
    const roll = form.querySelector("#roll");

    [email, password, confirmPassword, department, yearTerm, fullName, roll].forEach(clearFieldError);

    if (isEmpty(fullName.value) || isEmpty(roll.value)) {
      setFieldError(fullName, "Name and roll are required.");
      return;
    }

    if (!KUET_STUDENT_EMAIL_REGEX.test(email.value)) {
      setFieldError(email, "Use an email ending with @stud.kuet.ac.bd.");
      return;
    }

    if (!VALID_DEPARTMENTS.includes(department.value)) {
      setFieldError(department, "Please select a valid department.");
      return;
    }

    if (!VALID_YEAR_TERMS.includes(yearTerm.value)) {
      setFieldError(yearTerm, "Please select a valid year-term.");
      return;
    }

    if (password.value.length < 8) {
      setFieldError(password, "Password must be at least 8 characters long.");
      return;
    }

    if (password.value !== confirmPassword.value) {
      setFieldError(confirmPassword, "Passwords do not match.");
      return;
    }

    localStorage.setItem(
      STORAGE_KEYS.user,
      JSON.stringify({
        name: fullName.value.trim(),
        email: email.value.trim(),
        roll: roll.value.trim(),
        department: department.value,
        yearTerm: yearTerm.value
      })
    );

    form.reset();
    showToast("Sign up data validated and saved locally.");
  });
}

export function bindLoginValidation() {
  const loginEmailInput = document.getElementById("login-email");
  const form = loginEmailInput ? loginEmailInput.closest("form") : null;
  if (!form) {
    return;
  }

  form.addEventListener("submit", (event) => {
    event.preventDefault();

    const emailInput = form.querySelector("#login-email");
    const passwordInput = form.querySelector("#login-password");

    clearFieldError(emailInput);
    clearFieldError(passwordInput);

    if (!KUET_STUDENT_EMAIL_REGEX.test(emailInput.value)) {
      setFieldError(emailInput, "Use your @stud.kuet.ac.bd email.");
      return;
    }

    if (passwordInput.value.length < 8) {
      setFieldError(passwordInput, "Password must have at least 8 characters.");
      return;
    }

    showToast("Login validation passed. Redirecting to dashboard...");
    setTimeout(() => {
      window.location.href = "dashboard.html";
    }, 500);
  });
}

export function bindBkashReferenceValidation() {
  const bkashForms = Array.from(document.querySelectorAll("form")).filter((form) =>
    form.querySelector("input[name='bkash_reference']")
  );

  bkashForms.forEach((form) => {
    form.addEventListener("submit", (event) => {
      event.preventDefault();
      const refInput = form.querySelector("input[name='bkash_reference']");
      clearFieldError(refInput);

      const reference = refInput.value.trim();
      if (!/^[A-Za-z0-9]{6,30}$/.test(reference)) {
        setFieldError(refInput, "Enter a valid alphanumeric bKash reference (6-30 chars).");
        return;
      }

      const user = JSON.parse(localStorage.getItem(STORAGE_KEYS.user) || "{}");
      savePendingPayment(user.email || "unknown@stud.kuet.ac.bd", reference);
      refInput.value = "";
      showToast("bKash reference submitted for admin review.");
    });
  });
}

export function bindContentFormValidation() {
  const contentForms = Array.from(document.querySelectorAll("main section form"));
  contentForms.forEach((form) => {
    const textInputs = form.querySelectorAll("input[type='text'], input[type='url'], textarea");
    if (textInputs.length === 0) {
      return;
    }

    form.addEventListener("submit", (event) => {
      event.preventDefault();
      const empty = Array.from(textInputs).find((input) => isEmpty(input.value));
      if (empty) {
        setFieldError(empty, "This field is required.");
        return;
      }

      textInputs.forEach(clearFieldError);
      showToast("Form validated. Backend submission will be added in Phase 4.");
      form.reset();
    });
  });
}

export function bindMemberTableFilters() {
  const table = document.querySelector("#member-audit-table");
  const roleFilter = document.querySelector("#filter-role");
  const deptFilter = document.querySelector("#filter-department");
  const yearTermFilter = document.querySelector("#filter-year-term");
  const statusFilter = document.querySelector("#filter-status");

  if (!table || !roleFilter || !deptFilter || !yearTermFilter || !statusFilter) {
    return;
  }

  const rows = Array.from(table.querySelectorAll("tbody tr"));

  const applyFilters = () => {
    const activeRole = roleFilter.value;
    const activeDept = deptFilter.value;
    const activeYearTerm = yearTermFilter.value;
    const activeStatus = statusFilter.value;

    rows.forEach((row) => {
      const rowRole = row.dataset.role || "";
      const rowDept = row.dataset.department || "";
      const rowYearTerm = row.dataset.yearTerm || "";
      const rowStatus = row.dataset.status || "";

      const visible =
        (!activeRole || rowRole === activeRole) &&
        (!activeDept || rowDept === activeDept) &&
        (!activeYearTerm || rowYearTerm === activeYearTerm) &&
        (!activeStatus || rowStatus === activeStatus);

      row.hidden = !visible;
    });
  };

  [roleFilter, deptFilter, yearTermFilter, statusFilter].forEach((input) => {
    input.addEventListener("change", applyFilters);
  });
}
