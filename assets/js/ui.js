import { STORAGE_KEYS } from "./config.js";

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


