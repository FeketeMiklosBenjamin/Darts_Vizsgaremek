<script setup lang="ts">
import VueCountdown from '@chenfengyuan/vue-countdown';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { nextTick, onMounted, onUnmounted, ref, watch } from 'vue';
import { onBeforeRouteLeave, useRouter } from 'vue-router';
import { useMessagesStore } from '@/stores/MessagesStore';

const { status, user } = storeToRefs(useUserStore());
const { logout, refreshTk } = useUserStore();
const { getYourMessages, deleteMyMessages } = useMessagesStore();
const { forUserEmails, forAdminEmails } = storeToRefs(useMessagesStore());
const router = useRouter();

const remainingTime = ref(2 * 60 * 1000);
const hasRefreshed = ref(false);
let countdownInterval: any;
const isDropdownVisible = ref(false);  


const MessageDelete = async (id: string) => {
    await deleteMyMessages(id, user.value.accessToken);

    const userIndex = forUserEmails.value.findIndex(email => email.id === id);
    const AdminIndex = forAdminEmails.value.findIndex(email => email.id === id);
    if (AdminIndex !== -1) {
        forAdminEmails.value.splice(AdminIndex, 1);
    }
    if (userIndex !== -1) {
        forUserEmails.value.splice(userIndex, 1);
    }
}

const toggleDropdown = async () => {
    if (user.value.accessToken) {
        await getYourMessages(user.value.accessToken);
    }

    await nextTick();
    isDropdownVisible.value = !isDropdownVisible.value; 
};

onMounted(async () => {
    const savedTime = sessionStorage.getItem('Time');
    if (savedTime) {
        remainingTime.value = parseInt(savedTime);
    }
    
    if (status.value.isLoggedIn) {
        startBackgroundTimer();
    }

    window.addEventListener("beforeunload", handleBeforeUnload);
    onBeforeRouteLeave((to, from, next) => {
        keepTimeOnNavigate(); 
        next();
    });
});

watch(() => status.value.isLoggedIn, (newVal) => {
    if (newVal) {
        startBackgroundTimer();
    } else {
        stopBackgroundTimer();
    }
});

const keepTimeOnNavigate = () => {
    sessionStorage.setItem('Time', String(remainingTime.value));
};

const setAccesTk = async () => {
    try {
        await refreshTk();
        hasRefreshed.value = false;
        remainingTime.value = 15 * 60 * 1000;
        keepTimeOnNavigate();
    } catch (err) {}
}

const startBackgroundTimer = () => {
    if (countdownInterval) return;
    countdownInterval = setInterval(() => {
        remainingTime.value -= 1000;
        
        if (remainingTime.value <= 5 * 60 * 1000 && !hasRefreshed.value) {
            hasRefreshed.value = true;
            setAccesTk();
        }
        
        keepTimeOnNavigate();
    }, 1000);
}

const stopBackgroundTimer = () => {
    if (countdownInterval) {
        clearInterval(countdownInterval);
        countdownInterval = null;
    }
};


const onLogout = async () => {
    try {
        await logout();
        status.value._id = '';
        forUserEmails.value.splice(0, forUserEmails.value.length);
        forAdminEmails.value.splice(0, forAdminEmails.value.length);
        router.push('/');
    } catch (err) {
        status.value._id = '';
        await router.push('/');
    }
};

const handleBeforeUnload = async (event: BeforeUnloadEvent) => {
    await onLogout();
};

onUnmounted(() => {
    window.removeEventListener("beforeunload", handleBeforeUnload);
    sessionStorage.removeItem("Time");
    clearInterval(countdownInterval);
});


</script>

