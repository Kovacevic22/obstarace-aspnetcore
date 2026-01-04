import api from "./api.ts";
export interface LoginData {
    email: string;
    password: string;
}
export interface RegisterData {
    email: string;
    password: string;
    name: string;
    surname: string;
    phoneNumber: string;
    dateOfBirth: string;
    emergencyContact: string;
}

export interface UserDto{
    id: number;
    name: string;
    surname: string;
    email: string;
    role: number;
}

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