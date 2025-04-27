<script setup lang="ts">
import { ref } from 'vue';
import CardCompetition from './CardCompetition.vue';
import { storeToRefs } from 'pinia';
import { useUserStore } from '@/stores/UserStore';
import { useAnnouncedTmStore } from '@/stores/AnnouncedTmStore';
import PreviousCompetition from './PreviousCompetition.vue';

const { user } = storeToRefs(useUserStore());

const { alertCard } = useAnnouncedTmStore();

const areJoinedCards = ref(false);
const isRegisterPage = ref(true);

const changePage = () => {
    isRegisterPage.value = !isRegisterPage.value;
};
</script>

<template>
    <div class="row left-side">
        <div class="col-md-3 col-12 col-sm-8 offset-md-0 offset-sm-2 offset-0 p-0">
            <div class="row mx-5 dropdown d-flex mt-3">
                <button class="btn btn-secondary" type="button" id="dropdownMenuButton" data-toggle="dropdown"
                    aria-expanded="false">
                    Magyarázat <i class="bi bi-info-circle"></i>
                </button>
                <div class="dropdown-menu bg-light pb-0">
                    <div class="text-center">
                        <p class="success-text">Zöld színű keret: Amatőr verseny</p>
                        <p class="warning-text">Sárga színű keret: Haladó verseny</p>
                        <p class="danger-text">Piros színű keret: Profi verseny</p>
                        <p class="purple-text">Lila színű keret: Bajnok verseny</p>
                    </div>
                </div>
            </div>
            <div class="d-flex flex-column justify-content-center align-items-center custom-min-vh-md mt-5">
                <div class="row form-check form-switch justify-content-center d-flex glass-card pe-3" v-if="isRegisterPage">
                    <input class="col-4 form-check-input fs-5" type="checkbox" id="flexSwitchCheckDefault"
                        v-model="areJoinedCards" data-cy="switch"/>
                    <label v-if="user.role == 1" class="col-8 form-check-label fst-italic text-light fs-5"
                        for="flexSwitchCheckDefault" data-cy="text_in_switch">
                        {{ areJoinedCards ? "Regisztrált" : "Nevezés" }}
                    </label>
                    <label v-else class="col-8 form-check-label fst-italic text-light fs-5"
                        for="flexSwitchCheckDefault">
                        {{ areJoinedCards ? "Sorsolható" : "Összes" }}
                    </label>
                </div>
                <button class="btn btn-darkred text-white mb-3 mt-3" :disabled="isRegisterPage" @click="changePage">{{
                    user.role == 1 ? 'Jelentkezés' : 'Sorsolás' }}</button>
                <button class="btn btn-warning" :disabled="!isRegisterPage" @click="changePage" data-cy="previous_btn">Előző versenyek</button>
            </div>
        </div>

        <div class="col-12 offset-md-0 offset-sm-1 col-md-9 row main-div"
            :class="(alertCard.show ? 'd-flex justify-content-center align-items-center' : '')" v-if="isRegisterPage">
            <CardCompetition :are-joined-cards="areJoinedCards" />
            <div v-if="alertCard.show" data-cy="alert_message"
                class="alert alert-warning text-center fs-5 mx-auto w-50 d-flex justify-content-center align-items-center"
                style="height: 100px;">
                <i class="bi bi-exclamation-circle mx-2 d-inline"></i>
                <div class="d-inline">{{ alertCard.message }}</div>
            </div>
        </div>
        <div class="col-12 offset-md-0 offset-sm-1 col-md-9 row main-div"
            :class="(alertCard.show ? 'd-flex justify-content-center align-items-center' : '')" v-else>
            <PreviousCompetition :-is-one-card="false"/>
            <div v-if="alertCard.show" data-cy="alert_message"
                class="alert alert-warning text-center fs-5 mx-auto w-50 d-flex justify-content-center align-items-center"
                style="height: 100px;">
                <i class="bi bi-exclamation-circle mx-2 d-inline"></i>
                <div class="d-inline">{{ alertCard.message }}</div>
            </div>
        </div>
    </div>
</template>

<style scoped>
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
