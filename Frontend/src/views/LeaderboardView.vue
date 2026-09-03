<script setup>
import { ref, computed } from "vue";

import { Trophy } from "@lucide/vue";

import { useQuizStore } from "@/stores/quizStore";

import { topicService } from "@/services/topicService";

import { quizService } from "@/services/quizService";

import { execute } from "@/utils/storeHelper";

// =========================
// Store
// =========================

const quizStore = useQuizStore();

// =========================
// State
// =========================

const selectedType = ref("");

const selectedId = ref(null);

const topics = ref([]);

const leaderboard = ref([]);

const loadingTopics = ref(false);

const loadingLeaderboard = ref(false);

const error = ref("");

// =========================
// Loading Options
// =========================

const loadingOptions = computed(() => {
  if (selectedType.value === "quiz") {
    return quizStore.loading;
  }

  if (selectedType.value === "topic") {
    return loadingTopics.value;
  }

  return false;
});

// =========================
// Options
// =========================

const options = computed(() => {
  if (selectedType.value === "quiz") {
    return quizStore.publishedQuizzes.map((quiz) => ({
      id: quiz.id,

      name: quiz.title,
    }));
  } else if (selectedType.value === "topic") {
    return topics.value.map((topic) => ({
      id: topic.id,

      name: topic.name,
    }));
  }

  return [];
});

// =========================
// Type Change
// =========================

async function handleTypeChange() {
  selectedId.value = null;

  leaderboard.value = [];

  error.value = "";

  // =========================
  // Quiz
  // =========================

  if (selectedType.value === "quiz") {
    try {
      await quizStore.loadAllPublishedQuizzes();
    } catch (err) {
      error.value = err.response?.data?.message || "Failed to load quizzes.";
    }

    return;
  }

  // =========================
  // Topic
  // =========================

  if (selectedType.value === "topic") {
    try {
      await execute(
        loadingTopics,

        async () => {
          topics.value = await topicService.GetAllTopics();
        }
      );
    } catch (err) {
      error.value = err.response?.data?.message || "Failed to load topics.";
    }
  }
}

// =========================
// Load Leaderboard
// =========================

async function loadLeaderboard() {
  if (!selectedId.value) {
    return;
  }

  leaderboard.value = [];

  error.value = "";

  try {
    await execute(
      loadingLeaderboard,

      async () => {
        // =========================
        // Quiz Leaderboard
        // =========================

        if (selectedType.value === "quiz") {
          leaderboard.value = await quizService.getLeaderboard(selectedId.value);

          return;
        }

        // =========================
        // Topic Leaderboard
        // =========================

        if (selectedType.value === "topic") {
          leaderboard.value = await topicService.GetLeaderboard(selectedId.value);
        }
      }
    );
  } catch (err) {
    error.value = err.response?.data?.message || "Failed to load leaderboard.";
  }
}
</script>

