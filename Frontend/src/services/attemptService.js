import api from "@/api/axios";

export const attemptService = {

    // =========================
    // Student
    // =========================

    async createAttempt(quizId) {
        const response = await api.post("/attempts/create", null, {
            params: { quizId }
        });

        return response.data;
    },

    async submitAttempt(attemptId, answers) {
        const response = await api.post(
            `/attempts/${attemptId}/submit`,
            answers
        );

        return response.data;
    },

    async getMyAttempts(signal) {

        const response = await api.get("/attempts/user/me",{signal});

        return response.data;
    },

    async getMyOverallScore() {
        const response = await api.get("/attempts/user/me/overall-score");

        return response.data;
    },

    async reviewMyAttempt(attemptId) {
        const response = await api.get(
            `/attempts/user/me/${attemptId}/review`
        );

        return response.data;
    },

    // =========================
    // Admin / Teacher
    // =========================

    async getAttemptById(id) {
        const response = await api.get(`/attempts/${id}`);

        return response.data;
    },

    async getUserAttempts(userId) {
        const response = await api.get(`/attempts/user/${userId}`);

        return response.data;
    },

    async getUserOverallScore(userId) {
        const response = await api.get(
            `/attempts/user/${userId}/overall-score`
        );

        return response.data;
    },

    async reviewUserAttempt(userId, attemptId) {
        const response = await api.get(
            `/attempts/user/${userId}/${attemptId}/review`
        );

        return response.data;
    },

    // =========================
    // Temporary Testing
    // =========================

    async createAttemptFromUser(userId, quizId) {
        const response = await api.post(
            "/attempts/create-from-user",
            null,
            {
                params: {
                    userId,
                    quizId
                }
            }
        );

        return response.data;
    },

    async submitAttemptFromUser(userId, attemptId, answers) {
        const response = await api.post(
            "/attempts/submit-from-user",
            answers,
            {
                params: {
                    userId,
                    attemptId
                }
            }
        );

        return response.data;
    }

};