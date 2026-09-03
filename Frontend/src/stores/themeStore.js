import { defineStore } from "pinia";
import { ref } from "vue";

export const useThemeStore = defineStore("theme", () => {
    const theme = ref(localStorage.getItem("theme") || "light");
    function setTheme(newTheme) {
        theme.value = newTheme;
        localStorage.setItem("theme", newTheme);
    }

    return {
        theme,
        setTheme
    };
});