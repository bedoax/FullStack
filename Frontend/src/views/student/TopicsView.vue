<script setup>
import { onMounted } from "vue";

import { useStudentStore } from "@/stores/studentStore";

import TopicCard from "@/components/topics/TopicCard.vue";

const studentStore = useStudentStore();

onMounted(async () => {
  if (!studentStore.performance.length) {
    await studentStore.loadPerformance();
  }
});
</script>

<template>
  <section class="topics">
    <header class="topics__header">
      <h1>Topics</h1>

      <p>Track your performance across all topics.</p>
    </header>

    <div v-if="studentStore.loading" class="topics__loading">Loading...</div>

    <div v-else-if="!studentStore.performance.length" class="topics__empty">
      No topics available.
    </div>

    <div v-else class="topics__grid">
      <TopicCard
        v-for="topic in studentStore.performance"
        :key="topic.topicId"
        :topic="topic"
      />
    </div>
  </section>
</template>

<style scoped>
.topics {
  display: flex;

  flex-direction: column;

  gap: 32px;
}

.topics__header {
  display: flex;

  flex-direction: column;

  gap: 8px;
}

.topics__header h1 {
  font-size: 2rem;

  font-weight: 700;

  color: var(--text-primary);
}

.topics__header p {
  color: var(--text-secondary);
}

.topics__grid {
  display: grid;

  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));

  gap: 24px;
}

.topics__loading,
.topics__empty {
  display: flex;

  justify-content: center;

  align-items: center;

  min-height: 250px;

  color: var(--text-secondary);

  font-size: 1rem;
}
</style>
