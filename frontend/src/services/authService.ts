import api from "./api.ts";
export interface LoginData {
    Email: string;
    Password: string;
}
export interface RegisterData {
    Email: string;
    Password: string;
    Name: string;
    Surname: string;
    PhoneNumber: string;
    DateOfBirth: Date;
    EmergencyContact: string;
}

export interface UserDto{
    Id: number;
    Name: string;
    Surname: string;
    Email: string;
    Role: string;
}

export interface LoginResponse {
    Token: string;
    Expiration: string;
    User: UserDto;
}

export const authService = {
    login: async (loginData: LoginData) => {
        const response = await api.post("auth/login",loginData);
        return response.data;
    },
    register: async (registerData: RegisterData) =>{
        const response = await api.post("auth/register",registerData);
        return response.data;
    },
    logout: async () => {
        try{
            await api.post("auth/logout");
        }catch(e){
            console.error(e);
        }finally {
            window.location.href = "/";
        }
    }
}
export default authService;