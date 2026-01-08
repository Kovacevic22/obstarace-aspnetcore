import type {CreateRaceDto, RaceDto, RaceStatsDto, UpdateRaceDto} from "../Models/races.type.ts";
import api from "./api.ts";

export const raceService = {
    races: async (filters? : {search?:string, difficulty?:string, distance?:string}): Promise<RaceDto[]> => {
        const params = new URLSearchParams();
        if(filters?.search) params.append("search", filters.search);
        if(filters?.difficulty && filters.difficulty !== "all") params.append("difficulty", filters.difficulty);
        if(filters?.distance && filters.distance!=="Any distance" && filters.distance !== "all") params.append("distanceRange", filters.distance);
        const response = await api.get(`api/races?${params.toString()}`);
        return response.data;
    },
    raceDetails: async(slug:string):Promise<RaceDto> => {
        const params = new URLSearchParams();
        params.append("slug", slug);
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
    }
}
export default raceService;