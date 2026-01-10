import type {OrganiserDto} from "./organiser.type.ts";


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
    organiser?: OrganiserDto;
}