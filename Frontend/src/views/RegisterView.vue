<script setup>
import { computed, reactive } from "vue";
import { RouterLink } from "vue-router";
import { useRegister } from "@/composables/useRegister";

const { register, isLoading, serverError } = useRegister();

const form = reactive({
  username: "",
  email: "",
  password: "",
  confirmPassword: "",
});
const touched = reactive({
  username: false,
  email: false,
  password: false,
  confirmPassword: false,
});

const isPasswordMatched = computed(() => {
  return form.password === form.confirmPassword;
});

// الـ Regex الخاص بالإيميل
const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

const errors = computed(() => {
  const errs = {};

  if (form.username && form.username.trim().length < 3) {
    errs.username = "Username must be at least 3 characters.";
  }

  if (form.email && !emailRegex.test(form.email.trim())) {
    errs.email = "Please enter a valid email address.";
  }

  if (form.password && form.password.length < 6) {
    errs.password = "Password must be at least 6 characters.";
  }

  if (form.confirmPassword && !isPasswordMatched.value) {
    errs.confirmPassword = "Passwords do not match.";
  }

  return errs;
});

const isValid = computed(() => {
  const isFormFilled =
    form.username.trim() && form.email.trim() && form.password && form.confirmPassword;
  const hasNoErrors = Object.keys(errors.value).length === 0;

  return isFormFilled && hasNoErrors;
});

function handleBlur(field) {
  touched[field] = true;
}

async function handleSubmit() {
  // تعليم كل الحقول عند الضغط على إرسال
  Object.keys(touched).forEach((key) => (touched[key] = true));

  if (!isValid.value || isLoading.value) return;

  await register({
    username: form.username.trim(),
    email: form.email.trim(),
    password: form.password,
  });
}
</script>

<template>
  <div class="register-wrapper">
    <form @submit.prevent="handleSubmit" class="form-container">
      <h2>Create Account 🚀</h2>
      <p class="subtitle">Create your account to start using the platform</p>

      <div v-if="serverError" class="server-error">
        {{ serverError }}
      </div>

      <div class="field">
        <label for="username">Username</label>
        <input
          id="username"
          type="text"
          v-model.trim="form.username"
          @blur="handleBlur('username')"
          placeholder="Enter your username"
          :class="{ 'input-error': touched.username && errors.username }"
        />
        <span v-if="touched.username && errors.username" class="error">
          {{ errors.username }}
        </span>
      </div>

      <div class="field">
        <label for="email">Email</label>
        <input
          id="email"
          type="email"
          v-model.trim="form.email"
          @blur="handleBlur('email')"
          placeholder="Enter your email"
          :class="{ 'input-error': touched.email && errors.email }"
        />
        <span v-if="touched.email && errors.email" class="error">
          {{ errors.email }}
        </span>
      </div>

      <div class="field">
        <label for="password">Password</label>
        <input
          id="password"
          type="password"
          v-model="form.password"
          @blur="handleBlur('password')"
          placeholder="Enter your password"
          :class="{ 'input-error': touched.password && errors.password }"
        />
        <span v-if="touched.password && errors.password" class="error">
          {{ errors.password }}
        </span>
      </div>

      <div class="field">
        <label for="confirmPassword">Confirm Password</label>
        <input
          id="confirmPassword"
          type="password"
          v-model="form.confirmPassword"
          @blur="handleBlur('confirmPassword')"
          placeholder="Confirm your password"
          :class="{ 'input-error': touched.confirmPassword && errors.confirmPassword }"
        />
        <span v-if="touched.confirmPassword && errors.confirmPassword" class="error">
          {{ errors.confirmPassword }}
        </span>
      </div>

      <button type="submit" :disabled="!isValid || isLoading">
        {{ isLoading ? "Creating Account..." : "Register" }}
      </button>

      <p class="login-link">
        Already have an account?
        <RouterLink to="/login">Login</RouterLink>
      </p>
    </form>
  </div>
</template>

<style scoped>
.register-wrapper {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
  background: var(--background);
}

.form-container {
  width: 100%;
  max-width: 420px;
  padding: 32px;
  display: flex;
  flex-direction: column;
  gap: 18px;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 14px;
  box-shadow: 0 12px 30px rgba(0, 0, 0, 0.08);
}

h2 {
  margin: 0;
  text-align: center;
  font-size: 28px;
  color: var(--text-primary);
}

.subtitle {
  margin-top: -8px;
  margin-bottom: 6px;
  text-align: center;
  font-size: 0.95rem;
  color: var(--text-secondary);
}

.field {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

label {
  font-size: 0.9rem;
  font-weight: 600;
  color: var(--text-primary);
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
  padding: 10px;
  border-radius: 8px;
  text-align: center;
  font-size: 0.9rem;
  background: rgba(239, 68, 68, 0.12);
  color: var(--danger);
}

.login-link {
  text-align: center;
  font-size: 0.92rem;
  color: var(--text-secondary);
}

.login-link a {
  margin-left: 4px;
  color: var(--primary);
  text-decoration: none;
  font-weight: 600;
}

.login-link a:hover {
  text-decoration: underline;
}

@media (max-width: 480px) {
  .form-container {
    padding: 24px;
  }
}
</style>
