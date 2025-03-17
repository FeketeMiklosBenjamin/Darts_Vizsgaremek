<script setup lang="ts">
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';

const { status, user } = storeToRefs(useUserStore());

</script>

<template>
    <div class="background_color-feed">
        <div class="row">
            <div class="col-5 mt-5">
                <div class="bg-secondary rounded-circle border border-5 statistic-img" :class="{
                    'border-success': user.level == 'Amateur',
                    'border-warning': user.level == 'Advanced',
                    'border-danger': user.level == 'Professional'
                }">
                    <img :src=user.profilePictureUrl class="statistic-profileImg d-block" alt="Nincs">
                </div>
                <h1 class="display-6 text-white margin-statname">{{ user.username }}</h1>
            </div>
            <div class="col-7">
                <p class="text-white">Szia</p>
            </div>
        </div>
        <div class="row">
            <div class="col-5">
                <button class="btn btn-warning w-25 px-1 marginst-btn">Módosítás</button>
            </div>
        </div>
    </div>
</template>

<style scoped></style>