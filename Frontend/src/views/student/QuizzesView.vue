<script setup>
import { onMounted } from "vue";
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useAbortController } from "@/composables/useAbortController";
import { useQuizStore } from "@/stores/quizStore";
import QuizCard from "@/components/quizzes/QuizCard.vue";
const { signal } = useAbortController();
const router = useRouter();

const quizStore = useQuizStore();

const { myQuizzes, loading } = storeToRefs(quizStore);

onMounted(async () => {
  await quizStore.loadMyQuizzes(signal);
});

async function handleQuizAction(quiz) {
  // 1. Quiz unavailable
  if (!quiz.isAvailable) return;

  // 2. Continue active attempt
  if (quiz.hasActiveAttempt) {
    router.push({
      name: "exam",
      params: {
        attemptId: quiz.activeAttemptId,
      },
      query: {
        quizId: quiz.activeQuizId,
      },
    });

    return;
  }

  // 3. Can start a new attempt
  if (quiz.canStart) {
    router.push({
      name: "exam",
      params: {
        attemptId: "new",
      },
      query: {
        quizId: quiz.id,
      },
    });

    return;
  }

  // 4. No more attempts → review quiz
  /*
    router.push({
        name: "quiz-review",
        params: {
            quizId: quiz.id
        }
    });
    */
}
</script>

<template>
  <section class="quizzes">
    <header class="quizzes__header">
      <div>
        <h1>My Quizzes</h1>

        <p>Start, continue, or review your quizzes.</p>
      </div>
    </header>

    <div v-if="loading" class="quizzes__loading">Loading quizzes...</div>

    <div v-else-if="myQuizzes.length === 0" class="quizzes__empty">
      No quizzes available.
    </div>

    <div v-else class="quizzes__grid">
      <QuizCard
        v-for="quiz in myQuizzes"
        :key="quiz.id"
        :quiz="quiz"
        @click="handleQuizAction(quiz)"
      />
    </div>
  </section>
</template>

<style scoped>
.quizzes {
  display: flex;

  flex-direction: column;

  gap: 32px;
}

.quizzes__header {
  display: flex;

  justify-content: space-between;

  align-items: center;
}

.quizzes__header h1 {
  font-size: 2rem;

  font-weight: 700;

  color: var(--text-primary);
}

.quizzes__header p {
  margin-top: 6px;

  color: var(--text-secondary);
}

.quizzes__grid {
  display: grid;

  grid-template-columns: repeat(auto-fill, minmax(360px, 1fr));

  gap: 24px;
}

.quizzes__loading,
.quizzes__empty {
  display: flex;

  justify-content: center;

  align-items: center;

  min-height: 250px;

  color: var(--text-secondary);

  font-size: 1rem;
}
</style>
