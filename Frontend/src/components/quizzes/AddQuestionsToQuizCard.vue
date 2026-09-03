<script setup>
import { computed, onMounted, ref } from "vue";
import { Check, ChevronLeft, ChevronRight, Plus } from "lucide-vue-next";
import { quizService } from "@/services/quizService";
import { teacherService } from "@/services/teacherService";
import { topicService } from "@/services/topicService";

import QuestionSelectionCard from "@/components/quizzes/QuestionSelectionCard.vue";

const props = defineProps({
  quiz: {
    type: Object,
    required: true,
  },
});

const randomCount = ref(5);
const addingRandom = ref(false);

const topics = ref([]);
const topicsLoading = ref(false);
const selectedTopicId = ref(null);

const questions = ref([]);

const questionsPage = ref(1);

const questionsPageSize = ref(10);

const questionsTotalPages = ref(1);

const questionsTotalCount = ref(0);

const loading = ref(false);
const emit = defineEmits(["added", "cancel"]);

// =====================================
// State
// =====================================

const selectedQuestionIds = ref(new Set());

const saving = ref(false);

const error = ref("");

// =====================================
// Pagination
// =====================================

const currentPage = computed(() => questionsPage.value);

const pageSize = computed(() => questionsPageSize.value);

const totalPages = computed(() => {
  return questionsTotalPages.value || 1;
});

const hasQuestions = computed(() => {
  return questions.value.length > 0;
});

const hasSelection = computed(() => {
  return selectedQuestionIds.value.size > 0;
});

const selectedCount = computed(() => {
  return selectedQuestionIds.value.size;
});

// =====================================
// Selection
// =====================================

function isSelected(questionId) {
  return selectedQuestionIds.value.has(questionId);
}

function toggleQuestion(questionId) {
  const newSelection = new Set(selectedQuestionIds.value);

  if (newSelection.has(questionId)) {
    newSelection.delete(questionId);
  } else {
    newSelection.add(questionId);
  }

  selectedQuestionIds.value = newSelection;
}

// =====================================
// Pagination
// =====================================

async function loadQuestions(page = 1) {
  error.value = "";
  loading.value = true;

  try {
    const response = await teacherService.getQuestionsNotInQuiz(
      props.quiz.id,
      selectedTopicId.value,
      page,
      pageSize.value
    );

    questions.value = response.items;
    questionsPage.value = response.page;
    questionsPageSize.value = response.size;
    questionsTotalCount.value = response.totalCount;
    questionsTotalPages.value = response.totalPages;
  } catch (err) {
    console.error("Failed to load questions:", err);

    error.value = err.response?.data?.message || "Failed to load questions.";
  } finally {
    loading.value = false;
  }
}
async function loadTopics() {
  topicsLoading.value = true;

  try {
    topics.value = await topicService.GetAllTopics();
  } catch (err) {
    console.error("Failed to load topics:", err);

    error.value = err.response?.data?.message || "Failed to load topics.";
  } finally {
    topicsLoading.value = false;
  }
}
async function changeTopic() {
  questionsPage.value = 1;

  selectedQuestionIds.value = new Set();

  await loadQuestions(1);
}
async function goToPage(page) {
  if (
    page < 1 ||
    page > totalPages.value ||
    page === currentPage.value ||
    loading.value ||
    saving.value
  ) {
    return;
  }

  await loadQuestions(page);
}
async function addRandomQuestions() {
  error.value = "";

  if (randomCount.value <= 0) {
    error.value = "Please enter a valid number of questions.";
    return;
  }

  if (questionsTotalCount.value > 0 && randomCount.value > questionsTotalCount.value) {
    error.value = `You can only select up to ${questionsTotalCount.value} questions.`;
    return;
  }

  addingRandom.value = true;

  try {
    await quizService.addRandomQuestionsToQuiz(
      props.quiz.id,
      randomCount.value,
      selectedTopicId.value
    );
    emit("added");
  } catch (err) {
    console.error("Failed to add random questions:", err);
    error.value = err.response?.data?.message || "Failed to add random questions.";
  } finally {
    addingRandom.value = false;
  }
}
// =====================================
// Add Questions
// =====================================

