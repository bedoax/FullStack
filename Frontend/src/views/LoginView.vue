<script setup>
import { computed, reactive, onMounted } from "vue";
import { RouterLink, useRouter } from "vue-router";
import { useLogin } from "@/composables/useLogin";
import { useAuthStore } from "@/stores/authStore";

const router = useRouter();
const authStore = useAuthStore();
const { loginGoogle, login, isLoading, serverError } = useLogin();

const form = reactive({
  username: "",
  password: "",
});

const errors = reactive({
  username: "",
  password: "",
});

// التأكد من إدخال البيانات لتفعيل الزرار
const isValid = computed(() => {
  return form.username.trim() !== "" && form.password !== "";
});

function validateForm() {
  errors.username = "";
  errors.password = "";
  let isFormValid = true;

  if (!form.username.trim()) {
    errors.username = "Username is required.";
    isFormValid = false;
  }

  if (!form.password) {
    errors.password = "Password is required.";
    isFormValid = false;
  }

  return isFormValid;
}

function clearError(field) {
  if (errors[field]) {
    errors[field] = "";
  }
}

async function handleSubmit() {
  if (!validateForm()) return;

  const data = await login({
    username: form.username.trim(),
    password: form.password,
  });

  if (data) {
    const role = authStore.role?.toLowerCase() || "student";
    router.push(`/${role}/dashboard`);
  }
}
onMounted(() => {
  if (!window.google) {
    console.error("Google Identity Services is not loaded.");
    return;
  }

  window.google.accounts.id.initialize({
    client_id: "407840783970-f3s5vjrjhor6f1hgkibpuhifhfo1nb7k.apps.googleusercontent.com",
    callback: handleGoogleLogin,
  });

  window.google.accounts.id.renderButton(document.getElementById("google-button"), {
    theme: "outline",
    size: "large",
    width: 336,
    text: "signin_with",
  });
});

async function handleGoogleLogin(response) {
  const data = await loginGoogle(response.credential);
  if (data) {
    const role = authStore.role?.toLocaleLowerCase() || "student";
    router.push(`/${role}/dashboard`);
  }
}
</script>

<template>
  <div class="login-wrapper">
    <form @submit.prevent="handleSubmit" class="form-container">
      <h2>Welcome Back 👋</h2>
      <p class="subtitle">Sign in to continue</p>

      <div v-if="serverError" class="server-error">
        {{ serverError }}
      </div>

      <div class="field">
        <label for="username">Username</label>
        <input
          id="username"
          type="text"
          v-model.trim="form.username"
          @input="clearError('username')"
          placeholder="Enter your username"
          :class="{ 'input-error': errors.username }"
        />
        <span v-if="errors.username" class="error">
          {{ errors.username }}
        </span>
      </div>

      <div class="field">
        <div class="field-header">
          <label for="password">Password</label>
          <RouterLink to="/forgot-password" class="forgot-link">
            Forgot password?
          </RouterLink>
        </div>
        <input
          id="password"
          type="password"
          v-model="form.password"
          @input="clearError('password')"
          placeholder="Enter your password"
          :class="{ 'input-error': errors.password }"
        />
        <span v-if="errors.password" class="error">
          {{ errors.password }}
        </span>
      </div>

      <button type="submit" :disabled="!isValid || isLoading">
        {{ isLoading ? "Logging in..." : "Login" }}
      </button>

      <div class="google-button-wrapper">
        <div id="google-button"></div>
      </div>
      <!-- رابط إنشاء حساب جديد -->
      <p class="register-link">
        Don't have an account?
        <RouterLink to="/register">Create an account</RouterLink>
      </p>
    </form>
  </div>
</template>

<style scoped>
.login-wrapper {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
  background: var(--background);
}

.form-container {
  width: 100%;
  max-width: 400px;
  padding: 32px;
  display: flex;
  flex-direction: column;
  gap: 18px;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 16px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
}
.google-button-wrapper {
  width: 100%;
  display: flex;
  justify-content: center;
  margin-top: -2px;
}

#google-button {
  width: 100%;
  display: flex;
  justify-content: center;
}
h2 {
  margin: 0;
  text-align: center;
  color: var(--text-primary);
  font-size: 26px;
}

.subtitle {
  text-align: center;
  margin-top: -10px;
  margin-bottom: 4px;
  color: var(--text-secondary);
  font-size: 0.95rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.field-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

label {
  font-size: 0.9rem;
  font-weight: 600;
  color: var(--text-primary);
}

.forgot-link {
  font-size: 0.82rem;
  color: var(--primary);
  text-decoration: none;
  font-weight: 500;
}

.forgot-link:hover {
  text-decoration: underline;
}

input {
  padding: 12px 14px;
  border: 1px solid var(--border);
  border-radius: 8px;
  background: var(--surface);
  color: var(--text-primary);
  font-size: 15px;
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
  padding: 12px;
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
  background: rgba(239, 68, 68, 0.12);
  color: var(--danger);
}

.register-link {
  text-align: center;
  font-size: 0.9rem;
  color: var(--text-secondary);
  margin: 4px 0 0 0;
}

.register-link a {
  margin-left: 4px;
  color: var(--primary);
  font-weight: 600;
  text-decoration: none;
}

.register-link a:hover {
  text-decoration: underline;
}
</style>
