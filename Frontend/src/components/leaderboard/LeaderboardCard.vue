<script setup>
import { computed } from "vue";

const props = defineProps({
  user: {
    type: Object,
    required: true,
  },

  rank: {
    type: Number,
    required: true,
  },
});

const userInitial = computed(() => {
  return props.user.username?.charAt(0)?.toUpperCase() || "?";
});

const medal = computed(() => {
  const medals = {
    1: "🥇",
    2: "🥈",
    3: "🥉",
  };

  return medals[props.rank] || "";
});
</script>
<template>
  <article class="leaderboard-card">
    <!-- Rank -->

    <div class="leaderboard-card__rank">
      <span
        v-if="rank <= 3"
        class="leaderboard-card__medal"
        :class="`leaderboard-card__medal--${rank}`"
      >
        {{ medal }}
      </span>

      <span v-else class="leaderboard-card__number"> #{{ rank }} </span>
    </div>

    <!-- User -->

    <div class="leaderboard-card__user">
      <div class="leaderboard-card__avatar">
        {{ userInitial }}
      </div>

      <div class="leaderboard-card__info">
        <h3>
          {{ user.username }}
        </h3>

        <span> Student </span>
      </div>
    </div>

    <!-- Score -->

    <div class="leaderboard-card__score">
      <span> Score </span>

      <strong>
        {{ user.score }}
      </strong>
    </div>
  </article>
</template>

<style scoped>
.leaderboard-card {
  background: var(--card-background);

  border: 1px solid var(--border);

  border-radius: 16px;

  padding: 18px 20px;

  display: grid;

  grid-template-columns: 60px 1fr auto;

  align-items: center;

  gap: 18px;

  transition: transform 0.2s ease, box-shadow 0.2s ease, border-color 0.2s ease;
}

.leaderboard-card:hover {
  transform: translateY(-2px);

  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.06);
}

.leaderboard-card__rank {
  display: flex;

  align-items: center;

  justify-content: center;
}

.leaderboard-card__medal {
  font-size: 30px;

  line-height: 1;
}

.leaderboard-card__number {
  font-size: 16px;

  font-weight: 700;

  color: var(--text-secondary);
}

.leaderboard-card__user {
  display: flex;

  align-items: center;

  gap: 12px;

  min-width: 0;
}

.leaderboard-card__avatar {
  width: 44px;

  height: 44px;

  flex-shrink: 0;

  border-radius: 50%;

  background: var(--primary);

  color: var(--sidebar-active-text);

  display: flex;

  align-items: center;

  justify-content: center;

  font-weight: 700;

  font-size: 17px;
}

.leaderboard-card__info {
  min-width: 0;
}

.leaderboard-card__info h3 {
  margin: 0;

  color: var(--text-primary);

  font-size: 16px;

  font-weight: 700;

  white-space: nowrap;

  overflow: hidden;

  text-overflow: ellipsis;
}

.leaderboard-card__info span {
  display: block;

  margin-top: 4px;

  color: var(--text-secondary);

  font-size: 13px;
}

.leaderboard-card__score {
  display: flex;

  flex-direction: column;

  align-items: flex-end;

  gap: 4px;
}

.leaderboard-card__score span {
  color: var(--text-secondary);

  font-size: 12px;
}

.leaderboard-card__score strong {
  color: var(--primary);

  font-size: 20px;

  font-weight: 800;
}

@media (max-width: 600px) {
  .leaderboard-card {
    grid-template-columns: 45px 1fr auto;

    padding: 15px;

    gap: 12px;
  }

  .leaderboard-card__medal {
    font-size: 25px;
  }

  .leaderboard-card__avatar {
    width: 38px;

    height: 38px;

    font-size: 15px;
  }

  .leaderboard-card__score strong {
    font-size: 17px;
  }
}
</style>
