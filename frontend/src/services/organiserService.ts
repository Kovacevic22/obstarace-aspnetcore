import type {OrganiserPendingDto} from "../Models/organiser.type.ts";
import api from "./api.ts";


export const organiserService = {
    getPending: async (): Promise<OrganiserPendingDto[]>=>{
        const response = await api.get("api/organisers/pending");
        return response.data;
    }
}
export default organiserService