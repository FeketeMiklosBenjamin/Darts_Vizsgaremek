<script setup lang="ts">
import type CardModel from '@/models/CardModel';
import router from '@/router';
import { useAnnouncedTmStore } from '@/stores/AnnouncedTmStore';
import { storeToRefs } from 'pinia';

const props = defineProps<{
    competition: CardModel,
    isOneCard: boolean
}>();

const NavigateTo = (competition_id: string) => {
    sessionStorage.setItem("matchId", competition_id);
    router.push('/bracket');
}

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
    <div class="d-flex glass-card width-form-card justify-content-center">
        <div class="card bg-black text-light" :class="borderColor(props.competition.level)" style="max-width: 45vh;">
            <img :src="props.competition.backroundImageUrl" class="card-img-middle" alt="...">
            <div class="card-body">
                <div class="d-flex justify-content-center">
                    <h5 class="card-title text-center fst-italic">{{ props.competition.name }}</h5>
                </div>
                <div class="card-body">
                    <p class="card-title text-center text-decoration-underline mt-3">Verseny időtartama:
                    </p>
                    <p class="text-center m-0 small">
                        {{ new Date(props.competition.tournamentStartDate).toLocaleDateString(undefined, {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit',
                            hour: '2-digit',
                            minute: '2-digit'
                        }) }}<br>-<br>{{ new
                            Date((props.competition.tournamentEndDate)).toLocaleDateString(undefined, {
                                year: 'numeric',
                                month: '2-digit',
                                day: '2-digit',
                                hour: '2-digit',
                                minute: '2-digit'
                            }) }}
                    </p>
                </div>
                <button v-if="!isOneCard" type="button" class="btn btn-warning justify-content-center d-flex mt-4 mb-0 w-100"
                    @click="NavigateTo(props.competition.id)">Eredmények megtekintése</button>
            </div>
        </div>
    </div>
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