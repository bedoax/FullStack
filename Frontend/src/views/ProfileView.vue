<script setup>
import { ref, onMounted } from "vue";

import { User, Mail, Shield, Lock, Trash2, AlertTriangle } from "@lucide/vue";

import { userService } from "@/services/userService";
import { authService } from "@/services/authService";

import { execute } from "@/utils/storeHelper";
import { useAuthStore } from "@/stores/authStore";

// =========================
// Store
// =========================

const authStore = useAuthStore();

// =========================
// State
// =========================

const user = ref(null);
const loadingProfile = ref(false);
const loadingPassword = ref(false);
const deleting = ref(false);

const error = ref("");
const success = ref("");

const showDeleteModal = ref(false);
const deleteError = ref("");

// =========================
// Password Form
// =========================

const passwordForm = ref({
  oldPassword: "",
  newPassword: "",
  confirmPassword: "",
});

// =========================
// Load Profile
// =========================

async function loadProfile() {
  error.value = "";

  try {
    await execute(loadingProfile, async () => {
      user.value = await userService.getMyInformation();
    });
  } catch (err) {
    error.value = err.response?.data?.message || "Failed to load profile.";
  }
}

// =========================
// Change Password
// =========================

async function changePassword() {
  error.value = "";
  success.value = "";

  if (
    !passwordForm.value.oldPassword ||
    !passwordForm.value.newPassword ||
    !passwordForm.value.confirmPassword
  ) {
    error.value = "All password fields are required.";

    return;
  }

  if (passwordForm.value.newPassword !== passwordForm.value.confirmPassword) {
    error.value = "New passwords do not match.";

    return;
  }

  try {
    await execute(loadingPassword, async () => {
      await authService.changePassword({
        oldPassword: passwordForm.value.oldPassword,

        newPassword: passwordForm.value.newPassword,
      });
    });

    passwordForm.value = {
      oldPassword: "",
      newPassword: "",
      confirmPassword: "",
    };

    success.value = "Password changed successfully.";
  } catch (err) {
    error.value = err.response?.data?.message || "Failed to change password.";
  }
}

// =========================
// Open Delete Modal
// =========================

function openDeleteModal() {
  deleteError.value = "";
  showDeleteModal.value = true;
}

// =========================
// Close Delete Modal
// =========================

function closeDeleteModal() {
  if (deleting.value) return;

  showDeleteModal.value = false;
  deleteError.value = "";
}

// =========================
// Delete Account
// =========================

async function deleteAccount() {
  deleteError.value = "";

  try {
    await execute(deleting, async () => {
      await userService.deleteMyAccount();
    });

    showDeleteModal.value = false;

    authStore.logout();
  } catch (err) {
    deleteError.value = err.response?.data?.message || "Failed to delete account.";
  }
}

// =========================
// Mounted
// =========================

onMounted(() => {
  loadProfile();
});
</script>

