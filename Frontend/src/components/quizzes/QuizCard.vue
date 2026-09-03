<script setup>
import { computed } from "vue";

import { Play, RotateCcw, Lock, CheckCircle } from "@lucide/vue";

const props = defineProps({
  quiz: {
    type: Object,
    required: true,
  },
});

const status = computed(() => {
  if (!props.quiz.isAvailable) {
    return {
      text: "Unavailable",
      color: "#EF4444",
    };
  }

  if (props.quiz.hasActiveAttempt) {
    return {
      text: "In Progress",
      color: "#F59E0B",
    };
  }

  if (props.quiz.passed) {
    return {
      text: "Passed",
      color: "#22C55E",
    };
  }

  if (props.quiz.canStart) {
    return {
      text: "Available",
      color: "#3B82F6",
    };
  }

  return {
    text: "Attempts Finished",
    color: "#EF4444",
  };
});

const button = computed(() => {
  // 1. Quiz unavailable
  if (!props.quiz.isAvailable) {
    return {
      text: "Unavailable",
      icon: Lock,
      disabled: true,
    };
  }

  // 2. Active attempt exists
  if (props.quiz.hasActiveAttempt) {
    return {
      text: "Continue Quiz",
      icon: RotateCcw,
      disabled: false,
    };
  }

  // 3. Still has attempts → start new attempt
  if (props.quiz.canStart) {
    return {
      text: "Start Quiz",
      icon: Play,
      disabled: false,
    };
  }

  // 4. No attempts left → close
  return {
    text: "Attempts Finished",
    icon: Lock,
    disabled: true,
  };
});
</script>

<template>
  <article class="quiz-card">
    <header class="quiz-card__header">
      <div class="quiz-card__icon">
        <Play :size="24" />
      </div>

      <div class="quiz-card__title">
        <h3>
          {{ quiz.title }}
        </h3>

        <p>
          {{ quiz.description || "No description available." }}
        </p>
      </div>
    </header>

    <div class="quiz-card__body">
      <div class="quiz-card__stat">
        <span>Attempts</span>
        <strong>{{ quiz.attemptsUsed }} / {{ quiz.maxAttempts }}</strong>
      </div>

      <div class="quiz-card__stat">
        <span>Pass Score</span>
        <strong>{{ quiz.passPercentage }}%</strong>
      </div>

      <div class="quiz-card__stat">
        <span>Duration</span>
        <strong>{{ quiz.durationMinutes }} min</strong>
      </div>

      <div class="quiz-card__stat">
        <span>Status</span>
        <strong :style="{ color: status.color }">
          {{ status.text }}
        </strong>
      </div>
    </div>

    <button
      class="quiz-card__button"
      :disabled="button.disabled"
      :class="{
        'quiz-card__button--disabled': button.disabled,
      }"
    >
      <component :is="button.icon" :size="18" />

      {{ button.text }}
    </button>
  </article>
</template>

<style scoped>
.quiz-card {
  display: flex;
  flex-direction: column;
  gap: 24px;
  padding: 24px;
  border-radius: 18px;
  background: var(--surface);
  border: 1px solid var(--border);
  transition: 0.25s;
}

.quiz-card:hover {
  transform: translateY(-4px);
  border-color: var(--primary);
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
}

.quiz-card__header {
  display: flex;
  gap: 18px;
  align-items: center;
}

.quiz-card__icon {
  width: 58px;
  height: 58px;
  border-radius: 16px;
  display: flex;
  justify-content: center;
  align-items: center;
  background: var(--background);
  color: var(--primary);
  flex-shrink: 0;
}

.quiz-card__title {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.quiz-card__title h3 {
  font-size: 1.15rem;
  font-weight: 700;
  color: var(--text-primary);
}

.quiz-card__title p {
  color: var(--text-secondary);
  line-height: 1.5;
}

.quiz-card__body {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 18px;
}

.quiz-card__stat {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.quiz-card__stat span {
  font-size: 0.9rem;
  color: var(--text-secondary);
}

.quiz-card__stat strong {
  font-size: 1.1rem;
  color: var(--text-primary);
  font-weight: 700;
}

.quiz-card__button {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  padding: 14px;
  border: none;
  border-radius: 12px;
  cursor: pointer;
  font-size: 1rem;
  font-weight: 600;
  color: white;
  background: var(--primary);
  transition: 0.25s;
}

.quiz-card__button:hover:not(:disabled) {
  opacity: 0.9;
}

.quiz-card__button:disabled,
.quiz-card__button--disabled {
  background: var(--border) !important;
  color: var(--text-secondary) !important;
  cursor: not-allowed;
  opacity: 0.6;
}
</style>
