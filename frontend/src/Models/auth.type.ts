import type {OrganiserDto} from "./organiser.type.ts";
import type { RegisterParticipantDto } from "./participant.type.ts";


export interface LoginData {
    email: string;
    password: string;
}
export interface RegisterData {
    email: string;
    password: string;
    phoneNumber: string;
    organiser?: OrganiserDto;
    participant?: RegisterParticipantDto;
}