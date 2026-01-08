import type {CreateObstacleDto, ObstacleDto} from "../Models/obstacles.type.ts";
import api from "./api.ts";

export const obstacleService = {
    getObstacles: async (search?:string): Promise<ObstacleDto[]>=>{
        const params = new URLSearchParams();
        if (search) params.append("search", search);
        const response = await api.get("api/obstacles", {params});
        return response.data;
    },
    createObstacle: async (obstacle: CreateObstacleDto): Promise<ObstacleDto> => {
        const response = await api.post("api/obstacles",obstacle);
        return response.data;
    },
    removeObstacle: async (obstacleId: number) =>{
        await api.delete(`api/obstacles/${obstacleId}`);
    }
}
export default obstacleService;