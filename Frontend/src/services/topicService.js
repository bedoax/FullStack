import api from "@/api/axios";

export const topicService = {

    async GetAllTopics() {
        const response = await api.get(
            "/topics"
        );

        return response.data;
    },

    async GetTopicById(id) {
        const response = await api.get(
            `/topics/${id}`
        );

        return response.data;
    },

    async CreateTopic(topicData) {
        const response = await api.post(
            "/topics",
            topicData
        );

        return response.data;
    },

    async UpdateTopic(id, topicData) {
        const response = await api.put(
            `/topics/${id}`,
            topicData
        );

        return response.data;
    },

    async DeleteTopic(id) {
        const response = await api.delete(
            `/topics/${id}`
        );

        return response.data;
    },

    async GetTopicQuestions(topicId) {
        const response = await api.get(
            `/topics/${topicId}/questions`
        );

        return response.data;
    },

    async GetLeaderboard(topicId) {
        const response = await api.get(
            `/topics/${topicId}/leaderboard`
        );

        return response.data;
    },

    async GetStatistics(topicId) {
        const response = await api.get(
            `/topics/${topicId}/statistics`
        );

        return response.data;
    }

};