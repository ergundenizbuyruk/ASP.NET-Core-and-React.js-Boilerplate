import { createSlice } from "@reduxjs/toolkit";

interface UIState {
  globalLoading: boolean;
}

const initialState: UIState = {
  globalLoading: false,
};

export const uiSlice = createSlice({
  name: "ui",
  initialState,
  reducers: {
    showLoading: (state) => {
      state.globalLoading = true;
    },
    hideLoading: (state) => {
      state.globalLoading = false;
    },
  },
});

export const { showLoading, hideLoading } = uiSlice.actions;
export default uiSlice.reducer;
