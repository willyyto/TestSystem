import { HeroUIProvider } from "@heroui/system";
import { useNavigate } from "react-router-dom";
import { AuthProvider } from 'contexts/AuthContext';
import {ThemeProvider as NextThemesProvider} from "next-themes";
export default function Provider({
  children,
}: {
  children: React.ReactNode;
}) {
  const navigate = useNavigate();
  return (
      <AuthProvider>
        <HeroUIProvider navigate={navigate}>
          <NextThemesProvider attribute="class" defaultTheme="dark">
            {children}
          </NextThemesProvider>
        </HeroUIProvider>
      </AuthProvider>
  );
}
