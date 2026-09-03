<script setup>
import { ref, watch } from "vue";
import { X, Check } from "lucide-vue-next";

const props = defineProps({
  isOpen: {
    type: Boolean,
    default: false,
  },
  role: {
    type: Object,
    default: null,
  },
});

const emit = defineEmits(["close", "submit"]);

const roleName = ref("");
const loading = ref(false);
const error = ref("");

// متابعة التغيير في الـ role الممرر لتعبئة حقل الاسم تلقائياً
watch(
  () => props.role,
  (newRole) => {
    if (newRole) {
      roleName.value = newRole.name || "";
    }
  },
  { immediate: true }
);

async function handleSubmit() {
  if (!roleName.value.trim()) {
    error.value = "Role name cannot be empty";
    return;
  }

  loading.value = true;
  error.value = "";

  try {
    await emit("submit", { id: props.role.id, name: roleName.value.trim() });
    closeModal();
  } catch (err) {
    error.value = err.response?.data?.message || "Failed to update role name.";
  } finally {
    loading.value = false;
  }
}

function closeModal() {
  error.value = "";
  emit("close");
}
</script>

<template>
  <div v-if="isOpen && role" class="modal-overlay" @click.self="closeModal">
    <div class="modal">
      <header class="modal__header">
        <h3>Edit Role Name</h3>
        <button type="button" class="modal__close" @click="closeModal">
          <X :size="20" />
        </button>
      </header>

      <form @submit.prevent="handleSubmit" class="modal__form">
        <div class="form-group">
          <label for="editRoleName">Role Name</label>
          <input
            id="editRoleName"
            v-model="roleName"
            type="text"
            :disabled="loading"
            autofocus
          />
          <span v-if="error" class="error-text">{{ error }}</span>
        </div>

        <footer class="modal__footer">
          <button
            type="button"
            class="btn btn--secondary"
            @click="closeModal"
            :disabled="loading"
          >
            Cancel
          </button>
          <button type="submit" class="btn btn--primary" :disabled="loading">
            <Check :size="18" />
            <span>{{ loading ? "Saving..." : "Save Changes" }}</span>
          </button>
        </footer>
      </form>
    </div>
  </div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.6);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 1rem;
}

.modal {
  background: var(--surface, #ffffff);
  border: 1px solid var(--border, #e2e8f0);
  border-radius: 16px;
  width: 100%;
  max-width: 440px;
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.modal__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1.25rem 1.5rem;
  border-bottom: 1px solid var(--border, #e2e8f0);
}

.modal__header h3 {
  font-size: 1.15rem;
  font-weight: 700;
  color: var(--text-primary, #0f172a);
}

.modal__close {
  background: transparent;
  border: none;
  color: var(--text-secondary, #64748b);
  cursor: pointer;
  padding: 0.25rem;
  border-radius: 8px;
  transition: 0.2s;
}

.modal__close:hover {
  background: var(--sidebar-hover, #f1f5f9);
  color: var(--text-primary);
}

.modal__form {
  padding: 1.5rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin-bottom: 1.5rem;
}

.form-group label {
  font-size: 0.875rem;
  font-weight: 600;
  color: var(--text-primary, #0f172a);
}

.form-group input {
  padding: 0.75rem 1rem;
  border: 1px solid var(--border, #cbd5e1);
  border-radius: 10px;
  background: var(--background, #ffffff);
  color: var(--text-primary);
  font-size: 0.95rem;
  outline: none;
  transition: 0.2s;
}

.form-group input:focus {
  border-color: var(--primary, #6366f1);
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.15);
}

.error-text {
  font-size: 0.8rem;
  color: #ef4444;
}

.modal__footer {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: 0.75rem;
}

.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  padding: 0.6rem 1.2rem;
  border-radius: 10px;
  font-weight: 600;
  font-size: 0.9rem;
  cursor: pointer;
  border: none;
  transition: 0.2s;
}

.btn--secondary {
  background: var(--background, #f1f5f9);
  color: var(--text-secondary, #475569);
}

.btn--secondary:hover:not(:disabled) {
  background: #e2e8f0;
}

.btn--primary {
  background: var(--primary, #6366f1);
  color: #ffffff;
}

.btn--primary:hover:not(:disabled) {
  opacity: 0.9;
}

.btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
