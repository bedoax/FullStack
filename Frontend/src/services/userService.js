import api from "@/api/axios";

export const userService = {

    // =========================
    // Admin
    // =========================

    async GetUsers(page = 1, pageSize = 10) {
        const response = await api.get(
            `/users?page=${page}&pageSize=${pageSize}`
        );

        return response.data;
    },

    async GetUserById(id) {
        const response = await api.get(
            `/users/${id}`
        );

        return response.data;
    },

    async CreateAdmin(userData) {
        const response = await api.post(
            "/users/admins",
            userData
        );

        return response.data;
    },

    async CreateTeacher(userData) {
        const response = await api.post(
            "/users/teachers",
            userData
        );

        return response.data;
    },
    async getTeachers(){
        const response = await api.get("users/teachers");
        return response.data;
    },
    async activeUser(userId) {
        const response = await api.put(`users/${userId}/active`);
        return response;
    },
    async getStudents(page = 1, size = 10) {
        const response = await api.get("users/students", {
            params: {
                        page: page,
                        pageSize: size 
                    }
                    });
        return response.data;
},
    // =========================
    // Student
    // =========================


    async getMyInformation(){
        const response = await api.get(
            "/users/me"
        );
        return response.data;
    },
    async UpdateMyProfile(userData) {
        const response = await api.put(
            "/users/me",
            userData
        );

        return response.data;
    },

    async deleteMyAccount() {
        const response = await api.delete(
            "/users/me"
        );

        return response.data;
    },
    async deleteUser(userId){
      const response = await api.delete(`users/${userId}/delete`);
        return response;
    }

};