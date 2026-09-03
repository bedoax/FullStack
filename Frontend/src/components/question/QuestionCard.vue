<script setup>
import { Check } from "@lucide/vue";

const props = defineProps({
  question: {
    type: Object,
    required: true,
  },

  modelValue: {
    type: [Number, null],
    default: null,
  },

  questionNumber: {
    type: Number,
    default: 1,
  },

  totalQuestions: {
    type: Number,
    default: 1,
  },
});

const emit = defineEmits(["update:modelValue"]);

function selectOption(optionId) {
  emit("update:modelValue", optionId);
}

function isSelected(optionId) {
  return props.modelValue === optionId;
}
</script>

<template>
  <article class="question-card">
    <header class="question-card__header">
      <span class="question-card__number">
        Question
        {{ questionNumber }}
        /
        {{ totalQuestions }}
      </span>

      <span v-if="question.difficulty" class="question-card__difficulty">
        {{ question.difficulty }}
      </span>
    </header>

    <div class="question-card__content">
      <h2>
        {{ question.content }}
      </h2>
    </div>

    <div class="question-card__options">
      <button
        v-for="(option, index) in question.options"
        :key="option.id"
        type="button"
        class="question-option"
        :class="{
          'question-option--selected': isSelected(option.id),
        }"
        @click="selectOption(option.id)"
      >
        <span class="question-option__letter">
          {{ String.fromCharCode(65 + index) }}
        </span>

        <span class="question-option__content">
          {{ option.content }}
        </span>

        <span v-if="isSelected(option.id)" class="question-option__check">
          <Check :size="18" />
        </span>
      </button>
    </div>
  </article>
</template>

<style scoped>
.question-card {
  width: 100%;

  box-sizing: border-box;

  padding: 28px;

  background: var(--card-background);

  border: 1px solid var(--border);

  border-radius: 18px;
}

/* =========================
   Header
========================= */

.question-card__header {
  display: flex;

  align-items: center;

  justify-content: space-between;

  gap: 12px;

  margin-bottom: 26px;
}

.question-card__number {
  color: var(--text-secondary);

  font-size: 14px;

  font-weight: 600;
}

.question-card__difficulty {
  padding: 5px 10px;

  border-radius: 8px;

  background: var(--sidebar-hover);

  color: var(--text-secondary);

  font-size: 12px;

  font-weight: 600;

  text-transform: capitalize;
}

/* =========================
   Question
========================= */

.question-card__content {
  margin-bottom: 28px;
}

.question-card__content h2 {
  margin: 0;

  color: var(--text-primary);

  font-size: 21px;

  line-height: 1.6;

  font-weight: 600;
}

/* =========================
   Options
========================= */

.question-card__options {
  display: flex;

  flex-direction: column;

  gap: 12px;
}

.question-option {
  width: 100%;

  display: flex;

  align-items: center;

  gap: 14px;

  padding: 15px 16px;

  box-sizing: border-box;

  border: 1px solid var(--border);

  border-radius: 12px;

  background: var(--surface);

  color: var(--text-primary);

  text-align: left;

  cursor: pointer;

  transition: border-color 0.2s ease, background 0.2s ease, transform 0.15s ease;
}

.question-option:hover {
  border-color: var(--primary);

  background: var(--sidebar-hover);

  transform: translateY(-1px);
}

.question-option--selected {
  border-color: var(--primary);

  background: color-mix(in srgb, var(--primary) 10%, var(--surface));
}

/* =========================
   Letter
========================= */

.question-option__letter {
  width: 34px;

  height: 34px;

  flex-shrink: 0;

  display: flex;

  align-items: center;

  justify-content: center;

  border: 1px solid var(--border);

  border-radius: 9px;

  color: var(--text-secondary);

  font-size: 14px;

  font-weight: 700;
}

.question-option--selected .question-option__letter {
  background: var(--primary);

  border-color: var(--primary);

  color: white;
}

/* =========================
   Content
========================= */

.question-option__content {
  flex: 1;

  color: var(--text-primary);

  font-size: 15px;

  line-height: 1.5;
}

/* =========================
   Check
========================= */

.question-option__check {
  display: flex;

  align-items: center;

  justify-content: center;

  color: var(--primary);
}

/* =========================
   Responsive
========================= */

@media (max-width: 600px) {
  .question-card {
    padding: 20px;

    border-radius: 14px;
  }

  .question-card__content h2 {
    font-size: 18px;
  }

  .question-option {
    padding: 13px;
  }
}
</style>
