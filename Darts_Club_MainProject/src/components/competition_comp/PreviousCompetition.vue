<script setup lang="ts">
import { useAnnouncedTmStore } from '@/stores/AnnouncedTmStore';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref } from 'vue';
import type CardModel from '@/models/CardModel';
import PreviousCard from '../bracket_comp/PreviousCard.vue';

const { user } = storeToRefs(useUserStore());
const { getAllPreviousCompetition} = useAnnouncedTmStore();
const { PreviousComps, alertCard} = storeToRefs(useAnnouncedTmStore());

const PreviousCompetitons = ref<CardModel[]>([]);

onMounted(async () => {
        await getAllPreviousCompetition(user.value.accessToken);
        let alertMessageHelp = "";
        PreviousCompetitons.value = PreviousComps.value;
        if (PreviousCompetitons.value.length == 0) {
            alertMessageHelp = 'Nincs befejezett mérkőzés!'
        }
        alertCard.value.message = alertMessageHelp;
        alertCard.value.show = (alertCard.value.message != '');
})
</script>


<template>
    <div class="col-12 mx-3 mx-sm-0 col-sm-10 offset-0 offset-sm-1 offset-md-0 col-md-6 col-xl-4 p-2"
        v-for="comp in PreviousCompetitons" :key="comp.id">
        <PreviousCard :competition="comp" :is-one-card="false"/>
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