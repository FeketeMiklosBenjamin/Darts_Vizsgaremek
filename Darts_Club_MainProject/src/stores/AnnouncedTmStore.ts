import type CompetitionModel from "@/models/CompetitionModel";
import type RegisterCompetitionModel from "@/models/RegisterCompetitionModel";
import AnnouncedTmService from "@/services/AnnouncedTmService";
import { defineStore } from "pinia";


export const useAnnouncedTmStore = defineStore('AnnouncedTmStore', {
    state: () => ({
        status: {
            resp: '',
            success: false
        },
        matchHeader: <string>{},
        Competitions: [] as CompetitionModel[]
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
        getAllCompetition(accesstoken: string) {
            return AnnouncedTmService.getCompetition(accesstoken)
                .then((res) => {
                    this.Competitions = res.data.map((comp: CompetitionModel) => ({
                        id: comp.id,
                        hedaerId: comp.headerId,
                        joinStartDate: new Date(comp.joinStartDate).toLocaleDateString(undefined, {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit',
                            hour: '2-digit',
                            minute: '2-digit'
                        }),
                        joinEndDate: new Date(comp.joinEndDate).toLocaleDateString(undefined, {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit',
                            hour: '2-digit',
                            minute: '2-digit'
                        }),
                        maxPlayerJoin: comp.maxPlayerJoin,
                        matchHeader: comp.matchHeader,
                        registeredPlayers: comp.registeredPlayers
                    }));
                })
                .catch((err) => {
                    return Promise.reject(err);
                })
        },
        UserApplication(accesstoken: string, tournamentId: string) {
            return AnnouncedTmService.application(accesstoken, tournamentId)
                .then((res) => {
                   this.status.resp = "Sikeres jelentkezett a versenyre";
                })
                .catch((err) => {
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