import api from "./api.ts";
import type {LoginData, RegisterData} from "../Models/auth.type.ts";
import type {UserDto} from "../Models/users.type.ts";

export const authService = {
    login: async (loginData: LoginData) => {
        const response = await api.post("api/auth/login",loginData);
        return response.data;
    },
    register: async (registerData: RegisterData) =>{
        const response = await api.post("api/auth/register",registerData);
        return response.data;
    },
    me: async (): Promise<UserDto> =>{
        const response = await api.get("api/auth/me")
        return response.data;
    },
    logout: async () => {
        try{
            await api.post("api/auth/logout");
        }catch(e){
            console.error(e);
        }finally {
            window.location.href = "/";
        }
    }

}
export default authService;