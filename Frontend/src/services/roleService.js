import api from "@/api/axios";

export const roleService = {

    async GetAllRoles() {
        const response = await api.get("/roles");

        return response.data;
    },

    async GetRoleById(id) {
        const response = await api.get(
            `/roles/${id}`
        );

        return response.data;
    },

    async CreateRole(roleData) {
        const response = await api.post(
            "/roles",
            roleData
        );

        return response.data;
    },

    async UpdateRole(id, roleData) {
        const response = await api.put(
            `/roles/${id}`,
            roleData
        );

        return response.data;
    },

    async DeleteRole(id) {
        const response = await api.delete(
            `/roles/${id}`
        );

        return response.data;
    }

};