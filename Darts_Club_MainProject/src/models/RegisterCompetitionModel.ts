export default interface RegisterCompetitionModel {
    headerId?: string,
    name: string,
    setsCount: number,
    legsCount: number,
    startingPoint: number,
    password: string,
    validPassword?: string,
    level: string,
    maxPlayerJoin: number,
    joinStartDate: string,
    joinEndDate: string,
    matchDates: string[]
}