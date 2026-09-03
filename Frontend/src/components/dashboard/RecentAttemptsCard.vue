<script setup>
defineProps({
  attempts: {
    type: Array,

    default: () => [],
  },
});
</script>
<template>
  <section class="recent-attempts">
    <header class="recent-attempts__header">
      <h2>Recent Attempts</h2>
    </header>

    <div v-if="attempts.length" class="attempts">
      <article v-for="attempt in attempts" :key="attempt.attemptId" class="attempt">
        <div class="attempt__top">
          <h3>
            {{ attempt.quizTitle }}
          </h3>

          <span class="attempt__score"> {{ attempt.score }}% </span>
        </div>

        <div class="attempt__bottom">
          <span class="attempt__date">
            {{ attempt.completedAt }}
          </span>

          <span
            class="attempt__status"
            :class="{
              passed: attempt.isPassed,

              failed: !attempt.isPassed,
            }"
          >
            {{ attempt.isPassed ? "Passed" : "Failed" }}
          </span>
        </div>
      </article>
    </div>

    <div v-else class="empty">No attempts yet.</div>
  </section>
</template>

<style scoped>
.recent-attempts {
  background: var(--surface);

  border: 1px solid var(--border);

  border-radius: 16px;

  padding: 24px;
}

.recent-attempts__header {
  margin-bottom: 20px;
}

.recent-attempts__header h2 {
  font-size: 1.2rem;

  font-weight: 600;

  color: var(--text-primary);
}

.attempts {
  display: flex;

  flex-direction: column;

  gap: 18px;
}

.attempt {
  border-bottom: 1px solid var(--border);

  padding-bottom: 16px;
}

.attempt:last-child {
  border-bottom: none;

  padding-bottom: 0;
}

.attempt__top {
  display: flex;

  justify-content: space-between;

  align-items: center;

  margin-bottom: 8px;
}

.attempt__top h3 {
  font-size: 1rem;

  font-weight: 600;

  color: var(--text-primary);
}

.attempt__score {
  font-weight: 700;

  color: var(--primary);
}

.attempt__bottom {
  display: flex;

  justify-content: space-between;

  align-items: center;
}

.attempt__date {
  color: var(--text-secondary);

  font-size: 0.9rem;
}

.attempt__status {
  padding: 4px 10px;

  border-radius: 999px;

  font-size: 0.85rem;

  font-weight: 600;
}

.passed {
  background: rgba(34, 197, 94, 0.15);

  color: var(--success);
}

.failed {
  background: rgba(239, 68, 68, 0.15);

  color: var(--danger);
}

.empty {
  text-align: center;

  color: var(--text-secondary);

  padding: 24px 0;
}
</style>
