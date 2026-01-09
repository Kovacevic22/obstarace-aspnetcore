import api from "./api.ts";
import type {UpdateUserDto, UserDto, UserStatsDto} from "../Models/users.type.ts";

export const userService = {
    users: async (): Promise<UserDto[]> => {
      const users = await api.get("api/users");
      return users.data
    },
    stats: async (): Promise<UserStatsDto> => {
      const response = await api.get("api/users/stats");
      return response.data;
    },
    updateUser: async (userId:number, data:UpdateUserDto): Promise<UserDto> => {
        const response = await api.put(`api/users/${userId}`, data);
        return response.data;
    }
};
export default userService