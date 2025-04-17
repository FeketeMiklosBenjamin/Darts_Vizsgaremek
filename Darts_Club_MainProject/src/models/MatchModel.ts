import type PlayerModel from "./PlayerModel";

export default interface MatchModel {
    id: string,
    status: string,
    startDate: string,
    remainingPlayer: number,
    rowNumber: number,
    playerOne: PlayerModel,
    playerTwo: PlayerModel,
    won: boolean | null,
    playerOneResult: number,
    playerTwoResult: number
}