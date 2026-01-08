import type {RegistrationDto} from "../Models/registrations.type.ts";
import api from "./api.ts";

export const registrationService = {
    registrations: async(userId: number): Promise<RegistrationDto[]> => {
        const response = await api.get("api/registrations",
            {
                params: {userId}
            });
        return response.data;
    },
    deleteRegistration: async (registrationId: number)  => {
        await api.delete(`api/registrations/${registrationId}`);
    }
}