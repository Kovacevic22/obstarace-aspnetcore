export type Difficulty = "Easy" | "Medium" | "Hard";
export type Status = "Open" | "Closed" | "Finished";
export interface Race {
    id: number;
    name: string;
    slug: string;
    date: string | Date;
    description?: string;
    location: string;
    distance: number;
    difficulty: Difficulty;
    registrationDeadLine: string | Date;
    status: Status;
    imageUrl?: string;
    elevationGain: number;
    maxParticipants: number;
}