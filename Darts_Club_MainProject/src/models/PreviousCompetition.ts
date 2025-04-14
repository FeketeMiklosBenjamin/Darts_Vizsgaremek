import type MatchModel from "./MatchModel";

export default interface PreviousCompetitions {
    id: string,
    name: string,
    level: string,
    setsCount: number,
    legsCount: number,
    startingPoint: number,
    backroundImageUrl: string,
    tournamentStartDate: string,
    tournamentEndDate: string,
    matches: MatchModel[]
}