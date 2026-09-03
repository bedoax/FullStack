<script setup>
defineProps({
  title: {
    type: String,
    required: true,
  },

  value: {
    type: [String, Number],
    required: true,
  },

  icon: {
    type: [Object, Function],
    required: true,
  },
  color: {
    type: String,
    default: "var(--primary-color)",
  },
});
</script>
<template>
  <div class="stat-card__icon" :style="{ background: color }">
    <component :is="icon" :size="24" />
  </div>

  <div class="stat-card__content">
    <span class="stat-card__title">
      {{ title }}
    </span>

    <h2 class="stat-card__value">
      {{ value ?? "-" }}
    </h2>
  </div>
</template>

<style scoped>
.stat-card {
  display: flex;

  align-items: center;

  gap: 18px;

  padding: 22px;

  border-radius: 16px;

  background: var(--surface);

  border: 1px solid var(--border);

  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.06);

  transition: 0.25s;
}

.stat-card:hover {
  transform: translateY(-4px);

  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
}

.stat-card__icon {
  width: 58px;

  height: 58px;

  border-radius: 14px;

  display: flex;

  justify-content: center;

  align-items: center;

  background: var(--primary);

  color: var(--surface-color);
}

.stat-card__content {
  display: flex;

  flex-direction: column;
}

.stat-card__title {
  font-size: 0.9rem;

  color: var(--text-secondary);
}

.stat-card__value {
  margin-top: 4px;

  font-size: 1.7rem;

  font-weight: 700;

  color: var(--text-primary);
}
</style>
