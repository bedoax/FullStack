<script setup>
import { ref, onMounted } from "vue";
import { userService } from "@/services/userService";
import AdminTeacherCard from "@/components/teacher/AdminTeacherCard.vue";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

const teachers = ref([]);
const isLoading = ref(false);

const fetchTeachers = async () => {
  try {
    isLoading.value = true;
    teachers.value = await userService.getTeachers();
  } catch (error) {
    console.error("Failed to fetch teachers:", error);
  } finally {
    isLoading.value = false;
  }
};

const handleActivate = async (userId) => {
  try {
    await userService.activeUser(userId);
    const teacher = teachers.value.find((t) => t.id === userId);
    if (teacher) {
      teacher.isDeleted = false;
      toast.success("Actived successfully!");
    }
  } catch (error) {
    console.error("Failed to activate user:", error);
  }
};
const handleDelete = async (userId) => {
  try {
    await userService.deleteUser(userId);
    const teacher = teachers.value.find((t) => t.id === userId);
    if (teacher) {
      teacher.isDeleted = true;
      toast.success("Deleted successfully!");
    }
  } catch (error) {
    console.error("Failed to delete user:", error);
  }
};
onMounted(() => {
  fetchTeachers();
});
</script>

<template>
  <div class="teachers-container">
    <div v-if="isLoading" class="loading">Loading teachers...</div>

    <div v-else class="teachers-grid">
      <AdminTeacherCard
        v-for="teacher in teachers"
        :key="teacher.id"
        :teacher="teacher"
        @activate="handleActivate"
        @delete="handleDelete"
      />
    </div>
  </div>
</template>

<style scoped>
.teachers-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 16px;
  padding: 16px;
}

.loading {
  text-align: center;
  padding: 24px;
}
</style>
