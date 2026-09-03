```vue
<script setup>
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { Users, Mail, Trophy, CheckCircle2, XCircle } from "lucide-vue-next";
import { teacherService } from "@/services/teacherService";

const route = useRoute();

const quizId = Number(route.params.quizId);

const students = ref([]);
const loading = ref(false);
const error = ref("");

async function loadStudents() {
  loading.value = true;
  error.value = "";

  try {
    students.value = await teacherService.getStudentsOfQuiz(quizId);
  } catch (err) {
    console.error("Failed to load quiz students:", err);

    error.value = err.response?.data?.message || "Failed to load students for this quiz.";
  } finally {
    loading.value = false;
  }
}

onMounted(() => {
  loadStudents();
});
</script>

<template>
  <div class="quiz-students">
    <!-- Header -->

    <div class="quiz-students__header">
      <div>
        <span class="quiz-students__label"> Quiz Students </span>

        <p>Students who have attempts for this quiz.</p>
      </div>

      <div class="students-count">
        <Users :size="18" />

        {{ students.length }}
      </div>
    </div>

    <!-- Error -->

    <div v-if="error" class="state state--error">
      {{ error }}
    </div>

    <!-- Loading -->

    <div v-else-if="loading" class="state">
      <Users :size="28" />

      <span>Loading students...</span>
    </div>

    <!-- Empty -->

    <div v-else-if="students.length === 0" class="state">
      <Users :size="32" />

      <h4>No students yet</h4>

      <p>No students have attempted this quiz yet.</p>
    </div>

    <!-- Students -->

    <div v-else class="students-list">
      <div v-for="student in students" :key="student.userId" class="student-card">
        <!-- Student -->

        <div class="student-info">
          <div class="student-avatar">
            {{ student.username?.charAt(0)?.toUpperCase() || "?" }}
          </div>

          <div>
            <h4>
              {{ student.username }}
            </h4>

            <span> Student ID: {{ student.userId }} </span>
          </div>
        </div>

        <!-- Result -->

        <div class="student-result">
          <div class="score">
            <span class="result-label"> Score </span>

            <strong>
              {{
                student.score !== null && student.score !== undefined
                  ? `${student.score}%`
                  : "—"
              }}
            </strong>
          </div>

          <div v-if="student.passed === true" class="status status--passed">
            <CheckCircle2 :size="16" />

            Passed
          </div>

          <div v-else-if="student.passed === false" class="status status--failed">
            <XCircle :size="16" />

            Failed
          </div>

          <div v-else class="status status--pending">Pending</div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.quiz-students {
  display: flex;
  flex-direction: column;
  gap: 18px;
}

/* Header */

.quiz-students__header {
  display: flex;
  align-items: center;
  justify-content: space-between;

  gap: 20px;

  padding: 16px;

  border: 1px solid var(--border);
  border-radius: 12px;

  background: var(--background-color);
}

.quiz-students__label {
  display: block;

  margin-bottom: 4px;

  font-size: 12px;

  font-weight: 600;

  color: var(--text-secondary);
}

.quiz-students__header h3 {
  margin: 0;

  font-size: 18px;

  color: var(--text-primary);
}

.quiz-students__header p {
  margin: 5px 0 0;

  font-size: 13px;

  color: var(--text-secondary);
}

.students-count {
  display: flex;

  align-items: center;

  gap: 7px;

  padding: 9px 12px;

  border-radius: 9px;

  background: color-mix(in srgb, var(--primary) 10%, transparent);

  color: var(--primary);

  font-size: 14px;

  font-weight: 600;
}

/* Students */

.students-list {
  display: flex;

  flex-direction: column;

  gap: 10px;
}

.student-card {
  display: flex;

  align-items: center;

  justify-content: space-between;

  gap: 20px;

  padding: 14px 16px;

  border: 1px solid var(--border);

  border-radius: 12px;

  background: var(--surface);

  transition: border-color 0.2s, transform 0.2s;
}

.student-card:hover {
  border-color: color-mix(in srgb, var(--primary) 40%, var(--border));

  transform: translateY(-1px);
}

/* Student Info */

.student-info {
  display: flex;

  align-items: center;

  gap: 12px;

  min-width: 0;
}

.student-avatar {
  width: 40px;
  height: 40px;

  flex-shrink: 0;

  display: flex;

  align-items: center;
  justify-content: center;

  border-radius: 50%;

  background: color-mix(in srgb, var(--primary) 12%, transparent);

  color: var(--primary);

  font-size: 15px;

  font-weight: 700;
}

.student-info h4 {
  margin: 0 0 3px;

  color: var(--text-primary);

  font-size: 14px;

  font-weight: 600;
}

.student-info span {
  color: var(--text-secondary);

  font-size: 12px;
}

/* Result */

.student-result {
  display: flex;

  align-items: center;

  gap: 18px;
}

.score {
  display: flex;

  align-items: center;

  gap: 8px;
}

.result-label {
  color: var(--text-secondary);

  font-size: 12px;
}

.score strong {
  min-width: 48px;

  color: var(--text-primary);

  font-size: 15px;
}

/* Status */

.status {
  display: flex;

  align-items: center;

  justify-content: center;

  gap: 6px;

  min-width: 82px;

  padding: 6px 9px;

  border-radius: 7px;

  font-size: 12px;

  font-weight: 600;
}

.status--passed {
  background: rgba(34, 197, 94, 0.1);

  color: #16a34a;
}

.status--failed {
  background: rgba(239, 68, 68, 0.1);

  color: #dc2626;
}

.status--pending {
  background: rgba(234, 179, 8, 0.1);

  color: #ca8a04;
}

/* State */

.state {
  min-height: 220px;

  display: flex;

  flex-direction: column;

  align-items: center;

  justify-content: center;

  gap: 8px;

  text-align: center;

  color: var(--text-secondary);
}

.state h4 {
  margin: 4px 0 0;

  color: var(--text-primary);

  font-size: 17px;
}

.state p {
  margin: 0;

  font-size: 13px;
}

.state--error {
  min-height: auto;

  align-items: stretch;

  padding: 11px 13px;

  border-radius: 9px;

  background: rgba(239, 68, 68, 0.1);

  color: #dc2626;

  font-size: 13px;
}

/* Responsive */

@media (max-width: 650px) {
  .quiz-students__header {
    align-items: flex-start;
  }

  .student-card {
    align-items: flex-start;

    flex-direction: column;
  }

  .student-result {
    width: 100%;

    justify-content: space-between;
  }
}
</style>
