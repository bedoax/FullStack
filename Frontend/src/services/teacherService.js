import api from "@/api/axios";

export const teacherService = {

    // =========================
    // Teacher
    // =========================

    async getDashboard() {
        const response = await api.get(
            "/teachers/dashboard"
        );

        return response.data;
    },

    async getMyQuizzes(signal) {
        const response = await api.get(
            "/teachers/quizzes",{signal}
        );

        return response.data;
    },

    async getMyQuestions(page = 1, size = 10) {
        const response = await api.get("/teachers/questions", {
            params: { page, size }
        });
        return response.data;   // Must be { items, page, size, totalCount, totalPages }
    },

    async getQuizAttempts(quizId) {
        const response = await api.get(
            `/teachers/quizzes/${quizId}/attempts`
        );

        return response.data;
    },

    async getQuizStatistics(quizId) {
        const response = await api.get(
            `/teachers/quizzes/${quizId}/statistics`
        );

        return response.data;
    },

    async getQuizStudents(quizId) {
        const response = await api.get(
            `/teachers/quizzes/${quizId}/students`
        );

        return response.data;
    },
    async getQuestionsNotInQuiz(quizId, topicId = null,page = 1, size = 10) {
    const response = await api.get("/teachers/questions/not-in-quiz", {
        params: {
        quizId,
        topicId,
        page,
        size,
        },
     });

  return response.data;
},
async getStudentsOfQuiz(quizId) {
  const response = await api.get(
    `/teachers/quizzes/${quizId}/students`
  );

  return response.data;
},

    // =========================
    // Admin
    // =========================

    async getTeacherDashboard(teacherId) {
        const response = await api.get(
            `/teachers/${teacherId}/dashboard`
        );

        return response.data;
    },

    async getTeacherQuizzes(teacherId,signal) {
        const response = await api.get(
            `/teachers/${teacherId}/quizzes`,{sginal}
        );

        return response.data;
    },

    async getTeacherQuestions(teacherId) {
        const response = await api.get(
            `/teachers/${teacherId}/questions`
        );

        return response.data;
    },

    async getTeacherQuizAttempts(teacherId, quizId) {
        const response = await api.get(
            `/teachers/${teacherId}/quizzes/${quizId}/attempts`
        );

        return response.data;
    },

    async getTeacherQuizStatistics(teacherId, quizId) {
        const response = await api.get(
            `/teachers/${teacherId}/quizzes/${quizId}/statistics`
        );

        return response.data;
    },

    async getTeacherQuizStudents(teacherId, quizId) {
        const response = await api.get(
            `/teachers/${teacherId}/quizzes/${quizId}/students`
        );

        return response.data;
    }

};