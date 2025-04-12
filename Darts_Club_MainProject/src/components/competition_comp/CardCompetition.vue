<script setup lang="ts">
import type CompetitionModel from '@/models/CompetitionModel';
import type RegisteredPlayerModel from '@/models/RegisteredPlayerModel';
import VerifyModal from './VerifyModal.vue';
import { useAnnouncedTmStore } from '@/stores/AnnouncedTmStore';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, watchEffect } from 'vue';
import ResponseModal from './ResponseModal.vue';

const { user } = storeToRefs(useUserStore());
const { getAllCompetition, drawTournament, status, alertCard } = useAnnouncedTmStore();
const { Competitions } = storeToRefs(useAnnouncedTmStore());

const filteredCompetition = ref<CompetitionModel[]>([]);
const selectedComp = ref<CompetitionModel | null>(null);
const openState = ref<{ [key: string]: boolean }>({});
const showModal = ref(false);
const showAlert = ref(false);
const alertMessage = ref('');
const alertSuccess = ref(true);

const props = defineProps<{
    areJoinedCards: boolean;
}>();

watchEffect(() => {
    let alertMessageHelp = "";
    if (user.value.role == 1) {
        if (props.areJoinedCards) {
            filteredCompetition.value = Competitions.value.filter(x => x.matchHeader.level == user.value.level && new Date(x.joinEndDate) > new Date(Date.now()) && x.userJoined == props.areJoinedCards);
            if (filteredCompetition.value.length == 0) {
                alertMessageHelp = 'Még nem jelentkeztél egyetlen versenyre sem!'
            }
            
        }
        else {
            filteredCompetition.value = Competitions.value.filter(x => x.matchHeader.level == user.value.level && new Date(x.joinEndDate) > new Date(Date.now()) && x.userJoined == props.areJoinedCards && x.maxPlayerJoin != x.registeredPlayers);
            if (filteredCompetition.value.length == 0) {
                alertMessageHelp = 'Nincs meghírdetett verseny az ön szintjén!'
            }
            

        }
    }
    else {
        if (props.areJoinedCards) {
            filteredCompetition.value = Competitions.value.filter(x => new Date(x.joinEndDate) < new Date(Date.now()))
            if (filteredCompetition.value.length == 0) {
                alertMessageHelp = 'Nincs sorsolható verseny!'
            }
        }
        else {
            filteredCompetition.value = Competitions.value;
            if (filteredCompetition.value.length == 0) {
                alertMessageHelp = 'Nincs meghírdetve egyetlen verseny sem!'
            }
        }
    }
    alertCard.message = alertMessageHelp;
    alertCard.show = (alertCard.message != '');
});

const toggleOpen = (id: string) => {
    openState.value[id] = !openState.value[id];
};

onMounted(async () => {
    await getAllCompetition(user.value.accessToken);
    let alertMessageHelp = "";
    if (user.value.role != 2) {
        filteredCompetition.value = Competitions.value.filter(x => x.matchHeader.level == user.value.level && new Date(x.joinEndDate) > new Date(Date.now()) && x.userJoined == false && x.maxPlayerJoin != x.registeredPlayers);
        if (filteredCompetition.value.length == 0) {
            alertMessageHelp = 'Nincs meghírdetett verseny az ön szintjén!'
        }
    } else {
        filteredCompetition.value = Competitions.value;
        if (filteredCompetition.value.length == 0) {
            alertMessageHelp = 'Nincs meghírdetve egyetlen verseny sem!'
        }
    }
    alertCard.message = alertMessageHelp;
    alertCard.show = (alertCard.message != '');
})

const openVerifyModal = (compId: string) => {
    const foundComp = filteredCompetition.value.find(u => u.id === compId);

    if (foundComp) {
        selectedComp.value = foundComp;
        showModal.value = true;
    }
};

const drawTournamentClick = async (compId: string) => {
    try {
        await drawTournament(user.value.accessToken, compId)
    } catch (err) {
    }
    alertCard.message = status.resp;
    alertCard.show = status.success;
    if (alertCard.show) {
        await getAllCompetition(user.value.accessToken);
    }
    showAlert.value = true;
};

const closeVerifyModal = () => {
    showModal.value = false;
    selectedComp.value = null;
};

const onApplied = () => {
    showModal.value = false;
    alertCard.message = status.resp;
    alertCard.show = status.success;
    if (alertCard.show) {
        selectedComp.value!.userJoined = true;
        selectedComp.value!.registeredPlayers = (selectedComp.value!.registeredPlayers as number) += 1;
    }
    showAlert.value = true;
};


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
</script>


