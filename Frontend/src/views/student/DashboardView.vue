<script setup>
import { computed, onMounted } from "vue";

import { Trophy, BadgeCheck, ClipboardList, CheckCircle } from "@lucide/vue";

import { useStudentStore } from "@/stores/studentStore";

import DashboardStatCard from "@/components/dashboard/DashboardStatCard.vue";
import WeakTopicsCard from "@/components/dashboard/WeakTopicsCard.vue";
import PerformanceChart from "@/components/dashboard/PerformanceChart.vue";
import { useAbortController } from "@/composables/useAbortController";
const studentStore = useStudentStore();
const { signal } = useAbortController();
const stats = computed(() => [
  {
    title: "Skill Score",
    value: studentStore.dashboard.skillScore,
    icon: Trophy,
    color: "var(--success-color)",
  },

  {
    title: "Current Level",
    value: studentStore.dashboard.currentLevel,
    icon: BadgeCheck,
    color: "var(--info-color)",
  },

  {
    title: "Attempts",
    value: studentStore.dashboard.attempts,
    icon: ClipboardList,
    color: "var(--warning-color)",
  },

  {
    title: "Passed",
    value: studentStore.dashboard.passed,
    icon: CheckCircle,
    color: "var(--primary-color)",
  },
]);

onMounted(async () => {
  await Promise.all([
    studentStore.loadDashboard(signal),
    studentStore.loadPerformance(),
    studentStore.loadWeakTopics(),
  ]);
});
</script>
<template>
  <section class="dashboard">
    <header class="dashboard__header">
      <h1>Dashboard</h1>

      <p>Welcome Back 👋</p>
    </header>

    <div class="dashboard__stats">
      <DashboardStatCard
        v-for="card in stats"
        :key="card.title"
        :title="card.title"
        :value="card.value"
        :icon="card.icon"
        :color="card.color"
      />
    </div>

    <div class="dashboard__content">
      <WeakTopicsCard :topics="studentStore.weakTopics" />
    </div>

    <PerformanceChart :data="studentStore.performance || []" type="bar" />
  </section>
</template>

<style scoped>
.dashboard {
  display: flex;

  flex-direction: column;

  gap: 30px;
}

.dashboard__header h1 {
  font-size: 2rem;

  font-weight: 700;

  color: var(--text-primary);
}

.dashboard__header p {
  margin-top: 6px;

  color: var(--text-secondary);
}

.dashboard__stats {
  display: grid;

  grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));

  gap: 20px;
}

.dashboard__content {
  display: grid;

  grid-template-columns: 1fr;

  gap: 24px;
}
</style>
