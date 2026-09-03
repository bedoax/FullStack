<script setup>
import { computed, onMounted, ref } from "vue";
import { storeToRefs } from "pinia";
import { Plus, X, ChevronLeft, ChevronRight } from "lucide-vue-next";
import { useTeacherStore } from "@/stores/teacherStore";
import { questionService } from "@/services/questionService";
import TeacherQuestionCard from "@/components/question/TeacherQuestionCard.vue";

const teacherStore = useTeacherStore();

// =====================================
// Store
// =====================================

const {
  questions,
  topics,
  questionsPage,
  questionsPageSize,
  questionsTotalCount,
  questionsTotalPages,
  loading,
} = storeToRefs(teacherStore);

// =====================================
// Modal
// =====================================

const showModal = ref(false);
const editingQuestion = ref(null);
const saving = ref(false);
const error = ref("");

// =====================================
// Form
// =====================================

const form = ref({
  content: "",
  topicId: "",
  difficulty: "Easy",
  points: 1,
});

// =====================================
// Computed
// =====================================

const isEditing = computed(() => editingQuestion.value !== null);

// =====================================
// Load Questions
// =====================================

async function loadQuestions(page = 1) {
  await teacherStore.loadMyQuestions(page, questionsPageSize.value);
}

// =====================================
// Pagination
// =====================================

async function goToPage(page) {
  if (
    page < 1 ||
    page > questionsTotalPages.value ||
    page === questionsPage.value ||
    loading.value
  ) {
    return;
  }
  await loadQuestions(page);
}

// =====================================
// Create
// =====================================

function openCreateModal() {
  editingQuestion.value = null;
  form.value = {
    content: "",
    topicId: topics.value.length ? topics.value[0].id : "",
    difficulty: "Easy",
    points: 1,
  };
  error.value = "";
  showModal.value = true;
}

// =====================================
// Edit
// =====================================

function openEditModal(question) {
  editingQuestion.value = question;
  form.value = {
    content: question.content ?? "",
    topicId: question.topicId ?? "",
    difficulty: question.difficulty ?? "Easy",
    points: question.points ?? 1,
  };
  error.value = "";
  showModal.value = true;
}

// =====================================
// Close Modal
// =====================================

function closeModal() {
  if (saving.value) return;
  showModal.value = false;
  editingQuestion.value = null;
  error.value = "";
}

// =====================================
// Save Question
// =====================================

async function saveQuestion() {
  error.value = "";
  const content = form.value.content.trim();

  if (!content) {
    error.value = "Question content is required.";
    return;
  }
  if (!form.value.topicId) {
    error.value = "Please select a topic.";
    return;
  }
  if (!["Easy", "Medium", "Hard"].includes(form.value.difficulty)) {
    error.value = "Invalid difficulty.";
    return;
  }
  if (
    form.value.points === null ||
    form.value.points === undefined ||
    Number(form.value.points) <= 0
  ) {
    error.value = "Points must be greater than zero.";
    return;
  }

  saving.value = true;
  try {
    const payload = {
      content,
      topicId: Number(form.value.topicId),
      difficulty: form.value.difficulty,
      points: Number(form.value.points),
    };

    if (editingQuestion.value) {
      await questionService.UpdateQuestion(editingQuestion.value.id, payload);
    } else {
      await questionService.CreateQuestion(payload);
    }

    closeModal();
    await loadQuestions(questionsPage.value);
  } catch (err) {
    console.error("Question save error:", err);
    error.value = err.response?.data?.message || "Failed to save question.";
  } finally {
    saving.value = false;
  }
}

// =====================================
// Initial Load
// =====================================

onMounted(async () => {
  await teacherStore.loadAllTopics();
  await loadQuestions(1);
});
</script>

