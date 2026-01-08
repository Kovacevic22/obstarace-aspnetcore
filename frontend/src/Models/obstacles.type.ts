export enum Difficulty {
    Easy = 0,
    Normal = 1,
    Hard = 2
}

export interface ObstacleDto {
    id: number;
    name: string;
    description: string;
    difficulty: Difficulty;
}

export interface CreateObstacleDto {
    name: string;
    description: string;
    difficulty: Difficulty;
}

export interface UpdateObstacleDto {
    name: string;
    description: string;
    difficulty: Difficulty;
}