<template>
    <div class="shadow-lg stick">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark shadow-lg stick py-2">
            <div class="container">
                <a class="navbar-brand"><em class="display-6 title">Sons of the Fallen's</em></a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav"
                    aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ms-auto py-2">
                        <li v-if="status._id" class="nav-item me-3 mt-1 text-secondary">
                            <i class="bi bi-envelope fs-4" @click="toggleDropdown"></i>
                            <div :class="['dropdown-menu', { visible: isDropdownVisible }]">
                                <div v-if="user.role == 2">
                                    <div v-for="(email, index) in forAdminEmails" :key="index" class="message-box">
                                        <div class="message-box-content">
                                            <p class="fs-5 text-center mb-3">{{ email.title }}<i class="bi bi-x-circle text-danger mt-1" @click="MessageDelete(email.id)"></i></p>
                                            <p class="d-inline fst-italic">{{ email.emailAddress }}</p>
                                            <p class="d-inline ms-5 fst-italic">{{ email.sendDate }}</p>
                                        </div>
                                    </div>
                                </div>
                                <div v-else>
                                    <div v-for="(email, index) in forUserEmails" :key="index" class="message-box">
                                        <div class="message-box-content text-center">
                                            <p class="fs-5 mb-3">{{ email.title }}<i class="bi bi-x-circle text-danger mt-1" @click="MessageDelete(email.id!)"></i></p>
                                            <p class="fst-italic">{{ email.sendDate }}</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li>
                            <router-link v-if="status._id" :to="`/main-page`" class="nav-link nav-item m-2 text-secondary">
                                <i class="bi bi-house-door-fill"></i>
                            </router-link>
                        </li>
                        <li class="nav-item me-2 mt-2">
                            <router-link :to="status._id ? `/statistic/${user.id}` : '/sign-in'" class="nav-link no-underline">
                                {{ status._id ? user.username : 'Bejelentkezés' }}
                            </router-link>
                        </li>
                        <li class="nav-item rounded-circle border border-3 mt-1" :class="{
                            'border-success': user.level == 'Amateur',
                            'border-warning': user.level == 'Advanced',
                            'border-danger': user.level == 'Professional',
                            'border-secondary': user.role == 2,
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
                            <VueCountdown v-if="status._id" :time="15 * 60 * 1000" v-slot="{ minutes, seconds }" @end="onLogout">
                                <span class="text-light ms-2">{{ minutes }}:{{ String(seconds).padStart(2, '0') }}</span>
                            </VueCountdown>
                            <div v-if="user.role == 2" class="tooltip-container">
                                <i class="bi bi-person-badge-fill text-secondary ms-2"></i>
                                <div class="tooltip-text">Ön admin</div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
</template>


<style scoped>
.bi-x-circle {
    font-size: 1vw;
    position: absolute;
    left: 275px;
    cursor: pointer;
}

.message-box {
    background-color: #343a40;
    color: white;
    border: 2px solid black;
    border-radius: 5px;
    padding: 10px;
    padding-top: 0;
    margin: 5px 0;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.2);
}

.message-box-content {
    font-size: 14px;
}

.message-box p {
    margin: 5px 0;
}

.dropdown-menu {
    position: absolute;
    top: 90%;
    left: 60%;
    background-color: white;
    color: black;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    border: 2px solid gray;
    border-radius: 5px;
    min-width: 20vw;
    opacity: 0;
    overflow: hidden; 
    display: block;
    padding: 5px;
    z-index: 1;
    transition: max-height 0.3s ease, opacity 0.3s ease;
}

.dropdown-menu.visible {
    display: block;
    max-height: 500px;
    opacity: 1; 
}


.tooltip-container {
    position: relative;
    display: inline-block;
}

.tooltip-text {
    position: absolute;
    top: 30px;
    left: 50%;
    transform: translateX(-50%);
    background-color: black;
    color: white;
    padding: 5px 10px;
    border-radius: 5px;
    font-size: 14px;
    white-space: nowrap;
    visibility: hidden;
    transition: opacity 0.3s ease-in-out;
}

.tooltip-container:hover .tooltip-text {
    visibility: visible;
}

.bi:hover {
    color:azure;
}

.title {
    font-family: serif;
    cursor: context-menu;
}

.navbar-nav .nav-link {
    padding: 0;
}
</style>