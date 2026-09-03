```vue
<script setup>
import { ref } from "vue";
import { quizService } from "@/services/quizService";

const props = defineProps({
  quiz: {
    type: Object,
    required: true,
  },
});

const emit = defineEmits(["updated", "cancel"]);

const saving = ref(false);
const error = ref("");

// =====================================
// DateTime Helpers
// =====================================

function formatDateTimeLocal(value) {
  if (!value) {
    return "";
  }

  const date = new Date(value);

  if (Number.isNaN(date.getTime())) {
    return "";
  }

  const offset = date.getTimezoneOffset();

  const localDate = new Date(date.getTime() - offset * 60 * 1000);

  return localDate.toISOString().slice(0, 16);
}

function localDateTimeToUtc(value) {
  if (!value) {
    return null;
  }

  const date = new Date(value);

  if (Number.isNaN(date.getTime())) {
    return null;
  }

  return date.toISOString();
}

// =====================================
// Form
// =====================================

const form = ref({
  title: props.quiz.title ?? "",

  description: props.quiz.description ?? "",

  maxAttempts: props.quiz.maxAttempts ?? null,

  passPercentage: props.quiz.passPercentage ?? null,

  availableFrom: formatDateTimeLocal(props.quiz.availableFrom),

  availableTo: formatDateTimeLocal(props.quiz.availableTo),

  durationInMinutes: props.quiz.durationMinutes ?? null,

  isPublished: props.quiz.isPublished ?? false,
});

// =====================================
// Validation
// =====================================

function validateForm() {
  error.value = "";

  // ---------------------------------
  // Title
  // ---------------------------------

  const title = form.value.title.trim();

  if (!title) {
    error.value = "Quiz title is required.";

    return false;
  }

  if (title.length > 100) {
    error.value = "Quiz title cannot exceed 100 characters.";

    return false;
  }

  // ---------------------------------
  // Description
  // ---------------------------------

  const description = form.value.description.trim();

  if (description.length > 500) {
    error.value = "Description cannot exceed 500 characters.";

    return false;
  }

  // ---------------------------------
  // Max Attempts
  // ---------------------------------

  if (form.value.maxAttempts !== null && form.value.maxAttempts !== "") {
    const value = Number(form.value.maxAttempts);

    if (!Number.isInteger(value) || value < 1 || value > 100) {
      error.value = "Max attempts must be between 1 and 100.";

      return false;
    }
  }

  // ---------------------------------
  // Pass Percentage
  // ---------------------------------

  if (form.value.passPercentage !== null && form.value.passPercentage !== "") {
    const value = Number(form.value.passPercentage);

    if (Number.isNaN(value) || value < 0 || value > 100) {
      error.value = "Pass percentage must be between 0 and 100.";

      return false;
    }
  }

  // ---------------------------------
  // Duration
  // ---------------------------------

  if (form.value.durationInMinutes !== null && form.value.durationInMinutes !== "") {
    const value = Number(form.value.durationInMinutes);

    if (!Number.isInteger(value) || value < 1) {
      error.value = "Duration must be a positive integer.";

      return false;
    }
  }

  // ---------------------------------
  // Available From / To
  // ---------------------------------

  const from = form.value.availableFrom;

  const to = form.value.availableTo;

  if (from && to) {
    const fromDate = new Date(from);

    const toDate = new Date(to);

    if (Number.isNaN(fromDate.getTime()) || Number.isNaN(toDate.getTime())) {
      error.value = "Invalid availability date.";

      return false;
    }

    if (toDate <= fromDate) {
      error.value = "Available To must be later than Available From.";

      return false;
    }
  }

  return true;
}

// =====================================
// Save
// =====================================

async function saveQuiz() {
  if (!validateForm()) {
    return;
  }

  saving.value = true;
  error.value = "";

  try {
    const payload = {
      // ---------------------------------
      // Basic Info
      // ---------------------------------

      title: form.value.title.trim(),

      description: form.value.description.trim() || null,

      // ---------------------------------
      // Attempts
      // ---------------------------------

      maxAttempts:
        form.value.maxAttempts === "" || form.value.maxAttempts === null
          ? null
          : Number(form.value.maxAttempts),

      // ---------------------------------
      // Passing
      // ---------------------------------

      passPercentage:
        form.value.passPercentage === "" || form.value.passPercentage === null
          ? null
          : Number(form.value.passPercentage),

      // ---------------------------------
      // Availability
      // ---------------------------------

      availableFrom: localDateTimeToUtc(form.value.availableFrom),

      availableTo: localDateTimeToUtc(form.value.availableTo),

      // ---------------------------------
      // Duration
      // ---------------------------------

      durationInMinutes:
        form.value.durationInMinutes === "" || form.value.durationInMinutes === null
          ? null
          : Number(form.value.durationInMinutes),

      // ---------------------------------
      // Draft / Published
      // ---------------------------------

      isPublished: form.value.isPublished,
    };

    await quizService.updateQuiz(props.quiz.id, payload);

    emit("updated");
  } catch (err) {
    console.error("Quiz update error:", err);

    error.value = err.response?.data?.message || "Failed to update quiz.";
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
</script>

<template>
  <form class="quiz-form" @submit.prevent="saveQuiz">
    <!-- ================================= -->
    <!-- Title -->
    <!-- ================================= -->

    <div class="form-group">
      <label for="edit-quiz-title"> Title </label>

      <input
        id="edit-quiz-title"
        v-model="form.title"
        type="text"
        maxlength="100"
        :disabled="saving"
      />
    </div>

    <!-- ================================= -->
    <!-- Description -->
    <!-- ================================= -->

    <div class="form-group">
      <label for="edit-quiz-description"> Description </label>

      <textarea
        id="edit-quiz-description"
        v-model="form.description"
        rows="4"
        maxlength="500"
        :disabled="saving"
      />
    </div>

    <!-- ================================= -->
    <!-- Attempts / Pass / Duration -->
    <!-- ================================= -->

    <div class="form-row">
      <div class="form-group">
        <label for="edit-quiz-attempts"> Max Attempts </label>

        <input
          id="edit-quiz-attempts"
          v-model.number="form.maxAttempts"
          type="number"
          min="1"
          max="100"
          :disabled="saving"
        />
      </div>

      <div class="form-group">
        <label for="edit-quiz-pass"> Pass Percentage </label>

        <input
          id="edit-quiz-pass"
          v-model.number="form.passPercentage"
          type="number"
          min="0"
          max="100"
          step="0.01"
          :disabled="saving"
        />
      </div>

      <div class="form-group">
        <label for="edit-quiz-duration"> Duration (minutes) </label>

        <input
          id="edit-quiz-duration"
          v-model.number="form.durationInMinutes"
          type="number"
          min="1"
          :disabled="saving"
        />
      </div>
    </div>

    <!-- ================================= -->
    <!-- Available From -->
    <!-- ================================= -->

    <div class="form-group">
      <label for="edit-quiz-from"> Available From </label>

      <input
        id="edit-quiz-from"
        v-model="form.availableFrom"
        type="datetime-local"
        :disabled="saving"
      />
    </div>

    <!-- ================================= -->
    <!-- Available To -->
    <!-- ================================= -->

    <div class="form-group">
      <label for="edit-quiz-to"> Available To </label>

      <input
        id="edit-quiz-to"
        v-model="form.availableTo"
        type="datetime-local"
        :disabled="saving"
      />
    </div>

    <!-- ================================= -->
    <!-- Published / Draft -->
    <!-- ================================= -->

    <label class="publish-option">
      <input v-model="form.isPublished" type="checkbox" :disabled="saving" />

      <span>
        <strong>
          {{ form.isPublished ? "Published" : "Draft" }}
        </strong>

        <small>
          {{
            form.isPublished
              ? "Students can access this quiz according to its availability."
              : "Students cannot access this quiz."
          }}
        </small>
      </span>
    </label>

    <!-- ================================= -->
    <!-- Error -->
    <!-- ================================= -->

    <div v-if="error" class="form-error">
      {{ error }}
    </div>

    <!-- ================================= -->
    <!-- Actions -->
    <!-- ================================= -->

    <div class="modal__actions">
      <button type="button" class="cancel-button" :disabled="saving" @click="cancel">
        Cancel
      </button>

      <button type="submit" class="save-button" :disabled="saving">
        {{ saving ? "Saving..." : "Save Changes" }}
      </button>
    </div>
  </form>
</template>

<style scoped>
.quiz-form {
  display: flex;

  flex-direction: column;

  gap: 18px;
}

/* ================================= */
/* Form Row */
/* ================================= */

.form-row {
  display: grid;

  grid-template-columns: repeat(3, 1fr);

  gap: 16px;
}

/* ================================= */
/* Form Group */
/* ================================= */

.form-group {
  display: flex;

  flex-direction: column;

  gap: 7px;
}

.form-group label {
  font-size: 14px;

  font-weight: 600;

  color: var(--text-primary);
}

.form-group input,
.form-group textarea {
  width: 100%;

  box-sizing: border-box;

  padding: 11px 12px;

  border: 1px solid var(--border);

  border-radius: 10px;

  outline: none;

  background: var(--background-color);

  color: var(--text-primary);

  font-family: inherit;

  transition: 0.2s;
}

.form-group input:focus,
.form-group textarea:focus {
  border-color: var(--primary);

  box-shadow: 0 0 0 3px color-mix(in srgb, var(--primary) 12%, transparent);
}

.form-group textarea {
  resize: vertical;
}

/* ================================= */
/* Publish Option */
/* ================================= */

.publish-option {
  display: flex;

  align-items: flex-start;

  gap: 10px;

  padding: 13px;

  border-radius: 12px;

  background: var(--background-color);

  border: 1px solid var(--border);

  cursor: pointer;
}

.publish-option input {
  margin-top: 3px;

  accent-color: var(--primary);
}

.publish-option span {
  display: flex;

  flex-direction: column;

  gap: 3px;
}

.publish-option strong {
  color: var(--text-primary);

  font-size: 14px;
}

.publish-option small {
  color: var(--text-secondary);

  font-size: 12px;
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
/* Actions */
/* ================================= */

.modal__actions {
  display: flex;

  justify-content: flex-end;

  gap: 10px;

  padding-top: 4px;
}

.cancel-button,
.save-button {
  padding: 10px 16px;

  border: none;

  border-radius: 9px;

  font-weight: 600;

  cursor: pointer;

  transition: 0.2s;
}

.cancel-button {
  background: var(--sidebar-hover);

  color: var(--text-primary);
}

.cancel-button:hover:not(:disabled) {
  background: var(--border);
}

.save-button {
  background: var(--primary);

  color: white;
}

.save-button:hover:not(:disabled) {
  background: var(--primary-hover);
}

.cancel-button:disabled,
.save-button:disabled {
  opacity: 0.6;

  cursor: not-allowed;
}

/* ================================= */
/* Responsive */
/* ================================= */

@media (max-width: 700px) {
  .form-row {
    grid-template-columns: 1fr;
  }
}
</style>
