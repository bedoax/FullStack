import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from "@/stores/authStore";
import { authService } from "@/services/authService";

export function useForgotPassword() {
    const isLoading = ref(false);
    const serverError = ref('');
    const router = useRouter(); 

    async function forgotPassword(formData) {
        isLoading.value = true; 
        serverError.value = '';

        try {
            const authStore = useAuthStore();
            await authService.forgotPassword(formData.email);         
               // set reset email on authstore as seaion
            authStore.setResetEmail(formData.email);

            router.replace("/reset-password");
        } catch (err) {
            serverError.value = err.response?.data?.message || 'Failed to send reset code.';
        } finally {
            isLoading.value = false; 
        }
    }

    return { forgotPassword, isLoading, serverError };
}