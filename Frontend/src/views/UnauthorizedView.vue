<script setup>
import { computed } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/stores/authStore";

const router = useRouter();
const authStore = useAuthStore();

const isLoggedIn = computed(() => authStore.isAuthenticated);

function goBack() {
  if (!isLoggedIn.value) {
    router.replace("/login");
    return;
  }

  if (authStore.isAuthenticated && authStore.role) {
    router.replace(`/${authStore.role.toLowerCase()}/dashboard`);
  } else {
    authStore.logout();
    router.replace("/login");
  }
}
</script>

<template>
  <div class="page">
    <div class="card">
      <div class="icon">🚫</div>

      <h1>403</h1>

      <h2>Access Denied</h2>

      <p>You don't have permission to access this page.</p>

      <button @click="goBack">
        {{ isLoggedIn ? "Go To Dashboard" : "Login" }}
      </button>
    </div>
  </div>
</template>

<style scoped>
.page {
  min-height: 100vh;

  display: flex;

  justify-content: center;

  align-items: center;

  background: var(--background);
}

.card {
  width: 420px;

  padding: 40px;

  text-align: center;

  background: var(--surface);

  border: 1px solid var(--border);

  border-radius: 16px;

  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
}

.icon {
  font-size: 70px;

  margin-bottom: 15px;
}

h1 {
  margin: 0;

  font-size: 72px;

  color: var(--danger);
}

h2 {
  margin: 10px 0;

  color: var(--text-primary);
}

p {
  margin-bottom: 30px;

  line-height: 1.6;

  color: var(--text-secondary);
}

button {
  padding: 12px 24px;

  border: none;

  border-radius: 8px;

  background: var(--primary);

  color: white;

  font-size: 15px;

  font-weight: 600;

  cursor: pointer;

  transition: 0.25s;
}

button:hover {
  background: var(--primary-hover);
}
</style>
