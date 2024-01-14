import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import Backend from 'i18next-http-backend';
import LanguageDetector from 'i18next-browser-languagedetector';
import "i18next";

declare module "i18next" {
  interface CustomTypeOptions {
    returnNull: false;
  }
}
 
var i18nextLng = localStorage.getItem("i18nextLng");
if (!i18nextLng) {
  localStorage.setItem("i18nextLng", "tr");
}

i18n
  // load translation using http -> see /public/locales (i.e. https://github.com/i18next/react-i18next/tree/master/example/react/public/locales)
  // learn more: https://github.com/i18next/i18next-http-backend
  .use(Backend)
  // detect user language
  // learn more: https://github.com/i18next/i18next-browser-languageDetector
  .use(LanguageDetector)
  // pass the i18n instance to react-i18next.
  .use(initReactI18next)
  // init i18next
  // for all options read: https://www.i18next.com/overview/configuration-options
  .init({
    fallbackLng: 'tr',
    debug: false,
    returnNull: false,
    interpolation: {
      escapeValue: false,
    },
    // react: {
    //   wait: true
    // }
  });

i18n.changeLanguage("tr");
 
export default i18n;
export const languages = {
    tr: "tr-TR",
    en: "en-US"
  };