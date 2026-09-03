<script setup>
import { Menu, Moon, Sun, User, ChevronDown } from "@lucide/vue";
import { useAuthStore } from "@/stores/authStore";
import { useThemeStore } from "@/stores/themeStore";

const authStore = useAuthStore();
const themeStore = useThemeStore();

const emit = defineEmits(["toggle-sidebar"]);

function handleThemeToggle() {
  const nextTheme = themeStore.theme === "dark" ? "light" : "dark";
  themeStore.setTheme(nextTheme);
}
</script>

<template>
  <header class="navbar">
    <div class="navbar__left">
      <button
        class="icon-button"
        aria-label="Toggle Sidebar"
        @click="emit('toggle-sidebar')"
      >
        <Menu :size="22" />
      </button>

      <h2 class="logo">Quiz Platform</h2>
    </div>

    <div class="navbar__right">
      <button class="icon-button" aria-label="Toggle Theme" @click="handleThemeToggle">
        <Sun v-if="themeStore.theme === 'dark'" :size="20" />
        <Moon v-else :size="20" />
      </button>

      <button class="user-button">
        <User :size="18" class="user-icon" />
        <span>
          {{ authStore.username || authStore.role }}
        </span>
        <ChevronDown :size="16" class="chevron-icon" />
      </button>
    </div>
  </header>
</template>

<style scoped>
.navbar {
  height: 70px;
  padding: 0 24px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: var(--navbar-background);
  border-bottom: 1px solid var(--border);
  position: sticky;
  top: 0;
  z-index: 100;
  transition: padding 0.25s ease;
}

.navbar__left {
  display: flex;
  align-items: center;
  gap: 18px;
}

.logo {
  font-size: 22px;
  font-weight: 700;
  color: var(--primary);
  white-space: nowrap;
}

.navbar__right {
  display: flex;
  align-items: center;
  gap: 14px;
}

.icon-button {
  width: 42px;
  height: 42px;
  border-radius: 12px;
  display: flex;
  justify-content: center;
  align-items: center;
  background: transparent;
  color: var(--text-primary);
  border: none;
  cursor: pointer;
  transition: background 0.25s, transform 0.1s ease;
  flex-shrink: 0;
}

.icon-button:hover {
  background: var(--sidebar-hover);
}

.icon-button:active {
  transform: scale(0.95);
}

.user-button {
  height: 42px;
  padding: 0 14px;
  display: flex;
  align-items: center;
  gap: 10px;
  border-radius: 12px;
  background: transparent;
  color: var(--text-primary);
  border: none;
  cursor: pointer;
  transition: 0.25s;
  flex-shrink: 0;
}

.user-button:hover {
  background: var(--sidebar-hover);
}

.user-button span {
  font-size: 15px;
  font-weight: 500;
  white-space: nowrap;
}

.user-icon,
.chevron-icon {
  flex-shrink: 0;
}

/* ==========================================
   Responsive Breakpoints
   ========================================== */
@media (max-width: 768px) {
  .navbar {
    padding: 0 16px;
  }

  .navbar__right {
    gap: 8px;
  }

  .user-button span,
  .chevron-icon {
    display: none;
  }

  .user-button {
    padding: 0;
    width: 42px;
    justify-content: center;
  }

  .logo {
    font-size: 18px;
  }
}

@media (max-width: 400px) {
  .navbar__left {
    gap: 8px;
  }

  /* إخفاء الاسم النصي للشعار في الشاشات الصغرى جداً لتوفير المساحة */
  .logo {
    display: none;
  }
}
</style>
