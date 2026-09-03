<script setup>
import { computed, onMounted, ref } from "vue";
import { ChevronLeft, ChevronRight } from "lucide-vue-next";
import { questionService } from "@/services/questionService";
import QuestionCard from "@/components/question/QuestionCard.vue";

// =====================================
// State
// =====================================
const questions = ref([]);
const topics = ref([]);
const loading = ref(false);
const error = ref("");

// Pagination State
const currentPage = ref(1);
const pageSize = ref(10);
const totalPages = ref(1);
const totalCount = ref(0);

// =====================================
// Load Questions (Admin Endpoint)
// =====================================
async function loadQuestions(page = 1) {
  loading.value = true;
  error.value = "";

  try {
    const response = await questionService.getAllQuestions(page, pageSize.value);

    // التعامل مع الـ Response حسب الـ Payload اللي راجع من الـ Backend
    // (مثلاً: لو راجع PagedResult أو array مباشر)
    if (response.data) {
      questions.value = response.data.items || response.data;
      totalPages.value =
        response.data.totalPages ||
        Math.ceil((response.data.totalCount || questions.value.length) / pageSize.value);
      totalCount.value = response.data.totalCount || questions.value.length;
    } else {
      questions.value = response.items || response;
      totalPages.value = response.totalPages || 1;
      totalCount.value = response.totalCount || questions.value.length;
    }

    currentPage.value = page;
  } catch (err) {
    console.error("Failed to load questions:", err);
    error.value =
      err.response?.data?.message || "Failed to load questions. Please try again.";
  } finally {
    loading.value = false;
  }
}

// =====================================
// Pagination Actions
// =====================================
async function goToPage(page) {
  if (
    page < 1 ||
    page > totalPages.value ||
    page === currentPage.value ||
    loading.value
  ) {
    return;
  }
  await loadQuestions(page);
}

// =====================================
// Initial Load
// =====================================
onMounted(async () => {
  await loadQuestions(1);
});
</script>

<template>
  <section class="questions">
    <!-- ================================= -->
    <!-- Header (Admin View - Read Only)   -->
    <!-- ================================= -->
    <header class="questions__header">
      <div>
        <h1>Question Bank (Admin View)</h1>
        <p>Browse and review all questions across the platform.</p>
      </div>
    </header>

    <!-- ================================= -->
    <!-- Loading State                     -->
    <!-- ================================= -->
    <div v-if="loading" class="questions__state">
      <p>Loading questions...</p>
    </div>

    <!-- ================================= -->
    <!-- Error State                       -->
    <!-- ================================= -->
    <div v-else-if="error" class="questions__state form-error">
      <p>{{ error }}</p>
      <button
        type="button"
        class="pagination__button"
        @click="loadQuestions(currentPage)"
      >
        Retry
      </button>
    </div>

    <!-- ================================= -->
    <!-- Empty State                       -->
    <!-- ================================= -->
    <div v-else-if="questions.length === 0" class="questions__state">
      <div class="empty">
        <h3>No questions found</h3>
        <p>There are currently no questions created in the system.</p>
      </div>
    </div>

    <!-- ================================= -->
    <!-- Questions Grid                    -->
    <!-- ================================= -->
    <template v-else>
      <div class="questions__grid">
        <QuestionCard
          v-for="question in questions"
          :key="question.id"
          :question="question"
          :topics="topics"
          read-only
        />
      </div>

      <!-- ================================= -->
      <!-- Pagination                        -->
      <!-- ================================= -->
      <div v-if="totalPages > 1" class="pagination">
        <button
          type="button"
          class="pagination__button"
          :disabled="currentPage === 1 || loading"
          @click="goToPage(currentPage - 1)"
        >
          <ChevronLeft :size="18" />
          <span>Previous</span>
        </button>

        <div class="pagination__pages">
          <button
            v-for="page in totalPages"
            :key="page"
            type="button"
            class="pagination__page"
            :class="{ active: page === currentPage }"
            :disabled="loading"
            @click="goToPage(page)"
          >
            {{ page }}
          </button>
        </div>

        <button
          type="button"
          class="pagination__button"
          :disabled="currentPage === totalPages || loading"
          @click="goToPage(currentPage + 1)"
        >
          <span>Next</span>
          <ChevronRight :size="18" />
        </button>
      </div>
    </template>
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
/* State messages (loading / empty / error)        */
/* =============================================== */

.questions__state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 1rem;
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
}

.form-error {
  color: #dc2626;
  border-color: rgba(239, 68, 68, 0.3);
}

/* =============================================== */
/* Grid of question cards                          */
/* =============================================== */

.questions__grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
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
  grid-template-columns: repeat(10, 2.4rem);
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

  .pagination__pages {
    grid-template-columns: repeat(5, 2.4rem);
    gap: 0.3rem;
  }

  .pagination__button span {
    display: none;
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
