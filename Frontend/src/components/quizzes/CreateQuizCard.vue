<script setup>
import { ref } from "vue";
import { Plus, X, FileText } from "lucide-vue-next";

import { quizService } from "@/services/quizService";

const emit = defineEmits(["created", "cancel"]);

// =====================================
// Modal
// =====================================

const showModal = ref(false);

const saving = ref(false);

const error = ref("");

// =====================================
// Form
// =====================================

const form = ref({
  title: "",
  description: "",
  maxAttempts: null,
  passPercentage: null,
  isPublished: false,
  availableFrom: "",
  availableTo: "",
  durationInMinutes: null,
});

// =====================================
// Open
// =====================================

function openModal() {
  form.value = {
    title: "",
    description: "",
    maxAttempts: null,
    passPercentage: null,
    isPublished: false,
    availableFrom: "",
    availableTo: "",
    durationInMinutes: 60,
  };

  error.value = "";

  showModal.value = true;
}

function handleCancel() {
  if (saving.value) return;

  emit("cancel");
}

// =====================================
// Close
// =====================================

function closeModal() {
  if (saving.value) return;

  showModal.value = false;

  error.value = "";
}

// =====================================
// Date validation
// =====================================

function validateDates() {
  const from = form.value.availableFrom;

  const to = form.value.availableTo;

  if (!from && !to) {
    return true;
  }

  // Available From

  if (from) {
    const fromDate = new Date(from);

    const now = new Date();

    if (fromDate < now) {
      error.value = "Available From cannot be before the current date and time.";

      return false;
    }
  }

  // Available To

  if (to) {
    const toDate = new Date(to);

    if (from) {
      const fromDate = new Date(from);

      if (toDate <= fromDate) {
        error.value = "Available To must be later than Available From.";

        return false;
      }
    }
  }

  return true;
}

// =====================================
// Validation
// =====================================

function validateForm() {
  const title = form.value.title.trim();

  if (!title) {
    error.value = "Quiz title is required.";

    return false;
  }

  if (title.length > 100) {
    error.value = "Quiz title cannot exceed 100 characters.";

    return false;
  }

  const description = form.value.description.trim();

  if (description.length > 500) {
    error.value = "Description cannot exceed 500 characters.";

    return false;
  }

  if (form.value.maxAttempts !== null && form.value.maxAttempts !== "") {
    const maxAttempts = Number(form.value.maxAttempts);

    if (!Number.isInteger(maxAttempts) || maxAttempts < 1 || maxAttempts > 100) {
      error.value = "Max attempts must be between 1 and 100.";

      return false;
    }
  }

  if (form.value.passPercentage !== null && form.value.passPercentage !== "") {
    const passPercentage = Number(form.value.passPercentage);

    if (Number.isNaN(passPercentage) || passPercentage < 0 || passPercentage > 100) {
      error.value = "Pass percentage must be between 0 and 100.";

      return false;
    }
  }
  const duration = form.value.durationInMinutes;
  if (duration !== null && duration !== "") {
    const durationNum = Number(duration);

    if (!Number.isInteger(durationNum) || durationNum < 1) {
      error.value = "Duration must be a positive integer.";

      return false;
    }
  }

  return validateDates();
}

// =====================================
// Save
// =====================================

async function saveQuiz() {
  error.value = "";

  if (!validateForm()) {
    return;
  }

  saving.value = true;

  try {
    const payload = {
      title: form.value.title.trim(),
      description: form.value.description.trim() || null,

      maxAttempts:
        form.value.maxAttempts === "" || form.value.maxAttempts === null
          ? null
          : Number(form.value.maxAttempts),

      passPercentage:
        form.value.passPercentage === "" || form.value.passPercentage === null
          ? null
          : Number(form.value.passPercentage),

      isPublished: form.value.isPublished,

      availableFrom: toUtcISOString(form.value.availableFrom) || null,

      availableTo: toUtcISOString(form.value.availableTo) || null,

      durationInMinutes: Number(form.value.durationInMinutes),
    };

    console.log("CREATE QUIZ PAYLOAD:", payload);
    console.log("FORM:", form.value);
    await quizService.createQuiz(payload);

    // Close after successful creation

    emit("created");
  } catch (err) {
    console.log("Quiz creation error:", err);

    error.value = err.response?.data?.message || "Failed to create quiz.";
  } finally {
    saving.value = false;
  }
  function toUtcISOString(dateTimeLocal) {
    if (!dateTimeLocal) return null;

    return new Date(dateTimeLocal).toISOString();
  }
}
</script>

