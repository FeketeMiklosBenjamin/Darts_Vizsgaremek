<script setup lang="ts">
import type CompetitionModel from '@/models/CompetitionModel';
import VerifyModal from './VerifyModal.vue';
import { useAnnouncedTmStore } from '@/stores/AnnouncedTmStore';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, watch } from 'vue';
import ResponseModal from './ResponseModal.vue';

const { user } = useUserStore();
const { getAllCompetition, status } = useAnnouncedTmStore();
const { Competitions } = storeToRefs(useAnnouncedTmStore());

const filteredCompetition = ref<CompetitionModel[]>([]);
const selectedComp = ref<CompetitionModel | null>(null);
const openState = ref<{ [key: string]: boolean }>({});
const showModal = ref(false);
const showAlert = ref(false);
const alertMessage = ref('');
const alertSuccess = ref(true);

const toggleOpen = (id: string) => {
    openState.value[id] = !openState.value[id];
};

const areJoinedCards = ref(false);

watch(areJoinedCards, () => {
    filteredCompetition.value = Competitions.value.filter(x => x.matchHeader.level == user.level && new Date(x.joinEndDate) > new Date(Date.now()) && x.userJoined == areJoinedCards.value);
});

onMounted(async () => {
    await getAllCompetition(user.accessToken);
    if (user.role != 2) {
        filteredCompetition.value = Competitions.value.filter(x => x.matchHeader.level == user.level && new Date(x.joinEndDate) > new Date(Date.now()) && x.userJoined == false);
    } else {
        filteredCompetition.value = Competitions.value;
    }
})

const openVerifyModal = (compId: string) => {
    const foundComp = filteredCompetition.value.find(u => u.id === compId);
    
    if (foundComp) {
        selectedComp.value = foundComp;
        showModal.value = true;
    }
};

const closeVerifyModal = () => {
    showModal.value = false;
    selectedComp.value = null;
};

const onApplied = () => {
    showAlert.value = true;
    alertMessage.value = status.resp;
    alertSuccess.value = status.success;

    showModal.value = false;

    if (status.success && selectedComp.value) {
        filteredCompetition.value = filteredCompetition.value.filter(comp => comp.id !== selectedComp.value!.id);
        
        selectedComp.value.userJoined = true;
    }
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
    <div class="row left-side">
        <div class=" col-md-3 col-12 col-sm-8 offset-md-0 offset-sm-2 offset-0 p-0">
            <div class="row mx-5 dropdown d-flex mt-3">
                <button class="btn btn-secondary" type="button" id="dropdownMenuButton" data-toggle="dropdown"
                    aria-expanded="false">
                    Magyarázat <i class="bi bi-info-circle"></i>
                </button>
                <div class="dropdown-menu bg-light pb-0">
                    <div class="text-center">
                        <p class="success-text">Zöld színű keret: Amatör verseny</p>
                        <p class="warning-text">Sárga színű keret: Haladó verseny</p>
                        <p class="danger-text">Piros színű keret: Profi verseny</p>
                        <p class="purple-text">Lila színű keret: Bajnok verseny</p>
                    </div>
                </div>
            </div>
            <div class="d-flex flex-column justify-content-center align-items-center custom-min-vh-md mt-5">
                <div class="row form-check form-switch justify-content-center d-flex glass-card pe-3">
                    <input class="col-4 form-check-input fs-5" type="checkbox" id="flexSwitchCheckDefault"
                        v-model="areJoinedCards" />
                    <label class="col-8 form-check-label fst-italic text-light fs-5" for="flexSwitchCheckDefault">
                        {{ (areJoinedCards == false ? "Nevezés" : "Regisztrált") }}
                    </label>
                </div>
                <button class="btn btn-darkred text-white mb-3 mt-3">Jelentkezések</button>
                <button class="btn btn-warning">Előző versenyek</button>
            </div>
        </div>
        <div class="col-12 offset-md-0 offset-sm-1 col-md-9 row main-div">
            <div class="col-12 mx-3 mx-sm-0 col-sm-10 offset-0 offset-sm-1 offset-md-0 col-md-6 col-xl-4 p-2"
                v-for="comp in filteredCompetition" :key="comp.id">
                <div class="d-flex glass-card width-form-card justify-content-center">
                    <div class="card bg-black text-light" :class="borderColor(comp.matchHeader.level)"
                        style="max-width: 45vh;">
                        <img :src="comp.matchHeader.backroundImageUrl" class="card-img-middle" alt="...">
                        <div class="card-body">
                            <div class="d-flex justify-content-between">
                                <h5 class="card-title text-center fst-italic">{{ comp.matchHeader.name }}</h5>
                                <p style="width: 4rem;"><i class="bi bi-person-standing"></i>
                                    {{ comp.registeredPlayers }}/{{ comp.maxPlayerJoin }}</p>
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
                                    </div>
                                </div>
                            </div>
                            <button type="button"
                                :class="new Date(comp.joinStartDate) > new Date(Date.now()) ? 'btn-secondary' : 'btn-success'"
                                class="btn justify-content-center d-flex w-100"
                                :disabled="new Date(comp.joinStartDate) > new Date(Date.now())"
                                @click="openVerifyModal(comp.id)">Jelentkezés</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <VerifyModal v-if="showModal" :current-comp="selectedComp" :visible="showModal" @close="closeVerifyModal"
        @applied="onApplied" />
    <ResponseModal v-if="showAlert" :message="alertMessage" :success="alertSuccess" @close="showAlert = false"/>
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