export type SiteConfig = typeof siteConfig;
import AppRoutes from "navigation/AppRoutes.ts";

export const siteConfig = {
  name: "Vite + NextUI",
  description: "Make beautiful websites regardless of your design experience.",
  navItems: [
    {
      label: "Home",
      href: AppRoutes.root,
    },
    {
      label: "Dashboard",
      href: AppRoutes.dashboard,
    },
    {
      label: "Login",
      href: AppRoutes.login,
    }
  ],
  navMenuItems: [
    {
      label: "Home",
      href: AppRoutes.root,
    },
    {
      label: "Dashboard",
      href: AppRoutes.dashboard,
    },
    {
      label: "Logout",
      href: AppRoutes.logout,
    },
  ],
  sidebarItems: [
    {
      title: "Overview",
      items: [
        {title: "Home", icon: "solar:home-2-outline", href: AppRoutes.root},
        {title: "Dashboard", icon: "solar:widget-2-outline", href: AppRoutes.admindashboard},
        {title: "Test", icon: "solar:checklist-minimalistic-outline", href: AppRoutes.admintest},
        {title: "Company", icon: "solar:buildings-outline", href: AppRoutes.admincompany},
        {title: "Result", icon: "solar:archive-minimalistic-outline", href: "#"},
      ],
    },
    {
      title: "General",
      items: [
        {title: "User View", icon: "solar:user-id-outline", href: AppRoutes.adminuserview},
        {title: "Analytics", icon: "solar:chart-outline", href: "#"},
          {title: "Account Management", icon: "solar:users-group-rounded-outline", href: AppRoutes.adminuser},
      ],
    },
  ],
  links: {
    github: "https://github.com/nextui-org/nextui",
    twitter: "https://twitter.com/getnextui",
    docs: "https://nextui-docs-v2.vercel.app",
    discord: "https://discord.gg/9b6yyZKmH4",
    sponsor: "https://patreon.com/jrgarciadev",
  },
};
