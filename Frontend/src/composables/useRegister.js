import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { authService } from "@/services/authService";

export function useRegister() {
    const isLoading = ref(false);
    const serverError = ref('');
    const router = useRouter(); 

    async function register(formData) {
        isLoading.value = true; 
        serverError.value = '';

        try {
            await authService.register(formData);
            router.push('/login');
        } catch (err) {
            serverError.value = err.response?.data?.message || 'There was an error while registering the data';
        } finally {
            isLoading.value = false; 
        }
    }

    return { register, isLoading, serverError };
}