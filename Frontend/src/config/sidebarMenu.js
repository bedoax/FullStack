import {
  LayoutDashboard,
  BookOpen,
  FileText,
  BarChart3,
  Trophy,
  User,
  Users,
  Shield,
  ClipboardList,
  BookCheck,
  GraduationCap
} from "@lucide/vue";

export const sidebarMenu = {
  Student: [
    { label: "Dashboard", path: "/student/dashboard", icon: LayoutDashboard },
    { label: "Topics", path: "/student/topics", icon: BookOpen },
    { label: "Quizzes", path: "/student/quizzes", icon: FileText },
    { label: "Attempts", path: "/student/attempts", icon: BarChart3 },
    { label: "Leaderboard", path: "/student/leaderboard", icon: Trophy },
    { label: "Profile", path: "/student/profile", icon: User },
  ],

  Teacher: [
    { label: "Dashboard", path: "/teacher/dashboard", icon: LayoutDashboard },
    { label: "Topics", path: "/teacher/topics", icon: BookOpen },
    { label: "Questions", path: "/teacher/questions", icon: ClipboardList },
    { label: "Quizzes", path: "/teacher/quizzes", icon: FileText },
    { label: "Leaderboard", path: "/teacher/leaderboard", icon: Trophy },
    { label: "Profile", path: "/teacher/profile", icon: User },
  ],

  Admin: [
    { label: "Dashboard", path: "/admin/dashboard", icon: LayoutDashboard },
    { label: "Students", path: "/admin/students", icon: Users },
    { label: "Teachers", path: "/admin/teachers", icon: GraduationCap },
    { label: "Roles", path: "/admin/roles", icon: Shield },
    { label: "Topics", path: "/admin/topics", icon: BookOpen },
    { label: "Questions", path: "/admin/questions", icon: ClipboardList },
    { label: "Quizzes", path: "/admin/quizzes", icon: BookCheck },
    { label: "Leaderboard", path: "/admin/leaderboard", icon: Trophy },
    { label: "Profile", path: "/admin/profile", icon: User },
  ],
};