<script setup lang="ts">
import type AdminEmailModel from '@/models/AdminEmailModel';
import type UserFeedModel from '@/models/UserFeedModel';
import router from '@/router';
import { useMessagesStore } from '@/stores/MessagesStore';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
const { getYourMessages, deleteMyMessages } = useMessagesStore();
import { ref } from 'vue';

const { forAdminEmails, forUserEmails} = storeToRefs(useMessagesStore());
const { user } = useUserStore();
const emailId = sessionStorage.getItem("emailId");

let currentEmail = ref<AdminEmailModel | UserFeedModel | null>(null);

if (user.role == 2) 
    currentEmail.value = forAdminEmails.value.filter(x => x.id == emailId)[0];
else 
    currentEmail.value = forUserEmails.value.filter(x => x.id == emailId)[0];

const deleteMessage = async(id: string) => {
    console.log("Messagedeleted: "+ currentEmail.value?.id );
    if (user.role == 2) 
        forAdminEmails.value.filter(x=> x.id != currentEmail.value?.id)
    else 
        forUserEmails.value.filter(x=> x.id != currentEmail.value?.id)
    
    router.push("/main-page")
}
</script>

<template>
    <div class="background-color-view d-flex">


        <div class="email-box mx-auto">
            <div class="trash" @click="deleteMessage(currentEmail?.id!)">X</div>
    
        
            <div class="row ">
                <div class="text-center mt-2 display-6 fw-bold">
                    {{ currentEmail?.title }}
                </div>
            </div>
            <div class="row text">
                {{ currentEmail?.text }}
            </div>
        </div>
    </div>
</template>

<style scoped>
.display-6 {
    font-size: 2em;
}
.trash {
    position: absolute;
    top: 10px;
    right: 10px;
    border-radius: 50%;
    height: 35px;
    width: 35px;
    background-color: #ff4d4d;
    color: white;
    text-align: center;
    display: flex;
    justify-content: center;
    align-items: center;
    font-weight: bold;
    font-size: 1.2em;
    cursor: pointer;
    transition: background-color 0.3s ease, transform 0.2s ease;
}

.trash:hover {
    background-color: #e60000;
    transform: scale(1.1);
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
.text{
    padding: 15px; 
}
</style>
