<script setup>
import { ref } from "vue";

import Navbar from "@/components/layout/Navbar.vue";
import Sidebar from "@/components/layout/Sidebar.vue";

const collapsed = ref(false);
const isMobileOpen = ref(false);

function toggleSidebar() {
  if (window.innerWidth <= 768) {
    isMobileOpen.value = !isMobileOpen.value;
  } else {
    collapsed.value = !collapsed.value;
  }
}

function closeMobileMenu() {
  isMobileOpen.value = false;
}
</script>

<template>
  <div class="layout">
    <Sidebar
      :collapsed="collapsed"
      :is-mobile-open="isMobileOpen"
      @close-mobile="closeMobileMenu"
    />

    <div class="layout__content" :class="{ 'collapsed-margin': collapsed }">
      <Navbar :collapsed="collapsed" @toggle-sidebar="toggleSidebar" />

      <main class="layout__page">
        <RouterView />
      </main>
    </div>
  </div>
</template>

<style scoped>
.layout {
  display: flex;
  min-height: 100vh;
  background: var(--background-color);
}

.layout__content {
  flex: 1;
  display: flex;
  flex-direction: column;
  margin-left: 260px;
  transition: margin-left 0.25s cubic-bezier(0.4, 0, 0.2, 1);
  min-width: 0;
}

.layout__content.collapsed-margin {
  margin-left: 78px;
}

.layout__page {
  flex: 1;
  padding: 24px;
  overflow-y: auto;
  background: var(--background-color);
}

@media (max-width: 768px) {
  .layout__content {
    margin-left: 0 !important;
  }

  .layout__page {
    padding: 16px;
  }
}

@media (max-width: 480px) {
  .layout__page {
    padding: 12px;
  }
}
</style>
