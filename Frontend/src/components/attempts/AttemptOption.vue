<script setup>
defineProps({
  option: Object,
});
</script>
<template>
  <div
    class="option"
    :class="{
      correct: option.isCorrect,

      wrong: option.isSelectedByStudent && !option.isCorrect,

      selected: option.isSelectedByStudent,
    }"
  >
    <span class="option-content">
      {{ option.content }}
    </span>

    <span v-if="option.isCorrect"> ✓ Correct Answer </span>

    <span v-else-if="option.isSelectedByStudent"> Your Answer </span>
  </div>
</template>
<style scoped>
.option {
  padding: 14px 18px;

  border-radius: 12px;

  border: 1px solid var(--border);

  display: flex;

  justify-content: space-between;

  align-items: center;
}

.option.correct {
  background: transparent;

  border-color: #22c55e;
}

.option.wrong {
  background: transparent;

  border-color: #ef4444;
}

.option.selected {
  font-weight: 600;
}
</style>
