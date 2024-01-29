import { AddTrLocaleforPrimeReact } from "../lib/AddTrLocaleforPrimeReact";
import { LayoutProvider } from "./context/layoutcontext";
import { PrimeReactProvider } from "primereact/api";

interface RootLayoutProps {
  children: React.ReactNode;
}

export default function RootLayout({ children }: RootLayoutProps) {
  AddTrLocaleforPrimeReact();
  return (
    <PrimeReactProvider>
      <LayoutProvider>{children}</LayoutProvider>
    </PrimeReactProvider>
  );
}
