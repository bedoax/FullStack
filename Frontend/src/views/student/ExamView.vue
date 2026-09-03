<script setup>
import { ref, computed, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";

import { useAuthStore } from "@/stores/authStore";
import { useAttemptStore } from "@/stores/attemptStore";

import { attemptService } from "@/services/attemptService";
import { quizService } from "@/services/quizService";

import { execute } from "@/utils/storeHelper";

// =========================
// Router
// =========================

const route = useRoute();
const router = useRouter();

// =========================
// Stores
// =========================

const authStore = useAuthStore();
const attemptStore = useAttemptStore();

// =========================
// State
// =========================

const loading = ref(false);
const submitting = ref(false);

const error = ref("");

// =========================
// Quiz ID
// =========================
const attemptId = computed(() => {
  const value = route.params.attemptId;

  if (!value || value === "new") return null;

  const id = Number(value);

  return id > 0 ? id : null;
});

const quizId = computed(() => {
  const id = Number(route.query.quizId);

  return id > 0 ? id : null;
});

// =========================
// Current Questions
// =========================

const currentQuestions = computed(() => {
  return attemptStore.getCurrentQuestions();
});

// =========================
// Page Number
// =========================

const currentPage = computed(() => {
  return attemptStore.currentPage + 1;
});

const totalPages = computed(() => {
  return Math.ceil(attemptStore.questions.length / attemptStore.questionsPerPage);
});

// =========================
// Progress
// =========================

const progress = computed(() => {
  if (!attemptStore.questions.length) return 0;

  const answered = Object.keys(attemptStore.answers).length;

  return Math.round((answered / attemptStore.questions.length) * 100);
});

// =========================
// Start / Restore Attempt
// =========================

async function initializeExam() {
  error.value = "";

  try {
    await execute(loading, async () => {
      // =========================
      // Validate User
      // =========================

      const userId = authStore.userId;

      if (!userId) {
        throw new Error("User information is not available.");
      }

      // =========================
      // Validate Quiz
      // =========================

      if (!quizId.value) {
        throw new Error("Invalid quiz.");
      }

      // =========================
      // Restore Local Attempt
      // =========================

      const restored = attemptStore.restoreFromStorage();

      if (
        restored &&
        attemptStore.attemptId === attemptId.value &&
        attemptStore.quizId === quizId.value
      ) {
        return;
      }

      // أي Storage قديم/غير مطابق للـroute → امسحه
      attemptStore.clearAttempt();

      // =========================
      // New Attempt
      // =========================

      if (!attemptId.value) {
        const attempt = await attemptService.createAttempt(quizId.value);

        const questions = await quizService.getQuizQuestions(quizId.value);

        if (!questions?.length) {
          throw new Error("No questions found for this quiz.");
        }

        attemptStore.initializeAttempt(attempt.attemptId, quizId.value, questions);

        return;
      }

      // =========================
      // Existing Attempt
      // =========================

      const questions = await quizService.getQuizQuestions(quizId.value);

      if (!questions?.length) {
        throw new Error("No questions found for this quiz.");
      }

      attemptStore.initializeAttempt(attemptId.value, quizId.value, questions);
    });
  } catch (err) {
    error.value = err.response?.data?.message || err.message || "Failed to start exam.";
  }
}

// =========================
// Select Answer
// =========================

function selectAnswer(questionId, optionId) {
  attemptStore.selectAnswer(questionId, optionId);
}

// =========================
// Check Selected Answer
// =========================

function isSelected(questionId, optionId) {
  return attemptStore.getAnswer(questionId) === optionId;
}

// =========================
// Next
// =========================

function nextPage() {
  attemptStore.nextPage();
}

// =========================
// Previous
// =========================

function previousPage() {
  attemptStore.previousPage();
}

// =========================
// Submit
// =========================

async function submitExam() {
  error.value = "";

  // =========================
  // Validate All Questions
  // =========================

  const totalQuestions = attemptStore.questions.length;

  const answeredQuestions = Object.keys(attemptStore.answers).length;

  if (answeredQuestions !== totalQuestions) {
    error.value = "Please answer all questions before submitting.";

    return;
  }

  try {
    submitting.value = true;

    // =========================
    // Prepare Answers
    // =========================

    const answers = attemptStore.getFormattedAnswers();

    // =========================
    // Submit Attempt
    // =========================

    await attemptService.submitAttempt(attemptStore.attemptId, answers);

    attemptStore.clearAttempt();

    await router.push({
      name: "student-attempts",
    });

    console.log("4 - Navigation success");
  } catch (err) {
    console.error("NAVIGATION ERROR:", err);
    throw err;
  } finally {
    submitting.value = false;
  }
}

// =========================
// Mounted
// =========================

onMounted(() => {
  initializeExam();
});
</script>

<template>
  <main class="exam">
    <!-- ========================= -->
    <!-- Header -->
    <!-- ========================= -->

    <header class="exam__header">
      <div>
        <h1>Quiz</h1>

        <p>Answer all questions before submitting.</p>
      </div>

      <div class="exam__progress">
        <span> {{ progress }}% </span>

        <div class="exam__progress-bar">
          <div class="exam__progress-fill" :style="{ width: `${progress}%` }"></div>
        </div>
      </div>
    </header>

    <!-- ========================= -->
    <!-- Error -->
    <!-- ========================= -->

    <div v-if="error" class="exam__error">
      {{ error }}
    </div>

    <!-- ========================= -->
    <!-- Loading -->
    <!-- ========================= -->

    <div v-if="loading" class="exam__loading">Loading exam...</div>

    <!-- ========================= -->
    <!-- Questions -->
    <!-- ========================= -->

    <template v-else>
      <section class="exam__questions">
        <article
          v-for="(question, index) in currentQuestions"
          :key="question.id"
          class="exam-question"
        >
          <div class="exam-question__header">
            <span>
              Question
              {{ attemptStore.currentPage * attemptStore.questionsPerPage + index + 1 }}
            </span>

            <span> {{ question.points }} points </span>
          </div>

          <h2>
            {{ question.content }}
          </h2>

          <div class="exam-question__options">
            <button
              v-for="option in question.options"
              :key="option.id"
              type="button"
              class="exam-option"
              :class="{
                'exam-option--selected': isSelected(question.id, option.id),
              }"
              @click="selectAnswer(question.id, option.id)"
            >
              <span class="exam-option__radio">
                <span v-if="isSelected(question.id, option.id)"></span>
              </span>

              <span>
                {{ option.content }}
              </span>
            </button>
          </div>
        </article>
      </section>

      <!-- ========================= -->
      <!-- Navigation -->
      <!-- ========================= -->

      <footer class="exam__navigation">
        <button
          type="button"
          :disabled="!attemptStore.hasPreviousPage() || submitting"
          @click="previousPage"
        >
          Previous
        </button>

        <span>
          Page
          {{ currentPage }}
          of
          {{ totalPages }}
        </span>

        <button
          v-if="attemptStore.hasNextPage()"
          type="button"
          :disabled="submitting"
          @click="nextPage"
        >
          Next
        </button>

        <button
          v-else
          type="button"
          class="exam__submit"
          :disabled="submitting"
          @click="submitExam"
        >
          {{ submitting ? "Submitting..." : "Submit Attempt" }}
        </button>
      </footer>
    </template>
  </main>
