<script setup lang="ts">
import VueCountdown from '@chenfengyuan/vue-countdown';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { onMounted, onUnmounted, ref } from 'vue';
import { onBeforeRouteLeave, useRouter } from 'vue-router';

const { status, user } = storeToRefs(useUserStore());
const { logout, refreshTk, startCountdown } = useUserStore();
const router = useRouter();

const countdownKey = ref(0);
const remainingTime = ref(15 * 60 * 1000);

const updateBackgroundCountdown = () => {
    if (remainingTime.value <= 5 * 60 * 1000) {
        setAccesTk();
    }
};

const keepTimeOnNavigate = () => {
    localStorage.setItem('Time', String(remainingTime.value));
};

const setAccesTk = async () => {
    try {
        await refreshTk();
        countdownKey.value += 1;
    } catch (err) {}
}

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

let countdownInterval: any;

onMounted(() => {
    const savedTime = localStorage.getItem('Time');
    if (savedTime) {
        remainingTime.value = parseInt(savedTime);
    }

    countdownInterval = setInterval(() => {
        remainingTime.value -= 1000;
        updateBackgroundCountdown();
        if (remainingTime.value <= 0) {
            clearInterval(countdownInterval);
        }
    }, 1000);
    window.addEventListener("beforeunload", handleBeforeUnload);
    onBeforeRouteLeave((to, from, next) => {
        keepTimeOnNavigate(); 
        next();
    });
});

onUnmounted(() => {
    window.removeEventListener("beforeunload", handleBeforeUnload);
    clearInterval(countdownInterval);
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
                        <li class="nav-item me-2 mt-2">
                            <router-link :to="status._id ? '/statistic' : '/sign-in'" class="nav-link no-underline">
                                {{ status._id ? user.username : 'Bejelentkezés' }}
                            </router-link>
                        </li>
                        <li class="nav-item rounded-circle border border-3 mt-1" :class="{
                            'border-success': user.level == 'Amateur',
                            'border-warning': user.level == 'Advanced',
                            'border-danger': user.level == 'Professional',
                            'bg-white border-info px-1': status._id == ''
                        }">
                            <img v-if="status._id" :src="user.profilePictureUrl"
                                class="profileImg border-0 mx-auto d-block" alt="Nincs">
                            <i v-else class="bi bi-person"></i>
                        </li>
                        <li v-if="status._id" class="nav-item my-auto ms-4">
                            <a href="#" @click.prevent="onLogout" class="text-secondary">
                                <i class="bi bi-box-arrow-right"></i>
                            </a>
                            <VueCountdown v-if="status._id" :key="countdownKey"  :time="15 * 60 * 1000" v-slot="{ minutes, seconds }" @end="onLogout">
                                <span class="text-light ms-2">{{ minutes }}:{{ String(seconds).padStart(2, '0') }}</span>
                            </VueCountdown>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
</template>


<style scoped></style>