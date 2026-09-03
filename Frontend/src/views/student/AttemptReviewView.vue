<script setup>
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";

import { execute } from "@/utils/storeHelper";
import { attemptService } from "@/services/attemptService";

import AttemptReviewHeader from "@/components/attempts/AttemptReviewHeader.vue";
import AttemptQuestionCard from "@/components/attempts/AttemptQuestionCard.vue";

const route = useRoute();

const review = ref(null);

const loading = ref(false);

const error = ref("");

const attemptId = Number(route.params.attemptId);

onMounted(async () => {
  await execute(loading, async () => {
    review.value = await attemptService.reviewMyAttempt(attemptId);
  });
});
</script>
<template>
  <div class="attempt-review-page">
    <AttemptReviewHeader v-if="review" :review="review" />

    <div v-if="review" class="questions-list">
      <AttemptQuestionCard
        v-for="question in review.questions"
        :key="question.questionId"
        :question="question"
      />
    </div>
  </div>
</template>

<style scoped>
.attempt-review-page {
  max-width: 1000px;
  margin: auto;
  padding: 32px;

  display: flex;
  flex-direction: column;
  gap: 30px;
}

.questions-list {
  display: flex;
  flex-direction: column;
  gap: 20px;
}
</style>
