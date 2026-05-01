export const ROLES = [
  "member",
  "treasurer",
  "general-secretary",
  "vice-president",
  "president"
];

export const ADMIN_ROLES = new Set([
  "president",
  "vice-president",
  "general-secretary",
  "treasurer"
]);

export const VALID_DEPARTMENTS = [
  "CSE",
  "EEE",
  "MSE",
  "CE",
  "ME",
  "URP",
  "BTE",
  "IPE",
  "LE",
  "ECE"
];

export const VALID_YEAR_TERMS = ["1-1", "1-2", "2-1", "2-2", "3-1", "3-2", "4-1", "4-2"];

export const STORAGE_KEYS = {
  role: "kminds_demo_role",
  user: "kminds_demo_user",
  paymentQueue: "kminds_payment_queue",
  theme: "kminds_theme"
};

export const KUET_STUDENT_EMAIL_REGEX = /^[^@\s]+@stud\.kuet\.ac\.bd$/i;
