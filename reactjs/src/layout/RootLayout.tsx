import { AddLocaleTranslationforPrime } from "../lib/AddLocaleTranslationforPrime";
import { LayoutProvider } from "./context/layoutcontext";
import { PrimeReactProvider } from "primereact/api";

interface RootLayoutProps {
  children: React.ReactNode;
}

AddLocaleTranslationforPrime();

export default function RootLayout({ children }: RootLayoutProps) {
  return (
    <PrimeReactProvider>
      <LayoutProvider>{children}</LayoutProvider>
    </PrimeReactProvider>
  );
}
