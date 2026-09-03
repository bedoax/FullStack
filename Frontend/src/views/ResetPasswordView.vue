<script setup>
import { reactive } from "vue";
import { RouterLink } from "vue-router";
import { useAuthStore } from "@/stores/authStore";
import { useResetPassword } from "@/composables/useResetPassword";

const authStore = useAuthStore();
const { resetPassword, isLoading, serverError } = useResetPassword();

const form = reactive({
  otp: "",
  newPassword: "",
  confirmPassword: "",
});

const errors = reactive({
  otp: "",
  newPassword: "",
  confirmPassword: "",
});

function validateForm() {
  errors.otp = "";
  errors.newPassword = "";
  errors.confirmPassword = "";

  let isValid = true;

  if (!authStore.resetEmail) {
    errors.otp = "Reset session expired. Please request a new code.";
    return false;
  }

  if (!form.otp) {
    errors.otp = "Verification code is required.";
    isValid = false;
  } else if (!/^\d{6}$/.test(form.otp)) {
    errors.otp = "Verification code must be 6 digits.";
    isValid = false;
  }

  if (!form.newPassword) {
    errors.newPassword = "New password is required.";
    isValid = false;
  } else if (form.newPassword.length < 6) {
    errors.newPassword = "Password must be at least 6 characters.";
    isValid = false;
  }

  if (!form.confirmPassword) {
    errors.confirmPassword = "Confirm password is required.";
    isValid = false;
  } else if (form.confirmPassword !== form.newPassword) {
    errors.confirmPassword = "Passwords do not match.";
    isValid = false;
  }

  return isValid;
}

async function handleSubmit() {
  if (!validateForm()) return;

  await resetPassword({
    email: authStore.resetEmail,
    otp: form.otp,
    newPassword: form.newPassword,
  });
}
</script>

<template>
  <div class="reset-container">
    <form @submit.prevent="handleSubmit" class="form-container">
      <h2>Reset Password</h2>

      <p class="description">
        We've sent a 6-digit verification code to
        <strong>{{ authStore.resetEmail }}</strong>
      </p>

      <div v-if="serverError" class="server-error">
        {{ serverError }}
      </div>

      <div class="field">
        <label>Verification Code</label>

        <input
          type="text"
          inputmode="numeric"
          maxlength="6"
          v-model="form.otp"
          @input="form.otp = form.otp.replace(/\D/g, '')"
          placeholder="Enter 6-digit code"
        />

        <span class="error" v-if="errors.otp">
          {{ errors.otp }}
        </span>
      </div>

      <div class="field">
        <label>New Password</label>

        <input
          type="password"
          v-model="form.newPassword"
          placeholder="Enter new password"
        />

        <span class="error" v-if="errors.newPassword">
          {{ errors.newPassword }}
        </span>
      </div>

      <div class="field">
        <label>Confirm Password</label>

        <input
          type="password"
          v-model="form.confirmPassword"
          placeholder="Confirm password"
        />

        <span class="error" v-if="errors.confirmPassword">
          {{ errors.confirmPassword }}
        </span>
      </div>

      <button type="submit" :disabled="isLoading">
        {{ isLoading ? "Updating Password..." : "Reset Password" }}
      </button>

      <p class="login-link">
        Remember your password?

        <RouterLink to="/login"> Login </RouterLink>
      </p>
    </form>
  </div>
</template>

<style scoped>
.reset-container {
  min-height: 100vh;

  display: flex;

  justify-content: center;

  align-items: center;

  background: var(--background);
}

.form-container {
  width: 100%;

  max-width: 420px;

  padding: 32px;

  display: flex;

  flex-direction: column;

  gap: 16px;

  background: var(--surface);

  border: 1px solid var(--border);

  border-radius: 16px;

  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
}

h2 {
  margin: 0;

  text-align: center;

  color: var(--text-primary);
}

.description {
  margin: 0;

  text-align: center;

  font-size: 0.9rem;

  line-height: 1.5;

  color: var(--text-secondary);
}

.field {
  display: flex;

  flex-direction: column;

  gap: 6px;
}

input {
  padding: 12px 14px;

  border: 1px solid var(--border);

  border-radius: 8px;

  background: var(--surface);

  color: var(--text-primary);

  font-size: 15px;
}

input:focus {
  outline: none;

  border-color: var(--primary);

  box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.15);
}

button {
  margin-top: 8px;

  padding: 13px;

  border: none;

  border-radius: 8px;

  background: var(--primary);

  color: white;

  font-size: 15px;

  font-weight: 600;

  cursor: pointer;

  transition: 0.25s;
}

button:disabled {
  opacity: 0.6;

  cursor: not-allowed;
}

button:hover:not(:disabled) {
  background: var(--primary-hover);
}

.error {
  color: var(--danger);

  font-size: 0.82rem;
}

.server-error {
  padding: 10px;

  border-radius: 8px;

  text-align: center;

  background: rgba(239, 68, 68, 0.12);

  color: var(--danger);
}

.login-link {
  text-align: center;

  font-size: 0.9rem;

  color: var(--text-secondary);
}

.login-link a {
  color: var(--primary);

  text-decoration: none;

  font-weight: 600;
}

.login-link a:hover {
  text-decoration: underline;
}
</style>