</template>
<style scoped>
/* =========================
   Exam
========================= */

.exam {
  display: flex;
  flex-direction: column;
  gap: 24px;
  max-width: 1000px;
  margin: 0 auto;
  padding: 10px 0 30px;
}

/* =========================
   Header
========================= */

.exam__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 24px;
}

.exam__header h1 {
  margin: 0;
  color: var(--text-primary);
  font-size: 28px;
}

.exam__header p {
  margin: 6px 0 0;
  color: var(--text-secondary);
  font-size: 14px;
}

/* =========================
   Progress
========================= */

.exam__progress {
  min-width: 180px;

  display: flex;
  flex-direction: column;
  gap: 8px;
}

.exam__progress > span {
  align-self: flex-end;

  color: var(--text-secondary);
  font-size: 13px;
  font-weight: 600;
}

.exam__progress-bar {
  width: 100%;
  height: 8px;

  overflow: hidden;

  border-radius: 999px;

  background: var(--border);
}

.exam__progress-fill {
  height: 100%;

  border-radius: inherit;

  background: var(--primary);

  transition: width 0.3s ease;
}

/* =========================
   Messages
========================= */

.exam__error {
  padding: 12px 16px;

  border: 1px solid var(--danger);
  border-radius: 10px;

  background: color-mix(in srgb, var(--danger) 10%, transparent);

  color: var(--danger);

  font-size: 14px;
}

.exam__loading {
  padding: 60px 20px;

  text-align: center;

  color: var(--text-secondary);
}

/* =========================
   Questions
========================= */

