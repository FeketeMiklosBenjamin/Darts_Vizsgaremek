import type RegisterCompetitionModel from "@/models/RegisterCompetitionModel";
import { AnnouncedTm_Endpoint } from "./BackendService";

export default {
    createCompetition(accesstoken: string, data: RegisterCompetitionModel) {
        return AnnouncedTm_Endpoint.post('/', data, {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'application/json'
            }
        })
            .then((res) => {
                return res;
            })
            .catch((err) => {
                return Promise.reject(err.response);
            })
    }
}