<script setup>
import AttemptOption from "./AttemptOption.vue";

defineProps({
  question: Object,
});
</script>
<template>
  <div class="question-card">
    <div class="question-header">
      <h3>Question {{ question.questionId }}</h3>

      <span> {{ question.point }} pts </span>
    </div>

    <p class="question-content">
      {{ question.content }}
    </p>

    <div class="options">
      <AttemptOption
        v-for="option in question.options"
        :key="option.optionId"
        :option="option"
      />
    </div>

    <div class="question-result" :class="question.isCorrect ? 'success' : 'danger'">
      {{ question.isCorrect ? "Correct" : "Wrong" }}
    </div>
  </div>
</template>

<style scoped>
.question-card {
  background: var(--card-background);

  border: 1px solid var(--border);

  border-radius: 18px;

  padding: 24px;

  display: flex;

  flex-direction: column;

  gap: 18px;
}

.question-header {
  display: flex;

  justify-content: space-between;
}

.question-header h3 {
  margin: 0;
}

.question-content {
  font-size: 18px;

  font-weight: 500;
}

.options {
  display: flex;

  flex-direction: column;

  gap: 10px;
}

.question-result {
  width: max-content;

  padding: 6px 14px;

  border-radius: 20px;

  font-weight: 600;
}

.success {
  background: #dcfce7;

  color: #15803d;
}

.danger {
  background: #fee2e2;

  color: #b91c1c;
}
</style>
