import type {ParticipantDto} from "./participant.type.ts";
import type {OrganiserDto} from "./organiser.type.ts";

export enum Role {
    User = 0,
    Admin = 1,
    Organiser = 2,
}

export interface UserDto {
    id: number;
    email: string;
    phoneNumber: string;
    role: Role;
    banned: boolean;
    participant?: ParticipantDto;
    organiser?: OrganiserDto;
}

export interface UserStatsDto {
    totalUsers: number;
    bannedUsers: number;
}