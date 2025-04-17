<script setup lang="ts">
import type UserFeedModel from '@/models/UserFeedModel';
import { useMessagesStore } from '@/stores/MessagesStore';
import { onMounted, ref } from 'vue';
import { Modal } from 'bootstrap';
import { useUserStore } from '@/stores/UserStore';
import type AdminEmailModel from '@/models/AdminEmailModel';

const { user } = useUserStore();
const { sendUserFeed, sendAdminFeed, status } = useMessagesStore();
const processing = ref<boolean>(false);

const modal = ref<HTMLElement>();
let modalInstance: Modal;

onMounted(() => {
    if (modal.value) {
        modalInstance = new Modal(modal.value);
    }
    let currentMessage: AdminEmailModel | string | null;
    currentMessage = sessionStorage.getItem("currentEmail");
    if (currentMessage != undefined) {
        const currentMessageJSON = JSON.parse(currentMessage);
        feedform.value.title = `RE: ${currentMessageJSON.title}`,
        feedform.value.emailAddress = currentMessageJSON.emailAddress,
        sessionStorage.removeItem("currentEmail");
    }
});

const feedform = ref<UserFeedModel>({
    title: '',
    emailAddress: '',
    text: ''
});

async function onSend() {
    processing.value = true;

    await new Promise(resolve => setTimeout(resolve, 100));

    try {
        const accessToken = JSON.parse(sessionStorage.getItem('user') || '{}')?.accessToken;
        if (user.role == 2) {
            await sendAdminFeed(accessToken, feedform.value)
        } else {
            await sendUserFeed(accessToken, feedform.value)
        }

        status.success = true;
    } catch (err) { 
        status.success = false;
    }
    feedform.value.title = '';
    feedform.value.text = '';
    feedform.value.emailAddress = '';
    processing.value = false;
    modalInstance.show();

    setTimeout(() => {
        modalInstance.hide();
    }, 4000);
}


</script>

<template>
    <div class="background-color-view">
        <div class="main-div" style="max-height: 85vh;">
            <div class="row">
                <h1 class="display-5 text-center text-white margin-feed">Hibabejelentés</h1>
            </div>
            <div id="myModal" class="modal fade" tabindex="-1" role="dialog" ref="modal">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="alert text-center" data-cy="response_message"
                                :class="{ 'alert-success': status.success, 'alert-danger': !status.success }"><i
                                    class="bi me-3"
                                    :class="{ 'bi-check-circle': status.success, 'bi-x-circle': !status.success }"></i>{{
                                        status.resp }}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <form @submit.prevent="onSend()">
                <div class="row col-12 col-lg-6 col-md-8 col-sm-10 offset-lg-3 offset-md-2 offset-sm-1 offset-0">
                    <input type="text" id="title" placeholder="Cím..." v-model="feedform.title" class="form-control" data-cy="title_input">
                </div>
                <div v-if="user.role == 2"
                    class="row col-12 col-lg-6 col-md-8 col-sm-10 offset-lg-3 offset-md-2 offset-sm-1 offset-0">
                    <input type="text" id="email" placeholder="Email..." v-model="feedform.emailAddress"
                        class="form-control mt-2" data-cy="email_input">
                </div>
                <div class="row col-12 col-lg-6 col-md-8 col-sm-10 offset-lg-3 offset-md-2 offset-sm-1 offset-0">
                    <textarea id="subject" placeholder="Tárgy..." rows="12" v-model="feedform.text"
                        class="form-control mt-2" data-cy="subject_input"></textarea>
                </div>
                <div class="row col-10 col-lg-4 col-md-6 col-sm-8 offset-lg-4 offset-md-3 offset-sm-2 offset-1">
                    <button type="submit" class="btn btn-warning mt-4" :disabled="processing" data-cy="submit_button">Elküldés
                        <span v-if="processing" class="spinner-border spinner-border-sm"></span>
                    </button>
                </div>
            </form>
        </div>
    </div>
</template>

<style scoped>
.margin-feed {
    margin-top: 10vh;
}
</style>