async function addQuestions() {
  error.value = "";

  if (!hasSelection.value) {
    error.value = "Please select at least one question.";

    return;
  }

  saving.value = true;

  try {
    const questionIds = Array.from(selectedQuestionIds.value);

    await quizService.addQuestionsToQuiz(props.quiz.id, questionIds);

    selectedQuestionIds.value = new Set();

    emit("added");
  } catch (err) {
    console.error("Failed to add questions:", err);

    error.value = err.response?.data?.message || "Failed to add questions to this quiz.";
  } finally {
    saving.value = false;
  }
}

// =====================================
// Cancel
// =====================================

function cancel() {
  if (saving.value) {
    return;
  }

  emit("cancel");
}

// =====================================
// Initial Load
// =====================================

onMounted(async () => {
  await loadTopics();
  await loadQuestions(1);
});
</script>

<template>
  <div class="add-questions">
    <!-- ================================= -->
    <!-- Quiz Info -->
    <!-- ================================= -->

    <div class="quiz-info">
      <div>
        <span class="quiz-info__label">Quiz</span>

        <strong class="quiz-info__title">
          {{ quiz.title }}
        </strong>
      </div>

      <div class="quiz-info__count">{{ questionsTotalCount }} questions available</div>
    </div>
    <!-- ================================= -->
    <!-- Random Selection Box -->
    <!-- ================================= -->
    <div class="random-box">
      <div class="random-box__info">
        <strong>Random Selection</strong>
        <span>Add questions randomly from available pool</span>
      </div>

      <div class="random-box__controls">
        <input
          v-model.number="randomCount"
          type="number"
          min="1"
          :max="questionsTotalCount"
          class="random-input"
          :disabled="loading || saving || addingRandom || questionsTotalCount === 0"
        />

        <button
          type="button"
          class="random-button"
          :disabled="loading || saving || addingRandom || questionsTotalCount === 0"
          @click="addRandomQuestions"
        >
          <Plus :size="16" />
          {{ addingRandom ? "Adding..." : `Add ${randomCount || 0} Random` }}
        </button>
      </div>
    </div>
    <!-- ================================= -->
    <!-- Topic Filter -->
    <!-- ================================= -->
    <div class="topic-filter">
      <label for="topic"> Filter by topic </label>

      <select
        id="topic"
        v-model="selectedTopicId"
        :disabled="topicsLoading || loading || saving"
        @change="changeTopic"
      >
        <option :value="null">All Topics</option>

        <option v-for="topic in topics" :key="topic.id" :value="topic.id">
          {{ topic.name }}
        </option>
      </select>
    </div>
    <!-- ================================= -->
    <!-- Selection Summary -->
    <!-- ================================= -->

    <div class="selection-bar">
      <div>
        <strong>
          {{ selectedCount }}
        </strong>

        question{{ selectedCount === 1 ? "" : "s" }} selected
      </div>

      <button
        v-if="hasSelection"
        type="button"
        class="add-selected-button"
        :disabled="saving"
        @click="addQuestions"
      >
        <Plus :size="17" />

        {{ saving ? "Adding..." : "Add Selected" }}
      </button>
    </div>

    <!-- ================================= -->
    <!-- Error -->
    <!-- ================================= -->

    <div v-if="error" class="form-error">
      {{ error }}
    </div>

    <!-- ================================= -->
    <!-- Loading -->
    <!-- ================================= -->

    <div v-if="loading && questions.length === 0" class="questions-state">
      Loading questions...
    </div>

    <!-- ================================= -->
    <!-- Empty -->
    <!-- ================================= -->

    <div v-else-if="!hasQuestions" class="questions-state">
      <h3>No questions found</h3>

      <p>Create questions first before adding them to a quiz.</p>
    </div>

    <!-- ================================= -->
    <!-- Questions -->
    <!-- ================================= -->

    <template v-else>
      <div class="questions-list">
        <QuestionSelectionCard
          v-for="question in questions"
          :key="question.id"
          :question="question"
          :selected="isSelected(question.id)"
          :disabled="saving"
          @toggle="toggleQuestion"
        />
      </div>

      <!-- ================================= -->
      <!-- Pagination -->
      <!-- ================================= -->

      <div v-if="totalPages > 1" class="pagination">
        <button
          type="button"
          class="pagination__button"
          :disabled="currentPage === 1 || loading || saving"
          @click="goToPage(currentPage - 1)"
        >
          <ChevronLeft :size="18" />

          Previous
        </button>

        <div class="pagination__pages">
          <button
            v-for="page in totalPages"
            :key="page"
            type="button"
            class="pagination__page"
            :class="{
              active: page === currentPage,
            }"
            :disabled="loading || saving"
            @click="goToPage(page)"
          >
            {{ page }}
          </button>
        </div>

        <button
          type="button"
          class="pagination__button"
          :disabled="currentPage === totalPages || loading || saving"
          @click="goToPage(currentPage + 1)"
        >
          Next

          <ChevronRight :size="18" />
        </button>
      </div>
    </template>

    <!-- ================================= -->
    <!-- Actions -->
    <!-- ================================= -->

    <footer class="add-questions__actions">
      <button type="button" class="cancel-button" :disabled="saving" @click="cancel">
        Cancel
      </button>

      <button
        type="button"
        class="add-button"
        :disabled="!hasSelection || saving"
        @click="addQuestions"
      >
        <Check :size="17" />

        {{
          saving
            ? "Adding..."
            : `Add ${selectedCount} Question${selectedCount === 1 ? "" : "s"}`
        }}
      </button>
    </footer>
  </div>
