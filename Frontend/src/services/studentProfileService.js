import api from "@/api/axios";

export const studentProfileService = {

    async getProfile() {
        const response = await api.get("/StudentProfiles/me");
        return response.data;
    },

    async getSkillScore() {
        const response = await api.get("/StudentProfiles/me/skill-score");
        return response.data;
    },

    async getCurrentLevel() {
        const response = await api.get("/StudentProfiles/me/current-level");
        return response.data;
    },

    async getAttempts() {
        const response = await api.get("/StudentProfiles/me/attempts");
        return response.data;
    },

    async getDashboard(signal) {
        const response = await api.get("/StudentProfiles/me/dashboard",{signal});
        return response.data;
    },
    // start from here those for admin and teacher accsesess
    async getProfileByUser(userId) {
        const response = await api.get(`/StudentProfiles/user/${userId}`);
        return response.data;
    },

    async getSkillScoreByUser(userId) {
        const response = await api.get(`/StudentProfiles/user/${userId}/skill-score`);
        return response.data;
    },

    async getCurrentLevelByUser(userId) {
        const response = await api.get(`/StudentProfiles/user/${userId}/current-level`);
        return response.data;
    },

    async getAttemptsByUser(userId) {
        const response = await api.get(`/StudentProfiles/user/${userId}/attempts`);
        return response.data;
    },

    async getDashboardByUser(userId) {
        const response = await api.get(`/StudentProfiles/${userId}/dashboard`);
        return response.data;
    }

};