export enum Role {
    User = 0,
    Admin = 1,
    Organizer = 2,
}
export interface UserDto{
    id: number;
    name: string;
    surname: string;
    email: string;
    role: Role;
}
export interface UserStatsDto {
    totalUsers: number;
    bannedUsers: number;
}