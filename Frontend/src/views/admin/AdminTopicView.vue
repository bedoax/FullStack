<script setup>
import { onMounted } from "vue";
import { storeToRefs } from "pinia";

import { useTeacherStore } from "@/stores/teacherStore";
import TopicCard from "@/components/topics/TeacherTopicCard.vue";

const teacherStore = useTeacherStore();
const { topics, loading } = storeToRefs(teacherStore);

// ==============================
// Load topics
// ==============================
onMounted(async () => {
  await teacherStore.loadAllTopics();
});
</script>

<template>
  <section class="topics">
    <!-- ========================= -->
    <!-- Header -->
    <!-- ========================= -->
    <header class="topics__header">
      <div>
        <h1>Topics</h1>
      </div>
    </header>

    <!-- ========================= -->
    <!-- Loading -->
    <!-- ========================= -->
    <div v-if="loading" class="topics__state">Loading topics...</div>

    <!-- ========================= -->
    <!-- Empty -->
    <!-- ========================= -->
    <div v-else-if="topics.length === 0" class="topics__state">
      <div class="empty">
        <h3>No topics found</h3>
      </div>
    </div>

    <!-- ========================= -->
    <!-- Topics Grid -->
    <!-- ========================= -->
    <div v-else class="topics__grid">
      <TopicCard v-for="topic in topics" :key="topic.id" :topic="topic" readOnly />
    </div>
  </section>
</template>

<style scoped>
.topics {
  display: flex;
  flex-direction: column;
  gap: 28px;
}

.topics__header {
  display: flex;

  justify-content: space-between;

  align-items: center;

  gap: 20px;
}

.topics__header h1 {
  margin: 0;

  font-size: 32px;

  font-weight: 700;

  color: var(--text-primary);
}

.topics__grid {
  display: grid;

  grid-template-columns: repeat(auto-fill, minmax(340px, 1fr));

  gap: 22px;
}

.topics__state {
  min-height: 300px;

  display: flex;

  align-items: center;

  justify-content: center;

  color: var(--text-secondary);
}

.empty {
  display: flex;

  flex-direction: column;

  align-items: center;

  gap: 10px;

  text-align: center;
}

.empty h3 {
  margin: 0;

  color: var(--text-primary);

  font-size: 20px;
}

/* ================================= */
/* Responsive */
/* ================================= */

@media (max-width: 700px) {
  .topics__grid {
    grid-template-columns: 1fr;
  }
}
</style>
