import type {CreateRaceDto, RaceDto, RaceStatsDto, UpdateRaceDto} from "../Models/races.type.ts";
import api from "./api.ts";

export const raceService = {
    races: async (filters? : {search?:string, difficulty?:string, distance?:string}): Promise<RaceDto[]> => {
        const response = await api.get<RaceDto[]>("api/races", {
            params: {
                search: filters?.search,
                difficulty: filters?.difficulty !== "all" ? filters?.difficulty : undefined,
                distanceRange: (filters?.distance !== "Any distance" && filters?.distance !== "all")
                    ? filters?.distance
                    : undefined
            }
        });
        return response.data;
    },
    raceDetails: async(slug:string):Promise<RaceDto> => {
        const response = await api.get(`api/races/${slug}`);
        return response.data;
    },
    stats: async (): Promise<RaceStatsDto> => {
        const response = await api.get("api/races/stats");
        return response.data;
    },
    createRace: async(raceData:CreateRaceDto): Promise<RaceDto> => {
        const response = await api.post("api/races", raceData);
        return response.data;
    },
    getRaceById: async (id:number): Promise<RaceDto> => {
      const response = await api.get(`api/races/${id}`);
      return response.data;
    },
    deleteRace: async(id:number) => {
        await api.delete(`api/races/${id}`);
    },
    updateRace: async(id:number, data:UpdateRaceDto):Promise<RaceDto> => {
        const response = await api.put(`api/races/${id}`, data);
        return response.data;
    },
    getMyRaces: async():Promise<RaceDto[]> => {
        const response = await api.get(`api/races/my-races`);
        return response.data;
    }
}
export default raceService;