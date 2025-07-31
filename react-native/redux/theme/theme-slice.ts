import { createSlice } from "@reduxjs/toolkit";
import { Appearance } from "react-native";

export type Theme = "light" | "dark";

const systemColorScheme = Appearance.getColorScheme() || "light";

const themeSlice = createSlice({
  name: "theme",
  initialState: {
    theme: systemColorScheme as Theme,
  },
  reducers: {
    toggleTheme: (state) => {
      state.theme = state.theme === "dark" ? "light" : "dark";
    },
    setTheme: (state, action: { payload: Theme }) => {
      state.theme = action.payload;
    },
  },
});

export const { toggleTheme, setTheme } = themeSlice.actions;
export default themeSlice.reducer;
