<script setup lang="ts">
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import {onMounted, onUnmounted } from 'vue';
import { useRouter } from 'vue-router';

const { status, user } = storeToRefs(useUserStore());
const { logout } = useUserStore();
const router = useRouter();

const onLogout = async () => {
    try {
        await logout();
        status.value._id = '';  
        router.push('/');
    } catch (err) {
        status.value._id = '';
        router.push('/');
    }
};

const handleBeforeUnload = async (event: BeforeUnloadEvent) => {
    await onLogout();
};

onMounted(() => {
    window.addEventListener("beforeunload", handleBeforeUnload);
});

onUnmounted(() => {
    window.removeEventListener("beforeunload", handleBeforeUnload);
});

</script>


<template>
    <div class="shadow-lg stick">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark shadow-lg stick py-2">
        <div class="container">
            <a class="navbar-brand"><em class="display-6 title">Dart's Club</em></a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav"
                aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav ms-auto py-2">
                    <li class="nav-item me-3 mt-1">
                        <router-link :to="status._id ? '/statistic' : '/sign-in'" class="nav-link no-underline">
                            {{ status._id ? user.username : 'Bejelentkezés' }}
                        </router-link>
                    </li>
                    <li class="nav-item rounded-circle border border-3"
                        :class="user.level == 'Amateur' ? 'bg-secondary border-success' : 'bg-white border-info px-1' ">
                        <img v-if="status._id" :src="user.profilePictureUrl" class="profileImg border-0 mx-auto d-block" alt="Nincs">
                        <i v-else class="bi bi-person"></i>
                    </li>
                    <li class="nav-item my-auto ms-3">
                        <i class="bi bi-gear-fill" :class="status._id ? 'text-secondary' : 'text-white'"></i>
                    </li>
                    <li v-if="status._id" class="nav-item my-auto ms-4">
                        <a href="#" @click.prevent="onLogout" class="text-secondary">
                            <i class="bi bi-box-arrow-right"></i>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    </div>
</template>


<style scoped></style>