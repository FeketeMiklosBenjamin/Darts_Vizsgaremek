import type RegisterCompetitionModel from "@/models/RegisterCompetitionModel";
import AnnouncedTmService from "@/services/AnnouncedTmService";
import { defineStore } from "pinia";


export const useAnnouncedTmStore = defineStore('AnnouncedTmStore', {
    state: () => ({
        status: {
            resp: '',
        },
        Competitions: <RegisterCompetitionModel>{}
    }),
    actions: {
        registerCompetition(accesstoken: string, data: RegisterCompetitionModel) {
            return AnnouncedTmService.createCompetition(accesstoken, data)
                .then((res) => {
                    this.status.resp = "Sikeresen létrehozta a versenyt!";
                    return res;
                })
                .catch((err) => {
                    this.status.resp = err.data.message;
                    return Promise.reject(err);
                })
        }
    }
})