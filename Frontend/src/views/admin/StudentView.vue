<script setup>
import { ref, onMounted } from "vue";
import { userService } from "@/services/userService";
import AdminStudentCard from "@/components/students/AdminStudentCard.vue";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

const students = ref([]);
const isLoading = ref(false);
const currentPage = ref(1);
const pageSize = ref(10);
const totalPages = ref(1);

const fetchStudents = async () => {
  try {
    isLoading.value = true;
    const response = await userService.getStudents(currentPage.value, pageSize.value);
    students.value = response.items || [];
    totalPages.value = response.totalPages || 1;
  } catch (error) {
    console.error("Failed to fetch students:", error);
    toast.error("Failed to load students");
  } finally {
    isLoading.value = false;
  }
};

const handleActivate = async (userId) => {
  try {
    await userService.activeUser(userId);
    const student = students.value.find((t) => t.id === userId);
    if (student) {
      student.isDeleted = false;
      toast.success("Activated successfully!");
    }
  } catch (error) {
    console.error("Failed to activate user:", error);
    toast.error("Failed to activate user");
  }
};

const handleDelete = async (userId) => {
  try {
    await userService.deleteUser(userId);
    const student = students.value.find((t) => t.id === userId);
    if (student) {
      student.isDeleted = true;
      toast.success("Deleted successfully!");
    }
  } catch (error) {
    console.error("Failed to delete user:", error);
    toast.error("Failed to delete user");
  }
};

const changePage = (newPage) => {
  if (newPage >= 1 && newPage <= totalPages.value) {
    currentPage.value = newPage;
    fetchStudents();
  }
};

onMounted(() => {
  fetchStudents();
});
</script>

<template>
  <div class="students-container">
    <div v-if="isLoading" class="loading">Loading students...</div>

    <template v-else>
      <div v-if="students.length > 0" class="students-grid">
        <AdminStudentCard
          v-for="student in students"
          :key="student.id"
          :student="student"
          @activate="handleActivate"
          @delete="handleDelete"
        />
      </div>
      <div v-else class="empty-state">No students found.</div>

      <div v-if="totalPages > 1" class="pagination">
        <button
          class="btn-page"
          :disabled="currentPage === 1"
          @click="changePage(currentPage - 1)"
        >
          Previous
        </button>
        <span>Page {{ currentPage }} of {{ totalPages }}</span>
        <button
          class="btn-page"
          :disabled="currentPage === totalPages"
          @click="changePage(currentPage + 1)"
        >
          Next
        </button>
      </div>
    </template>
  </div>
</template>

<style scoped>
.students-container {
  padding: 16px;
}

.students-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 16px;
}

.loading,
.empty-state {
  text-align: center;
  padding: 24px;
  color: var(--text-secondary);
}

.pagination {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 16px;
  margin-top: 24px;
}

.btn-page {
  padding: 8px 16px;
  border-radius: 8px;
  border: 1px solid var(--border);
  background: var(--card-background);
  color: var(--text-primary);
  cursor: pointer;
  font-weight: 600;
}

.btn-page:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
