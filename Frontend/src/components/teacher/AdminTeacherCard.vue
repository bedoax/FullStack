<script setup>
import { computed } from "vue";
const props = defineProps({
  teacher: {
    type: Object,
    required: true,
  },
});
const emit = defineEmits(["activate", "delete", "cancel"]);
const statusText = computed(() => {
  return props.teacher.isDeleted ? "Deleted" : "Active";
});
const formattedDate = computed(() => {
  if (!props.teacher.createdAt) return "-";
  return new Date(props.teacher.createdAt).toLocaleDateString();
});
</script>

<template>
  <article class="teacher-card">
    <div class="teacher-card__body">
      <div class="teacher-card__field">
        <span>Name</span>
        <strong>{{ teacher.name }}</strong>
      </div>

      <div class="teacher-card__field">
        <span>Email</span>
        <strong>{{ teacher.email }}</strong>
      </div>

      <div class="teacher-card__field">
        <span>Status</span>
        <strong :class="{ 'is-deleted': teacher.isDeleted }">{{ statusText }}</strong>
      </div>

      <div class="teacher-card__field">
        <span>Quizzes</span>
        <strong>{{ teacher.quizzes }}</strong>
      </div>

      <div class="teacher-card__field">
        <span>Questions</span>
        <strong>{{ teacher.questions }}</strong>
      </div>

      <div class="teacher-card__field">
        <span>Created At</span>
        <strong>{{ formattedDate }}</strong>
      </div>
    </div>

    <!-- أزرار التحكم والـ Emits -->
    <div class="teacher-card__actions">
      <button
        v-if="teacher.isDeleted"
        class="btn btn--activate"
        @click="emit('activate', teacher.id)"
      >
        Activate
      </button>

      <button v-else class="btn btn--delete" @click="emit('delete', teacher.id)">
        Delete
      </button>
    </div>
  </article>
</template>

<style scoped>
.teacher-card {
  background-color: var(--card-background);
  border: 1px solid var(--border);
  border-radius: 12px;
  padding: 16px 20px;
  transition: border-color 0.25s, box-shadow 0.25s, background-color 0.25s;
}

.teacher-card:hover {
  border-color: var(--primary);
}

.teacher-card__body {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(140px, 1fr));
  gap: 16px;
  margin-bottom: 16px;
}

.teacher-card__field {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.teacher-card__field span {
  font-size: 0.75rem;
  font-weight: 600;
  color: var(--text-secondary);
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.teacher-card__field strong {
  font-size: 0.95rem;
  color: var(--text-primary);
  word-break: break-word;
}
.is-active {
  color: var(--success) !important;
}

.is-deleted {
  color: var(--danger) !important;
}
.teacher-card__actions {
  display: flex;
  gap: 10px;
  justify-content: flex-end;
  border-top: 1px solid var(--border);
  padding-top: 12px;
}

.btn {
  padding: 8px 16px;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  font-weight: 600;
  font-size: 0.875rem;
  transition: background-color 0.25s, transform 0.1s, opacity 0.25s;
}

.btn:active {
  transform: scale(0.98);
}
.btn--activate {
  background-color: var(--success);
  color: #ffffff;
}

.btn--activate:hover {
  background-color: var(--primary-hover);
}
.btn--delete {
  background-color: var(--danger);
  color: #ffffff;
}

.btn--delete:hover {
  opacity: 0.9;
}
</style>
