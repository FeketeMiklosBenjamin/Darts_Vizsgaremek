import type MatchHeaderModel from "./MatchHeadersModel"
import type RegisteredPlayerModel from "./RegisteredPlayerModel"

export default interface CompetitionModel {
    id: string,
    headerId: string,
    joinStartDate: string,
    joinEndDate: string,
    maxPlayerJoin: number,
    matchHeader: MatchHeaderModel
    registeredPlayers: number | RegisteredPlayerModel[],
    userJoined?: boolean
}