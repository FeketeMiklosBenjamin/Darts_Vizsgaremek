<script setup lang="ts">
import { computed, onUnmounted, ref, onMounted } from 'vue';
import { useAnnouncedTmStore } from '@/stores/AnnouncedTmStore';
import { storeToRefs } from 'pinia';
import { useUserStore } from '@/stores/UserStore';
import router from '@/router';
import Bracket from './Bracket.vue';
import type IBracketNode from '@/interface/IBracketNode';
import type MatchModel from '@/models/MatchModel';
import type IMatch from '@/interface/IMatch';
import type MatchResultModel from '@/models/MatchResultModel';

const { getOnlyOnePreviousComp, getMatchResult } = useAnnouncedTmStore();
const { user } = storeToRefs(useUserStore());
const { OnePrevious, MatchStats } = storeToRefs(useAnnouncedTmStore());

const windowWidth = ref(window.innerWidth);
const exampleBracketData = ref<IBracketNode>();
const getMaxReaminingPlayer = ref<number>();
const showPopup = ref(false);
const selectedTeamId = ref<string | undefined>();



const borderColor = (level: string) => {
    switch (level) {
        case "Amateur":
            return "success-border";
        case "Advanced":
            return "warning-border";
        case "Professional":
            return "danger-border";
        case "Champion":
            return "purple-border";
        default:
            return "";
    }
};

const bracketPadding = computed(() => {
    const value = getMaxReaminingPlayer.value;
    const width = windowWidth.value;

    if (!value) return '100px';

    let basePadding = 100;

    switch (value) {
        case 4:
            basePadding = 280;
            break;
        case 8:
            basePadding = 180;
            break;
        case 16:
            basePadding = 100;
            break;
        default:
            basePadding = 100;
    }

    if (width < 450) return `${basePadding * 0.0}px`;
    if (width < 768) return `${basePadding * 0.3}px`;
    if (width < 992) return `${basePadding * 0.3}px`;
    return `${basePadding}px`;
});

onMounted(async () => {
    const handleResize = () => {
        windowWidth.value = window.innerWidth;
    };
    window.addEventListener('resize', handleResize);

    onUnmounted(() => {
        window.removeEventListener('resize', handleResize);
    });

    let matchId = sessionStorage.getItem("matchId");
    await getOnlyOnePreviousComp(user.value.accessToken, matchId!);
    getMaxReaminingPlayer.value = OnePrevious.value.matches.reduce((max, match) => Math.max(max, match.remainingPlayer), 0);
    exampleBracketData.value = generateBracketStructure(OnePrevious.value.matches);
});

const GoBack = () => {
    sessionStorage.removeItem("matchId");
    router.push(user.value.role == 1 ? '/competition' : '/competition-selection');
};

interface BracketGeneratorConfig {
    initialPlayers: number;
    rounds: {
        remainingPlayers: number;
        title: string;
    }[];
}

const generateBracketStructure = (matches: MatchModel[]): IBracketNode => {
    const config: BracketGeneratorConfig = {
        initialPlayers: getMaxReaminingPlayer.value!,
        rounds: [
            { remainingPlayers: 2, title: "Döntő" },
            { remainingPlayers: 4, title: "Elődöntő" },
            { remainingPlayers: 8, title: "Negyeddöntő" },
            { remainingPlayers: 16, title: "Nyolcaddöntő" }
        ]
    };

    const filteredRounds = config.rounds.filter(r => r.remainingPlayers <= config.initialPlayers);

    const createEmptyMatch = (round: number, position: number): IMatch => ({
        id: `empty-${round}-${position}`,
        title: `${filteredRounds.find(r => r.remainingPlayers === round)?.title} ${position}`,
        team1: { id: `t1-${round}-${position}`, name: "---------", score: undefined },
        team2: { id: `t2-${round}-${position}`, name: "---------", score: undefined },
        winner: ""
    });

    const findMatch = (round: number, position: number): IMatch | undefined => {
        const match = matches.find(m =>
        m.remainingPlayer === round &&
        m.rowNumber === position
    );

        return match ? {
            id: match.id,
            title: `${filteredRounds.find(r => r.remainingPlayers === round)?.title} ${position}`,
            team1: {
                id: match.playerOne.id,
                name: match.playerOne.username,
                score: match.playerOneResult ?? undefined
            },
            team2: {
                id: match.playerTwo.id,
                name: match.playerTwo.username,
                score: match.playerTwoResult ?? undefined
            },
            winner: match.won !== null ? (match.won ? match.playerOne.id : match.playerTwo.id) : ""
        } : undefined;
    };

    const buildNode = (roundIndex: number, position: number): IBracketNode => {
        const currentRound = filteredRounds[roundIndex];
        const nextRound = filteredRounds[roundIndex + 1];

        const match = findMatch(currentRound.remainingPlayers, position) ||
            createEmptyMatch(currentRound.remainingPlayers, position);

        const node: IBracketNode = {
            match,
            hasParent: roundIndex < filteredRounds.length - 1,
            children: []
        };

        if (nextRound) {
            const childPositions = [
                position * 2 - 1,
                position * 2
            ];

            node.children = childPositions.map(pos =>
                buildNode(roundIndex + 1, pos)
            );
        }

        return node;
    };

    return buildNode(0, 1);
};

