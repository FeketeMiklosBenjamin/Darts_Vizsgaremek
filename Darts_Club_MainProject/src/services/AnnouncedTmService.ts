import type RegisterCompetitionModel from "@/models/RegisterCompetitionModel";
import { AnnouncedTm_Endpoint } from "./BackendService";

export default {
    createCompetition(accesstoken: string, data: RegisterCompetitionModel) {
        return AnnouncedTm_Endpoint.post('', data, {
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
    },
    getCompetition(accesstoken: string) {
        return AnnouncedTm_Endpoint.get('', {
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
    },
    uploadMatchImage(accesstoken: string, image: File, matchId: string) {
        const formData = new FormData();
        formData.append('file', image);

        return AnnouncedTm_Endpoint.put(`/background/upload/${matchId}`, formData, {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'multipart/form-data',
            },
        })
            .then((res) => {
                return res.data;
            })
            .catch((err) => {
                return Promise.reject(err.response);
            });
    }
}