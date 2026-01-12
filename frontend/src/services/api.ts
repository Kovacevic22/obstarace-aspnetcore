import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:5235",
    headers: {
        "Content-Type": "application/json",
    },
    withCredentials: true
});

api.interceptors.response.use(
    (response)=>response,
    (error) => {
        if (!error.response) {
            console.error("BACKEND_OFFLINE");
            if (window.location.pathname !== "/offline") {
                window.location.href = "/offline";
            }
            return Promise.reject(error);
        }
        if(error.response?.status === 401) {
            console.error("Unauthorized");
        }
        return Promise.reject(error);
    }
);

export default api;