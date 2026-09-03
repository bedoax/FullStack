import { defineStore } from "pinia";
import { ref } from "vue";

import { execute } from "@/utils/storeHelper";
import { quizService } from "@/services/quizService";

export const useQuizStore = defineStore("quiz", () => {

    const publishedQuizzes = ref([]);
    const draftQuizzes = ref([]);
    const quizQuestions = ref([]);
    const myQuizzes = ref([]);
    const quizLeaderboard = ref([]);
    const selectedQuiz = ref(null);
    const loading = ref(false);

    async function loadAllPublishedQuizzes(signal) {

        await execute(loading, async () => {
            publishedQuizzes.value =
                await quizService.getAllPublishedQuizzes(signal);
        });

    }
async function loadMyQuizzes(signal) {

    await execute(loading, async () => {

        myQuizzes.value =
            await quizService.getMyQuizzes(signal);

    });

}
    async function loadPublishedQuizById(id) {

        await execute(loading, async () => {
            selectedQuiz.value =
                await quizService.getPublishedQuizById(id);
        });

    }
        async function loadAllDraftQuizzes(signal) {

        await execute(loading, async () => {
            draftQuizzes.value =
                await quizService.getAllDraftQuizzes(signal);
        });

    }

    async function loadDraftQuizById(id) {

        await execute(loading, async () => {
            selectedQuiz.value =
                await quizService.getDraftQuizById(id);
        });

    }
    async function loadQuizQuestions(quizId) {

        await execute(loading, async () => {
            quizQuestions.value =
                await quizService.getQuizQuestions(quizId);
        });

    }

    async function loadQuizLeaderboard(quizId,signal) {

        await execute(loading, async () => {
            quizLeaderboard.value =
                await quizService.getLeaderboard(quizId,signal);
        });

    }

    function clear() {

        publishedQuizzes.value = [];
        draftQuizzes.value = [];
        quizQuestions.value = [];
        quizLeaderboard.value = [];
        myQuizzes.value = [];
        selectedQuiz.value = null;
    }
function clearQuizDetails() {
    quizQuestions.value = [];
    quizLeaderboard.value = [];
    selectedQuiz.value = null;
}
return {
        publishedQuizzes,
        myQuizzes,
        draftQuizzes,
        selectedQuiz,
        quizQuestions,
        quizLeaderboard,
        loading,
        loadMyQuizzes,
        loadAllPublishedQuizzes,
         loadPublishedQuizById,
         loadAllDraftQuizzes,
        loadDraftQuizById,

        loadQuizQuestions,
        loadQuizLeaderboard,

        clear,
        clearQuizDetails,
    };

});