<template>
  <section class="questions">
    <!-- ================================= -->
    <!-- Header -->
    <!-- ================================= -->

    <header class="questions__header">
      <div>
        <h1>Questions</h1>
        <p>Manage the questions in your question bank.</p>
      </div>
      <button type="button" class="add-question-button" @click="openCreateModal">
        <Plus :size="18" />
        Add Question
      </button>
    </header>

    <!-- ================================= -->
    <!-- Loading -->
    <!-- ================================= -->

    <div v-if="loading" class="questions__state">Loading questions...</div>

    <!-- ================================= -->
    <!-- Empty -->
    <!-- ================================= -->

    <div v-else-if="questions.length === 0" class="questions__state">
      <div class="empty">
        <h3>No questions found</h3>
        <p>Create your first question to start building your question bank.</p>
        <button type="button" class="add-question-button" @click="openCreateModal">
          <Plus :size="18" />
          Add Question
        </button>
      </div>
    </div>

    <!-- ================================= -->
    <!-- Questions -->
    <!-- ================================= -->

    <template v-else>
      <div class="questions__grid">
        <TeacherQuestionCard
          v-for="question in questions"
          :key="question.id"
          :question="question"
          :topics="topics"
          @edit="openEditModal"
        />
      </div>

      <!-- ================================= -->
      <!-- Pagination -->
      <!-- ================================= -->

      <div v-if="questionsTotalPages > 1" class="pagination">
        <button
          type="button"
          class="pagination__button"
          :disabled="questionsPage === 1 || loading"
          @click="goToPage(questionsPage - 1)"
        >
          <ChevronLeft :size="18" />
          Previous
        </button>

        <div class="pagination__pages">
          <button
            v-for="page in questionsTotalPages"
            :key="page"
            type="button"
            class="pagination__page"
            :class="{ active: page === questionsPage }"
            :disabled="loading"
            @click="goToPage(page)"
          >
            {{ page }}
          </button>
        </div>

        <button
          type="button"
          class="pagination__button"
          :disabled="questionsPage === questionsTotalPages || loading"
          @click="goToPage(questionsPage + 1)"
        >
          Next
          <ChevronRight :size="18" />
        </button>
      </div>
    </template>

    <!-- ================================= -->
    <!-- Create / Edit Modal -->
    <!-- ================================= -->

    <Teleport to="body">
      <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
        <div class="modal">
          <!-- Modal Header -->
          <header class="modal__header">
            <div>
              <h2>{{ isEditing ? "Edit Question" : "Add Question" }}</h2>
              <p>
                {{
                  isEditing
                    ? "Update the question information."
                    : "Create a new question."
                }}
              </p>
            </div>
            <button
              type="button"
              class="modal__close"
              :disabled="saving"
              @click="closeModal"
            >
              <X :size="20" />
            </button>
          </header>

          <!-- Form -->
          <form class="question-form" @submit.prevent="saveQuestion">
            <!-- Content -->
            <div class="form-group">
              <label for="question-content">Question</label>
              <textarea
                id="question-content"
                v-model="form.content"
                rows="5"
                maxlength="2000"
                placeholder="Write the question..."
                :disabled="saving"
              />
            </div>

            <!-- Topic -->
            <div class="form-group">
              <label for="question-topic">Topic</label>
              <select id="question-topic" v-model="form.topicId" :disabled="saving">
                <option value="" disabled>Select a topic</option>
                <option v-for="topic in topics" :key="topic.id" :value="topic.id">
                  {{ topic.name }}
                </option>
              </select>
            </div>

            <!-- Difficulty + Points -->
            <div class="form-row">
              <div class="form-group">
                <label for="question-difficulty">Difficulty</label>
                <select
                  id="question-difficulty"
                  v-model="form.difficulty"
                  :disabled="saving"
                >
                  <option value="Easy">Easy</option>
                  <option value="Medium">Medium</option>
                  <option value="Hard">Hard</option>
                </select>
              </div>
              <div class="form-group">
                <label for="question-points">Points</label>
                <input
                  id="question-points"
                  v-model.number="form.points"
                  type="number"
                  min="1"
                  step="1"
                  :disabled="saving"
                />
              </div>
            </div>

            <!-- Error -->
            <div v-if="error" class="form-error">{{ error }}</div>

            <!-- Actions -->
            <div class="modal__actions">
              <button
                type="button"
                class="cancel-button"
                :disabled="saving"
                @click="closeModal"
              >
                Cancel
              </button>
              <button type="submit" class="save-button" :disabled="saving">
                {{
                  saving ? "Saving..." : isEditing ? "Update Question" : "Create Question"
                }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </Teleport>
  </section>
</template>

<style scoped>
/* =============================================== */
/* Section Layout                                  */
/* =============================================== */

.questions {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1.5rem;
}

/* =============================================== */
/* Header                                          */
/* =============================================== */

.questions__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 1rem;
  margin-bottom: 2rem;
}

