import en from "@/assets/locales/en/translation.json";
import tr from "@/assets/locales/tr/translation.json";
import * as Localization from "expo-localization";
import i18n from "i18next";
import { initReactI18next } from "react-i18next";

i18n.use(initReactI18next).init({
  compatibilityJSON: "v4",
  lng: Localization.getLocales()[0].languageCode ?? undefined,
  fallbackLng: "en",
  resources: {
    en: { translation: en },
    tr: { translation: tr },
  },
  interpolation: {
    escapeValue: false,
  },
});

export default i18n;
