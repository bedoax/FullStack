<script setup>
import { Check } from "lucide-vue-next";

const props = defineProps({
  question: {
    type: Object,
    required: true,
  },

  selected: {
    type: Boolean,
    default: false,
  },

  disabled: {
    type: Boolean,
    default: false,
  },
});

const emit = defineEmits(["toggle"]);

function toggle() {
  if (props.disabled) return;

  emit("toggle", props.question.id);
}
</script>

<template>
  <button
    type="button"
    class="question-selection-card"
    :class="{
      'question-selection-card--selected': selected,
    }"
    :disabled="disabled"
    @click="toggle"
  >
    <!-- Selection indicator -->
    <div
      class="question-selection-card__check"
      :class="{
        'question-selection-card__check--selected': selected,
      }"
    >
      <Check v-if="selected" :size="16" />
    </div>

    <!-- Question content -->
    <div class="question-selection-card__content">
      <div class="question-selection-card__header">
        <span class="question-selection-card__id"> Question #{{ question.id }} </span>
      </div>

      <p class="question-selection-card__text">
        {{ question.content }}
      </p>
    </div>
  </button>
</template>

<style scoped>
.question-selection-card {
  width: 100%;

  display: flex;

  align-items: flex-start;

  gap: 14px;

  padding: 15px;

  border: 1px solid var(--border);

  border-radius: 12px;

  background: var(--surface);

  color: var(--text-primary);

  text-align: left;

  cursor: pointer;

  transition: background 0.2s ease, border-color 0.2s ease, box-shadow 0.2s ease,
    transform 0.2s ease;
}

.question-selection-card:hover:not(:disabled) {
  border-color: var(--primary);

  background: color-mix(in srgb, var(--primary) 4%, var(--surface));
}

.question-selection-card:disabled {
  opacity: 0.65;

  cursor: not-allowed;
}

/* ============================= */
/* Selected */
/* ============================= */

.question-selection-card--selected {
  border-color: var(--primary);

  background: color-mix(in srgb, var(--primary) 9%, var(--surface));

  box-shadow: 0 0 0 1px color-mix(in srgb, var(--primary) 25%, transparent);
}

/* ============================= */
/* Check */
/* ============================= */

.question-selection-card__check {
  width: 22px;

  height: 22px;

  flex-shrink: 0;

  display: flex;

  align-items: center;

  justify-content: center;

  margin-top: 1px;

  border: 2px solid var(--border);

  border-radius: 6px;

  background: var(--surface);

  color: white;

  transition: background 0.2s ease, border-color 0.2s ease;
}

.question-selection-card__check--selected {
  border-color: var(--primary);

  background: var(--primary);
}

/* ============================= */
/* Content */
/* ============================= */

.question-selection-card__content {
  min-width: 0;

  flex: 1;
}

.question-selection-card__header {
  margin-bottom: 6px;
}

.question-selection-card__id {
  font-size: 11px;

  font-weight: 600;

  color: var(--primary);

  text-transform: uppercase;

  letter-spacing: 0.04em;
}

.question-selection-card__text {
  margin: 0;

  color: var(--text-primary);

  font-size: 14px;

  line-height: 1.55;
}
</style>