.questions__header h1 {
  font-size: 1.75rem;
  font-weight: 700;
  color: var(--text-primary);
}

.questions__header p {
  color: var(--text-secondary);
  font-size: 0.95rem;
  margin-top: 0.2rem;
}

/* =============================================== */
/* Add Question Button                             */
/* =============================================== */

.add-question-button {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.6rem 1.2rem;
  background: var(--primary);
  color: #fff;
  border: none;
  border-radius: 10px;
  font-weight: 600;
  font-size: 0.95rem;
  transition: 0.2s;
  cursor: pointer;
}

.add-question-button:hover {
  background: var(--primary-hover);
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(34, 197, 94, 0.3);
}

.add-question-button:active {
  transform: translateY(0);
}

/* =============================================== */
/* State messages (loading / empty)                */
/* =============================================== */

.questions__state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 4rem 2rem;
  text-align: center;
  background: var(--surface);
  border-radius: 16px;
  border: 1px solid var(--border);
}

.questions__state .empty h3 {
  font-size: 1.3rem;
  font-weight: 600;
  color: var(--text-primary);
  margin-bottom: 0.5rem;
}

.questions__state .empty p {
  color: var(--text-secondary);
  margin-bottom: 1.5rem;
}

/* =============================================== */
/* Grid of question cards                          */
/* =============================================== */

.questions__grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2.5rem;
}

/* =============================================== */
/* Pagination                                      */
/* =============================================== */

.pagination {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.75rem;
  flex-wrap: wrap;
  margin-top: 2rem;
}

.pagination__button {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  padding: 0.5rem 1rem;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 10px;
  font-weight: 500;
  color: var(--text-primary);
  transition: 0.2s;
  cursor: pointer;
}

.pagination__button:hover:not(:disabled) {
  background: var(--sidebar-hover);
  border-color: var(--primary);
}

.pagination__button:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

/* --- Page number grid --- */
.pagination__pages {
  display: grid;
  grid-template-columns: repeat(10, 2.4rem); /* 10 columns, each 2.4rem wide */
  gap: 0.4rem;
  justify-content: center;
}

.pagination__page {
  width: 2.4rem;
  height: 2.4rem;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 10px;
  border: 1px solid transparent;
  background: transparent;
  font-weight: 500;
  font-size: 0.9rem;
  color: var(--text-secondary);
  transition: 0.2s;
  cursor: pointer;
}

.pagination__page:hover:not(:disabled) {
  background: var(--sidebar-hover);
  border-color: var(--border);
}

.pagination__page.active {
  background: var(--primary);
  color: #fff;
  border-color: var(--primary);
}

.pagination__page:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

/* =============================================== */
/* Modal Overlay & Container                       */
/* =============================================== */

.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 1rem;
}

.modal {
  background: var(--surface);
  border-radius: 20px;
  max-width: 600px;
  width: 100%;
  max-height: 90vh;
  overflow-y: auto;
  padding: 2rem 2rem 1.5rem;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.2);
  animation: modalFadeIn 0.25s ease;
}

