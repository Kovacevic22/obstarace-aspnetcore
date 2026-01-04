import type {RaceDto} from "../Models/races.type.ts";
import api from "./api.ts";

export const raceService = {
    races: async (filters? : {search?:string, difficulty?:string, distance?:string}): Promise<RaceDto[]> => {
        const params = new URLSearchParams();
        if(filters?.search) params.append("search", filters.search);
        if(filters?.difficulty && filters.difficulty !== "all") params.append("difficulty", filters.difficulty);
        if(filters?.distance && filters.distance!=="Any distance" && filters.distance !== "all") params.append("distanceRange", filters.distance);
        const response = await api.get(`api/races?${params.toString()}`);
        return response.data;
    }
}
export default raceService;