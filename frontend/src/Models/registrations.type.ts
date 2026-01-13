import type {UserDto} from "./users.type.ts";
import type {ParticipantDto} from "./participant.type.ts";
import type {RaceDto} from "./races.type.ts";

export enum RegistrationStatus {
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2
}

export interface RegistrationDto {
    id: number;
    userId: number;
    raceId: number;
    bibNumber: string;
    count: number;
    status: RegistrationStatus;
    race: RaceDto;
    user: UserDto;
    participant: ParticipantDto;
}