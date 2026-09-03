<script setup>
import { computed } from "vue";
import { useRouter } from "vue-router";
import { GraduationCap, LogOut } from "@lucide/vue";

import { sidebarMenu } from "@/config/sidebarMenu";
import { useAuthStore } from "@/stores/authStore";

const props = defineProps({
  collapsed: {
    type: Boolean,
    default: false,
  },
  isMobileOpen: {
    type: Boolean,
    default: false,
  },
});

defineEmits(["close-mobile"]);

const authStore = useAuthStore();
const router = useRouter();

const menu = computed(() => {
  return sidebarMenu[authStore.role] ?? [];
});

function logout() {
  authStore.logout();
  router.replace("/login");
}
</script>

<template>
  <!-- خلفية معتمة للموبايل تعتمد على إشارة الإغلاق -->
  <div v-if="isMobileOpen" class="sidebar-overlay" @click="$emit('close-mobile')"></div>

  <aside class="sidebar" :class="{ collapsed, 'mobile-open': isMobileOpen }">
    <RouterLink to="/" class="sidebar__header">
      <GraduationCap class="logo-icon" :size="30" />
      <span class="logo"> Quiz Platform </span>
    </RouterLink>

    <nav class="sidebar__menu" aria-label="Sidebar Navigation">
      <RouterLink
        v-for="item in menu"
        :key="item.path"
        :to="item.path"
        class="menu-item"
        active-class="active"
        exact-active-class="active"
        :title="collapsed ? item.label : ''"
        @click="$emit('close-mobile')"
      >
        <component :is="item.icon" :size="20" class="menu-icon" />
        <span>{{ item.label }}</span>
      </RouterLink>
    </nav>

    <button class="logout-btn" :title="collapsed ? 'Logout' : ''" @click="logout">
      <LogOut :size="20" class="logout-icon" />
      <span> Logout </span>
    </button>
  </aside>
</template>

<style scoped>
.sidebar {
  width: 260px;
  height: 100vh;
  background: var(--sidebar-background);
  border-right: 1px solid var(--border);
  display: flex;
  flex-direction: column;
  transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1), width 0.25s ease;
  position: fixed;
  left: 0;
  top: 0;
  z-index: 200;
}

.sidebar.collapsed {
  width: 78px;
}

.sidebar__header {
  height: 70px;
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 0 20px;
  border-bottom: 1px solid var(--border);
  text-decoration: none;
  overflow: hidden;
}

.logo-icon {
  color: var(--primary);
  flex-shrink: 0;
}

.logo {
  font-size: 1.2rem;
  font-weight: 700;
  color: var(--primary);
  white-space: nowrap;
  transition: opacity 0.2s ease;
}

.sidebar__menu {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 6px;
  padding: 16px 10px;
  overflow-y: auto;
}

.menu-item {
  height: 46px;
  display: flex;
  align-items: center;
  gap: 14px;
  padding: 0 16px;
  border-radius: 12px;
  color: var(--text-primary);
  text-decoration: none;
  transition: all 0.2s ease;
  white-space: nowrap;
}

.menu-item:hover {
  background: var(--sidebar-hover);
}

.menu-item.active {
  background: var(--primary);
  color: #ffffff;
}

.menu-item.active span {
  color: #ffffff;
}

.menu-item span {
  white-space: nowrap;
  color: var(--text-primary);
  transition: opacity 0.2s ease;
}

.menu-icon,
.logout-icon {
  flex-shrink: 0;
}

.logout-btn {
  margin: 16px 10px;
  height: 46px;
  display: flex;
  align-items: center;
  gap: 14px;
  padding: 0 16px;
  border: none;
  border-radius: 12px;
  background: transparent;
  color: #ef4444;
  cursor: pointer;
  transition: all 0.2s ease;
  white-space: nowrap;
}

.logout-btn:hover {
  background: rgba(239, 68, 68, 0.12);
}

/* حالة الانكماش للشاشات الكبيرة (Desktop) */
.sidebar.collapsed .menu-item,
.sidebar.collapsed .logout-btn {
  justify-content: center;
  padding: 0;
}

.sidebar.collapsed .logo,
.sidebar.collapsed .menu-item span,
.sidebar.collapsed .logout-btn span {
  display: none;
}

/* الطبقة المعتمة للموبايل */
.sidebar-overlay {
  display: none;
}

/* الشاشات الصغيرة (الموبايل) */
@media (max-width: 768px) {
  .sidebar {
    width: 260px !important;
    transform: translateX(-100%);
  }

  .sidebar.mobile-open {
    transform: translateX(0);
    box-shadow: 4px 0 24px rgba(0, 0, 0, 0.15);
  }

  /* إجبار إظهار النصوص داخل الموبايل بغض النظر عن حالة collapsed */
  .sidebar .logo,
  .sidebar .menu-item span,
  .sidebar .logout-btn span {
    display: inline-block !important;
  }

  .sidebar .menu-item,
  .sidebar .logout-btn {
    justify-content: flex-start !important;
    padding: 0 16px !important;
  }

  .sidebar-overlay {
    display: block;
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background: rgba(0, 0, 0, 0.4);
    backdrop-filter: blur(2px);
    z-index: 190;
  }
}
</style>
