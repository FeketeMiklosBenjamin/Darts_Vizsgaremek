<script setup lang="ts">
import type RegisterCompetitionModel from '@/models/RegisterCompetitionModel'
import { useAnnouncedTmStore } from '@/stores/AnnouncedTmStore'
import { useUserStore } from '@/stores/UserStore';
import { Modal } from 'bootstrap';
import { nextTick, onMounted, ref, watch } from 'vue'

const { user } = useUserStore();
const { status, matchHeader } = useAnnouncedTmStore();
const { registerCompetition, uploadMatchHeader } = useAnnouncedTmStore();

const matchImg = ref<File | null>(null);

const modal = ref<HTMLElement>();
let modalInstance: Modal;

onMounted(async () => {
    await nextTick();
    if (modal.value) {
        modalInstance = new Modal(modal.value);
    }
});

const matchImgUrl = ref<string>('https://res.cloudinary.com/dvikunqov/image/upload/v1743843175/darts_background_pictures/ftrmvy0bpxjgoxj5tmzm.jpg');

const handleFileChange = (event: Event) => {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput?.files?.length) {
        matchImg.value = fileInput.files[0];
        matchImgUrl.value = URL.createObjectURL(matchImg.value);
    }
    else {
        matchImgUrl.value = "https://res.cloudinary.com/dvikunqov/image/upload/v1743843175/darts_background_pictures/ftrmvy0bpxjgoxj5tmzm.jpg"
    }
};


const processing = ref<boolean>(false);

const competitionForm = ref<RegisterCompetitionModel>({
    headerId: '',
    name: '',
    setsCount: 0,
    legsCount: 3,
    startingPoint: 501,
    password: '',
    validPassword: '',
    level: 'Amatőr',
    maxPlayerJoin: 16,
    joinStartDate: '',
    joinEndDate: '',
    matchDates: [],
})

const rounds = ref(["nyolcaddöntő", "negyeddöntő", "elődöntő", "döntő"])
const currentIndex = ref(0)
const roundDates = ref(["", "", "", ""]);
const joinStartD = ref<string>('');
const joinEndD = ref<string>('');
const isSecondPage = ref(false);

let borderColor: string = "success-border";
let isOpen = ref(false);


const nextRound = () => {
    currentIndex.value = (currentIndex.value + 1) % rounds.value.length
}

const prevRound = () => {
    currentIndex.value = (currentIndex.value - 1 + rounds.value.length) % rounds.value.length
}

watch(
    () => competitionForm.value.maxPlayerJoin,
    (newValue, oldValue) => {
        updateAvailableRounds(newValue, oldValue);
    }
);

watch(() => competitionForm.value.level, (level: string) => {
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
})

const updateAvailableRounds = (newValueString: Number, oldValueString: Number) => {
    let newValue = Number(newValueString);
    let oldValue = Number(oldValueString);
    let operator = newValue > oldValue;
    let steps = ((operator) ? ((newValue / oldValue) / 2) : (oldValue / newValue) / 2);

    if (operator) {
        if (rounds.value.length == 2) {
            if (newValue == 8) {
                rounds.value.unshift("negyeddöntő");
                roundDates.value.unshift("")
            }
            else {
                rounds.value.unshift("nyolcaddöntő", "negyeddöntő");
                roundDates.value.unshift("", "")
            }
        }
        else {
            rounds.value.unshift("nyolcaddöntő");
            roundDates.value.unshift("")
        }
    }
    else {
        rounds.value.splice(0, steps);
        roundDates.value.splice(0, steps);
    }
    currentIndex.value = 0
}

async function Send() {
    status.resp = '';
    processing.value = true;
    await new Promise(resolve => setTimeout(resolve, 100));

    if (competitionForm.value.password != competitionForm.value.validPassword) {
        status.resp = "A két jelszó nem egyezik meg!";
        processing.value = false;
    }

    if (joinStartD.value == '' || joinEndD.value == '' || (roundDates.value.find(x => x == '') != undefined)) {
        status.resp = "Hiányzó dátum!";
        processing.value = false;
    }

    if (joinStartD.value != '') {
        competitionForm.value.joinStartDate = new Date(joinStartD.value).toISOString()
    }

    if (joinEndD.value != '') {
        competitionForm.value.joinEndDate = new Date(joinEndD.value).toISOString()
    }

    try {
        competitionForm.value.matchDates = [];
        roundDates.value.forEach(match => {
            competitionForm.value.matchDates.push(new Date(match).toISOString());
        });
    } catch (error) {

    }

    if (status.resp == '') {
        try {
            await registerCompetition(user.accessToken, competitionForm.value);

            if (matchImg.value) {
                await uploadMatchHeader(user.accessToken, matchImg.value, matchHeader);
            }

            joinStartD.value = '';
            joinEndD.value = '';
            roundDates.value = Array(rounds.value.length).fill('');
            borderColor = "success-border";
            currentIndex.value = 0;

            competitionForm.value = {
                headerId: '',
                name: '',
                setsCount: 0,
                legsCount: 3,
                startingPoint: 501,
                password: '',
                validPassword: '',
                level: 'Amatőr',
                maxPlayerJoin: 16,
                joinStartDate: '',
                joinEndDate: '',
                matchDates: [],
            }
        } catch (err) {
            status.success = false;
            processing.value = false;
        }
    } else { }
    processing.value = false;
    modalInstance.show();

    setTimeout(() => {
        modalInstance.hide();
        status.success = false;
    }, 4000);
}

