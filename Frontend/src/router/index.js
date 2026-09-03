import { createRouter, createWebHistory } from "vue-router";
import { useAuthStore } from "@/stores/authStore";
import HomeView from "../views/HomeView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),

  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView,
    },
    {
      path: "/about",
      name: "about",
      component: () => import("../views/AboutView.vue"),
    },
    {
      path: "/register",
      name: "register",
      component: () => import("../views/RegisterView.vue"),
      meta: { guestOnly: true },
    },
    {
      path: "/login",
      name: "login",
      component: () => import("../views/LoginView.vue"),
      meta: { guestOnly: true },
    },
    {
      path: "/forgot-password",
      name: "forgot-password",
      component: () => import("../views/ForgotPasswordView.vue"),
    },
    {
      path: "/reset-password",
      name: "reset-password",
      component: () => import("../views/ResetPasswordView.vue"),
    },


    {
      path: "/unauthorized",
      name: "Unauthorized",
      component: () => import("../views/UnauthorizedView.vue"),
    },


    {
      path: "/student",
      component: () => import("@/layouts/StudentLayout.vue"),
      meta: { requiresAuth: true, roles: ["Student"] },
      redirect: "/student/dashboard",
      children: [
        {
          path: "dashboard",
          name: "student-dashboard",
          component: () => import("@/views/student/DashboardView.vue"),
        },
        {
          path: "topics",
          name: "student-topics",
          component: () => import("@/views/student/TopicsView.vue"),
        },
        {
          path: "quizzes",
          name: "student-quizzes",
          component: () => import("@/views/student/QuizzesView.vue"),
        },
        {
          path: "attempts",
          name: "student-attempts",
          component: () => import("@/views/student/AttemptsView.vue"),
        },
        {
          path: "attempts/:attemptId",
          name: "exam",
          component: () => import("@/views/student/ExamView.vue"),
        },
        {
          path: "attempts/:attemptId/review",
          name: "attempt-review",
          component: () => import("@/views/student/AttemptReviewView.vue"),
        },
        // Shared Pages inside Student Layout
        {
          path: "leaderboard",
          name: "student-leaderboard",
          component: () => import("../views/LeaderboardView.vue"),
        },
        {
          path: "profile",
          name: "student-profile",
          component: () => import("../views/ProfileView.vue"),
        },
      ],
    },


    {
      path: "/teacher",
      component: () => import("@/layouts/TeacherLayout.vue"),
      meta: { requiresAuth: true, roles: ["Teacher"] },
      redirect: "/teacher/dashboard",
      children: [
        {
          path: "dashboard",
          name: "teacher-dashboard",
          component: () => import("@/views/teacher/DashboardView.vue"),
        },
        {
          path: "topics",
          name: "teacher-topics",
          component: () => import("@/views/teacher/TopicsView.vue"),
        },
        {
          path: "questions",
          name: "teacher-questions",
          component: () => import("@/views/teacher/QuestionsView.vue"),
        },
        {
          path: "quizzes",
          name: "teacher-quizzes",
          component: () => import("@/views/teacher/QuizzesView.vue"),
        },
        {
          path: "students",
          name: "teacher-students",
          component: () => import("@/views/teacher/StudentsView.vue"),
        },
        {
          path: "quizzes/:quizId/students",
          name: "teacher-quiz-students",
          component: () => import("@/views/teacher/TeacherQuizStudentsView.vue"),
        },
        // Shared Pages inside Teacher Layout
        {
          path: "leaderboard",
          name: "teacher-leaderboard",
          component: () => import("../views/LeaderboardView.vue"),
        },
        {
          path: "profile",
          name: "teacher-profile",
          component: () => import("../views/ProfileView.vue"),
        },
      ],
    },


    {
      path: "/admin",
      component: () => import("@/layouts/AdminLayout.vue"),
      meta: { requiresAuth: true, roles: ["Admin"] },
      redirect: "/admin/dashboard",
      children: [
        {
          path: "dashboard",
          name: "admin-dashboard",
          component: () => import("@/views/admin/DashboardView.vue"),
        },
        {
          path: "teachers",
          name: "admin-teachers",
          component: () => import("@/views/admin/TeacherView.vue"),
        },
        {
          path: "students",
          name: "admin-students",
          component: () => import("@/views/admin/StudentView.vue"),
        },
        {
          path: "topics",
          name: "admin-topics",
          component: () => import("@/views/admin/AdminTopicView.vue"),
        },
        {
          path: "quizzes",
          name: "admin-quizzes",
          component: () => import("@/views/admin/AdminQuizzView.vue"),
        },
        {
          path: "questions",
          name: "admin-questions",
          component: () => import("@/views/admin/AdminQuestionView.vue"),
        },
        {
          path: "roles",
          name: "admin-roles",
          component: () => import("@/views/admin/RolesView.vue"),
        },
        // Shared Pages inside Admin Layout
        {
          path: "leaderboard",
          name: "admin-leaderboard",
          component: () => import("../views/LeaderboardView.vue"),
        },
        {
          path: "profile",
          name: "admin-profile",
          component: () => import("../views/ProfileView.vue"),
        },
      ],
    },

    // 404 Route
    {
      path: "/:pathMatch(.*)*",
      name: "not-found",
      component: () => import("../views/NotFoundView.vue"),
    },
  ],
});

function getDashboardRoute(role) {
  if (!role) return { name: "home" };
  return { name: `${role.toLowerCase()}-dashboard` };
}

router.beforeEach(async (to) => {
  const authStore = useAuthStore();

  const isGuestOnly = to.matched.some(
    (route) => route.meta.guestOnly
  );

  const requiresAuth = to.matched.some(
    (route) => route.meta.requiresAuth
  );



  if (isGuestOnly) {
    if (authStore.isAuthenticated) {
      return getDashboardRoute(authStore.role);
    }

    return true;
  }



  if (requiresAuth && !authStore.isAuthenticated) {
    const refreshed = await authStore.refreshToken();

    if (!refreshed) {
      return {
        name: "login",
        query: { redirect: to.fullPath },
      };
    }
  }



  const routeWithRoles = to.matched.find(
    (route) => route.meta.roles
  );

  if (routeWithRoles) {
    const allowedRoles = routeWithRoles.meta.roles;

    if (!allowedRoles.includes(authStore.role)) {
      return { name: "Unauthorized" };
    }
  }

  return true;
});

export default router;