<script setup>
defineProps({
  review: Object,
});
</script>
<template>
  <div class="review-header">
    <div>
      <h1>Attempt Review</h1>

      <p>Review your answers and result</p>
    </div>

    <div class="review-stats">
      <div class="stat">
        <span> Score </span>

        <strong>
          {{ review.score }}
        </strong>
      </div>

      <div class="stat">
        <span> Percentage </span>

        <strong> {{ review.percentage }}% </strong>
      </div>
    </div>
  </div>
</template>

<style scoped>
.review-header {
  background: var(--card-background);

  padding: 24px;

  border-radius: 18px;

  border: 1px solid var(--border);

  display: flex;

  justify-content: space-between;

  align-items: center;
}

.review-header h1 {
  margin: 0;

  font-size: 28px;
}

.review-header p {
  color: var(--text-secondary);
}

.review-stats {
  display: flex;

  gap: 20px;
}

.stat {
  background: rgba(0, 0, 0, 0.04);

  padding: 15px 25px;

  border-radius: 12px;

  display: flex;

  flex-direction: column;
}

.stat span {
  font-size: 13px;

  color: #777;
}

.stat strong {
  font-size: 24px;
}
</style>
