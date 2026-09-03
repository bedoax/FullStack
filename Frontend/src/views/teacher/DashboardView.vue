<script setup>
import { onMounted } from "vue";
import { storeToRefs } from "pinia";

import { BookOpen, FileText, Users, Trophy } from "@lucide/vue";

import { useTeacherStore } from "@/stores/teacherStore";
import { useAbortController } from "@/composables/useAbortController";

const teacherStore = useTeacherStore();
const { signal } = useAbortController();
const { dashboard, loading } = storeToRefs(teacherStore);

onMounted(async () => {
  await teacherStore.loadDashboard(signal);
});
</script>

<template>
  <section class="dashboard">
    <!-- Header -->

    <header class="dashboard__header">
      <div>
        <h1>Teacher Dashboard</h1>

        <p>Overview of your quizzes, questions, and students.</p>
      </div>
    </header>

    <!-- Loading -->

    <div v-if="loading" class="dashboard__loading">Loading dashboard...</div>

    <!-- Dashboard -->

    <div v-else class="dashboard__content">
      <div class="dashboard__cards">
        <!-- Quizzes -->

        <div class="dashboard-card">
          <div class="dashboard-card__icon">
            <FileText :size="24" />
          </div>

          <div class="dashboard-card__content">
            <span> Quizzes </span>

            <strong>
              {{ dashboard.quizzes }}
            </strong>
          </div>
        </div>

        <!-- Questions -->

        <div class="dashboard-card">
          <div class="dashboard-card__icon">
            <BookOpen :size="24" />
          </div>

          <div class="dashboard-card__content">
            <span> Questions </span>

            <strong>
              {{ dashboard.questions }}
            </strong>
          </div>
        </div>

        <!-- Students -->

        <div class="dashboard-card">
          <div class="dashboard-card__icon">
            <Users :size="24" />
          </div>

          <div class="dashboard-card__content">
            <span> Students </span>

            <strong>
              {{ dashboard.students }}
            </strong>
          </div>
        </div>

        <!-- Average Pass Rate -->

        <div class="dashboard-card">
          <div class="dashboard-card__icon">
            <Trophy :size="24" />
          </div>

          <div class="dashboard-card__content">
            <span> Average Pass Rate </span>

            <strong> {{ dashboard.averagePassRate.toFixed(2) }}% </strong>
          </div>
        </div>
      </div>

      <!-- Summary -->

      <div class="dashboard__summary">
        <div class="summary-card">
          <div>
            <h2>Your Teaching Overview</h2>

            <p>
              Keep track of your quizzes, question bank, students, and overall quiz
              performance.
            </p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.dashboard {
  display: flex;

  flex-direction: column;

  gap: 32px;
}

/* Header */

.dashboard__header {
  display: flex;

  justify-content: space-between;

  align-items: center;
}

.dashboard__header h1 {
  margin: 0;

  font-size: 32px;

  font-weight: 700;

  color: var(--text-primary);
}

.dashboard__header p {
  margin-top: 8px;

  color: var(--text-secondary);
}

/* Loading */

.dashboard__loading {
  min-height: 250px;

  display: flex;

  justify-content: center;

  align-items: center;

  color: var(--text-secondary);

  font-size: 18px;
}

/* Content */

.dashboard__content {
  display: flex;

  flex-direction: column;

  gap: 28px;
}

/* Cards */

.dashboard__cards {
  display: grid;

  grid-template-columns: repeat(4, minmax(0, 1fr));

  gap: 24px;
}

.dashboard-card {
  display: flex;

  align-items: center;

  gap: 18px;

  padding: 24px;

  background: var(--card-background);

  border: 1px solid var(--border);

  border-radius: 16px;

  transition: 0.25s ease;
}

.dashboard-card:hover {
  transform: translateY(-4px);

  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.08);
}

/* Icon */

.dashboard-card__icon {
  width: 52px;

  height: 52px;

  flex-shrink: 0;

  display: flex;

  justify-content: center;

  align-items: center;

  border-radius: 14px;

  background: color-mix(in srgb, var(--primary) 15%, transparent);

  color: var(--primary);
}

/* Card Content */

.dashboard-card__content {
  display: flex;

  flex-direction: column;

  gap: 6px;
}

.dashboard-card__content span {
  font-size: 14px;

  color: var(--text-secondary);
}

.dashboard-card__content strong {
  font-size: 28px;

  font-weight: 700;

  color: var(--text-primary);
}

/* Summary */

.dashboard__summary {
  display: grid;

  grid-template-columns: 1fr;
}

.summary-card {
  padding: 28px;

  background: var(--card-background);

  border: 1px solid var(--border);

  border-radius: 16px;
}

.summary-card h2 {
  margin: 0;

  font-size: 21px;

  color: var(--text-primary);
}

.summary-card p {
  margin-top: 10px;

  max-width: 700px;

  line-height: 1.6;

  color: var(--text-secondary);
}

/* Responsive */

@media (max-width: 1000px) {
  .dashboard__cards {
    grid-template-columns: repeat(2, minmax(0, 1fr));
  }
}

@media (max-width: 600px) {
  .dashboard__cards {
    grid-template-columns: 1fr;
  }

  .dashboard__header h1 {
    font-size: 26px;
  }

  .dashboard-card {
    padding: 20px;
  }
}
</style>
