import { defineStore } from "pinia";
import { ref } from "vue";

import { execute } from "@/utils/storeHelper";

import { studentProfileService } from "@/services/studentProfileService";
import { studentPerformanceService } from "@/services/studentPerformanceService";

export const useStudentStore = defineStore("student", () => {

    const profile = ref({
        userId: null,
        currentLevel: "",
        skillScore: 0,
        totalAttempts: 0,
        lastAssessmentDate: null,
    });

    const dashboard = ref({
        skillScore: 0,
        currentLevel: "",
        attempts: 0,
        passed: 0,
        weakTopics: [],
    });
    const recentAttempts = ref([]);
    const performance = ref([]);
    const weakTopics = ref([]);
    const loading = ref(false);

    async function loadProfile() {

        await execute(loading, async () => {
            profile.value = await studentProfileService.getProfile();
        });

    }

    async function loadDashboard(signal) {

        await execute(loading, async () => {
            dashboard.value = await studentProfileService.getDashboard(signal);
        });

    }

    async function loadPerformance() {

        await execute(loading, async () => {
            performance.value = await studentPerformanceService.getMyPerformance();
        });

    }

    async function loadWeakTopics() {

        await execute(loading, async () => {
            weakTopics.value =
                await studentPerformanceService.getWeakTopics();
        });

    }

    function clear() {

        profile.value = {
            userId: null,
            currentLevel: "",
            skillScore: 0,
            totalAttempts: 0,
            lastAssessmentDate: null,
        };

        dashboard.value = {
            skillScore: 0,
            currentLevel: "",
            attempts: 0,
            passed: 0,
            weakTopics: [],
        };

        performance.value = [];
        weakTopics.value = [];
    }

    return {
        profile,
        dashboard,
        performance,
        weakTopics,
        loading,

        loadProfile,
        loadDashboard,
        loadPerformance,
        loadWeakTopics,

        clear,
    };

});