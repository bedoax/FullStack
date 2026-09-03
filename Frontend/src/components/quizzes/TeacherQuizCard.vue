<script setup>
import { computed } from "vue";
import { useRouter } from "vue-router";
import {
  FileText,
  Clock,
  Target,
  RotateCcw,
  Calendar,
  CheckCircle2,
  Pencil,
  ListPlus,
  Users,
} from "lucide-vue-next";

const router = useRouter();

const props = defineProps({
  quiz: {
    type: Object,
    required: true,
  },
  readOnly: {
    type: Boolean,
    default: false,
  },
});

const emit = defineEmits(["edit", "manage-questions"]);

// ==============================
// Safe Field Extraction (Handling API field differences)
// ==============================

// 1. Status / Published
const isPublished = computed(() => {
  if (typeof props.quiz.isPublished === "boolean") return props.quiz.isPublished;
  if (typeof props.quiz.published === "boolean") return props.quiz.published;
  if (props.quiz.status) return props.quiz.status.toLowerCase() === "published";
  return true;
});

const statusText = computed(() => {
  return isPublished.value ? "Published" : "Draft";
});

// 2. Dates Mapping
const availableFrom = computed(() => {
  return (
    props.quiz.availableFrom ||
    props.quiz.startDate ||
    props.quiz.startTime ||
    props.quiz.from
  );
});

const availableTo = computed(() => {
  return (
    props.quiz.availableTo || props.quiz.endDate || props.quiz.endTime || props.quiz.to
  );
});

const createdAt = computed(() => {
  return props.quiz.createdAt || props.quiz.createdOn || props.quiz.creationDate;
});

// 3. Duration
const duration = computed(() => {
  return props.quiz.durationMinutes ?? props.quiz.duration ?? 0;
});

function viewStudents() {
  router.push({
    name: "teacher-quiz-students",
    params: {
      quizId: props.quiz.id,
    },
  });
}

// ==============================
// Date formatting
// ==============================

