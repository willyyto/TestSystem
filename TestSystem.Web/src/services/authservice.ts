// src/services/authService.ts
import axios from "axios";

const API_URL = "https://your-api-url/api/auth/";

const login = (username: string, password: string) => {
  return axios.post(API_URL + "login", {
    username,
    password,
  });
};

export default {
  login,
};
