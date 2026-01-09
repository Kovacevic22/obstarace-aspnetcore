export enum Role {
    User = 0,
    Admin = 1,
    Organizer = 2,
}

export interface UserDto {
    id: number;
    name: string;
    surname: string;
    email: string;
    phoneNumber: string;
    dateOfBirth: string;
    emergencyContact: string;
    role: Role;
    banned: boolean;
    activity: UserActivityDto;
}

export interface UserStatsDto {
    totalUsers: number;
    bannedUsers: number;
}
export interface UserActivityDto {
    totalRaces: number;
    finishedRaces: number;
}
export interface UpdateUserDto {
    phoneNumber: string;
    dateOfBirth: string;
    emergencyContact: string;
}