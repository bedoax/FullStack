<script setup>
import { computed } from "vue";
import { Pencil, BookOpen } from "@lucide/vue";

const props = defineProps({
  question: {
    type: Object,
    required: true,
  },
  topics: {
    type: Array,
    default: () => [],
  },
  readOnly: {
    type: Boolean,
    default: false,
  },
});

const emit = defineEmits(["edit"]);

const topicName = computed(() => {
  const topic = props.topics.find((t) => t.id === props.question.topicId);
  return topic ? topic.name : "Unknown";
});

function handleEdit() {
  emit("edit", props.question);
}
</script>

<template>
  <article class="question-card">
    <header class="question-card__header">
      <div class="question-card__topic">
        <BookOpen :size="18" />
        <span>{{ topicName }}</span>
      </div>
      <button
        v-if="!readOnly"
        type="button"
        class="edit-button"
        title="Edit question"
        @click="handleEdit"
      >
        <Pencil :size="17" />
      </button>
    </header>

    <div class="question-card__content">
      <p>{{ question.content }}</p>
    </div>

    <footer class="question-card__footer">
      <span
        class="difficulty"
        :class="`difficulty--${question.difficulty?.toLowerCase()}`"
      >
        {{ question.difficulty || "Unknown" }}
      </span>
      <span class="points">{{ question.points ?? 0 }} Points</span>
    </footer>
  </article>
</template>

<style scoped>
.question-card {
  display: flex;

  flex-direction: column;

  gap: 18px;

  padding: 22px;

  background: var(--surface);

  border: 1px solid var(--border);

  border-radius: 16px;

  transition: 0.2s ease;
}

.question-card:hover {
  transform: translateY(-3px);

  border-color: var(--primary);

  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.08);
}

/* Header */

.question-card__header {
  display: flex;

  align-items: center;

  justify-content: space-between;

  gap: 12px;
}

.question-card__topic {
  display: flex;

  align-items: center;

  gap: 8px;

  color: var(--primary);

  font-size: 14px;

  font-weight: 600;
}

.edit-button {
  width: 36px;

  height: 36px;

  display: flex;

  align-items: center;

  justify-content: center;

  border: none;

  border-radius: 9px;

  background: transparent;

  color: var(--text-secondary);

  cursor: pointer;

  transition: 0.2s;
}

.edit-button:hover {
  background: var(--sidebar-hover);

  color: var(--primary);
}

/* Content */

.question-card__content {
  min-height: 80px;
}

.question-card__content p {
  margin: 0;

  color: var(--text-primary);

  font-size: 16px;

  line-height: 1.6;
}

/* Footer */

.question-card__footer {
  display: flex;

  align-items: center;

  justify-content: space-between;

  gap: 10px;

  padding-top: 14px;

  border-top: 1px solid var(--border);
}

.difficulty {
  padding: 5px 10px;

  border-radius: 999px;

  font-size: 12px;

  font-weight: 600;
}

/* Easy */

.difficulty--easy {
  background: rgba(34, 197, 94, 0.12);

  color: #16a34a;
}

/* Medium */

.difficulty--medium {
  background: rgba(234, 179, 8, 0.12);

  color: #ca8a04;
}

/* Hard */

.difficulty--hard {
  background: rgba(239, 68, 68, 0.12);

  color: #dc2626;
}

.points {
  color: var(--text-secondary);

  font-size: 13px;

  font-weight: 500;
}
</style>