function formatDate(date) {
  if (!date) return "Not set";

  const parsedDate = new Date(date);
  if (isNaN(parsedDate.getTime())) return "Not set";

  return parsedDate.toLocaleDateString("en-US", {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}
</script>

<template>
  <article class="quiz-card">
    <!-- ================================= -->
    <!-- Header -->
    <!-- ================================= -->

    <header class="quiz-card__header">
      <div class="quiz-card__icon">
        <FileText :size="24" />
      </div>

      <div class="quiz-card__title">
        <h3>
          {{ quiz.title }}
        </h3>

        <p>
          {{ quiz.description || "No description available." }}
        </p>
      </div>

      <span
        class="quiz-card__status"
        :class="{
          published: isPublished,
          draft: !isPublished,
        }"
      >
        <CheckCircle2 :size="15" />

        {{ statusText }}
      </span>
    </header>

    <!-- ================================= -->
    <!-- Quiz Info -->
    <!-- ================================= -->

    <div class="quiz-card__info">
      <!-- Max Attempts -->

      <div class="info-item">
        <div class="info-item__icon">
          <RotateCcw :size="17" />
        </div>

        <div>
          <span> Max Attempts </span>

          <strong>
            {{ quiz.maxAttempts ?? "Unlimited" }}
          </strong>
        </div>
      </div>

      <!-- Pass Percentage -->

      <div class="info-item">
        <div class="info-item__icon">
          <Target :size="17" />
        </div>

        <div>
          <span> Pass Percentage </span>

          <strong>
            {{ quiz.passPercentage != null ? quiz.passPercentage + "%" : "Not set" }}
          </strong>
        </div>
      </div>

      <!-- Duration -->

      <div class="info-item">
        <div class="info-item__icon">
          <Clock :size="17" />
        </div>

        <div>
          <span> Duration </span>

          <strong> {{ duration }} min </strong>
        </div>
      </div>
    </div>

    <!-- ================================= -->
    <!-- Availability -->
    <!-- ================================= -->

    <div class="quiz-card__availability">
      <div class="availability-item">
        <Calendar :size="16" />

        <span>
          From:
          <strong>
            {{ formatDate(availableFrom) }}
          </strong>
        </span>
      </div>

      <div class="availability-item">
        <Calendar :size="16" />

        <span>
          To:
          <strong>
            {{ formatDate(availableTo) }}
          </strong>
        </span>
      </div>
    </div>

    <!-- ================================= -->
    <!-- Created -->
    <!-- ================================= -->

    <div class="quiz-card__created">
      Created:

      {{ formatDate(createdAt) }}
    </div>

    <!-- ================================= -->
    <!-- Actions -->
    <!-- ================================= -->

    <footer v-if="!readOnly" class="quiz-card__actions">
      <button
        type="button"
        class="action-button action-button--secondary"
        @click="emit('edit', quiz)"
      >
        <Pencil :size="17" />

        Edit
      </button>

      <button
        type="button"
        class="action-button action-button--primary"
        @click="emit('manage-questions', quiz)"
      >
        <ListPlus :size="17" />

        Manage Questions
      </button>

      <button type="button" @click="viewStudents" class="action-button view-student">
        <Users :size="16" />
        View Students
      </button>
    </footer>
  </article>
</template>

<style scoped>
.quiz-card {
  display: flex;
  flex-direction: column;
  gap: 18px;
  padding: 20px;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 18px;
  transition: transform 0.25s ease, border-color 0.25s ease, box-shadow 0.25s ease;
}

.quiz-card:hover {
  transform: translateY(-4px);
  border-color: var(--primary);
  box-shadow: 0 10px 30px rgba(34, 197, 94, 0.12);
}

.quiz-card__header {
  display: flex;
  align-items: flex-start;
  gap: 14px;
}

.quiz-card__icon {
  width: 52px;
  height: 52px;
  flex-shrink: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 14px;
  color: var(--primary);
  background: color-mix(in srgb, var(--primary) 12%, transparent);
  border: 1px solid color-mix(in srgb, var(--primary) 22%, transparent);
}

.quiz-card__title {
  flex: 1;
  min-width: 0;
}

.quiz-card__title h3 {
  margin: 0;
  font-size: 18px;
  font-weight: 700;
  color: var(--text-primary);
  line-height: 1.3;
}

.quiz-card__title p {
  margin: 6px 0 0;
  color: var(--text-secondary);
  font-size: 14px;
  line-height: 1.5;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.quiz-card__status {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 6px 10px;
  border-radius: 999px;
  font-size: 12px;
  font-weight: 600;
  white-space: nowrap;
  flex-shrink: 0;
}

.quiz-card__status.published {
  color: #15803d;
  background: rgba(34, 197, 94, 0.12);
}

.quiz-card__status.draft {
  color: #b45309;
  background: rgba(245, 158, 11, 0.12);
}

.quiz-card__info {
  display: grid;
  grid-template-columns: repeat(1, 1fr);
  gap: 10px;
}

.info-item {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 10px 12px;
  border-radius: 12px;
  background: var(--background-color);
  border: 1px solid var(--border);
  min-width: 0;
}

.info-item__icon {
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--primary);
  flex-shrink: 0;
}

.info-item span {
  display: block;
  font-size: 11px;
  color: var(--text-secondary);
  white-space: nowrap;
}

.info-item strong {
  display: block;
  margin-top: 2px;
  font-size: 13px;
  color: var(--text-primary);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.quiz-card__availability {
  display: flex;
  flex-direction: column;
  gap: 8px;
  padding-top: 2px;
}

.availability-item {
  display: flex;
  align-items: center;
  gap: 8px;
  color: var(--text-secondary);
  font-size: 13px;
}

.availability-item svg {
  color: var(--primary);
  flex-shrink: 0;
}

.availability-item strong {
  color: var(--text-primary);
  font-weight: 600;
}

.quiz-card__created {
  padding-top: 12px;
  border-top: 1px solid var(--border);
  color: var(--text-secondary);
  font-size: 12px;
}

.quiz-card__actions {
  display: flex;
  gap: 10px;
  padding-top: 4px;
  flex-wrap: wrap;
}

.action-button {
  flex: 1;
  height: 44px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 0 14px;
  border: none;
  border-radius: 12px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  white-space: nowrap;
}

.action-button:active {
  transform: scale(0.97);
}

.action-button--secondary {
  background: var(--sidebar-hover);
  color: var(--text-primary);
}

.action-button--secondary:hover {
  background: var(--border);
}

.action-button--primary {
  background: var(--primary);
  color: white;
}

.view-student {
  background: var(--sidebar-hover);
  color: var(--text-primary);
}

.action-button--primary:hover {
  background: var(--primary-hover);
}

@media (max-width: 600px) {
  .quiz-card {
    padding: 16px;
    gap: 16px;
  }

  .quiz-card__header {
    position: relative;
    flex-wrap: wrap;
    gap: 12px;
  }

  .quiz-card__status {
    margin-left: 0;
    align-self: flex-start;
  }

  .quiz-card__info {
    grid-template-columns: repeat(3, 1fr);
    gap: 8px;
  }

  .info-item {
    padding: 8px 10px;
    gap: 6px;
  }

  .info-item span {
    font-size: 10px;
  }

  .info-item strong {
    font-size: 12px;
  }

  .quiz-card__actions {
    flex-direction: column;
    gap: 8px;
    height: 150px;
  }

  .action-button {
    width: 100%;
  }
}

@media (max-width: 400px) {
  .quiz-card__info {
    grid-template-columns: repeat(2, 1fr);
  }
}
</style>
