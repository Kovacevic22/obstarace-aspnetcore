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