import type {RegistrationDto} from "../Models/registrations.type.ts";
import api from "./api.ts";

export const registrationService = {
    registrations: async(): Promise<RegistrationDto[]> => {
        const response = await api.get("api/registrations")
        return response.data;
    },
    deleteRegistration: async (registrationId: number)  => {
        await api.delete(`api/registrations/${registrationId}`);
    },
    createRegistration: async (raceId:number)  => {
         await api.post<RegistrationDto>("api/registrations", {
            raceId,
        });
    },
    getParticipantsForRace: async (raceId?: number): Promise<RegistrationDto[]>  => {
        const response = await api.get<RegistrationDto[]>("/api/registrations/registrations-on-race",{
            params: {raceId}
        });
        return response.data
    }
}