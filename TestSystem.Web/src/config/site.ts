export type SiteConfig = typeof siteConfig;

export const siteConfig = {
  name: "Vite + NextUI",
  description: "Make beautiful websites regardless of your design experience.",
  navItems: [
    {
      label: "Home",
      href: "/",
    },
    {
        label: "Dashboard",
        href: "/Dashboard",
    },
    {
      label: "Admin",
        href: "/Admin/Dashboard",
    },
    {
        label: "Login",
        href: "/Login",
    },
    {
      label: "Logout",
      href: "/Logout",
    },
  ],
  navMenuItems: [
    {
      label: "Home",
      href: "/dashboard",
    },
    {
      label: "Admin",
      href: "/AdminDashboard",
    },
    {
      label: "Login",
      href: "/Login",
    },
    {
      label: "Logout",
      href: "/logout",
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
