import type PlayerModel from "./PlayerModel";

export default interface MatchModel {
    id: string,
    status: string,
    startDate: string,
    remainingPlayer: number,
    rowNumber: number,
    player: PlayerModel[]
}