</template>

<style scoped>
.add-questions {
  display: flex;
  flex-direction: column;
  gap: 18px;
}

/* ================================= */
/* Quiz Info */
/* ================================= */

.quiz-info {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;

  padding: 14px 16px;

  border: 1px solid var(--border);
  border-radius: 12px;

  background: var(--background-color);
}

.quiz-info__label {
  display: block;

  margin-bottom: 4px;

  font-size: 12px;

  color: var(--text-secondary);
}

.quiz-info__title {
  color: var(--text-primary);

  font-size: 15px;
}

.quiz-info__count {
  color: var(--text-secondary);

  font-size: 13px;

  white-space: nowrap;
}

/* ================================= */
/* Selection Bar */
/* ================================= */

.selection-bar {
  display: flex;

  align-items: center;

  justify-content: space-between;

  gap: 12px;

  padding: 12px 14px;

  border-radius: 10px;

  background: color-mix(in srgb, var(--primary) 8%, transparent);

  border: 1px solid color-mix(in srgb, var(--primary) 18%, transparent);

  color: var(--text-primary);

  font-size: 14px;
}

.selection-bar strong {
  color: var(--primary);
}

/* ================================= */
/* Add Selected */
/* ================================= */

.add-selected-button {
  display: flex;

  align-items: center;

  gap: 6px;

  padding: 8px 12px;

  border: none;

  border-radius: 8px;

  background: var(--primary);

  color: white;

  font-size: 13px;

  font-weight: 600;

  cursor: pointer;
}

.add-selected-button:hover {
  background: var(--primary-hover);
}

.add-selected-button:disabled {
  opacity: 0.6;

  cursor: not-allowed;
}

/* ================================= */
/* Questions */
/* ================================= */

.questions-list {
  display: flex;

  flex-direction: column;

  gap: 10px;

  max-height: 480px;

  overflow-y: auto;

  padding-right: 4px;
}

/* ================================= */
/* State */
/* ================================= */

.questions-state {
  min-height: 240px;

  display: flex;

  flex-direction: column;

  align-items: center;

  justify-content: center;

  text-align: center;

  color: var(--text-secondary);
}

.questions-state h3 {
  margin: 0 0 6px;

  color: var(--text-primary);

  font-size: 18px;
}

