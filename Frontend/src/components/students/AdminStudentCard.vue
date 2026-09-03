<script setup>
import { computed } from "vue";

const props = defineProps({
  student: {
    type: Object,
    required: true,
  },
});

const emit = defineEmits(["activate", "delete"]);

const statusText = computed(() => {
  return props.student.isDeleted ? "Deleted" : "Active";
});

const formattedDate = computed(() => {
  if (!props.student.createdAt) return "-";
  return new Date(props.student.createdAt).toLocaleDateString();
});
</script>

<template>
  <article class="student-card">
    <div class="student-card__body">
      <div class="student-card__field">
        <span>Name</span>
        <strong>{{ student.name }}</strong>
      </div>

      <div class="student-card__field">
        <span>Email</span>
        <strong>{{ student.email }}</strong>
      </div>

      <div class="student-card__field">
        <span>Status</span>
        <strong :class="student.isDeleted ? 'is-deleted' : 'is-active'">
          {{ statusText }}
        </strong>
      </div>

      <div class="student-card__field">
        <span>Created At</span>
        <strong>{{ formattedDate }}</strong>
      </div>
    </div>
    <div class="student-card__actions">
      <button
        v-if="student.isDeleted"
        class="btn btn--activate"
        @click="emit('activate', student.id)"
      >
        Activate
      </button>

      <button v-else class="btn btn--delete" @click="emit('delete', student.id)">
        Delete
      </button>
    </div>
  </article>
</template>

<style scoped>
.student-card {
  background-color: var(--card-background);
  border: 1px solid var(--border);
  border-radius: 12px;
  padding: 16px 20px;
  transition: border-color 0.25s, box-shadow 0.25s, background-color 0.25s;
}

.student-card:hover {
  border-color: var(--primary);
}

.student-card__body {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(140px, 1fr));
  gap: 16px;
  margin-bottom: 16px;
}

.student-card__field {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.student-card__field span {
  font-size: 0.75rem;
  font-weight: 600;
  color: var(--text-secondary);
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.student-card__field strong {
  font-size: 0.95rem;
  color: var(--text-primary);
  word-break: break-word;
}

.is-active {
  color: var(--success, #22c55e) !important;
}

.is-deleted {
  color: var(--danger, #ef4444) !important;
}

.student-card__actions {
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
  background-color: var(--success, #22c55e);
  color: #ffffff;
}

.btn--activate:hover {
  opacity: 0.9;
}

.btn--delete {
  background-color: var(--danger, #ef4444);
  color: #ffffff;
}

.btn--delete:hover {
  opacity: 0.9;
}
</style>