<template>
  <section class="profile">
    <!-- =========================
             Header
        ========================== -->

    <header class="profile__header">
      <div>
        <h1>Profile</h1>

        <p>Manage your account and security settings.</p>
      </div>
    </header>

    <!-- =========================
             Global Error
        ========================== -->

    <div v-if="error" class="profile__message profile__message--error">
      {{ error }}
    </div>

    <!-- =========================
             Success
        ========================== -->

    <div v-if="success" class="profile__message profile__message--success">
      {{ success }}
    </div>

    <!-- =========================
             Loading
        ========================== -->

    <div v-if="loadingProfile" class="profile__loading">Loading profile...</div>

    <template v-else>
      <!-- =========================
                 Account Information
            ========================== -->

      <article class="profile-card">
        <header class="profile-card__header">
          <div class="profile-card__icon">
            <User :size="22" />
          </div>

          <div>
            <h2>Account Information</h2>

            <p>Your account details.</p>
          </div>
        </header>

        <div class="profile-card__body">
          <!-- Username -->

          <div class="profile-field">
            <User class="profile-field__icon" :size="18" />

            <div>
              <span> Username </span>

              <strong>
                {{ user?.username }}
              </strong>
            </div>
          </div>

          <!-- Email -->

          <div class="profile-field">
            <Mail class="profile-field__icon" :size="18" />

            <div>
              <span> Email </span>

              <strong>
                {{ user?.email }}
              </strong>
            </div>
          </div>

          <!-- Role -->

          <div class="profile-field">
            <Shield class="profile-field__icon" :size="18" />

            <div>
              <span> Role </span>

              <strong>
                {{ user?.roleName }}
              </strong>
            </div>
          </div>
        </div>
      </article>

      <!-- =========================
                 Change Password
            ========================== -->

      <article class="profile-card" v-if="!authStore.signInByGoogle">
        <header class="profile-card__header">
          <div class="profile-card__icon">
            <Lock :size="22" />
          </div>

          <div>
            <h2>Change Password</h2>

            <p>Update your account password.</p>
          </div>
        </header>

        <form class="profile-form" @submit.prevent="changePassword">
          <!-- Current Password -->

          <div class="profile-form__field">
            <label> Current Password </label>

            <input
              v-model="passwordForm.oldPassword"
              type="password"
              placeholder="Enter current password"
              autocomplete="current-password"
            />
          </div>

          <!-- New Password -->

          <div class="profile-form__field">
            <label> New Password </label>

            <input
              v-model="passwordForm.newPassword"
              type="password"
              placeholder="Enter new password"
              autocomplete="new-password"
            />
          </div>

          <!-- Confirm Password -->

          <div class="profile-form__field">
            <label> Confirm New Password </label>

            <input
              v-model="passwordForm.confirmPassword"
              type="password"
              placeholder="Confirm new password"
              autocomplete="new-password"
            />
          </div>

          <!-- Submit -->

          <button type="submit" class="profile-form__button" :disabled="loadingPassword">
            {{ loadingPassword ? "Changing..." : "Change Password" }}
          </button>
        </form>
      </article>

      <!-- =========================
                 Danger Zone
            ========================== -->

      <article class="profile-card profile-card--danger">
        <header class="profile-card__header">
          <div class="profile-card__icon profile-card__icon--danger">
            <Trash2 :size="22" />
          </div>

          <div>
            <h2>Danger Zone</h2>

            <p>Permanently delete your account.</p>
          </div>
        </header>

        <div class="profile-danger">
          <div>
            <strong> Delete Account </strong>

            <p>Once deleted, your account cannot be recovered.</p>
          </div>

          <button
            type="button"
            class="profile-danger__button"
            :disabled="deleting"
            @click="openDeleteModal"
          >
            <Trash2 :size="17" />

            Delete Account
          </button>
        </div>
      </article>
    </template>

    <!-- =========================
             Delete Modal
        ========================== -->

    <Teleport to="body">
      <div
        v-if="showDeleteModal"
        class="delete-modal__overlay"
        @click.self="closeDeleteModal"
      >
        <div
          class="delete-modal"
          role="dialog"
          aria-modal="true"
          aria-labelledby="delete-account-title"
        >
          <!-- Icon -->

          <div class="delete-modal__icon">
            <AlertTriangle :size="28" />
          </div>

          <!-- Content -->

          <div class="delete-modal__content">
            <h2 id="delete-account-title">Delete Account</h2>

            <p>Are you sure you want to delete your account?</p>

            <span>
              This action cannot be undone. Your account and associated data will be
              permanently deleted.
            </span>
          </div>

          <!-- Error -->

          <div v-if="deleteError" class="delete-modal__error">
            {{ deleteError }}
          </div>

          <!-- Actions -->

          <div class="delete-modal__actions">
            <button
              type="button"
              class="delete-modal__cancel"
              :disabled="deleting"
              @click="closeDeleteModal"
            >
              Cancel
            </button>

            <button
              type="button"
              class="delete-modal__confirm"
              :disabled="deleting"
              @click="deleteAccount"
            >
              {{ deleting ? "Deleting..." : "Delete Account" }}
            </button>
          </div>
        </div>
      </div>
    </Teleport>
  </section>
