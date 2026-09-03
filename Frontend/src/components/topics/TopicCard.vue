<script setup>
import { computed } from "vue";
import { BookOpen } from "@lucide/vue";

const props = defineProps({
  topic: {
    type: Object,

    required: true,
  },
});

const progress = computed(() => {
  return Number(props.topic.successRate ?? 0);
});
</script>

<template>
  <article class="topic-card">
    <div class="topic-card__header">
      <div class="topic-card__icon">
        <BookOpen :size="24" />
      </div>

      <div class="topic-card__title">
        <h3>
          {{ topic.topicName }}
        </h3>

        <p>
          {{ topic.description || "No description available." }}
        </p>
      </div>
    </div>

    <div class="topic-card__stats">
      <div class="stat">
        <span> Success Rate </span>

        <strong> {{ progress }}% </strong>
      </div>

      <div class="stat">
        <span> Correct </span>

        <strong>
          {{ topic.correctAnswers ?? 0 }}
        </strong>
      </div>

      <div class="stat">
        <span> Wrong </span>

        <strong>
          {{ topic.wrongAnswers ?? 0 }}
        </strong>
      </div>
    </div>

    <div class="progress">
      <div class="progress__bar" :style="{ width: progress + '%' }"></div>
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

  box-shadow: 0 10px 30px rgba(34, 197, 94, 0.15);
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

  border-radius: 16px;

  flex-shrink: 0;

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
  font-size: 1.15rem;

  font-weight: 600;

  color: var(--text-color-primary);
}

.topic-card__title p {
  color: var(--text-secondary);

  line-height: 1.5;
}

.topic-card__stats {
  display: grid;

  grid-template-columns: repeat(3, 1fr);

  gap: 16px;
}

.stat {
  display: flex;

  flex-direction: column;

  gap: 8px;
}

.stat span {
  font-size: 0.9rem;

  color: var(--text-secondary);
}

.stat strong {
  font-size: 1.2rem;

  font-weight: 700;

  color: var(--text-color-primary);
}

.progress {
  height: 10px;

  background: var(--background);

  border-radius: 999px;

  overflow: hidden;
}

.progress__bar {
  height: 100%;

  border-radius: 999px;

  background: linear-gradient(90deg, var(--primary), var(--success));

  transition: width 0.4s ease;
}
</style>
