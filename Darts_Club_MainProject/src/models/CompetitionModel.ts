import type MatchHeaderModel from "./MatchHeadersModel"

export default interface CompetitionModel {
    id: string,
    headerId: string,
    joinStartDate: string,
    joinEndDate: string,
    maxPlayerJoin: number,
    matchHeader: MatchHeaderModel
    registeredPlayers: number,
    userJoined: boolean
}