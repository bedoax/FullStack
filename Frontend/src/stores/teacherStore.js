import { defineStore } from "pinia";
import { ref } from "vue";

import { execute } from "@/utils/storeHelper";
import { teacherService } from "@/services/teacherService";
import { topicService } from "@/services/topicService";

export const useTeacherStore = defineStore("teacher", () => {

    const dashboard = ref({
        quizzes: 0,
        questions: 0,
        students: 0,
        averagePassRate: 0,
    });

    const quizzes = ref([]);

    const questionsPage = ref(1);

    const questionsPageSize = ref(10);

    const questionsTotalCount = ref(0);

    const questionsTotalPages = ref(0);

    const questions = ref([]);

    const students = ref([]);

    const attempts = ref([]);
    
    const topics = ref([]);

    const statistics = ref(null);

    const loading = ref(false);

    async function loadDashboard(signal) {

        await execute(loading, async () => {
            dashboard.value = await teacherService.getDashboard(signal);
        });

    }
    async function loadAllTopics() {

        await execute(loading, async () => {
            topics.value = await topicService.GetAllTopics();
        });

    }
    async function loadMyQuizzes(signal) {

        await execute(loading, async () => {
            quizzes.value = await teacherService.getMyQuizzes(signal);

        });

    }

    async function loadMyQuestions(page,size) {

        await execute(loading, async () => {
             const result = await teacherService.getMyQuestions(page, size);
            
          questions.value = result.items;
        questionsPage.value = result.page;
        questionsPageSize.value = result.size;
        questionsTotalCount.value = result.totalCount;
        questionsTotalPages.value = result.totalPages;
        });

    }

    async function loadQuizStudents(quizId) {

        await execute(loading, async () => {
            students.value = await teacherService.getQuizStudents(quizId);
        });

    }

    async function loadQuizAttempts(quizId) {

        await execute(loading, async () => {
            attempts.value = await teacherService.getQuizAttempts(quizId);
        });

    }

    async function loadQuizStatistics(quizId) {

        await execute(loading, async () => {
            statistics.value = await teacherService.getQuizStatistics(quizId);
        });

    }

    function clear() {

        dashboard.value = {
            quizzes: 0,
            questions: 0,
            students: 0,
            averagePassRate: 0,
        };

        quizzes.value = [];

        questions.value = [];

        students.value = [];

        attempts.value = [];

        statistics.value = null;

        questionsPage.value = 1;
        questionsPageSize.value = 10;
        questionsTotalCount.value = 0;
        questionsTotalPages.value = 0;
    }

    return {
        dashboard,
        quizzes,
        questions,
        students,
        attempts,
        statistics,
        loading,
        topics,
        questionsPage,
        questionsPageSize,
        questionsTotalCount,
        questionsTotalPages,
        loadDashboard,
        loadMyQuizzes,
        loadMyQuestions,
        loadQuizStudents,
        loadQuizAttempts,
        loadQuizStatistics,
        loadAllTopics,
        clear,
    };
});