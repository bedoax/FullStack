<script setup>
import { onMounted, ref } from "vue";

import { quizService } from "@/services/quizService";
import TeacherQuizCard from "@/components/quizzes/TeacherQuizCard.vue";
import { useAbortController } from "@/composables/useAbortController";

// =====================================
// Reactive States
// =====================================
const quizzes = ref([]);
const loading = ref(false);
const { signal } = useAbortController();

// =====================================
// Fetch Quizzes
// =====================================
async function fetchQuizzes() {
  loading.value = true;

  try {
    const res = await quizService.getAllPublishedQuizzes(signal);
    quizzes.value = res.data || res || [];
  } catch (err) {
    console.error("Failed to load quizzes:", err);
  } finally {
    loading.value = false;
  }
}

// =====================================
// Initial Load
// =====================================
onMounted(() => {
  fetchQuizzes();
});
</script>

<template>
  <section class="quizzes">
    <!-- ================================= -->
    <!-- Header -->
    <!-- ================================= -->
    <header class="quizzes__header">
      <div>
        <h1>Quizzes</h1>
      </div>
    </header>

    <!-- ================================= -->
    <!-- Loading -->
    <!-- ================================= -->
    <div v-if="loading && quizzes.length === 0" class="quizzes__state">
      Loading quizzes...
    </div>

    <!-- ================================= -->
    <!-- Empty -->
    <!-- ================================= -->
    <div v-else-if="quizzes.length === 0" class="quizzes__state">
      <div class="empty">
        <h3>No quizzes found</h3>
      </div>
    </div>

    <!-- ================================= -->
    <!-- Quiz Cards -->
    <!-- ================================= -->
    <div v-else class="quizzes__grid">
      <TeacherQuizCard v-for="quiz in quizzes" :key="quiz.id" :quiz="quiz" read-only />
    </div>
  </section>
</template>

<style scoped>
.quizzes {
  display: flex;
  flex-direction: column;
  gap: 28px;
}

/* ================================= */
/* Header */
/* ================================= */
.quizzes__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 20px;
}

.quizzes__header h1 {
  margin: 0;
  font-size: 32px;
  font-weight: 700;
  color: var(--text-primary);
}

/* ================================= */
/* Grid */
/* ================================= */
.quizzes__grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(340px, 1fr));
  gap: 22px;
}

/* ================================= */
/* State */
/* ================================= */
.quizzes__state {
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
  .quizzes__header {
    flex-direction: column;
    align-items: stretch;
  }

  .quizzes__grid {
    grid-template-columns: 1fr;
  }
}
</style>
