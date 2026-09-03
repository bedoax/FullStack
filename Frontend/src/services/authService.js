import api from "@/api/axios";

export const authService = {
  async register(userData) {
    const response = await api.post("/Auth/register", userData);
    return response.data;
  },

  async login(userData) {
    const response = await api.post("/Auth/login", userData);
    return response.data;
  },

  async forgotPassword(email) {
    const response = await api.post(
      "/Auth/request-password-reset",
      { email }
    );

    return response.data;
  },
async loginGoogle(idToken) {
    const response = await api.post("/Auth/google", {
        idToken
    });

    return response.data;
},
  async resetPassword(data) {
    const response = await api.post("/Auth/reset-password", data);
    return response.data;
  },

  async changePassword(data) {
    const response = await api.post("/Auth/change-password", data);
    return response.data;
  },

  async refreshToken() {
    const response = await api.post("/Auth/refresh-token");
    return response.data;
  },

  async logout() {
    const response = await api.post("/Auth/logout");
    return response.data;
  },
};