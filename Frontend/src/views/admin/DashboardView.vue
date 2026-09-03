<script setup></script>

<template>
  <div class="empty-state">
    <div class="empty-state__card">
      <div class="empty-state__icon">🛠️</div>
      <h2>شغالين عليها أهو!</h2>
      <p>معلش يا صحبي لسه بعمل فيها فا خليك صبور شويه 😃</p>
    </div>
  </div>
</template>

<style scoped>
.empty-state {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 60vh;
  padding: 20px;
  direction: rtl;
}

.empty-state__card {
  text-align: center;
  background-color: #f8fafc;
  border: 1px dashed #cbd5e1;
  border-radius: 12px;
  padding: 40px 24px;
  max-width: 400px;
  width: 100%;
}

.empty-state__icon {
  font-size: 3rem;
  margin-bottom: 12px;
}

.empty-state__card h2 {
  font-size: 1.25rem;
  color: #1e293b;
  margin-bottom: 8px;
}

.empty-state__card p {
  color: #64748b;
  font-size: 0.95rem;
  line-height: 1.5;
  margin: 0;
}
</style>
