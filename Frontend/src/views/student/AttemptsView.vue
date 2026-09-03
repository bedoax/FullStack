<script setup>
import { onMounted, ref } from "vue";

import AttemptCard from "@/components/attempts/AttemptCard.vue";
import { attemptService } from "@/services/attemptService";
import { useAbortController } from "@/composables/useAbortController";
const { signal } = useAbortController();
const attempts = ref([]);

const loading = ref(false);

const error = ref("");

async function loadAttempts(signal) {
  loading.value = true;
  error.value = "";
  try {
    attempts.value = await attemptService.getMyAttempts(signal);
  } catch (err) {
    error.value = err.response?.data?.message || "Failed to load attempts.";
  } finally {
    loading.value = false;
  }
}

onMounted(() => {
  loadAttempts(signal);
});
</script>

<template>
  <div class="attempts">
    <header class="attempts__header">
      <h1>My Attempts</h1>

      <p>Review all quizzes you have completed.</p>
    </header>

    <div v-if="loading" class="attempts__loading">Loading...</div>

    <div v-else-if="error" class="attempts__error">
      {{ error }}
    </div>

    <div v-else-if="attempts.length === 0" class="attempts__empty">
      No attempts found.
    </div>

    <div v-else class="attempts__grid">
      <AttemptCard v-for="attempt in attempts" :key="attempt.id" :attempt="attempt" />
    </div>
  </div>
</template>

<style scoped>
.attempts {
  max-width: 1200px;
  margin: auto;
  padding: 32px;
}

.attempts__header {
  margin-bottom: 32px;
}

.attempts__header h1 {
  margin: 0;
  font-size: 32px;
  font-weight: 700;
}

.attempts__header p {
  margin-top: 8px;
  color: #6b7280;
}

.attempts__grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(360px, 1fr));
  gap: 24px;
}

.attempts__loading,
.attempts__error,
.attempts__empty {
  padding: 40px;
  text-align: center;
  font-size: 18px;
}

.attempts__error {
  color: #dc2626;
}

.attempts__empty {
  color: #6b7280;
}
</style>