<template>
    <div class="col-12 mx-3 mx-sm-0 col-sm-10 offset-0 offset-sm-1 offset-md-0 col-md-6 col-xl-4 p-2"
        v-for="comp in filteredCompetition" :key="comp.id">
        <div class="d-flex glass-card width-form-card justify-content-center">
            <div class="card bg-black text-light" :class="borderColor(comp.matchHeader.level)" style="max-width: 45vh;">
                <img :src="comp.matchHeader.backroundImageUrl" class="card-img-middle" alt="...">
                <div class="card-body">
                    <div class="d-flex justify-content-between">
                        <h5 class="card-title text-center fst-italic">{{ comp.matchHeader.name }}</h5>
                        <p style="width: 4rem;"><i class="bi bi-person-standing"></i>
                            {{ user.role == 1 ? comp.registeredPlayers : (comp.registeredPlayers as
                                RegisteredPlayerModel[]).length }}/{{ comp.maxPlayerJoin }}</p>
                    </div>
                    <div class="card-body">
                        <p class="card-title text-center text-decoration-underline mt-2">Jelentkezés időtartama:
                        </p>
                        <p class="text-center m-0 small">
                            {{ new Date(comp.joinStartDate).toLocaleDateString(undefined, {
                                year: 'numeric',
                                month: '2-digit',
                                day: '2-digit',
                                hour: '2-digit',
                                minute: '2-digit'
                            }) }}<br>-<br>{{ new
                                Date((comp.joinEndDate)).toLocaleDateString(undefined, {
                                    year: 'numeric',
                                    month: '2-digit',
                                    day: '2-digit',
                                    hour: '2-digit',
                                    minute: '2-digit'
                                }) }}
                        </p>
                        <div class="d-flex justify-content-center p-0">
                            <button type="button" @click="toggleOpen(comp.id)"
                                :class="openState[comp.id] ? 'bi bi-caret-up btn text-light' : 'bi bi-caret-down-fill btn text-light'"
                                style="font-size: 20px;">
                            </button>
                        </div>
                        <div :class="['collapse', { show: openState[comp.id] }]">
                            <div class="card card-body bg-dark text-light border-secondary">
                                <p class="text-center">További információk a versenyről:</p>
                                <p class="card-title text-center text-decoration-underline">Verseny időtartama:
                                </p>
                                <p class="text-center small">
                                    {{ new Date(comp.matchHeader.tournamentStartDate).toLocaleDateString() }} -
                                    {{ new Date(comp.matchHeader.tournamentEndDate).toLocaleDateString() }}
                                </p>
                                <div class="d-flex justify-content-center pe-4">
                                    <ul class="small">
                                        <li>Setek száma: {{ comp.matchHeader.setsCount }}</li>
                                        <li>Legek száma: {{ comp.matchHeader.legsCount }}</li>
                                        <li>Kézdőpont: {{ comp.matchHeader.startingPoint }}</li>
                                    </ul>
                                </div>
                                <p class="text-left card-title text-decoration-underline">Fordulók időpontjai:
                                </p>
                                <ul class="small">
                                    <li v-for="date in comp.matchHeader.matches" :key="date">
                                        {{ new Date(date).toLocaleDateString() }}
                                    </li>
                                </ul>
                                <div v-if="user.role == 2">
                                    <p class="text-left card-title text-decoration-underline">Jelentkezett játékosok:
                                    </p>
                                    <ul class="small">
                                        <li v-for="player in (comp.registeredPlayers as RegisteredPlayerModel[])"
                                            :key="player.id">
                                            {{ player.username }}
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <button type="button" v-if="!comp.userJoined && comp.userJoined != undefined"
                        :class="(new Date(comp.joinStartDate) > new Date(Date.now())) ? 'btn-secondary' : 'btn-success'"
                        class="btn justify-content-center d-flex w-100"
                        :disabled="(new Date(comp.joinStartDate) > new Date(Date.now()))"
                        @click="openVerifyModal(comp.id)">{{ "Jelentkezés" }}
                        {{ (new Date(comp.joinStartDate) > new Date(Date.now())) ? "zárolva" : "" }}</button>

                    <button type="button" v-if="comp.userJoined == undefined"
                        :class="(new Date(comp.joinEndDate) > new Date(Date.now())) ? 'btn-secondary' : 'btn-success'"
                        :disabled="(new Date(comp.joinEndDate) > new Date(Date.now()))"
                        class="btn justify-content-center d-flex w-100" @click="drawTournamentClick(comp.id)">{{
                            "Sorsolás" }}
                        {{ (new Date(comp.joinEndDate) > new Date(Date.now())) ? "zárolva" : "" }}</button>
                </div>
            </div>
        </div>
    </div>
    <VerifyModal v-if="showModal" :current-comp="selectedComp" :visible="showModal" @close="closeVerifyModal"
        @applied="onApplied" />
    <ResponseModal v-if="showAlert" :message="alertMessage" :success="alertSuccess" @close="showAlert = false" />
</template>

<style scoped>
.width-form-card {
    min-height: 60vh;
    width: 100%;
    background-color: black;
    padding: 10px;
    border-radius: 10px;
    overflow-y: auto;
}

.glass-card {
    background: rgba(0, 0, 0, 0.65);
    border: 5px solid gray;
}

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