const handleMatchClick = async (matchId: string) => {
    await getMatchResult(user.value.accessToken, matchId!);
    if (MatchStats) {
        MatchStats.value = MatchStats.value
        showPopup.value = true;
    }
};

const handleSelectTeam = (teamId: string) => {
    selectedTeamId.value = teamId;
};

const handleDeselect = () => {
    selectedTeamId.value = undefined;
};

const closePopup = () => {
    showPopup.value = false;
    MatchStats.value = null;
};

const NavigateToStatistic = (userId: string) => {
    router.push(`/statistic/${userId}`)
}

</script>

<template>
    <div class="row left-side">
        <div class="col-lg-3 col-md-4 col-12 col-sm-8 offset-md-0 offset-sm-2 offset-0 p-0">
            <div class="d-flex justify-content-center m-3">
                <div class="card bg-black text-light" :class="borderColor(OnePrevious.level)" style="max-width: 45vh;">
                    <img :src="OnePrevious.backroundImageUrl" class="card-img-middle" alt="...">
                    <div class="card-body">
                        <div class="d-flex justify-content-center">
                            <h5 class="card-title text-center fst-italic">{{ OnePrevious.name }}</h5>
                        </div>
                        <div class="card-body">
                            <p class="card-title text-center text-decoration-underline mt-2">Verseny időtartama:</p>
                            <p class="text-center m-0 small">
                                {{ new Date(OnePrevious.tournamentStartDate).toLocaleDateString(undefined, {
                                    year: 'numeric',
                                    month: '2-digit',
                                    day: '2-digit',
                                    hour: '2-digit',
                                    minute: '2-digit'
                                }) }}<br>-<br>{{
                                    new Date((OnePrevious.tournamentEndDate)).toLocaleDateString(undefined, {
                                        year: 'numeric',
                                        month: '2-digit',
                                        day: '2-digit',
                                        hour: '2-digit',
                                        minute: '2-digit'
                                    })
                                }}
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row justify-content-center d-flex">
                <button class="btn btn-darkred text-white mb-3 mt-3 col-8" @click="GoBack">Vissza a
                    versenyekhez</button>
            </div>
        </div>
        <div class="row ms-sm-2 ms-3 offset-2 col-12 offset-md-0 offset-sm-1 col-lg-9 col-md-8 mt-3"
            v-if="exampleBracketData">
            <div class="bracket-side row col-12 main-div" :style="{ paddingRight: bracketPadding }">
                <Bracket :bracket-node="exampleBracketData" :highlighted-team-id="selectedTeamId"
                    @onMatchClick="handleMatchClick" @onSelectedTeam="handleSelectTeam"
                    @onDeselectedTeam="handleDeselect" class="bracket-wrapper" />
            </div>
        </div>

        <div v-if="showPopup" class="popup-overlay" @click.self="closePopup">
            <div class="popup-card">
                <div class="popup-header">

                </div>
                <div class="popup-content row d-flex justify-content-center text-center">
                    <div class="col-2">
                        <img :src="MatchStats?.playerOne.profilePicture"
                            :class="`statistic-profileImg me-3 ${borderColor}`" alt="Nincs" />
                    </div>
                    <div class="col-4"
                        :style="(MatchStats?.playerOneStat?.won ? 'color: rgb(184, 134, 11);' : 'color: white')">
                        <div class="d-flex justify-content-center align-items-center fst-italic cursor" @click="NavigateToStatistic(MatchStats!.playerOne.id)">
                            <h2>{{ MatchStats?.playerOne.username }}</h2>
                        </div>
                    </div>
                    <div class="col-4"
                        :style="(MatchStats?.playerTwoStat?.won ? 'color: rgb(184, 134, 11);' : 'color: white')">
                        <div class="d-flex justify-content-center align-items-center fst-italic cursor" @click="NavigateToStatistic(MatchStats!.playerTwo.id)">
                            <h2>{{ MatchStats?.playerTwo.username }}</h2>
                        </div>
                    </div>
                    <div class="col-2">
                        <img :src="MatchStats?.playerTwo.profilePicture"
                            :class="`statistic-profileImg me-3 ${borderColor}`" alt="Nincs" />
                    </div>
                </div>
                <div class="row col-8 offset-2">
                    <div class="col-6 text-center">
                        <h3 v-if="MatchStats?.playerOneStat != null">{{ (MatchStats?.playerOneStat?.setsWon
                            > 1) ?
                            MatchStats?.playerOneStat?.setsWon : MatchStats?.playerOneStat?.legsWon }}</h3>
                    </div>
                    <div class="col-6 text-center">
                        <h3 v-if="MatchStats?.playerTwoStat != null">{{ (MatchStats?.playerTwoStat?.setsWon
                            > 1) ?
                            MatchStats?.playerTwoStat?.setsWon : MatchStats?.playerTwoStat?.legsWon }}</h3>
                    </div>
                </div>
                <div v-if="MatchStats?.playerOneStat != null && MatchStats.playerTwoStat != null && MatchStats.playerOneStat.appeared && MatchStats.playerTwoStat.appeared"
                    class="row col-8 offset-2 mt-3">
                    <table class="table table-dark table-bordered border-5">
                        <tbody>
                            <tr v-if="MatchStats.playerOneStat.setsWon != 0 && MatchStats.playerTwoStat.setsWon != 0">
                                <td> {{ MatchStats?.playerOneStat?.setsWon }} db</td>
                                <td colspan="2">
                                    <span>Nyert setek száma</span>
                                </td>
                                <td>{{ MatchStats?.playerTwoStat?.setsWon }} db
                                </td>
                            </tr>
                            <tr>
                                <td> {{ MatchStats?.playerOneStat?.legsWon }} db</td>
                                <td colspan="2">
                                    <span>Nyert legek száma</span>
                                </td>
                                <td>{{ MatchStats?.playerTwoStat?.legsWon }} db
                                </td>
                            </tr>
                            <tr>
                                <td> {{ MatchStats?.playerOneStat?.averages }}</td>
                                <td colspan="2">
                                    <span>Átlagok</span>
                                </td>
                                <td>{{ MatchStats?.playerTwoStat?.averages }}
                                </td>
                            </tr>
                            <tr>
                                <td> {{ MatchStats?.playerOneStat?.max180s }} db</td>
                                <td colspan="2">
                                    <span>180-asok száma</span>
                                </td>
                                <td>{{ MatchStats?.playerTwoStat?.max180s }} db
                                </td>
                            </tr>
                            <tr>
                                <td> {{ MatchStats?.playerOneStat?.checkoutPercentage }} %</td>
                                <td colspan="2">
                                    <span>Kiszállózási %</span>
                                </td>
                                <td>{{ MatchStats?.playerTwoStat?.checkoutPercentage }} %
                                </td>
                            </tr>
                            <tr>
                                <td> {{ MatchStats?.playerOneStat?.highestCheckout }}</td>
                                <td colspan="2">
                                    <span>Legnagyobb kiszálló</span>
                                </td>
                                <td>{{ MatchStats?.playerTwoStat?.highestCheckout }}
                                </td>
                            </tr>
                            <tr>
                                <td> {{ MatchStats?.playerOneStat?.nineDarter }} db</td>
                                <td colspan="2">
                                    <span>Kilenc nyilasok száma</span>
                                </td>
                                <td>{{ MatchStats?.playerTwoStat?.nineDarter }} db
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div v-if="MatchStats?.playerOneStat == null && MatchStats?.playerTwoStat == null"
                    class="row col-8 offset-2 mt-3">
                    <div class="alert alert-warning d-flex align-items-center justify-content-center">
                        <div class="text-center fw-bold">
                            <p class="mb-1">
                                <span class="bi bi-exclamation-circle me-2"></span> A statisztikák csak a befejezett mérkőzésekért érhetőek el!
                            </p>
                            <p class="mb-0">
                                A meccs kezdeze: {{ MatchStats?.startDate }}
                            </p>
                        </div>
                    </div>
                </div>
                <div v-if="MatchStats?.playerOneStat?.appeared == false || MatchStats?.playerTwoStat?.appeared == false"
                    class="row col-8 offset-2 mt-3">
                    <div class="alert alert-warning d-flex align-items-center justify-content-center">
                        <div class="text-center fw-bold">
                            <p class="mb-1" v-if="MatchStats.playerOneStat?.appeared || MatchStats.playerTwoStat?.appeared">
                                <span class="bi bi-exclamation-circle me-2"></span> A {{ (MatchStats.playerOneStat?.appeared ? MatchStats.playerTwo.username : MatchStats.playerOne.username) }} nevű versenyző nem jelent meg a meccsen, ezért {{ (MatchStats.playerOneStat?.appeared ? MatchStats.playerOne.username : MatchStats.playerTwo.username) }} nevű versenyző {{ MatchStats.remainingPlayer == 2 ? "nyerte meg a versenyt!" : "jutott tovább!" }}
                            </p>
                            <p class="mb-1" v-else>
                                <span class="bi bi-exclamation-circle me-2"></span> Egyikük sem jelent meg a versenyen, ezért sorsolással {{ MatchStats.playerOneStat?.won ? MatchStats.playerOne.username : MatchStats.playerTwo.username }} nevű versenyző {{ MatchStats.remainingPlayer == 2 ? "nyerte meg a versenyt!" : "jutott tovább!" }}
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.bracket-wrapper {
    overflow-x: auto;
    overflow-y: auto;
    -webkit-overflow-scrolling: touch;
    scrollbar-width: none;
}

