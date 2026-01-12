export enum OrganiserStatus {
    Pending = 0,
    Approved = 1,
    Rejected = 2
}

export interface OrganiserDto {
    organisationName: string;
    description: string;
    status?: OrganiserStatus;
}

export interface OrganiserPendingDto {
    userId: number;
    userName: string;
    userSurname: string;
    userEmail: string;
    organisationName: string;
    description: string;
}