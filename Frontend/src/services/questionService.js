import api from "@/api/axios";

export const questionService = {

    // =========================
    // Student
    // =========================

    async GetQuestionById(id) {
        const response = await api.get(`/questions/${id}`);

        return response.data;
    },

    async GetQuestionsByDifficulty(difficulty, count = 10) {
        const response = await api.get(
            `/questions/difficulties/${difficulty}`,
            {
                params: {
                    count
                }
            }
        );

        return response.data;
    },

    // =========================
    // Teacher / Admin
    // =========================

    async CreateQuestion(questionData) {
        const response = await api.post(
            "/questions",
            questionData
        );

        return response.data;
    },

    async UpdateQuestion(id, questionData) {
        const response = await api.put(
            `/questions/${id}`,
            questionData
        );

        return response.data;
    },

    async DeleteQuestion(id) {
        const response = await api.delete(
            `/questions/${id}`
        );

        return response.data;
    },

    async GetQuestionByIdForAdminOrTeacher(id) {
        const response = await api.get(
            `/questions/AdminOrTeacher/${id}`
        );

        return response.data;
    },

    async GetQuestionStatistics(questionId) {
        const response = await api.get(
            `/questions/${questionId}/statics`
        );

        return response.data;
    },
async getAllQuestions(page = 1, pageSize = 10) {
    const response = await api.get("/questions/all-questions", {
      params: {
        page: Number(page) || 1,
        pageSize: Number(pageSize) || 10,
      },
    });
    return response.data;
  },

};