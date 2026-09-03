import { ref } from "vue";
import { authService } from "@/services/authService";
import { useAuthStore } from "@/stores/authStore";
export function useLogin(){
    const isLoading = ref(false);
    const serverError = ref('');
    const authStore = useAuthStore();
    async function login(formData){
        isLoading.value = true; 
        serverError.value = '';
        try{
            const data = await authService.login(formData);
            authStore.setAuth(data);
            return data;
        }catch(err){
            serverError.value = err.response?.data?.message || 'username or password not correct';
            return null;
        }finally{
            isLoading.value = false;
        }
        
    }
    async function loginGoogle(idToken) 
    {
    isLoading.value = true;
    serverError.value = "";
    try{
        const data = await authService.loginGoogle(idToken);
        authStore.setAuth(data);
        return data;
    }catch(err){
        serverError.value = 
        err.response?.data?.message || "Google login failed";
        return null;
    }finally{
        isLoading.value = false;
    }    
    }
    return {loginGoogle,login,isLoading,serverError};
}