</script>

<template>
    <div id="myModal" class="modal fade" tabindex="-1" role="dialog" ref="modal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="alert text-center"
                        :class="{ 'alert-success': status.success, 'alert-danger': !status.success }"><i class="bi me-3"
                            :class="{ 'bi-check-circle': status.success, 'bi-x-circle': !status.success }"></i>
                        {{ status.resp }}</div>
                </div>
            </div>
        </div>
    </div>
    <div class="row main-div">
        <div class="col-md-6 col-sm-8 offset-md-0 offset-sm-2 offset-0 col-12 mb-5">
            <div class="col-12 col-xl-7 offset-xl-3 col-lg-8 offset-lg-2 mt-md-5 mt-0">
                <div class="glass-card width-form">
                    <div class="form-check form-switch justify-content-center d-flex gap-3 mb-4">
                        <input class="form-check-input fs-5" type="checkbox" id="flexSwitchCheckDefault"
                            v-model="isSecondPage" />
                        <label class="form-check-label fst-italic text-light fs-5" for="flexSwitchCheckDefault">
                            {{ (isSecondPage == false ? "1." : "2.") }} oldal
                        </label>
                    </div>
                    <div v-if="!isSecondPage">
                        <div class="input-group mb-3">
                            <span class="input-group-text" id="competition-name">Verseny neve:</span>
                            <input type="text" class="form-control" placeholder="California Groupama"
                                aria-label="competition-name" aria-describedby="competition-name"
                                v-model="competitionForm.name" />
                        </div>
                        <div class="mb-3 text-white fst-italic">
                            <label for="SetCount" class="form-label">Setek száma: {{ competitionForm.setsCount
                                }}</label>
                            <input type="range" class="form-range custom-range" min="0" max="12" id="SetCount"
                                v-model="competitionForm.setsCount" />
                        </div>
                        <div class="mb-3 text-white fst-italic">
                            <label for="LegCount" class="form-label">Legek száma: {{ competitionForm.legsCount
                                }}</label>
                            <input type="range" class="form-range custom-range" min="3" max="20" id="LegCount"
                                v-model="competitionForm.legsCount" />
                        </div>
                        <div class="row justify-content-center">
                            <label class="text-white text-center fs-5 fst-italic mb-2">Kezdőpontok száma:</label>
                            <div class="col-auto text-white">
                                <div class="form-check">
                                    <input class="form-check-input" type="radio" name="startingPoint" id="301"
                                        v-model="competitionForm.startingPoint" value="301" />
                                    <label class="form-check-label" for="301">301</label>
                                </div>
                            </div>
                            <div class="col-auto text-white">
                                <div class="form-check">
                                    <input class="form-check-input" type="radio" name="startingPoint" id="501"
                                        v-model="competitionForm.startingPoint" value="501" />
                                    <label class="form-check-label" for="501">501</label>
                                </div>
                            </div>
                            <div class="col-auto text-white">
                                <div class="form-check">
                                    <input class="form-check-input" type="radio" name="startingPoint" id="701"
                                        v-model="competitionForm.startingPoint" value="701" />
                                    <label class="form-check-label" for="701">701</label>
                                </div>
                            </div>
                        </div>
                        <div class="input-group mt-4">
                            <span class="input-group-text" id="competition-psw">Verseny jelszava:</span>
                            <input type="password" class="form-control" aria-label="competition-psw"
                                aria-describedby="competition-psw" v-model="competitionForm.password" />
                        </div>
                        <div class="input-group mt-4">
                            <span class="input-group-text" id="competition-psw">Jelszó újra:</span>
                            <input type="password" class="form-control" aria-label="competition-psw"
                                aria-describedby="competition-psw" v-model="competitionForm.validPassword" />
                        </div>
                        <label class="text-white text-center mt-3 fst-italic fs-5"
                            for="competition-level">Szint:</label>
                        <div class="d-flex justify-content-center align-items-center">
                            <select class="form-select form-select-xl w-100 text-start text-center"
                                id="competition-level" v-model="competitionForm.level">
                                <option value="Amatőr">Amatőr</option>
                                <option value="Haladó">Haladó</option>
                                <option value="Profi">Professzionális</option>
                                <option value="Bajnok">Bajnok</option>
                            </select>
                        </div>
                    </div>
                    <div v-else>
                        <div class="mt-3 text-white fst-italic fs-5">
                            <label for="start_competition" class="form-label">Jelentkezés kezdete:</label>
                            <input type="datetime-local" class="form-control" id="start_competition"
                                v-model="joinStartD" />
                        </div>
                        <div class="my-3 text-white fst-italic fs-5">
                            <label for="end_competition" class="form-label">Jelentkezés vége:</label>
                            <input type="datetime-local" class="form-control" id="end_competition" v-model="joinEndD" />
                        </div>
                        <label class="text-white fst-italic fs-5" for="competition-maxPlayer">Hány fő:</label>
                        <div class="d-flex justify-content-center align-items-center">
                            <select class="form-select form-select-xl w-100 text-start" id="competition-maxPlayer"
                                v-model="competitionForm.maxPlayerJoin">
                                <option value="4">4</option>
                                <option value="8">8</option>
                                <option value="16">16</option>
                            </select>
                        </div>
                        <div class="mt-3">
                            <label for="background_file" class="form-label text-white fst-italic fs-5">Verseny
                                háttérképe:</label>
                            <input class="form-control" type="file" id="background_file" @change="handleFileChange" />
                        </div>
                        <div class="mt-3 text-white fst-italic" style="font-size: 18px">
                            <label for="matches_date" class="form-label">
                                Versenyek időpontjai: ({{ rounds[currentIndex] }})
                            </label>
                            <div class="d-flex gap-2">
                                <button class="btn btn-secondary bi bi-arrow-left-short" @click="prevRound"></button>
                                <input type="datetime-local" class="form-control" id="matches_date"
                                    v-model="roundDates[currentIndex]" />
                                <button class="btn btn-secondary bi bi-arrow-right-short" @click="nextRound"></button>
                            </div>
                        </div>
                    </div>
                    <button type="button" class="btn btn-darkred text-white w-100 mt-5" :disabled="processing"
                        @click="Send">
                        Elküldés <span v-if="processing" class="spinner-border spinner-border-sm"></span>
                    </button>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-sm-8 offset-md-0 offset-sm-2 offset-0 col-12 mb-5">
            <div class="col-12 col-xl-7 offset-xl-3 col-lg-8 offset-lg-2 mt-md-5 mt-0">
                <div class="glass-card width-form">
                    <div class="d-flex justify-content-center">
                        <div class="card bg-black text-light" :class="borderColor" style="max-width: 45vh;">
                            <img :src="matchImgUrl" class="card-img-middle" alt="...">
                            <div class="card-body">
                                <div class="d-flex justify-content-between">
                                    <h5 class="card-title text-center fst-italic">{{ competitionForm.name }}</h5>
                                    <p style="width: 3.7rem;"><i class="bi bi-person-standing"></i> 0/{{
                                        competitionForm.maxPlayerJoin }}</p>
                                </div>
                                <div class="card-body">
                                    <p class="card-title text-center text-decoration-underline mt-2">Jelentkezés
                                        időtartama:
                                    </p>
                                    <p class="text-center m-0 small">{{ new
                                        Date(joinStartD).toLocaleDateString(undefined, {
                                            year: 'numeric',
                                            month: '2-digit',
                                            day: '2-digit',
                                            hour: '2-digit',
                                            minute: '2-digit'
                                        }) }}<br>-<br>{{ new
                                            Date((joinEndD)).toLocaleDateString(undefined, {
                                                year: 'numeric',
                                                month: '2-digit',
                                                day: '2-digit',
                                                hour: '2-digit',
                                                minute: '2-digit'
                                            }) }}</p>
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
                                            <p class="text-center small">{{ new Date(roundDates[0]).toLocaleDateString()
                                                }}
                                                -
                                                {{ new
                                                    Date(roundDates[roundDates.length - 1]).toLocaleDateString() }}</p>
                                            <div class="d-flex justify-content-center pe-4">
                                                <ul class="small">
                                                    <li>Setek száma: {{ competitionForm.setsCount }}</li>
                                                    <li>Legek száma: {{ competitionForm.legsCount }}</li>
                                                    <li>Kézdőpont: {{ competitionForm.startingPoint }}</li>
                                                </ul>

                                            </div>
                                            <p class="text-left card-title text-decoration-underline">Fordulók
                                                időpontjai:
                                            </p>
                                            <div>
                                                <ul class="small">
                                                    <li v-for="date in roundDates" :key="date">{{ new
                                                        Date(date).toLocaleDateString(undefined, {
                                                            year: 'numeric',
                                                            month: '2-digit',
                                                            day: '2-digit',
                                                            hour: '2-digit',
                                                            minute: '2-digit'
                                                        }) }}</li>
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
.btn-secondary {
    background-color: rgb(194, 46, 46);
    border-color: rgb(194, 46, 46);
}

.btn-secondary:hover {
    background-color: rgb(170, 30, 30);
    border-color: rgb(170, 30, 30);
}

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

.form-check {
    margin-top: 2%;
}

.custom-range::-webkit-slider-thumb {
    background: rgb(194, 46, 46);
}

.custom-range::-moz-range-thumb {
    background: rgb(194, 46, 46);
}

.custom-range::-ms-thumb {
    background: rgb(194, 46, 46);
}

.form-label {
    text-shadow: 4px 4px 5px rgba(0, 0, 0, 0.5);
}
</style>