<template>
  <section class="leaderboard">
    <!-- Header -->

    <header class="leaderboard__header">
      <div>
        <h1>Leaderboard</h1>

        <p>See how students rank in quizzes and topics.</p>
      </div>

      <div class="leaderboard__icon">
        <Trophy :size="28" />
      </div>
    </header>

    <!-- Filters -->

    <div class="leaderboard__filters">
      <!-- Type -->

      <div class="leaderboard__field">
        <label> Type </label>

        <select v-model="selectedType" @change="handleTypeChange">
          <option value="">Select type</option>

          <option value="quiz">Quiz</option>

          <option value="topic">Topic</option>
        </select>
      </div>

      <!-- Quiz / Topic -->

      <div v-if="selectedType" class="leaderboard__field">
        <label>
          {{ selectedType === "quiz" ? "Quiz" : "Topic" }}
        </label>

        <select v-model="selectedId" :disabled="loadingOptions">
          <option :value="null">
            {{
              loadingOptions
                ? "Loading..."
                : `Select ${selectedType === "quiz" ? "quiz" : "topic"}`
            }}
          </option>

          <option v-for="item in options" :key="item.id" :value="item.id">
            {{ item.name }}
          </option>
        </select>
      </div>

      <!-- Button -->

      <button
        class="leaderboard__button"
        :disabled="!selectedId || loadingLeaderboard || loadingOptions"
        @click="loadLeaderboard"
      >
        <Trophy v-if="!loadingLeaderboard" :size="18" />

        {{ loadingLeaderboard ? "Loading..." : "View Leaderboard" }}
      </button>
    </div>

    <!-- Error -->

    <div v-if="error" class="leaderboard__error">
      {{ error }}
    </div>

    <!-- Loading -->

    <div v-if="loadingLeaderboard" class="leaderboard__loading">
      Loading leaderboard...
    </div>

    <!-- Results -->

    <div v-else-if="leaderboard.length" class="leaderboard__list">
      <div
        v-for="(student, index) in leaderboard"
        :key="student.userId"
        class="leaderboard__row"
      >
        <div class="leaderboard__rank">
          <span v-if="index < 3" class="leaderboard__medal">
            {{ index === 0 ? "🥇" : index === 1 ? "🥈" : "🥉" }}
          </span>

          <span v-else> #{{ index + 1 }} </span>
        </div>

        <div class="leaderboard__student">
          <div class="leaderboard__avatar">
            {{ student.username?.charAt(0).toUpperCase() }}
          </div>

          <div>
            <strong>
              {{ student.username }}
            </strong>

            <span> Student </span>
          </div>
        </div>

        <div class="leaderboard__score">
          <span> Score </span>

          <strong>
            {{ student.score }}
          </strong>
        </div>
      </div>
    </div>

    <!-- Empty -->

    <div v-else-if="selectedId && !loadingLeaderboard" class="leaderboard__empty">
      <Trophy :size="42" />

      <h3>No leaderboard data</h3>

      <p>There are no results available for this selection yet.</p>
    </div>

    <!-- Initial State -->

    <div v-else class="leaderboard__empty leaderboard__empty--initial">
      <Trophy :size="42" />

      <h3>Select a leaderboard</h3>

      <p>Choose a quiz or topic to view its leaderboard.</p>
    </div>
  </section>
</template>
<style scoped>
.leaderboard {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

/* =========================
   Header
========================= */

.leaderboard__header {
  display: flex;
  align-items: center;
  justify-content: space-between;

  padding: 24px;

  background: var(--card-background);

  border: 1px solid var(--border);

  border-radius: 16px;
}

.leaderboard__header h1 {
  margin: 0;

  color: var(--text-primary);

  font-size: 28px;
}

.leaderboard__header p {
  margin: 8px 0 0;

  color: var(--text-secondary);
}

.leaderboard__icon {
  width: 56px;
  height: 56px;

  display: flex;

  align-items: center;
  justify-content: center;

  border-radius: 50%;

  background: var(--sidebar-hover);

  color: var(--primary);
}

/* =========================
   Filters
========================= */

.leaderboard__filters {
  display: grid;

  grid-template-columns:
    minmax(160px, 1fr)
    minmax(220px, 2fr)
    auto;

  gap: 16px;

  align-items: end;

  padding: 20px;

  background: var(--card-background);

  border: 1px solid var(--border);

  border-radius: 16px;
}

.leaderboard__field {
  display: flex;

  flex-direction: column;

  gap: 8px;
}

.leaderboard__field label {
  font-size: 13px;

  font-weight: 600;

  color: var(--text-secondary);
}

.leaderboard__field select {
  width: 100%;

  padding: 11px 12px;

  border: 1px solid var(--border);

  border-radius: 10px;

  background: var(--surface);

  color: var(--text-primary);

  font-size: 14px;

  outline: none;

  transition: 0.2s;
}

.leaderboard__field select:focus {
  border-color: var(--primary);

  box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.12);
}

.leaderboard__field select:disabled {
  opacity: 0.65;

  cursor: not-allowed;
}

/* =========================
   Button
========================= */

