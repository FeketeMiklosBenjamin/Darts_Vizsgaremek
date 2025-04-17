import type RegisterCompetitionModel from "@/models/RegisterCompetitionModel";
import { AnnouncedTm_Endpoint, Matches_Endpoint, Tournaments_Endpoint } from "./BackendService";

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
    getPreviousComp(accesstoken: string, matchHeaderId: string) {
        return Tournaments_Endpoint.get(`/${matchHeaderId}`, {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'application/json'
            }
        })
            .then((res) => {
                return res;
            })
            .catch((err) => {
                return Promise.reject(err);
            })
    },
    getMatchData(accesstoken: string, matchId: string) {
        return Matches_Endpoint.get(`/${matchId}`, {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'application/json'
            }
        })
            .then((res) => {
                return res;
            })
            .catch((err) => {
                return Promise.reject(err);
            })
    },
    getPreviousCompetitions(accesstoken: string) {
        return Tournaments_Endpoint.get('/', {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'application/json'
            }
        }) 
            .then((res) => {
                return res;
            })
            .catch((err) => {
                return Promise.reject(err);
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
    application(accesstoken: string, tournamentId: string) {
        return AnnouncedTm_Endpoint.post(`/register/${tournamentId}`, {}, {
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
            });
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
    },
    drawCompetition(accesstoken: string, tournamentId: string) {
        return AnnouncedTm_Endpoint.delete(`/draw/${tournamentId}`, {
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
            });
    }
}