.questions-state p {
  margin: 0;

  font-size: 14px;
}

/* ================================= */
/* Error */
/* ================================= */

.form-error {
  padding: 10px 12px;

  border-radius: 9px;

  background: rgba(239, 68, 68, 0.1);

  color: #dc2626;

  font-size: 14px;
}

/* ================================= */
/* Pagination */
/* ================================= */

.pagination {
  display: flex;

  align-items: center;

  justify-content: center;

  gap: 16px;

  padding: 6px 0;
}

.pagination__pages {
  display: flex;
  flex-wrap: wrap;
  flex-grow: 1;

  gap: 6px;
}

.pagination__button,
.pagination__page {
  height: 36px;

  border: 1px solid var(--border);

  border-radius: 8px;

  background: var(--surface);

  color: var(--text-primary);

  cursor: pointer;

  transition: 0.2s;
}

.pagination__button {
  display: flex;

  align-items: center;

  gap: 6px;

  padding: 0 11px;
}

.pagination__page {
  width: 36px;
}

.pagination__button:hover:not(:disabled),
.pagination__page:hover:not(:disabled) {
  border-color: var(--primary);

  color: var(--primary);
}

.pagination__page.active {
  background: var(--primary);

  border-color: var(--primary);

  color: white;
}

.pagination__button:disabled,
.pagination__page:disabled {
  opacity: 0.45;

  cursor: not-allowed;
}

/* ================================= */
/* Actions */
/* ================================= */

.add-questions__actions {
  display: flex;

  justify-content: flex-end;

  gap: 10px;

  padding-top: 16px;

  border-top: 1px solid var(--border);
}

.cancel-button,
.add-button {
  display: flex;

  align-items: center;

  justify-content: center;

  gap: 7px;

  height: 40px;

  padding: 0 15px;

  border: none;

  border-radius: 9px;

  font-size: 13px;

  font-weight: 600;

  cursor: pointer;
}

.cancel-button {
  background: var(--sidebar-hover);

  color: var(--text-primary);
}

.cancel-button:hover:not(:disabled) {
  background: var(--border);
}

.add-button {
  background: var(--primary);

  color: white;
}

.add-button:hover:not(:disabled) {
  background: var(--primary-hover);
}

.cancel-button:disabled,
.add-button:disabled {
  opacity: 0.5;

  cursor: not-allowed;
}
.random-box {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  padding: 12px 16px;
  background: var(--surface);
  border: 1px dashed var(--border);
  border-radius: 10px;
}

.random-box__info {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.random-box__info strong {
  font-size: 14px;
  color: var(--text-primary);
}

.random-box__info span {
  font-size: 12px;
  color: var(--text-secondary);
}

.random-box__controls {
  display: flex;
  align-items: center;
  gap: 8px;
}

.random-input {
  width: 70px;
  height: 36px;
  padding: 0 8px;
  border: 1px solid var(--border);
  border-radius: 8px;
  background: var(--background-color);
  color: var(--text-primary);
  text-align: center;
}

.random-button {
  display: flex;
  align-items: center;
  gap: 6px;
  height: 36px;
  padding: 0 14px;
  border: none;
  border-radius: 8px;
  background: var(--primary);
  color: white;
  font-weight: 600;
  font-size: 13px;
  cursor: pointer;
  white-space: nowrap;
}

.random-button:hover:not(:disabled) {
  background: var(--primary-hover);
}

.random-button:disabled,
.random-input:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
/* ================================= */
/* Responsive */
/* ================================= */

@media (max-width: 600px) {
  .quiz-info,
  .selection-bar {
    flex-direction: column;

    align-items: stretch;
  }

  .quiz-info__count {
    white-space: normal;
  }

  .questions-list {
    max-height: 400px;
  }

  .pagination {
    flex-wrap: wrap;
  }

  .add-questions__actions {
    flex-direction: column-reverse;
  }

  .cancel-button,
  .add-button {
    width: 100%;
  }
}
</style>
