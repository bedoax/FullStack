<script setup>
import { ref, onMounted } from "vue";
import { Plus } from "lucide-vue-next";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";

import AdminRoleCard from "@/components/roles/AdminRoleCard.vue";
import CreateRoleCard from "@/components/roles/CreateRoleCard.vue";
import EditRoleCard from "@/components/roles/EditRoleCard.vue";
import { roleService } from "@/services/roleService";

// State
const roles = ref([]);
const loading = ref(false);
const error = ref("");

// Modals State
const isCreateOpen = ref(false);
const isEditOpen = ref(false);
const selectedRole = ref(null);
async function loadRoles() {
  loading.value = true;
  error.value = "";

  try {
    const response = await roleService.GetAllRoles();
    roles.value = response.data || response;
  } catch (err) {
    console.error("Failed to load roles:", err);
    error.value = err.response?.data?.message || "Failed to load roles.";
    toast.error(error.value);
  } finally {
    loading.value = false;
  }
}

// Delete Role
async function handleDeleteRole(role) {
  if (!confirm(`Are you sure you want to delete the role: "${role.name}"?`)) {
    return;
  }

  try {
    loading.value = true;
    await roleService.DeleteRole(role.id);
    roles.value = roles.value.filter((r) => r.id !== role.id);

    toast.success(`Role "${role.name}" deleted successfully!`);
  } catch (err) {
    console.error("Delete failed:", err);
    const msg = err.response?.data?.message || "Failed to delete role.";
    toast.error(msg);
  } finally {
    loading.value = false;
  }
}

// Create Role
async function handleCreateRole(name) {
  try {
    await roleService.CreateRole({ name });
    toast.success(`Role "${name}" created successfully!`);
    isCreateOpen.value = false;
    await loadRoles();
  } catch (err) {
    console.error("Create failed:", err);
    const msg = err.response?.data?.message || "Failed to create role.";
    toast.error(msg);
  }
}

// Edit Role
function openEditModal(role) {
  selectedRole.value = { ...role };
  isEditOpen.value = true;
}

async function handleUpdateRole({ id, name }) {
  try {
    await roleService.UpdateRole(id, { name });
    toast.success("Role updated successfully!");
    isEditOpen.value = false;
    await loadRoles();
  } catch (err) {
    console.error("Update failed:", err);
    const msg = err.response?.data?.message || "Failed to update role.";
    toast.error(msg);
  }
}

onMounted(() => {
  loadRoles();
});
</script>

<template>
  <section class="roles-view">
    <!-- Header -->
    <header class="roles-view__header">
      <div>
        <h1>Roles Management</h1>
        <p>Manage user roles and system access.</p>
      </div>
      <button type="button" class="create-btn" @click="isCreateOpen = true">
        <Plus :size="18" />
        <span>Create Role</span>
      </button>
    </header>

    <!-- State Messages -->
    <div v-if="loading" class="roles-view__state">
      <p>Loading roles...</p>
    </div>

    <div v-else-if="error" class="roles-view__state form-error">
      <p>{{ error }}</p>
      <button type="button" class="retry-btn" @click="loadRoles">Retry</button>
    </div>

    <div v-else-if="roles.length === 0" class="roles-view__state">
      <h3>No Roles Found</h3>
      <p>Start by creating a new role.</p>
    </div>

    <!-- Grid -->
    <div v-else class="roles-view__grid">
      <AdminRoleCard
        v-for="role in roles"
        :key="role.id"
        :role="role"
        @edit="openEditModal"
        @delete-role="handleDeleteRole"
      />
    </div>

    <!-- Modals -->
    <CreateRoleCard
      :is-open="isCreateOpen"
      @close="isCreateOpen = false"
      @submit="handleCreateRole"
    />

    <EditRoleCard
      :is-open="isEditOpen"
      :role="selectedRole"
      @close="isEditOpen = false"
      @submit="handleUpdateRole"
    />
  </section>
</template>

<style scoped>
.roles-view {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1.5rem;
}

.roles-view__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
  margin-bottom: 2rem;
  flex-wrap: wrap;
}

.roles-view__header h1 {
  font-size: 1.75rem;
  font-weight: 700;
  color: var(--text-primary, #0f172a);
}

.roles-view__header p {
  color: var(--text-secondary, #64748b);
  font-size: 0.95rem;
}

.create-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.65rem 1.25rem;
  background: var(--primary, #6366f1);
  color: #ffffff;
  border: none;
  border-radius: 10px;
  font-weight: 600;
  font-size: 0.9rem;
  cursor: pointer;
  transition: 0.2s;
}

.create-btn:hover {
  opacity: 0.9;
}

.roles-view__grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.5rem;
}

.roles-view__state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 4rem 2rem;
  background: var(--surface, #ffffff);
  border-radius: 16px;
  border: 1px solid var(--border, #e2e8f0);
}

.retry-btn {
  margin-top: 1rem;
  padding: 0.5rem 1rem;
  background: var(--primary, #6366f1);
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
}
</style>
