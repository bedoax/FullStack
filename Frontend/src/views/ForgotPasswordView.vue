<script setup>
import { computed, reactive } from "vue";
import { RouterLink } from "vue-router";
import { useForgotPassword } from "@/composables/useForgotPassword";

const { forgotPassword, isLoading, serverError } = useForgotPassword();

const form = reactive({
  email: "",
});

const errors = reactive({
  email: "",
});

// Regex قوي للتحقق من صيغة الإيميل
const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

// التحقق الفوري للتأكد من إن الزرار ينور فقط عند إدخال إيميل صحيح
const isValid = computed(() => {
  return emailRegex.test(form.email.trim());
});

function validateForm() {
  errors.email = "";
  let isFormValid = true;

  if (!form.email.trim()) {
    errors.email = "Email is required.";
    isFormValid = false;
  } else if (!emailRegex.test(form.email.trim())) {
    errors.email = "Please enter a valid email address.";
    isFormValid = false;
  }

  return isFormValid;
}

// مسح رسالة الخطأ أول ما المستخدم يبدأ يكتب
function handleInput() {
  if (errors.email) {
    errors.email = "";
  }
}

async function handleSubmit() {
  if (!validateForm()) return;

  await forgotPassword({ email: form.email.trim() });
}
</script>

<template>
  <div class="forgot-container">
    <form @submit.prevent="handleSubmit" class="form-container">
      <h2>Forgot Password</h2>

      <p class="description">
        Enter your email address and we'll send you a verification code.
      </p>

      <div v-if="serverError" class="server-error">
        {{ serverError }}
      </div>

      <div class="field">
        <label for="email">Email</label>

        <input
          id="email"
          type="email"
          v-model.trim="form.email"
          @input="handleInput"
          placeholder="Enter your email"
          :class="{ 'input-error': errors.email }"
        />

        <span v-if="errors.email" class="error">
          {{ errors.email }}
        </span>
      </div>

      <button type="submit" :disabled="!isValid || isLoading">
        {{ isLoading ? "Sending..." : "Send Reset Code" }}
      </button>

      <p class="login-link">
        Remember your password?
        <RouterLink to="/login">Login</RouterLink>
      </p>
    </form>
  </div>
</template>

<style scoped>
.forgot-container {
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;
  background: var(--background);
}

.form-container {
  width: 100%;
  max-width: 420px;
  display: flex;
  flex-direction: column;
  gap: 18px;
  padding: 32px;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 16px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
}

h2 {
  margin: 0;
  text-align: center;
  font-size: 28px;
  color: var(--text-primary);
}

.description {
  margin: 0;
  text-align: center;
  font-size: 0.95rem;
  line-height: 1.5;
  color: var(--text-secondary);
}

.field {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

label {
  font-weight: 600;
  color: var(--text-primary);
}

input {
  padding: 12px 14px;
  border: 1px solid var(--border);
  border-radius: 8px;
  font-size: 15px;
  color: var(--text-primary);
  background: var(--surface);
  transition: all 0.2s ease;
}

input:focus {
  outline: none;
  border-color: var(--primary);
  box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.15);
}

input.input-error {
  border-color: var(--danger);
}

input.input-error:focus {
  box-shadow: 0 0 0 3px rgba(239, 68, 68, 0.15);
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
  transition: all 0.2s ease;
}

button:hover:not(:disabled) {
  background: var(--primary-hover);
}

button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.error {
  color: var(--danger);
  font-size: 0.82rem;
}

.server-error {
  padding: 12px;
  border-radius: 8px;
  text-align: center;
  font-size: 0.9rem;
  color: var(--danger);
  background: rgba(239, 68, 68, 0.12);
}

.login-link {
  text-align: center;
  font-size: 0.9rem;
  color: var(--text-secondary);
}

.login-link a {
  margin-left: 6px;
  color: var(--primary);
  font-weight: 600;
  text-decoration: none;
}

.login-link a:hover {
  text-decoration: underline;
}
</style>