.table-dark {
    border: 2px solid black;
    border-radius: 5px;
}

td {
    text-align: center;
}

.cursor {
    cursor: pointer;
}

.bracket-wrapper::-webkit-scrollbar {
    display: none;
}

.bracket-side {
    background: rgba(0, 0, 0, 0.25);
    border: 5px solid rgb(184, 134, 11);
    border-radius: 10px;
    height: 80vh;
}

.statistic-profileImg {
    width: 80%;
    height: auto;
    aspect-ratio: 1;
    border-radius: 100%;
}

.popup-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.popup-card {
    background: rgba(0, 0, 0, 0.9);
    border: 2px solid goldenrod;
    border-radius: 10px;
    padding: 20px;
    width: 70%;
    color: white;
}

.popup-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;
}

.close-btn {
    background: none;
    border: none;
    color: white;
    font-size: 1.5rem;
    cursor: pointer;
    padding: 0 8px;
}

/* .popup-content {
    display: flex;
    flex-direction: column;
    gap: 1rem;
} */

/* .team {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 8px;
    border-radius: 4px;
}

.vs-text {
    text-align: center;
    font-weight: bold;
    margin: 10px 0;
}

.score {
    color: goldenrod;
    font-weight: bold;
    margin-left: 1rem;
}

.winner {
    color: #28a745;
    font-weight: bold;
} */

@media (max-width: 768px) {
    .left-side {
        max-height: 90vh;
        overflow-y: auto;
        overflow-x: hidden;
        scrollbar-width: none;
        -ms-overflow-style: none;
    }

    .left-side::-webkit-scrollbar {
        display: none;
    }
}

@media (min-width: 768px) {
    .custom-min-vh-md {
        min-height: 100vh;
    }
}

.custom-min-vh-md .btn {
    width: 150px;
}
</style>