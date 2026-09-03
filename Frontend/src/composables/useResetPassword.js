import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from "@/stores/authStore";
import { authService } from "@/services/authService";

export function useResetPassword() {
    const isLoading = ref(false);
    const serverError = ref('');
    const router = useRouter(); 

    async function resetPassword(formData) {
        isLoading.value = true; 
        serverError.value = '';

        try {
            const authStore = useAuthStore();
            await authService.resetPassword(formData);
            authStore.clearResetEmail();
            router.replace("/login");
        } catch (err) {
            serverError.value = err.response?.data?.message || 'Failed to reset password.';
        } finally {
            isLoading.value = false; 
        }
    }

    return { resetPassword, isLoading, serverError };
}