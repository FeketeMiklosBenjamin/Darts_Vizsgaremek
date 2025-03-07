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
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark py-2">
            <div class="container">
                <a class="navbar-brand"><em class="display-6 title">Dart's Club</em></a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav"
                    aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ms-auto py-2">
                        <li class="nav-item">
                            <div v-if="status._id">
                                <router-link :to="`/statistic`" class="no-underline">
                                    <a class="nav-link">{{ user.username }}</a>
                                </router-link>
                            </div>
                            <div v-else>
                                <router-link :to="`/sign-in`" class="no-underline">
                                    <a class="nav-link" href="#">Bejelentkezés</a>
                                </router-link>
                            </div>
                        </li>
                        <div v-if="status._id" class="ms-1 m-auto px-1">
                            <li class="nav-item bg-secondary rounded-circle border border-3 border-danger text-white">
                                <img :src=user.profilePictureUrl class="img profileImg border-0 mx-auto d-block"
                                    alt="Nincs">
                            </li>
                        </div>
                        <div v-else class="ms-1 m-auto px-1">
                            <li class="nav-item bg-white rounded-circle px-1 border border-3 border-info">
                                <i class="bi bi-person"></i>
                            </li>
                        </div>
                        <div v-if="status._id" class="my-auto ms-3">
                            <li class="nav-item text-secondary my-auto">
                                <i class="bi bi-gear-fill"></i>
                            </li>
                        </div>
                        <div v-else class="my-auto ms-3">
                            <li class="nav-item text-white my-auto">
                                <i class="bi bi-gear-fill"></i>
                            </li>
                        </div>
                        <div v-if="status._id" class="my-auto ms-4">
                            <li class="nav-item my-auto">
                                <a href="#" @click="onLogout" class="text-secondary">
                                <i class="bi bi-box-arrow-right"></i></a>
                            </li>
                        </div>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
</template>


<style scoped></style>