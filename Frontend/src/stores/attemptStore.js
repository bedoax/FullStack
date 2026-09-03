import { ref } from "vue";
import { defineStore } from "pinia";

export const useAttemptStore = defineStore("attempt", () => {



    const attemptId = ref(null);
    const quizId = ref(null);



    const questions = ref([]);


    const answers = ref({});



    const currentPage = ref(0);

    const questionsPerPage = 5;



    function initializeAttempt(id, quiz, quizQuestions) {

        attemptId.value = id;
        quizId.value = quiz;

        questions.value = quizQuestions;

        answers.value = {};

        currentPage.value = 0;

        saveToStorage();
    }



    function selectAnswer(questionId, optionId) {

        answers.value[questionId] = optionId;

        saveToStorage();
    }



    function getAnswer(questionId) {

        return answers.value[questionId] ?? null;
    }



    function getCurrentQuestions() {

        const start =
            currentPage.value * questionsPerPage;

        const end =
            start + questionsPerPage;

        return questions.value.slice(start, end);
    }



    function nextPage() {

        if (!hasNextPage())
            return;

        currentPage.value++;

        saveToStorage();
    }

    function previousPage() {

        if (!hasPreviousPage())
            return;

        currentPage.value--;

        saveToStorage();
    }

    function hasNextPage() {

        return (
            (currentPage.value + 1) *
            questionsPerPage
            <
            questions.value.length
        );
    }

    function hasPreviousPage() {

        return currentPage.value > 0;
    }



    function getFormattedAnswers() {

        return Object.entries(answers.value)
            .map(([questionId, optionId]) => ({
                questionId: Number(questionId),
                selectedOptionId: optionId
            }));
    }



    function saveToStorage() {

        const attemptState = {

            attemptId: attemptId.value,

            quizId: quizId.value,

            questions: questions.value,

            answers: answers.value,

            currentPage: currentPage.value

        };

        localStorage.setItem(
            "quiz_attempt",
            JSON.stringify(attemptState)
        );
    }



    function restoreFromStorage() {

        const stored =
            localStorage.getItem("quiz_attempt");

        if (!stored)
            return false;

        try {

            const state =
                JSON.parse(stored);

            attemptId.value =
                state.attemptId ?? null;

            quizId.value =
                state.quizId ?? null;

            questions.value =
                state.questions ?? [];

            answers.value =
                state.answers ?? {};

            currentPage.value =
                state.currentPage ?? 0;

            return true;

        }
        catch {

            clearAttempt();

            return false;
        }
    }



    function clearAttempt() {

        attemptId.value = null;

        quizId.value = null;

        questions.value = [];

        answers.value = {};

        currentPage.value = 0;

        localStorage.removeItem(
            "quiz_attempt"
        );
    }

    return {

        attemptId,
        quizId,

        questions,
        answers,

        currentPage,
        questionsPerPage,

        initializeAttempt,

        selectAnswer,
        getAnswer,

        getCurrentQuestions,

        nextPage,
        previousPage,

        hasNextPage,
        hasPreviousPage,

        getFormattedAnswers,

        saveToStorage,
        restoreFromStorage,

        clearAttempt
    };
});