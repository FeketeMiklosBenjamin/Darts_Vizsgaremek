<script setup lang="ts">
import { useAnnouncedTmStore } from '@/stores/AnnouncedTmStore';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, watch } from 'vue';

const { user } = useUserStore();
const { getAllCompetition } = useAnnouncedTmStore();
const { Competitions } = storeToRefs(useAnnouncedTmStore());


onMounted(async () => {
    await getAllCompetition(user.accessToken);
})

let isOpen = ref(false);

let borderColor: string = "";
watch(() => Competitions.value, (newCompetitions) => {
    newCompetitions.forEach(comp => {
        const level = comp.matchHeader.level;
        switch (level) {
            case "Amatőr":
                borderColor = "success-border";
                break;
            case "Haladó":
                borderColor = "warning-border";
                break;
            case "Profi":
                borderColor = "danger-border";
                break;
            case "Bajnok":
                borderColor = "purple-border";
                break;
        }
    });
});


</script>

<template>
    <div class="main div row">
        <div class="col-3">
            <div class="row mx-5 dropdown d-flex justify-content-center mt-3">
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
            <div class="d-flex flex-column justify-content-center align-items-center min-vh-100 mt-5">
                <button class="btn btn-darkred text-white mb-3 mt-5">Jelentkezés</button>
                <button class="btn btn-warning">Előző versenyek</button>
            </div>
            <div class="col-8">
                    <div class="glass-card width-form" v-for="comp in Competitions" :key="comp.id">
                        <div class="d-flex justify-content-center">
                            <div class="card bg-black text-light" :class="borderColor" style="max-width: 45vh;">
                                <img :src="comp.matchHeader.backroundImageUrl" class="card-img-middle" alt="...">
                                <div class="card-body">
                                    <div class="d-flex justify-content-between">
                                        <h5 class="card-title text-center fst-italic">{{ comp.matchHeader.name }}</h5>
                                        <p style="width: 3.7rem;"><i class="bi bi-person-standing"></i>
                                            {{ comp.registeredPlayers }}/{{
                                                comp.maxPlayerJoin }}</p>
                                    </div>
                                    <div class="card-body">
                                        <p class="card-title text-center text-decoration-underline mt-2">Jelentkezés
                                            időtartama:
                                        </p>
                                        <p class="text-center m-0">{{ new Date(comp.joinStartDate).toLocaleDateString()
                                            }} - {{
                                                new
                                                    Date((comp.joinEndDate)).toLocaleDateString() }}</p>
                                        <div class="d-flex justify-content-center p-0">
                                            <button type="button" data-bs-toggle="collapse" @click="isOpen = !isOpen"
                                                :class="isOpen ? 'bi bi-caret-up btn text-light' : 'bi bi-caret-down-fill btn text-light'"
                                                style="font-size: 20px;">
                                            </button>
                                        </div>
                                        <div :class="['collapse', { show: isOpen }]" id="extraInfo">
                                            <div class="card card-body bg-dark text-light border-secondary">
                                                <p class="text-center">További információk a versenyről:</p>
                                                <p class="card-title text-center text-decoration-underline">Verseny
                                                    időtartama:
                                                </p>
                                                <p class="text-center small">{{ new
                                                    Date(comp.matchHeader.tournamentStartDate).toLocaleDateString()
                                                }}
                                                    -
                                                    {{ new
                                                        Date(comp.matchHeader.tournamentEndDate).toLocaleDateString() }}</p>
                                                <div class="d-flex justify-content-center pe-4">
                                                    <ul class="small">
                                                        <li>Setek száma: {{ comp.matchHeader.setsCount }}</li>
                                                        <li>Legek száma: {{ comp.matchHeader.legsCount }}</li>
                                                        <li>Kézdőpont: {{ comp.matchHeader.startingPoint }}</li>
                                                    </ul>

                                                </div>
                                                <p class="text-left card-title text-decoration-underline">Fordulók
                                                    időpontjai:
                                                </p>
                                                <div>
                                                    <ul class="small">
                                                        <li v-for="date in comp.matchHeader.matches" :key="date">{{ new
                                                            Date(date).toLocaleDateString() }}</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <a href="#" class="btn btn-primary justify-content-center d-flex">Jelentkezés</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</template>

<style scoped>
.width-form {
    width: 100%;
    min-height: 60vh;
    background-color: black;
    padding: 20px;
    border-radius: 10px;
    overflow-y: auto;
}

.glass-card {
    padding: 20px;
    background: rgba(0, 0, 0, 0.65);
    border: 5px solid gray;
}
</style>