</template>

<style scoped>
.profile {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.profile__header h1 {
  margin: 0;

  color: var(--text-primary);

  font-size: 28px;
}

.profile__header p {
  margin: 6px 0 0;

  color: var(--text-secondary);
}

.profile__loading {
  padding: 40px;

  text-align: center;

  color: var(--text-secondary);
}

.profile__message {
  padding: 12px 16px;

  border-radius: 10px;

  font-size: 14px;
}

.profile__message--error {
  background: color-mix(in srgb, var(--danger) 10%, transparent);

  color: var(--danger);

  border: 1px solid var(--danger);
}

.profile__message--success {
  background: color-mix(in srgb, var(--success) 10%, transparent);

  color: var(--success);

  border: 1px solid var(--success);
}

/* =========================
   Card
========================= */

.profile-card {
  background: var(--card-background);

  border: 1px solid var(--border);

  border-radius: 16px;

  padding: 24px;
}

.profile-card--danger {
  border-color: color-mix(in srgb, var(--danger) 35%, var(--border));
}

.profile-card__header {
  display: flex;

  align-items: center;

  gap: 14px;

  margin-bottom: 24px;
}

.profile-card__icon {
  width: 46px;
  height: 46px;

  border-radius: 12px;

  display: flex;

  align-items: center;
  justify-content: center;

  background: var(--sidebar-hover);

  color: var(--primary);
}

.profile-card__icon--danger {
  background: color-mix(in srgb, var(--danger) 10%, transparent);

  color: var(--danger);
}

.profile-card__header h2 {
  margin: 0;

  color: var(--text-primary);

  font-size: 18px;
}

.profile-card__header p {
  margin: 5px 0 0;

  color: var(--text-secondary);

  font-size: 14px;
}

/* =========================
   Account Information
========================= */

.profile-card__body {
  display: grid;

  grid-template-columns: repeat(2, 1fr);

  gap: 16px;
}

.profile-field {
  display: flex;

  align-items: center;

  gap: 12px;

  padding: 14px;

  border: 1px solid var(--border);

  border-radius: 12px;
}

.profile-field__icon {
  color: var(--text-secondary);
}

.profile-field div {
  display: flex;

  flex-direction: column;

  gap: 4px;
}

.profile-field span {
  font-size: 13px;

  color: var(--text-secondary);
}

.profile-field strong {
  color: var(--text-primary);

  font-size: 15px;
}

/* =========================
   Password Form
========================= */

.profile-form {
  display: grid;

  grid-template-columns: repeat(2, 1fr);

  gap: 18px;
}

.profile-form__field {
  display: flex;

  flex-direction: column;

  gap: 7px;
}

.profile-form__field label {
  font-size: 14px;

  color: var(--text-primary);

  font-weight: 500;
}

.profile-form__field input {
  width: 100%;

  box-sizing: border-box;

  padding: 11px 13px;

  border: 1px solid var(--border);

  border-radius: 10px;

  background: var(--surface);

  color: var(--text-primary);

  outline: none;
}

.profile-form__field input:focus {
  border-color: var(--primary);

  box-shadow: 0 0 0 3px color-mix(in srgb, var(--primary) 15%, transparent);
}

.profile-form__button {
  grid-column: 1 / -1;

  width: fit-content;

  padding: 11px 18px;

  border: none;

  border-radius: 10px;

  background: var(--primary);

  color: white;

  cursor: pointer;

  font-weight: 600;
}

.profile-form__button:hover {
  background: var(--primary-hover);
}

.profile-form__button:disabled {
  opacity: 0.6;

  cursor: not-allowed;
}

/* =========================
   Danger Zone
========================= */

.profile-danger {
  display: flex;

  align-items: center;

  justify-content: space-between;

  gap: 20px;
}

.profile-danger strong {
  color: var(--text-primary);
}

.profile-danger p {
  margin: 5px 0 0;

  color: var(--text-secondary);

  font-size: 14px;
}

.profile-danger__button {
  display: inline-flex;

  align-items: center;

  justify-content: center;

  gap: 8px;

  padding: 10px 16px;

  border: 1px solid var(--danger);

  border-radius: 10px;

  background: transparent;

  color: var(--danger);

  cursor: pointer;

  font-weight: 600;

  white-space: nowrap;

  transition: background 0.2s ease, color 0.2s ease, opacity 0.2s ease;
}

.profile-danger__button:hover {
  background: var(--danger);

  color: white;
}

.profile-danger__button:disabled {
  opacity: 0.6;

  cursor: not-allowed;
}

/* =========================
   Delete Modal
========================= */

.delete-modal__overlay {
  position: fixed;

  inset: 0;

  z-index: 1000;

  display: flex;

  align-items: center;

  justify-content: center;

  padding: 24px;

  background: rgba(0, 0, 0, 0.65);

  backdrop-filter: blur(5px);
}

.delete-modal {
  width: min(100%, 440px);

  box-sizing: border-box;

  padding: 32px;

  border-radius: 18px;

  background: var(--card-background);

  border: 1px solid var(--border);

  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);

  text-align: center;

  animation: delete-modal-in 0.2s ease-out;
}