.exam__questions {
  display: flex;
  flex-direction: column;
  gap: 18px;
}

/* =========================
   Question Card
========================= */

.exam-question {
  padding: 24px;

  border: 1px solid var(--border);
  border-radius: 16px;

  background: var(--card-background);

  transition: border-color 0.2s ease, box-shadow 0.2s ease;
}

.exam-question:hover {
  border-color: color-mix(in srgb, var(--primary) 35%, var(--border));

  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.05);
}

/* =========================
   Question Header
========================= */

.exam-question__header {
  display: flex;
  align-items: center;
  justify-content: space-between;

  margin-bottom: 18px;
}

.exam-question__header span:first-child {
  color: var(--primary);

  font-size: 13px;
  font-weight: 700;
}

.exam-question__header span:last-child {
  color: var(--text-secondary);

  font-size: 13px;
}

.exam-question h2 {
  margin: 0 0 22px;

  color: var(--text-primary);

  font-size: 18px;
  line-height: 1.6;

  font-weight: 600;
}

/* =========================
   Options
========================= */

.exam-question__options {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.exam-option {
  width: 100%;

  display: flex;
  align-items: center;

  gap: 12px;

  padding: 14px 16px;

  border: 1px solid var(--border);
  border-radius: 11px;

  background: var(--surface);

  color: var(--text-primary);

  text-align: left;

  cursor: pointer;

  font-size: 14px;
  line-height: 1.5;

  transition: border-color 0.2s ease, background 0.2s ease, transform 0.15s ease;
}

.exam-option:hover {
  border-color: var(--primary);

  background: color-mix(in srgb, var(--primary) 5%, var(--surface));

  transform: translateY(-1px);
}

/* =========================
   Option Radio
========================= */

.exam-option__radio {
  flex-shrink: 0;

  width: 19px;
  height: 19px;

  display: flex;
  align-items: center;
  justify-content: center;

  border: 2px solid var(--border);

  border-radius: 50%;

  background: transparent;

  transition: border-color 0.2s ease, background 0.2s ease;
}

.exam-option--selected {
  border-color: var(--primary);

  background: color-mix(in srgb, var(--primary) 8%, var(--surface));
}

.exam-option--selected .exam-option__radio {
  border-color: var(--primary);

  background: var(--primary);
}

.exam-option--selected .exam-option__radio span {
  width: 7px;
  height: 7px;

  border-radius: 50%;

  background: white;
}

/* =========================
   Navigation
========================= */

.exam__navigation {
  display: flex;
  align-items: center;
  justify-content: center;

  gap: 14px;

  padding: 18px;

  border: 1px solid var(--border);
  border-radius: 14px;

  background: var(--card-background);
}

.exam__navigation > span {
  min-width: 100px;

  color: var(--text-secondary);

  font-size: 14px;

  text-align: center;
}

.exam__navigation button {
  min-width: 100px;

  padding: 10px 16px;

  border: 1px solid var(--border);
  border-radius: 10px;

  background: var(--surface);

  color: var(--text-primary);

  cursor: pointer;

  font-size: 14px;
  font-weight: 600;

  transition: background 0.2s ease, border-color 0.2s ease, transform 0.15s ease,
    opacity 0.2s ease;
}

.exam__navigation button:hover:not(:disabled) {
  border-color: var(--primary);

  background: color-mix(in srgb, var(--primary) 6%, var(--surface));

  transform: translateY(-1px);
}

.exam__navigation button:disabled {
  opacity: 0.5;

  cursor: not-allowed;
}

/* =========================
   Submit
========================= */

.exam__navigation .exam__submit {
  border-color: var(--primary);

  background: var(--primary);

  color: white;
}

.exam__navigation .exam__submit:hover:not(:disabled) {
  border-color: var(--primary-hover);

  background: var(--primary-hover);

  color: white;
}

/* =========================
   Responsive
========================= */

@media (max-width: 700px) {
  .exam {
    padding: 0 12px 24px;
  }

  .exam__header {
    flex-direction: column;
    align-items: stretch;
  }

  .exam__progress {
    width: 100%;
    min-width: 0;
  }

  .exam__progress > span {
    align-self: flex-start;
  }

  .exam-question {
    padding: 18px;
  }

  .exam-question h2 {
    font-size: 16px;
  }

  .exam__navigation {
    flex-wrap: wrap;
  }

  .exam__navigation > span {
    order: -1;

    width: 100%;
  }

  .exam__navigation button {
    flex: 1;
  }
}
</style>
