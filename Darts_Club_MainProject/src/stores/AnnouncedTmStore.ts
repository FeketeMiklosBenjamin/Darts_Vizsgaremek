import type CardModel from "@/models/CardModel";
import type PreviousCompetitions from "@/models/PreviousCompetition";
import type RegisterCompetitionModel from "@/models/RegisterCompetitionModel";
import type MatchModel from "@/models/MatchModel";
import AnnouncedTmService from "@/services/AnnouncedTmService";
import { defineStore } from "pinia";
import type CompetitionModel from "@/models/CompetitionModel";
import type PlayerModel from "@/models/PlayerModel";


export const useAnnouncedTmStore = defineStore('AnnouncedTmStore', {
    state: () => ({
        status: {
            resp: '',
            success: false
        },
        alertCard: {
            message: '',
            show: false,
        },
        matchId: '',
        matchHeader: <string>{},
        Competitions: [] as CompetitionModel[],
        PreviousComps: [] as CardModel[],
        OnePrevious: <PreviousCompetitions>{}
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
        getOnlyOnePreviousComp(accesstoken: string, matchHeaderId: string) {
            return AnnouncedTmService.getPreviousComp(accesstoken, matchHeaderId)
                .then((res) => {
                    this.OnePrevious = {
                        id: res.data.id,
                        name: res.data.name,
                        level: res.data.name,
                        setsCount: res.data.setsCount,
                        legsCount: res.data.legsCount,
                        startingPoint: res.data.startingPoint,
                        backroundImageUrl: res.data.backroundImageUrl,
                        tournamentStartDate: new Date(res.data.tournamentStartDate).toLocaleString(undefined, {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit',
                            hour: '2-digit',
                            minute: '2-digit'
                        }),
                        tournamentEndDate: new Date(res.data.tournamentEndDate).toLocaleString(undefined, {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit',
                            hour: '2-digit',
                            minute: '2-digit'
                        }),
                        matches: res.data.matches.map((match: MatchModel) => ({
                            id: match.id,
                            status: match.status,
                            startDate: match.startDate,
                            remainingPlayer: match.remainingPlayer,
                            rowNumber: match.rowNumber,
                            player: match.player.map((person: PlayerModel) => ({
                                id: person.id,
                                username: person.username
                            }))
                        }))
                    }
                })
                .catch((err) => {
                    return Promise.reject(err);
                })
        },
        getAllPreviousCompetition(accesstoken: string) {
            return AnnouncedTmService.getPreviousCompetitions(accesstoken)
                .then((res) => {
                    this.PreviousComps = res.data.map((comp: CardModel) => ({
                        id: comp.id,
                        name: comp.name,
                        level: comp.level,
                        backroundImageUrl: comp.backroundImageUrl,
                        tournamentStartDate: new Date(comp.tournamentStartDate).toLocaleString(undefined, {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit',
                            hour: '2-digit',
                            minute: '2-digit'
                        }),
                        tournamentEndDate: new Date(comp.tournamentEndDate).toLocaleString(undefined, {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit',
                            hour: '2-digit',
                            minute: '2-digit'
                        })
                    }))
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
                        registeredPlayers: comp.registeredPlayers,
                        userJoined: comp.userJoined != undefined ? comp.userJoined : undefined
                    }));
                })
                .catch((err) => {
                    return Promise.reject(err);
                })
        },
        UserApplication(accesstoken: string, tournamentId: string) {
            return AnnouncedTmService.application(accesstoken, tournamentId)
                .then(() => {
                    this.status.resp = "Sikeres jelentkezett a versenyre";
                    this.status.success = true;
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
                    this.status.success = true;
                    return res;
                })
                .catch((err) => {
                    this.status.success = false;
                    this.status.resp = err.data.message;
                    return Promise.reject(err);
                })
        },
        drawTournament(accesstoken: string, tournamentId: string) {
            return AnnouncedTmService.drawCompetition(accesstoken, tournamentId)
                .then(() => {
                    this.status.resp = "Sikeresen sorsoltuk a versenyt!";
                    this.status.success = true;
                })
                .catch((err) => {
                    this.status.success = false;
                    this.status.resp = err.data.message;
                    return Promise.reject(err);
                })
        },
    }
})