.delete-modal__icon {
  width: 58px;
  height: 58px;

  margin: 0 auto 20px;

  display: flex;

  align-items: center;
  justify-content: center;

  border-radius: 50%;

  background: color-mix(in srgb, var(--danger) 12%, transparent);

  color: var(--danger);
}

.delete-modal__content h2 {
  margin: 0 0 10px;

  color: var(--text-primary);

  font-size: 22px;
}

.delete-modal__content p {
  margin: 0 0 10px;

  color: var(--text-primary);

  font-size: 15px;

  font-weight: 600;
}

.delete-modal__content span {
  display: block;

  color: var(--text-secondary);

  font-size: 14px;

  line-height: 1.6;
}

.delete-modal__error {
  margin-top: 20px;

  padding: 10px 12px;

  border-radius: 8px;

  background: color-mix(in srgb, var(--danger) 10%, transparent);

  color: var(--danger);

  font-size: 13px;
}

.delete-modal__actions {
  display: flex;

  gap: 12px;

  margin-top: 28px;
}

.delete-modal__actions button {
  flex: 1;

  height: 44px;

  border: 0;

  border-radius: 9px;

  font-size: 14px;

  font-weight: 600;

  cursor: pointer;

  transition: background 0.2s ease, opacity 0.2s ease, transform 0.2s ease;
}

.delete-modal__actions button:not(:disabled):active {
  transform: scale(0.98);
}

.delete-modal__cancel {
  background: var(--surface);

  color: var(--text-primary);

  border: 1px solid var(--border) !important;
}

.delete-modal__cancel:hover {
  background: var(--sidebar-hover);
}

.delete-modal__confirm {
  background: var(--danger);

  color: white;
}

.delete-modal__confirm:hover {
  filter: brightness(0.9);
}

.delete-modal__actions button:disabled {
  cursor: not-allowed;

  opacity: 0.6;
}

/* =========================
   Animation
========================= */

@keyframes delete-modal-in {
  from {
    opacity: 0;

    transform: translateY(8px) scale(0.97);
  }

  to {
    opacity: 1;

    transform: translateY(0) scale(1);
  }
}

/* =========================
   Responsive
========================= */

@media (max-width: 700px) {
  .profile-card__body,
  .profile-form {
    grid-template-columns: 1fr;
  }

  .profile-danger {
    flex-direction: column;

    align-items: flex-start;
  }

  .profile-danger__button {
    width: 100%;
  }

  .delete-modal {
    padding: 24px;
  }

  .delete-modal__actions {
    flex-direction: column;
  }
}
</style>
