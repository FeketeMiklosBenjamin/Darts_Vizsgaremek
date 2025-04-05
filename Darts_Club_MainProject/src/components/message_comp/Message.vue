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
    <div class="background-color-view">
        <div class="main-div" style="max-height: 95vh;">
            <div class="email-box col-12 col-md-6 col-lg-4 col-sm-10 offset-sm-1 offset-lg-4 offset-md-3 mb-3">
                <div class="row">
                    <div class="col-1">
                        <div class="text-center text-success mt-3 fs-5">
                            <i class="bi bi-arrow-return-left" @click="NavigateToMain"></i>
                        </div>
                    </div>
                    <div class="col-10">
                        <div class="d-flex justify-content-center text-center mt-2 display-5 fw-bold caveat-text">
                            {{ currentEmail?.title }}
                        </div>
                    </div>
                    <div class="col-1">
                        <div class="text-end mt-3 text-danger fs-5" @click="deleteMessage(currentEmail?.id!)">X
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="text-center mt-5 caveat-text fs-2">
                        {{ currentEmail?.text }}
                    </div>
                </div>
                <div v-if="user.role == 2" class="row">
                    <div class="text-end me-5 caveat-text fs-4 mt-3">{{ currentEmail?.emailAddress }}</div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Caveat:wght@400..700&display=swap');
.caveat-text {
    font-family: "Caveat", cursive;
    font-optical-sizing: auto;
    font-weight: 400;
    font-style: normal;
}

.bi {
    cursor: pointer;
}

.text-danger {
    cursor: pointer;
}

.display-6 {
    font-size: 2em;
}

.email-box {
    max-height: 100%;
    overflow-y: auto;
    background-color: white;
    border: 2px solid black;
    box-shadow: 10px 10px 2px 1px rgba(15, 15, 15, 0.877);
    border-radius: 5px;
    margin-top: 10vh;
    padding: 2rem;
    scrollbar-width: none;
    -ms-overflow-style: none;
}

.email-box::-webkit-scrollbar {
    display: none;
}
</style>