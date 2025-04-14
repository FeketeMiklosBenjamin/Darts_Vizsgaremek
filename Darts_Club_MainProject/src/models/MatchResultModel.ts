import type PlayerModel from "./PlayerModel";
import type PlayerStatModel from "./PlayerStatModel";

export default interface MatchResultModel {
    id: string;
    status: string;
    startDate: string;
    remainingPlayer: number;
    rowNumber: number;
    playerOne: PlayerModel;
    playerTwo: PlayerModel;
    playerOneStat: PlayerStatModel | null;
    playerTwoStat: PlayerStatModel | null;
}