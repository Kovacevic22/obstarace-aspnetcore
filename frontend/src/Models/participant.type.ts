export interface ParticipantDto {
    userId: number;
    name: string;
    surname: string;
    dateOfBirth: string;
    emergencyContact: string;
    activity: ParticipantActivityDto;
}

export interface ParticipantActivityDto {
    totalRaces: number;
    finishedRaces: number;
}

export interface RegisterParticipantDto {
    name: string;
    surname: string;
    dateOfBirth: string;
    emergencyContact: string;
}

export interface UpdateParticipantDto {
    dateOfBirth: string;
    emergencyContact: string;
}