<template>
  <!-- ================================= -->
  <!-- Create Quiz Card -->
  <!-- ================================= -->

  <article class="create-quiz-card">
    <div class="create-quiz-card__icon">
      <FileText :size="26" />
    </div>

    <div class="create-quiz-card__content">
      <h2>Create a Quiz</h2>

      <p>
        Create a new quiz and configure its attempts, passing percentage and availability.
      </p>
    </div>

    <button type="button" class="create-quiz-card__button" @click="openModal">
      <Plus :size="18" />

      Add Quiz
    </button>
  </article>

  <!-- ================================= -->
  <!-- Modal -->
  <!-- ================================= -->

  <Teleport to="body">
    <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal">
        <!-- Header -->

        <header class="modal__header">
          <div>
            <h2>Create Quiz</h2>

            <p>Create a new quiz for your students.</p>
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

        <form class="quiz-form" @submit.prevent="saveQuiz">
          <!-- Title -->

          <div class="form-group">
            <label for="quiz-title"> Title </label>

            <input
              id="quiz-title"
              v-model="form.title"
              type="text"
              maxlength="100"
              placeholder="e.g. C# Fundamentals Quiz"
              :disabled="saving"
            />
          </div>

          <!-- Description -->

          <div class="form-group">
            <label for="quiz-description"> Description </label>

            <textarea
              id="quiz-description"
              v-model="form.description"
              rows="4"
              maxlength="500"
              placeholder="Describe this quiz..."
              :disabled="saving"
            />
          </div>

          <!-- Attempts / Pass Percentage -->

          <div class="form-row">
            <div class="form-group">
              <label for="quiz-attempts"> Max Attempts </label>

              <input
                id="quiz-attempts"
                v-model.number="form.maxAttempts"
                type="number"
                min="1"
                max="100"
                placeholder="Unlimited"
                :disabled="saving"
              />

              <small> Leave empty for unlimited attempts. </small>
            </div>
            <div class="form-group">
              <label for="quiz-duration"> Duration (minutes) </label>

              <input
                id="quiz-duration"
                v-model.number="form.durationInMinutes"
                type="number"
                min="1"
                placeholder="e.g. 30"
                :disabled="saving"
              />

              <small> Leave empty if not specified. </small>
            </div>

            <div class="form-group">
              <label for="quiz-pass"> Pass Percentage </label>

              <input
                id="quiz-pass"
                v-model.number="form.passPercentage"
                type="number"
                min="0"
                max="100"
                step="0.01"
                placeholder="e.g. 70"
                :disabled="saving"
              />

              <small> Leave empty if not specified. </small>
            </div>
          </div>

          <!-- Available From -->

          <div class="form-group">
            <label for="quiz-from"> Available From </label>

            <input
              id="quiz-from"
              v-model="form.availableFrom"
              type="datetime-local"
              :disabled="saving"
            />
          </div>

          <!-- Available To -->

          <div class="form-group">
            <label for="quiz-to"> Available To </label>

            <input
              id="quiz-to"
              v-model="form.availableTo"
              type="datetime-local"
              :disabled="saving"
            />
          </div>

          <!-- Published -->

          <label class="publish-option">
            <input v-model="form.isPublished" type="checkbox" :disabled="saving" />

            <span>
              <strong> Publish Quiz </strong>

              <small> Students will be able to access this quiz. </small>
            </span>
          </label>

          <!-- Error -->

          <div v-if="error" class="form-error">
            {{ error }}
          </div>

          <!-- Actions -->

          <div class="modal__actions">
            <button
              type="button"
              class="cancel-button"
              :disabled="saving"
              @click="handleCancel"
            >
              Cancel
            </button>

            <button type="submit" class="save-button" :disabled="saving">
              {{ saving ? "Creating..." : "Create Quiz" }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </Teleport>
</template>

<style scoped>
/* ================================= */
/* Card */
/* ================================= */

.create-quiz-card {
  display: flex;

  align-items: center;

  gap: 18px;

  padding: 22px 24px;

  background: var(--surface);

  border: 1px dashed var(--border);

  border-radius: 18px;
}

.create-quiz-card__icon {
  width: 54px;

  height: 54px;

  flex-shrink: 0;

  display: flex;

  align-items: center;

  justify-content: center;

  border-radius: 15px;

  color: var(--primary);

  background: color-mix(in srgb, var(--primary) 12%, transparent);
}

.create-quiz-card__content {
  flex: 1;
}

.create-quiz-card__content h2 {
  margin: 0;

  font-size: 19px;

  color: var(--text-primary);
}

.create-quiz-card__content p {
  margin: 5px 0 0;

  color: var(--text-secondary);

  font-size: 14px;

  line-height: 1.5;
}

.create-quiz-card__button {
  display: flex;

  align-items: center;

  justify-content: center;

  gap: 8px;

  padding: 11px 17px;

  border: none;

  border-radius: 10px;

  background: var(--primary);

  color: white;

  font-size: 14px;

  font-weight: 600;

  cursor: pointer;

  transition: 0.2s;

  white-space: nowrap;
}

.create-quiz-card__button:hover {
  background: var(--primary-hover);
}

/* ================================= */
/* Modal */
/* ================================= */

.modal-overlay {
  position: fixed;

  inset: 0;

  z-index: 1000;

  display: flex;

  align-items: center;

  justify-content: center;

  padding: 20px;

  background: rgba(0, 0, 0, 0.5);

  backdrop-filter: blur(3px);
}

.modal {
  width: min(560px, 100%);

  max-height: 90vh;

  overflow-y: auto;

  padding: 24px;

  background: var(--surface);

  border: 1px solid var(--border);

  border-radius: 18px;

  box-shadow: 0 25px 60px rgba(0, 0, 0, 0.2);
}

/* ================================= */
/* Modal Header */
/* ================================= */

.modal__header {
  display: flex;

  align-items: flex-start;

  justify-content: space-between;

  gap: 20px;
}

.modal__header h2 {
  margin: 0;

  font-size: 22px;

  color: var(--text-primary);
}

.modal__header p {
  margin: 6px 0 0;

  color: var(--text-secondary);

  font-size: 14px;
}

.modal__close {
  width: 36px;

  height: 36px;

  display: flex;

  align-items: center;

  justify-content: center;

  border: none;

  border-radius: 9px;

  background: transparent;

  color: var(--text-secondary);

  cursor: pointer;
}

.modal__close:hover {
  background: var(--sidebar-hover);

  color: var(--text-primary);
}

/* ================================= */
/* Form */
/* ================================= */

.quiz-form {
  display: flex;

  flex-direction: column;

  gap: 18px;

  margin-top: 24px;
}

.form-row {
  display: grid;

  grid-template-columns: 1fr 1fr;

  gap: 16px;
}

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

.form-group input:disabled,
.form-group textarea:disabled {
  opacity: 0.65;

  cursor: not-allowed;
}

.form-group small {
  font-size: 12px;

  color: var(--text-secondary);
}

/* ================================= */
/* Publish */
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

.cancel-button:hover {
  background: var(--border);
}

.save-button {
  background: var(--primary);

  color: white;
}

.save-button:hover {
  background: var(--primary-hover);
}

.save-button:disabled,
.cancel-button:disabled,
.modal__close:disabled {
  opacity: 0.6;

  cursor: not-allowed;
}

/* ================================= */
/* Responsive */
/* ================================= */

@media (max-width: 700px) {
  .create-quiz-card {
    align-items: flex-start;

    flex-wrap: wrap;
  }

  .create-quiz-card__content {
    min-width: 200px;
  }

  .create-quiz-card__button {
    width: 100%;
  }

  .form-row {
    grid-template-columns: 1fr;
  }
}
</style>
