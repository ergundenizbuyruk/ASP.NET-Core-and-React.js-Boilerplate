import GlobalSpinner from "@/components/global-spinner";
import "@/global.css";
import { RootState, store } from "@/redux/store";
import "@/services/api/interceptors";
import { useFonts } from "expo-font";
import { Slot, useRouter } from "expo-router";
import { useEffect } from "react";
import { SafeAreaView } from "react-native";
import "react-native-reanimated";
import { Provider, useSelector } from "react-redux";

export default function RootLayout() {
  const [loaded] = useFonts({
    SpaceMono: require("../assets/fonts/SpaceMono-Regular.ttf"),
  });

  if (!loaded) return null;

  return (
    <Provider store={store}>
      <ThemedLayout />
    </Provider>
  );
}

function ThemedLayout() {
  const theme = useSelector((state: RootState) => state.theme.theme);
  const isAuthenticated = useSelector(
    (state: RootState) => state.auth.isAuthenticated
  );
  const router = useRouter();

  useEffect(() => {
    if (!isAuthenticated) {
      router.replace("/auth/login");
    }
  }, [isAuthenticated]);

  return (
    <SafeAreaView className={`${theme === "dark" ? "dark" : ""} flex-1`}>
      <GlobalSpinner />
      <Slot />
    </SafeAreaView>
  );
}
