import type RegisterCompetitionModel from "@/models/RegisterCompetitionModel";
import AnnouncedTmService from "@/services/AnnouncedTmService";
import { defineStore } from "pinia";


export const useAnnouncedTmStore = defineStore('AnnouncedTmStore', {
    state: () => ({
        status: {
            resp: '',
            success: false
        },
        matchHeader: <string>{}
    }),
    actions: {
        registerCompetition(accesstoken: string, data: RegisterCompetitionModel) {
            return AnnouncedTmService.createCompetition(accesstoken, data)
                .then((res) => {
                    this.status.success = true;
                    this.status.resp = "Sikeresen létrehozta a versenyt!";
                    this.matchHeader = res.data.headerId;
                    return res;
                })
                .catch((err) => {
                    this.status.success = false;
                    this.status.resp = err.data.message;
                    return Promise.reject(err);
                })
        },
        uploadMatchHeader(accesstoken: string, image: File, matchId: string) {
            return AnnouncedTmService.uploadMatchImage(accesstoken, image, matchId)
                .then((res) => {
                    return res;
                })
                .catch((err) => {
                    return Promise.reject(err);
                })
        }
    }
})