import type {ObstacleDto} from "./obstacles.type.ts";
export type DistanceRange = "short" | "mid" | "long" | "all";
export enum Difficulty {
    Easy = 0,
    Medium = 1,
    Hard = 2,
}
export enum Status
{
    UpComing =0,
    OnGoing = 1,
    Completed = 2,
    Cancelled = 3,
}
export interface RaceDto {
    id: number;
    name: string;
    slug: string;
    date: string;
    description: string;
    location: string;
    distance: number;
    difficulty: Difficulty;
    registrationDeadLine: string;
    status: Status;
    imageUrl?: string;
    elevationGain: number;
    maxParticipants: number;
    obstacleIds: number[];
    obstacles: ObstacleDto[];
}
export interface RaceStatsDto {
    totalRaces: number;
    totalKilometers: number;
    archivedCount: number;
}

export interface CreateRaceDto {
    name: string;
    slug: string;
    date: string;
    description: string;
    location: string;
    distance: number;
    difficulty: number;
    registrationDeadLine: string;
    status: number;
    imageUrl: string;
    elevationGain: number;
    maxParticipants: number;
    obstacleIds: number[];
}
export interface UpdateRaceDto {
    name: string;
    slug: string;
    date: string;
    description: string;
    location: string;
    distance: number;
    difficulty: number;
    registrationDeadLine: string;
    status: number;
    imageUrl: string;
    elevationGain: number;
    maxParticipants: number;
    obstacleIds: number[];
}