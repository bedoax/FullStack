<script setup>
import { BookOpen, Pencil } from "@lucide/vue";

const props = defineProps({
  topic: {
    type: Object,
    required: true,
  },
  readOnly: {
    type: Boolean,
    default: false,
  },
});

const emit = defineEmits(["edit"]);

function handleEdit() {
  emit("edit", props.topic);
}
</script>

<template>
  <article class="topic-card">
    <div class="topic-card__header">
      <div class="topic-card__icon">
        <BookOpen :size="24" />
      </div>

      <div class="topic-card__title">
        <h3>
          {{ topic.name }}
        </h3>

        <p>
          {{ topic.description || "No description available." }}
        </p>
      </div>
    </div>

    <!-- يظهر فقط إذا لم تكن الكارت للعرض فقط -->
    <div v-if="!readOnly" class="topic-card__footer">
      <button
        class="topic-card__edit"
        type="button"
        @click="handleEdit"
        title="Edit topic"
      >
        <Pencil :size="18" />

        <span> Edit </span>
      </button>
    </div>
  </article>
</template>

<style scoped>
.topic-card {
  display: flex;

  flex-direction: column;

  gap: 24px;

  padding: 24px;

  background: var(--surface);

  border: 1px solid var(--border);

  border-radius: 18px;

  transition: 0.25s;
}

.topic-card:hover {
  transform: translateY(-4px);

  border-color: var(--primary);

  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
}

.topic-card__header {
  display: flex;

  align-items: center;

  gap: 18px;
}

.topic-card__icon {
  display: flex;

  justify-content: center;

  align-items: center;

  width: 56px;

  height: 56px;

  flex-shrink: 0;

  border-radius: 16px;

  color: var(--primary);

  background: color-mix(in srgb, var(--primary) 12%, transparent);

  border: 1px solid color-mix(in srgb, var(--primary) 22%, transparent);
}

.topic-card__title {
  display: flex;

  flex-direction: column;

  gap: 6px;
}

.topic-card__title h3 {
  margin: 0;

  font-size: 1.15rem;

  font-weight: 600;

  color: var(--text-color-primary);
}

.topic-card__title p {
  margin: 0;

  color: var(--text-secondary);

  line-height: 1.5;
}

.topic-card__footer {
  display: flex;

  justify-content: flex-end;

  padding-top: 4px;

  border-top: 1px solid var(--border);
}

.topic-card__edit {
  display: flex;

  align-items: center;

  justify-content: center;

  gap: 8px;

  padding: 9px 14px;

  border: none;

  border-radius: 10px;

  background: var(--primary);

  color: white;

  cursor: pointer;

  transition: 0.2s;
}

.topic-card__edit:hover {
  background: var(--primary-hover);
}

.topic-card__edit span {
  font-size: 14px;

  font-weight: 500;
}
</style>
