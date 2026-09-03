<script setup>
import { onMounted, ref } from "vue";
import { storeToRefs } from "pinia";
import { Plus, X } from "lucide-vue-next";

import { useTeacherStore } from "@/stores/teacherStore";
import { useAbortController } from "@/composables/useAbortController";

import TeacherQuizCard from "@/components/quizzes/TeacherQuizCard.vue";
import CreateQuizCard from "@/components/quizzes/CreateQuizCard.vue";
import AddQuestionsToQuizCard from "@/components/quizzes/AddQuestionsToQuizCard.vue";
import EditQuizCard from "@/components/quizzes/EditQuizCard.vue";

const teacherStore = useTeacherStore();
const { signal } = useAbortController();

const { quizzes, loading } = storeToRefs(teacherStore);

// =====================================
// Modal state
// =====================================
const showEditQuiz = ref(false);

const showCreateQuiz = ref(false);

const showAddQuestions = ref(false);

const selectedQuiz = ref(null);

// =====================================
// Create Quiz
// =====================================

function openCreateQuiz() {
  showCreateQuiz.value = true;
}

function closeCreateQuiz() {
  showCreateQuiz.value = false;
}

function openEditQuiz(quiz) {
  selectedQuiz.value = quiz;
  showEditQuiz.value = true;
}

function closeEditQuiz() {
  showEditQuiz.value = false;
  selectedQuiz.value = null;
}
async function handleQuizUpdated() {
  closeEditQuiz();
  await teacherStore.loadMyQuizzes(signal);
}
// =====================================
// Add Questions
// =====================================

function openAddQuestions(quiz) {
  selectedQuiz.value = quiz;

  showAddQuestions.value = true;
}

function closeAddQuestions() {
  showAddQuestions.value = false;

  selectedQuiz.value = null;
}

// =====================================
// Success
// =====================================

async function handleQuizCreated() {
  closeCreateQuiz();

  await teacherStore.loadMyQuizzes(signal);
}

async function handleQuestionsAdded() {
  closeAddQuestions();
}

// =====================================
// Initial Load
// =====================================

onMounted(async () => {
  await teacherStore.loadMyQuizzes(signal);
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

        <p>Create and manage your quizzes.</p>
      </div>

      <button type="button" class="add-quiz-button" @click="openCreateQuiz">
        <Plus :size="18" />

        Add Quiz
      </button>
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

        <p>Create your first quiz to start building an assessment.</p>

        <button type="button" class="add-quiz-button" @click="openCreateQuiz">
          <Plus :size="18" />

          Add Quiz
        </button>
      </div>
    </div>

    <!-- ================================= -->
    <!-- Quiz Cards -->
    <!-- ================================= -->

    <div v-else class="quizzes__grid">
      <TeacherQuizCard
        v-for="quiz in quizzes"
        :key="quiz.id"
        :quiz="quiz"
        @manage-questions="openAddQuestions"
        @edit="openEditQuiz"
      />
    </div>

    <!-- ================================= -->
    <!-- Create Quiz Modal -->
    <!-- ================================= -->

    <Teleport to="body">
      <div v-if="showCreateQuiz" class="modal-overlay" @click.self="closeCreateQuiz">
        <div class="modal">
          <header class="modal__header">
            <div>
              <h2>Create Quiz</h2>

              <p>Create a new quiz for your students.</p>
            </div>

            <button type="button" class="modal__close" @click="closeCreateQuiz">
              <X :size="20" />
            </button>
          </header>

          <CreateQuizCard @created="handleQuizCreated" @cancel="closeCreateQuiz" />
        </div>
      </div>
    </Teleport>

    <!-- ================================= -->
    <!-- Add Questions Modal -->
    <!-- ================================= -->

    <Teleport to="body">
      <div v-if="showAddQuestions" class="modal-overlay" @click.self="closeAddQuestions">
        <div class="modal modal--questions">
          <header class="modal__header">
            <div>
              <h2>Add Questions</h2>

              <p v-if="selectedQuiz">
                Add questions to:

                <strong>
                  {{ selectedQuiz.title }}
                </strong>
              </p>
            </div>

            <button type="button" class="modal__close" @click="closeAddQuestions">
              <X :size="20" />
            </button>
          </header>

          <AddQuestionsToQuizCard
            v-if="selectedQuiz"
            :quiz="selectedQuiz"
            @added="handleQuestionsAdded"
            @cancel="closeAddQuestions"
          />
        </div>
      </div>
    </Teleport>

    <!-- ================================= -->
    <!-- Edit Quiz Modal -->
    <!-- ================================= -->
    <Teleport to="body">
      <div v-if="showEditQuiz" class="modal-overlay" @click.self="closeEditQuiz">
        <div class="modal">
          <header class="modal__header">
            <div>
              <h2>Edit Quiz</h2>

              <p v-if="selectedQuiz">
                Update
                <strong>{{ selectedQuiz.title }}</strong>
              </p>
            </div>

            <button type="button" class="modal__close" @click="closeEditQuiz">
              <X :size="20" />
            </button>
          </header>

          <EditQuizCard
            v-if="selectedQuiz"
            :quiz="selectedQuiz"
            @updated="handleQuizUpdated"
            @cancel="closeEditQuiz"
          />
        </div>
      </div>
    </Teleport>
  </section>
</template>

<style scoped>
.quizzes {
  display: flex;
  flex-direction: column;
  gap: 28px;
}

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
  line-height: 1.2;
}

.quizzes__header p {
  margin: 7px 0 0;
  color: var(--text-secondary);
  font-size: 15px;
}

.add-quiz-button {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 12px 20px;
  min-height: 44px;
  border: none;
  border-radius: 12px;
  background: var(--primary);
  color: white;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  white-space: nowrap;
  flex-shrink: 0;
}

.add-quiz-button:hover {
  background: var(--primary-hover);
}

.add-quiz-button:active {
  transform: scale(0.98);
}

.quizzes__grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 22px;
}

