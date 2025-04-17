import type ITeam from "./ITeam";
import type IFeedIn from "./IFeedIn";

export default interface IMatch {
    id: string;
    title?: string;
    winner?: string | number;
    team1?: ITeam;
    team2?: ITeam;
    feedIn?: IFeedIn;
}