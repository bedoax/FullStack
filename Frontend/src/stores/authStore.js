/*//old version without import { ref, computed } from "vue";
import { defineStore } from "pinia";
import { authService } from "@/services/authService";
export const useAuthStore = defineStore("auth", () => {

    const initialState = {
        accessToken: "",
        refreshToken: "",
        accessTokenExpiresAt: "",
        refreshTokenExpiresAt: "",
        userId: null,
        email: "",
        roleName: "",
        username: ""
    };

    const auth = ref({
        ...initialState
    });

const resetEmail = ref(
    JSON.parse(sessionStorage.getItem("resetEmail") || '""')
  );

    // =========================
    // Computed
    // =========================

    const isAuthenticated = computed(() => {

        if (!auth.value.accessToken)
            return false;

        if (!auth.value.accessTokenExpiresAt)
            return false;

        return (
            Date.parse(auth.value.accessTokenExpiresAt)
            > Date.now()
        );
    });

    const userId = computed(() => auth.value.userId);

    const username = computed(() => auth.value.username);

    const email = computed(() => auth.value.email);

    const role = computed(() => auth.value.roleName);


    // =========================
    // Auth State
    // =========================

    function setAuth(data) {

        auth.value = {
            ...initialState,
            ...data
        };

        localStorage.setItem(
            "auth",
            JSON.stringify(auth.value)
        );
    }


    // =========================
    // Initialize
    // =========================

    function initialize() {

        const storedAuth =
            localStorage.getItem("auth");

        if (!storedAuth)
            return;

        try {

            const data =
                JSON.parse(storedAuth);

            auth.value = {
                ...initialState,
                ...data
            };

        }
        catch (error) {

            console.error(
                "Failed to restore authentication.",
                error
            );

            clearAuth();
        }
    }


function updateAuth(data) {

    auth.value = {
        ...auth.value,

        accessToken: data.accessToken,
        refreshToken: data.refreshToken,
        accessTokenExpiresAt:
            data.accessTokenExpiresAt,
        refreshTokenExpiresAt:
            data.refreshTokenExpiresAt,

        userId: data.userId,
        email: data.email,
        username: data.username,
        roleName: data.roleName,
    };

    localStorage.setItem(
        "auth",
        JSON.stringify(auth.value)
    );
}




    // =========================
    // Logout
    // =========================

    function clearAuth() {

        auth.value = {
            ...initialState
        };

        localStorage.removeItem("auth");
    }


    function logout() {

        clearAuth();

        clearResetEmail();
    }


    // =========================
    // Reset Password Email
    // =========================

    function setResetEmail(email) {

        resetEmail.value = email;

        sessionStorage.setItem(
            "resetEmail",
            JSON.stringify(email)
        );
    }


    function clearResetEmail() {

        resetEmail.value = "";

        sessionStorage.removeItem(
            "resetEmail"
        );
    }


    return {

        auth,

        userId,
        username,
        email,
        role,

        isAuthenticated,

        resetEmail,

        setAuth,
        initialize,
        updateAuth,
        clearAuth,
        logout,

        setResetEmail,
        clearResetEmail
    };
});
*/

import { ref, computed } from "vue";
import { defineStore } from "pinia";
import { authService } from "@/services/authService";
import router from "@/router";

export const useAuthStore = defineStore("auth", () => {
  const initialState = {
    accessToken: "",
    accessTokenExpiresAt: "",
    userId: null,
    email: "",
    roleName: "",
    username: "",
    signInByGoogle:false
  };

  const auth = ref({ ...initialState });

  const resetEmail = ref(
    JSON.parse(sessionStorage.getItem("resetEmail") || '""')
  );

  const isAuthenticated = computed(() => {
    if (!auth.value.accessToken || !auth.value.accessTokenExpiresAt) {
      return false;
    }
    return Date.parse(auth.value.accessTokenExpiresAt) > Date.now();
  });

  const userId = computed(() => auth.value.userId);
  const username = computed(() => auth.value.username);
  const email = computed(() => auth.value.email);
  const role = computed(() => auth.value.roleName);
  const accessToken = computed(() => auth.value.accessToken);
  const signInByGoogle = computed(()=>auth.value.signInByGoogle);


  function setAuth(data) {
    auth.value = {
      ...initialState,
      ...data,
    };
  }

  function setAccessToken(newToken, expiresAt) {
    auth.value.accessToken = newToken;
    auth.value.accessTokenExpiresAt = expiresAt;
  }

  function clearAuth() {
    auth.value = { ...initialState };
    localStorage.removeItem("auth");
    localStorage.removeItem("token");
    localStorage.removeItem("user");
  }

  async function refreshToken() {
    try {
      const data = await authService.refreshToken();
      setAuth(data);
      return true;
    } catch (error) {
      clearAuth();
      return false;
    }
  }

  async function logout() {
    try {
      await authService.logout();
    } catch (error) {
      console.error("Logout error:", error);
    } finally {
      clearAuth();
      clearResetEmail();
      router.push("/login");
    }
  }


  function setResetEmail(emailVal) {
    resetEmail.value = emailVal;
    sessionStorage.setItem("resetEmail", JSON.stringify(emailVal));
  }

  function clearResetEmail() {
    resetEmail.value = "";
    sessionStorage.removeItem("resetEmail");
  }

  return {
    auth,
    userId,
    username,
    email,
    role,
    accessToken,
    isAuthenticated,
    resetEmail,
    signInByGoogle,
    setAuth,
    setAccessToken,
    clearAuth,
    refreshToken,
    logout,

    setResetEmail,
    clearResetEmail,
  };
});


