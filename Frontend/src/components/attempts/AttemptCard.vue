<script setup>
import { computed } from "vue";
import { useRouter } from "vue-router";
import { Eye, Trophy } from "lucide-vue-next";

const router = useRouter();

const props = defineProps({
  attempt: {
    type: Object,
    required: true,
  },
});

const status = computed(() => {
  if (props.attempt.passed === true) {
    return {
      text: "Passed",
      color: "#22c55e",
    };
  }

  if (props.attempt.passed === false) {
    return {
      text: "Failed",
      color: "#ef4444",
    };
  }

  return {
    text: "Not Submitted",
    color: "#f59e0b",
  };
});

function formatDate(date) {
  if (!date) return "-";

  return new Date(date).toLocaleString();
}

function reviewAttempt() {
  router.push({
    name: "attempt-review",
    params: {
      attemptId: props.attempt.id,
    },
  });
}
</script>

<template>
  <div class="attempt-card">
    <header class="attempt-card__header">
      <div class="attempt-card__icon">
        <Trophy :size="24" />
      </div>

      <div class="attempt-card__title">
        <h3>
          {{ attempt.quizTitle }}
        </h3>

        <p>Attempt #{{ attempt.attemptNumber }}</p>
      </div>
    </header>

    <div class="attempt-card__body">
      <div class="attempt-card__stat">
        <span> Score </span>

        <strong>
          {{ attempt.score }}
        </strong>
      </div>

      <div class="attempt-card__stat">
        <span> Percentage </span>

        <strong> {{ attempt.percentage }}% </strong>
      </div>

      <div class="attempt-card__stat">
        <span> Status </span>

        <strong :style="{ color: status.color }">
          {{ status.text }}
        </strong>
      </div>

      <div class="attempt-card__stat">
        <span> Started </span>

        <strong>
          {{ formatDate(attempt.startedAt) }}
        </strong>
      </div>

      <div class="attempt-card__stat">
        <span> Submitted </span>

        <strong>
          {{ formatDate(attempt.submittedAt) }}
        </strong>
      </div>
    </div>

    <button class="attempt-card__button" @click="reviewAttempt">
      <Eye :size="18" />

      Review Attempt
    </button>
  </div>
</template>

<style scoped>
.attempt-card {
  background: var(--card-background);

  border-radius: 16px;

  padding: 20px;

  border: 1px solid var(--border);

  display: flex;

  flex-direction: column;

  gap: 20px;

  transition: 0.25s;
}

.attempt-card:hover {
  transform: translateY(-4px);

  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.08);
}

.attempt-card__header {
  display: flex;

  align-items: center;

  gap: 16px;
}

.attempt-card__icon {
  width: 56px;

  height: 56px;

  border-radius: 50%;

  background: color-mix(in srgb, var(--primary) 15%, transparent);

  display: flex;

  justify-content: center;

  align-items: center;

  color: var(--primary);
}

.attempt-card__title h3 {
  margin: 0;

  font-size: 20px;

  color: var(--text-primary);
}

.attempt-card__title p {
  margin-top: 6px;

  color: var(--text-secondary);
}

.attempt-card__body {
  display: grid;

  grid-template-columns: repeat(2, 1fr);

  gap: 16px;
}

.attempt-card__stat {
  display: flex;

  flex-direction: column;

  gap: 6px;
}

.attempt-card__stat span {
  font-size: 13px;

  color: var(--text-secondary);
}

.attempt-card__stat strong {
  font-size: 15px;

  color: var(--text-primary);
}

.attempt-card__button {
  border: none;

  border-radius: 10px;

  padding: 12px;

  background: var(--primary);

  color: white;

  display: flex;

  justify-content: center;

  align-items: center;

  gap: 8px;

  cursor: pointer;

  transition: 0.2s;
}

.attempt-card__button:hover {
  background: var(--primary-hover);
}

/* Status */

.status-success {
  color: var(--success);
}

.status-warning {
  color: var(--warning);
}

.status-danger {
  color: var(--danger);
}
</style>
