import api from "@/api/axios";

export const studentPerformanceService = {

    async getMyPerformance() {
        const response = await api.get("/performance/me");
        return response.data;
    },

    async getMyPerformanceByTopic(topicId) {
        const response = await api.get(`/performance/me/topic/${topicId}`);
        return response.data;
    },

    async getWeakTopics() {
        const response = await api.get("/performance/me/weak-topics");
        return response.data;
    },
    // start from here those for admin and teacher accsesess

    async getPerformanceByUser(userId) {
        const response = await api.get(`/performance/user/${userId}`);
        return response.data;
    },

    async getPerformanceByUserAndTopic(userId, topicId) {
        const response = await api.get(`/performance/user/${userId}/topic/${topicId}`);
        return response.data;
    },

    async getWeakTopicsByUser(userId) {
        const response = await api.get(`/performance/${userId}/weak-topics`);
        return response.data;
    }

};