.leaderboard__button {
  min-height: 42px;

  padding: 0 18px;

  border: none;

  border-radius: 10px;

  background: var(--primary);

  color: var(--sidebar-active-text);

  display: flex;

  align-items: center;

  justify-content: center;

  gap: 8px;

  font-weight: 600;

  cursor: pointer;

  transition: 0.2s;
}

.leaderboard__button:hover:not(:disabled) {
  background: var(--primary-hover);
}

.leaderboard__button:disabled {
  opacity: 0.6;

  cursor: not-allowed;
}

/* =========================
   Error
========================= */

.leaderboard__error {
  padding: 14px 16px;

  border: 1px solid var(--danger);

  border-radius: 10px;

  background: rgba(239, 68, 68, 0.08);

  color: var(--danger);
}

/* =========================
   Leaderboard List
========================= */

.leaderboard__list {
  display: flex;

  flex-direction: column;

  gap: 10px;
}

.leaderboard__row {
  display: grid;

  grid-template-columns: 70px 1fr 120px;

  align-items: center;

  gap: 16px;

  padding: 16px 20px;

  background: var(--card-background);

  border: 1px solid var(--border);

  border-radius: 14px;

  transition: 0.2s;
}

.leaderboard__row:hover {
  transform: translateY(-2px);

  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.06);
}

/* =========================
   Rank
========================= */

.leaderboard__rank {
  display: flex;

  align-items: center;

  justify-content: center;

  color: var(--text-secondary);

  font-weight: 700;
}

.leaderboard__medal {
  font-size: 24px;
}

/* =========================
   Student
========================= */

.leaderboard__student {
  display: flex;

  align-items: center;

  gap: 12px;
}

.leaderboard__student > div:last-child {
  display: flex;

  flex-direction: column;

  gap: 3px;
}

.leaderboard__student strong {
  color: var(--text-primary);
}

.leaderboard__student span {
  font-size: 12px;

  color: var(--text-secondary);
}

.leaderboard__avatar {
  width: 42px;
  height: 42px;

  display: flex;

  align-items: center;
  justify-content: center;

  border-radius: 50%;

  background: var(--sidebar-hover);

  color: var(--primary);

  font-weight: 700;
}

/* =========================
   Score
========================= */

.leaderboard__score {
  display: flex;

  flex-direction: column;

  align-items: flex-end;

  gap: 4px;
}

.leaderboard__score span {
  font-size: 12px;

  color: var(--text-secondary);
}

.leaderboard__score strong {
  color: var(--primary);

  font-size: 18px;
}

/* =========================
   Loading
========================= */

.leaderboard__loading {
  display: flex;

  align-items: center;

  justify-content: center;

  min-height: 180px;

  color: var(--text-secondary);
}

/* =========================
   Empty
========================= */

.leaderboard__empty {
  min-height: 220px;

  display: flex;

  flex-direction: column;

  align-items: center;

  justify-content: center;

  gap: 8px;

  padding: 30px;

  background: var(--card-background);

  border: 1px dashed var(--border);

  border-radius: 16px;

  text-align: center;

  color: var(--text-secondary);
}

.leaderboard__empty svg {
  color: var(--primary);
}

.leaderboard__empty h3 {
  margin: 6px 0 0;

  color: var(--text-primary);
}

.leaderboard__empty p {
  margin: 0;

  color: var(--text-secondary);
}

/* =========================
   Responsive
========================= */

@media (max-width: 768px) {
  .leaderboard__filters {
    grid-template-columns: 1fr;
  }

  .leaderboard__row {
    grid-template-columns: 50px 1fr auto;

    padding: 14px;
  }
}

@media (max-width: 500px) {
  .leaderboard__header {
    padding: 18px;
  }

  .leaderboard__header h1 {
    font-size: 22px;
  }

  .leaderboard__row {
    grid-template-columns: 40px 1fr;
  }

  .leaderboard__score {
    grid-column: 2;

    align-items: flex-start;
  }
}
</style>