@keyframes modalFadeIn {
  from {
    opacity: 0;
    transform: scale(0.96) translateY(10px);
  }
  to {
    opacity: 1;
    transform: scale(1) translateY(0);
  }
}

/* Modal header */
.modal__header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.modal__header h2 {
  font-size: 1.4rem;
  font-weight: 700;
  color: var(--text-primary);
}

.modal__header p {
  color: var(--text-secondary);
  font-size: 0.9rem;
  margin-top: 0.2rem;
}

.modal__close {
  flex-shrink: 0;
  width: 2.2rem;
  height: 2.2rem;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 10px;
  background: transparent;
  border: none;
  color: var(--text-secondary);
  transition: 0.2s;
  cursor: pointer;
}

.modal__close:hover:not(:disabled) {
  background: var(--sidebar-hover);
  color: var(--text-primary);
}

.modal__close:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

/* =============================================== */
/* Form                                            */
/* =============================================== */

.question-form {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

.form-group label {
  font-weight: 600;
  font-size: 0.9rem;
  color: var(--text-primary);
}

.form-group input,
.form-group textarea,
.form-group select {
  width: 100%;
  padding: 0.65rem 0.9rem;
  border: 1px solid var(--border);
  border-radius: 10px;
  background: var(--background);
  color: var(--text-primary);
  font-size: 0.95rem;
  transition: 0.2s;
}

.form-group input:focus,
.form-group textarea:focus,
.form-group select:focus {
  border-color: var(--primary);
  box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.15);
  outline: none;
}

.form-group textarea {
  resize: vertical;
  min-height: 100px;
}

.form-group input:disabled,
.form-group textarea:disabled,
.form-group select:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

/* Error message */
.form-error {
  padding: 0.6rem 0.9rem;
  background: rgba(239, 68, 68, 0.1);
  border: 1px solid rgba(239, 68, 68, 0.25);
  border-radius: 10px;
  color: var(--danger);
  font-size: 0.9rem;
}

/* =============================================== */
/* Modal Actions (buttons)                         */
/* =============================================== */

.modal__actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.8rem;
  margin-top: 0.5rem;
}

.cancel-button,
.save-button {
  padding: 0.6rem 1.4rem;
  border-radius: 10px;
  font-weight: 600;
  font-size: 0.95rem;
  border: none;
  transition: 0.2s;
  cursor: pointer;
}

.cancel-button {
  background: transparent;
  color: var(--text-secondary);
  border: 1px solid var(--border);
}

.cancel-button:hover:not(:disabled) {
  background: var(--sidebar-hover);
  color: var(--text-primary);
}

.cancel-button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.save-button {
  background: var(--primary);
  color: #fff;
}

.save-button:hover:not(:disabled) {
  background: var(--primary-hover);
  box-shadow: 0 4px 12px rgba(34, 197, 94, 0.3);
}

.save-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* =============================================== */
/* Responsive                                      */
/* =============================================== */

@media (max-width: 640px) {
  .questions {
    padding: 1.5rem 1rem;
  }

  .questions__header {
    flex-direction: column;
    align-items: stretch;
    text-align: center;
  }

  .add-question-button {
    justify-content: center;
  }

  .form-row {
    grid-template-columns: 1fr;
  }

  .modal {
    padding: 1.5rem 1.2rem;
  }

  .modal__actions {
    flex-direction: column-reverse;
  }

  .cancel-button,
  .save-button {
    width: 100%;
    justify-content: center;
  }

  .pagination__pages {
    grid-template-columns: repeat(5, 2.4rem); /* 5 columns on mobile */
    gap: 0.3rem;
  }

  .pagination__button span {
    display: none; /* hide "Previous"/"Next" text */
  }

  .pagination__button {
    padding: 0.5rem 0.8rem;
  }
}

@media (max-width: 400px) {
  .pagination__pages {
    grid-template-columns: repeat(4, 2.2rem);
  }
  .pagination__page {
    width: 2.2rem;
    height: 2.2rem;
    font-size: 0.8rem;
  }
}
</style>
