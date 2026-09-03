import api from "@/api/axios";

export const quizService = {

    // =========================
    // Student / Teacher / Admin
    // =========================

    async getAllPublishedQuizzes(signal) {
        const response = await api.get("/quizzes/published",{signal});

        return response.data;
    },

    async getPublishedQuizById(id) {
        const response = await api.get(`/quizzes/${id}/published`);

        return response.data;
    },
    async getAllDraftQuizzes() {
        const response = await api.get("/quizzes/draft");

        return response.data;
    },

    async getDraftQuizById(id) {
        const response = await api.get(`/quizzes/${id}/drafted`);

        return response.data;
    },

    async getQuizQuestions(quizId) {
        const response = await api.get(
            `/quizzes/${quizId}/questions`
        );

        return response.data;
    },

    async getLeaderboard(quizId,signal) {
        const response = await api.get(
            `Quizzes/${quizId}/leaderboard`,{signal}
        );

        return response.data;
    },
    async getMyQuizzes(signal) {

    const response = await api.get(
        "/quizzes/me",{signal}
    );

    return response.data;

    },
async addRandomQuestionsToQuiz(quizId, count, topicId = null) {
  const params = { count };
  if (topicId) params.topicId = topicId;
  const response = await api.post(`Quizzes/${quizId}/add-random-questions`, null, {
    params,
  });

  return response.data;
},

    // =========================
    // Teacher / Admin
    // =========================

    async createQuiz(quizData) {
        const response = await api.post(
            "/quizzes",
            quizData
        );

        return response.data;
    },

    async updateQuiz(id, quizData) {
        const response = await api.put(
            `/quizzes/${id}`,
            quizData
        );

        return response.data;
    },

    async deleteQuiz(id) {
        const response = await api.delete(
            `/quizzes/${id}`
        );

        return response.data;
    },

    async getAllDraftQuizzes(signal) {
        const response = await api.get(
            "/quizzes/drafted",{signal}
        );

        return response.data;
    },

    async getDraftQuizById(id) {
        const response = await api.get(
            `/quizzes/${id}/draft`
        );

        return response.data;
    },

    async getQuizQuestionsWithAnswers(quizId) {
        const response = await api.get(
            `/quizzes/${quizId}/questions-with-answers`
        );

        return response.data;
    },
    async addQuestionsToQuiz(quizId, questionIds) {
    const response = await api.post(
        `/quizzes/${quizId}/add-questions`,
        questionIds
    );
    return response.data;

}
};