import { NextUIProvider } from "@nextui-org/system";
import { useNavigate } from "react-router-dom";
import {ThemeProvider as NextThemesProvider} from "next-themes";
export default function Provider({
  children,
}: {
  children: React.ReactNode;
}) {
  const navigate = useNavigate();
  return (
    <NextUIProvider navigate={navigate}>
      <NextThemesProvider attribute="class" defaultTheme="dark">
        {children}
      </NextThemesProvider>
    </NextUIProvider>
    
  );
}
