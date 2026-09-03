<script setup>
import { onMounted, ref } from "vue";
import { storeToRefs } from "pinia";
import { Plus, X } from "lucide-vue-next";

import { useTeacherStore } from "@/stores/teacherStore";
import { topicService } from "@/services/topicService";

import TeacherTopicCard from "@/components/topics/TeacherTopicCard.vue";

const teacherStore = useTeacherStore();

const { topics, loading } = storeToRefs(teacherStore);

// ==============================
// Form state
// ==============================

const showModal = ref(false);

const editingTopic = ref(null);

const saving = ref(false);

const error = ref("");

const form = ref({
  name: "",
  description: "",
});

// ==============================
// Create
// ==============================

function openCreateModal() {
  editingTopic.value = null;

  form.value = {
    name: "",
    description: "",
  };

  error.value = "";

  showModal.value = true;
}

// ==============================
// Edit
// ==============================

function openEditModal(topic) {
  editingTopic.value = topic;

  form.value = {
    name: topic.name,
    description: topic.description ?? "",
  };

  error.value = "";

  showModal.value = true;
}

// ==============================
// Close
// ==============================

function closeModal() {
  if (saving.value) return;

  showModal.value = false;

  editingTopic.value = null;

  error.value = "";
}

// ==============================
// Save
// ==============================

async function saveTopic() {
  error.value = "";

  const name = form.value.name.trim();

  if (!name) {
    error.value = "Topic name is required.";
    return;
  }

  saving.value = true;

  try {
    const payload = {
      name,
      description: form.value.description.trim() || null,
    };

    if (editingTopic.value) {
      await topicService.UpdateTopic(editingTopic.value.id, payload);
    } else {
      await topicService.CreateTopic(payload);
    }

    // العملية نجحت
    showModal.value = false;
    editingTopic.value = null;
    error.value = "";

    await teacherStore.loadAllTopics();
  } catch (err) {
    console.error("Topic save error:", err);

    error.value = err.response?.data?.message || "Failed to save topic.";
  } finally {
    saving.value = false;
  }
}

// ==============================
// Load topics
// ==============================

onMounted(async () => {
  await teacherStore.loadAllTopics();
});
</script>

<template>
  <section class="topics">
    <!-- ========================= -->
    <!-- Header -->
    <!-- ========================= -->

    <header class="topics__header">
      <div>
        <h1>Topics</h1>

        <p>Manage the topics used in your question bank.</p>
      </div>

      <button class="add-topic-button" type="button" @click="openCreateModal">
        <Plus :size="18" />

        Add Topic
      </button>
    </header>

    <!-- ========================= -->
    <!-- Loading -->
    <!-- ========================= -->

    <div v-if="loading" class="topics__state">Loading topics...</div>

    <!-- ========================= -->
    <!-- Empty -->
    <!-- ========================= -->

    <div v-else-if="topics.length === 0" class="topics__state">
      <div class="empty">
        <h3>No topics found</h3>

        <p>Create your first topic to organize your questions.</p>

        <button class="add-topic-button" type="button" @click="openCreateModal">
          <Plus :size="18" />

          Add Topic
        </button>
      </div>
    </div>

    <!-- ========================= -->
    <!-- Topics -->
    <!-- ========================= -->

    <div v-else class="topics__grid">
      <TeacherTopicCard
        v-for="topic in topics"
        :key="topic.id"
        :topic="topic"
        @edit="openEditModal"
      />
    </div>

    <!-- ========================= -->
    <!-- Create / Edit Modal -->
    <!-- ========================= -->

    <Teleport to="body">
      <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
        <div class="modal">
          <!-- Modal Header -->

          <header class="modal__header">
            <div>
              <h2>
                {{ editingTopic ? "Edit Topic" : "Add Topic" }}
              </h2>

              <p>
                {{
                  editingTopic ? "Update the topic information." : "Create a new topic."
                }}
              </p>
            </div>

            <button
              class="modal__close"
              type="button"
              :disabled="saving"
              @click="closeModal"
            >
              <X :size="20" />
            </button>
          </header>

          <!-- Form -->

          <form class="topic-form" @submit.prevent="saveTopic">
            <!-- Name -->

            <div class="form-group">
              <label for="topic-name"> Topic Name </label>

              <input
                id="topic-name"
                v-model="form.name"
                type="text"
                placeholder="e.g. Object Oriented Programming"
                maxlength="200"
                :disabled="saving"
              />
            </div>

            <!-- Description -->

            <div class="form-group">
              <label for="topic-description"> Description </label>

              <textarea
                id="topic-description"
                v-model="form.description"
                rows="4"
                placeholder="Describe this topic..."
                :disabled="saving"
              />
            </div>

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
                @click="closeModal"
              >
                Cancel
              </button>

              <button type="submit" class="save-button" :disabled="saving">
                {{
                  saving ? "Saving..." : editingTopic ? "Update Topic" : "Create Topic"
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
.topics {
  display: flex;

  flex-direction: column;

  gap: 28px;
}

.topics__header {
  display: flex;

  justify-content: space-between;

  align-items: center;

  gap: 20px;
}

.topics__header h1 {
  margin: 0;

  font-size: 32px;

  font-weight: 700;

  color: var(--text-primary);
}

.topics__header p {
  margin: 7px 0 0;

  color: var(--text-secondary);
}

.add-topic-button {
  display: flex;

  align-items: center;

  justify-content: center;

  gap: 8px;

  padding: 11px 16px;

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

.add-topic-button:hover {
  background: var(--primary-hover);
}

.add-topic-button:disabled {
  opacity: 0.6;

  cursor: not-allowed;
}

.topics__grid {
  display: grid;

  grid-template-columns: repeat(auto-fill, minmax(340px, 1fr));

  gap: 22px;
}

.topics__state {
  min-height: 300px;

  display: flex;

  align-items: center;

  justify-content: center;

  color: var(--text-secondary);
}

.empty {
  display: flex;

  flex-direction: column;

  align-items: center;

  gap: 10px;

  text-align: center;
}

.empty h3 {
  margin: 0;

  color: var(--text-primary);

  font-size: 20px;
}

.empty p {
  margin: 0 0 10px;

  color: var(--text-secondary);
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
  width: min(520px, 100%);

  background: var(--surface);

  border: 1px solid var(--border);

  border-radius: 18px;

  padding: 24px;

  box-shadow: 0 25px 60px rgba(0, 0, 0, 0.2);
}

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

.topic-form {
  display: flex;

  flex-direction: column;

  gap: 18px;

  margin-top: 24px;
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

.form-error {
  padding: 10px 12px;

  border-radius: 9px;

  background: rgba(239, 68, 68, 0.1);

  color: #dc2626;

  font-size: 14px;
}

/* ================================= */
/* Modal actions */
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
.cancel-button:disabled {
  opacity: 0.6;

  cursor: not-allowed;
}

/* ================================= */
/* Responsive */
/* ================================= */

@media (max-width: 700px) {
  .topics__header {
    flex-direction: column;

    align-items: stretch;
  }

  .add-topic-button {
    width: 100%;
  }

  .topics__grid {
    grid-template-columns: 1fr;
  }
}
</style>
