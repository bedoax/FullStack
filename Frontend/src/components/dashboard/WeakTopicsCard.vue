<script setup>
defineProps({
  topics: {
    type: Array,

    default: () => [],
  },
});
</script>
<template>
  <section class="weak-topics">
    <header class="weak-topics__header">
      <h2>Weak Topics</h2>
    </header>

    <div v-if="topics.length" class="topics">
      <div v-for="topic in topics" :key="topic.topicId" class="topic">
        <div class="topic__info">
          <span class="topic__name">
            {{ topic.topicName }}
          </span>

          <span class="topic__score"> {{ topic.successRate }}% </span>
        </div>

        <div class="progress">
          <div class="progress__bar" :style="{ width: topic.successRate + '%' }"></div>
        </div>
      </div>
    </div>

    <div v-else class="empty">🎉 No weak topics.</div>
  </section>
</template>

<style scoped>
.weak-topics {
  background: var(--surface);

  border: 1px solid var(--border);

  border-radius: 16px;

  padding: 24px;
}

.weak-topics__header {
  margin-bottom: 20px;
}

.weak-topics__header h2 {
  font-size: 1.2rem;

  font-weight: 600;

  color: var(--text-primary);
}

.topics {
  display: flex;

  flex-direction: column;

  gap: 20px;
}

.topic {
  display: flex;

  flex-direction: column;

  gap: 8px;
}

.topic__info {
  display: flex;

  justify-content: space-between;

  align-items: center;
}

.topic__name {
  font-weight: 500;

  color: var(--text-primary);
}

.topic__score {
  color: var(--text-secondary);

  font-size: 0.9rem;
}

.progress {
  width: 100%;

  height: 10px;

  background: var(--border);

  border-radius: 999px;

  overflow: hidden;
}

.progress__bar {
  height: 100%;

  background: var(--warning);

  border-radius: 999px;

  transition: 0.3s;
}

.empty {
  color: var(--text-secondary);

  text-align: center;

  padding: 24px 0;
}
</style>
