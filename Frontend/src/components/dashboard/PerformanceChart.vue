<script setup>
import { computed } from "vue";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  ArcElement,
  Tooltip,
  Legend,
  Filler,
} from "chart.js";

import { Line, Bar, Pie, Doughnut } from "vue-chartjs";

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  ArcElement,
  Tooltip,
  Filler,
  Legend
);

const props = defineProps({
  type: {
    type: String,
    default: "line",
  },
  data: {
    type: Array,
    default: () => [],
  },
});

const chartData = computed(() => ({
  labels: props.data.map((item) => item.topicName),

  datasets: [
    {
      label: "Success Rate",

      data: props.data.map((item) => item.successRate),

      borderColor: "#22c55e",

      backgroundColor: ["#22c55e", "#3b82f6", "#f59e0b", "#ef4444", "#8b5cf6", "#06b6d4"],

      tension: 0.35,

      fill: true,
    },
  ],
}));

const chartOptions = computed(() => ({
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      display: false,
    },
  },
  scales:
    props.type === "pie" || props.type === "doughnut"
      ? {}
      : {
          y: {
            beginAtZero: true,
            max: 100,
          },
        },
}));

const chartComponents = {
  line: Line,
  bar: Bar,
  pie: Pie,
  doughnut: Doughnut,
};

const chartComponent = computed(() => {
  return chartComponents[props.type] ?? Line;
});
</script>

<template>
  <section class="performance-chart">
    <header class="performance-chart__header">
      <h2>Performance History</h2>
    </header>

    <div class="performance-chart__body">
      <component :is="chartComponent" :data="chartData" :options="chartOptions" />
    </div>
  </section>
</template>

<style scoped>
.performance-chart {
  background: var(--surface);

  border: 1px solid var(--border);

  border-radius: 16px;

  padding: 24px;

  height: 420px;

  display: flex;

  flex-direction: column;
}

.performance-chart__header {
  margin-bottom: 20px;
}

.performance-chart__header h2 {
  font-size: 1.2rem;

  font-weight: 600;

  color: var(--text-primary);
}

.performance-chart__body {
  flex: 1;

  min-height: 0;

  position: relative;
}
</style>