.quizzes__state {
  min-height: 280px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--text-secondary);
  padding: 20px;
}

.empty {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
  text-align: center;
  max-width: 400px;
}

.empty h3 {
  margin: 0;
  color: var(--text-primary);
  font-size: 20px;
  font-weight: 600;
}

.empty p {
  margin: 0 0 12px;
  color: var(--text-secondary);
  font-size: 14px;
  line-height: 1.5;
}

.modal-overlay {
  position: fixed;
  inset: 0;
  z-index: 1000;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
  background: rgba(0, 0, 0, 0.5);
  backdrop-filter: blur(4px);
}

.modal {
  width: min(560px, 100%);
  max-height: 88vh;
  overflow-y: auto;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 18px;
  padding: 24px;
  box-shadow: 0 25px 60px rgba(0, 0, 0, 0.2);
}

.modal--questions {
  width: min(900px, 100%);
}

.modal__header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 16px;
  margin-bottom: 20px;
}

.modal__header h2 {
  margin: 0;
  font-size: 22px;
  color: var(--text-primary);
  font-weight: 700;
}

.modal__header p {
  margin: 6px 0 0;
  color: var(--text-secondary);
  font-size: 14px;
}

.modal__header strong {
  color: var(--text-primary);
}

.modal__close {
  width: 40px;
  height: 40px;
  flex-shrink: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  border: none;
  border-radius: 10px;
  background: transparent;
  color: var(--text-secondary);
  cursor: pointer;
  transition: all 0.2s ease;
}

.modal__close:hover {
  background: var(--sidebar-hover);
  color: var(--text-primary);
}

.modal__close:active {
  transform: scale(0.92);
}

@media (max-width: 768px) {
  .quizzes {
    gap: 20px;
  }

  .quizzes__header {
    flex-direction: column;
    align-items: flex-start;
    gap: 16px;
  }

  .quizzes__header h1 {
    font-size: 24px;
  }

  .quizzes__header p {
    font-size: 14px;
  }

  .add-quiz-button {
    width: 100%;
    height: 48px;
    font-size: 15px;
    border-radius: 14px;
  }

  .quizzes__grid {
    grid-template-columns: 1fr;
    gap: 16px;
  }

  .modal-overlay {
    padding: 12px;
    align-items: flex-end;
  }

  .modal {
    padding: 20px 16px;
    max-height: 85vh;
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
    border-top-left-radius: 20px;
    border-top-right-radius: 20px;
  }

  .modal__header h2 {
    font-size: 19px;
  }
}

@media (max-width: 480px) {
  .empty {
    padding: 0 10px;
  }

  .empty h3 {
    font-size: 18px;
  }

  .modal-overlay {
    padding: 0;
  }

  .modal {
    border-radius: 20px 20px 0 0;
    border-bottom: none;
  }
}
</style>
