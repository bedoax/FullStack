<script setup>
import { Edit2, Trash2 } from "lucide-vue-next";

const props = defineProps({
  role: {
    type: Object,
    required: true,
  },
});

const emit = defineEmits(["edit", "select-role", "delete-role"]);

function onSelectRole() {
  emit("select-role", props.role);
}

function onDeleteRole() {
  emit("delete-role", props.role);
}
</script>

<template>
  <article class="role-card">
    <div class="role-card__header">
      <span class="role-card__id">#{{ role.id }}</span>

      <div class="role-card__actions">
        <button
          type="button"
          class="role-card__btn role-card__btn--edit"
          title="Edit Role Name"
          @click="$emit('edit', role)"
        >
          <Edit2 :size="16" />
        </button>

        <button
          type="button"
          class="role-card__btn role-card__btn--delete"
          title="Delete Role"
          @click="onDeleteRole"
        >
          <Trash2 :size="16" />
        </button>
      </div>
    </div>

    <div class="role-card__content">
      <h3 class="role-card__name">{{ role.name }}</h3>
    </div>
  </article>
</template>

<style scoped>
.role-card {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  background: var(--surface, #ffffff);
  border: 1px solid var(--border, #e2e8f0);
  border-radius: 16px;
  padding: 1.25rem;
  transition: all 0.25s ease;
}

.role-card:hover {
  transform: translateY(-2px);
  border-color: var(--primary, #6366f1);
  box-shadow: 0 10px 20px -8px rgba(0, 0, 0, 0.08);
}

.role-card__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 0.75rem;
}

.role-card__id {
  font-size: 0.8rem;
  font-weight: 600;
  color: var(--primary, #6366f1);
  background: rgba(99, 102, 241, 0.1);
  padding: 0.2rem 0.6rem;
  border-radius: 20px;
}

.role-card__actions {
  display: flex;
  align-items: center;
  gap: 0.4rem;
}

.role-card__btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 32px;
  height: 32px;
  border-radius: 8px;
  border: 1px solid var(--border, #e2e8f0);
  background: transparent;
  color: var(--text-secondary, #64748b);
  cursor: pointer;
  transition: 0.2s;
}

.role-card__btn--edit:hover {
  background: var(--primary, #6366f1);
  color: #ffffff;
  border-color: var(--primary, #6366f1);
}

.role-card__btn--delete:hover {
  background: #ef4444;
  color: #ffffff;
  border-color: #ef4444;
}

.role-card__name {
  font-size: 1.2rem;
  font-weight: 700;
  color: var(--text-primary, #0f172a);
  text-transform: capitalize;
}
</style>
