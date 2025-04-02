<script setup lang="ts">
import type AdminEmailModel from '@/models/AdminEmailModel';
import type UserFeedModel from '@/models/UserFeedModel';
import router from '@/router';
import { useMessagesStore } from '@/stores/MessagesStore';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, watch } from 'vue';

const { deleteMyMessages } = useMessagesStore();
const { forAdminEmails, forUserEmails } = storeToRefs(useMessagesStore());
const { user } = useUserStore();
const emailId = sessionStorage.getItem("emailId")
let currentEmail = ref<AdminEmailModel | UserFeedModel>();

const getCurrentEmail = () => {
    if (emailId) {
        if (user.role == 2) {
            return forAdminEmails.value.find(x => x.id == emailId);
        } else {
            return forUserEmails.value.find(x => x.id == emailId);
        }
    }
};

watch([forAdminEmails, forUserEmails], () => {
    currentEmail.value = getCurrentEmail();
    if (currentEmail.value) {
        sessionStorage.setItem('currentEmail', JSON.stringify(currentEmail.value));
    }
}, { immediate: true });

onMounted(() => {
    const savedEmail = sessionStorage.getItem("currentEmail");
    if (savedEmail) {
        currentEmail.value = JSON.parse(savedEmail);
    } else {
        currentEmail.value = getCurrentEmail();
    }
});

const NavigateToMain = async () => {
    sessionStorage.removeItem("currentEmail");
    sessionStorage.removeItem("emailId");    
    router.push("main-page");
} 

const deleteMessage = async (id: string) => {
    await deleteMyMessages(id, user.accessToken);
    NavigateToMain();
}
</script>

<template>
    <link href="https://fonts.googleapis.com/css2?family=Tangerine:wght@400;700&display=swap" rel="stylesheet">
    <div class="background-color-view d-flex">
        <div class="email-box mx-auto">
            <div class="row ">
                <div class="col-md-4">
                    <div class="text-center text-success mt-3 me-5 fs-5">
                        <i class="bi bi-arrow-return-left" @click="NavigateToMain"></i>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="text-center mt-2 display-5 fw-bold font">
                        {{ currentEmail?.title }}
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="text-end me-4 mt-2 text-danger fs-4" @click="deleteMessage(currentEmail?.id!)">X</div>
                </div>
            </div>
            <div class="row">
                <div class="text-center mt-5 font fs-2">
                    {{ currentEmail?.text }}
                </div>
            </div>
            <div v-if="user.role == 2" class="row">
                <div class="text-end me-5 font fs-4 email">{{ currentEmail?.emailAddress }}</div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.bi {
    cursor: pointer;
}

.font {
    font-family: 'Tangerine', cursive;
    font-weight: 700;
}

.text-danger {
    cursor: pointer;
    text-shadow: 4px 4px 5px rgba(0, 0, 0, 0.5);
}

.display-6 {
    font-size: 2em;
}

.email {
    position: absolute;
    bottom: 5%;
    right: 0;
}

.email-box {
    position: relative;
    width: 40vw;
    height: 70vh;
    background-color: white;
    border: 2px solid black;
    box-shadow: 10px 10px 2px 1px rgba(15, 15, 15, 0.877);
    border-radius: 5px;
    margin-top: 15vh;
}
</style>
