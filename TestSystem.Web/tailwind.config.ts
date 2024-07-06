import type { Config } from 'tailwindcss'

export default {
  content: ["./index.html", "./node_modules/primereact/**/*.{js,ts,jsx,tsx}", "./src/**/*.{js,ts,jsx,tsx,mdx}"],
  theme: {
    extend: {},
  },
  plugins: [],
} satisfies Config

