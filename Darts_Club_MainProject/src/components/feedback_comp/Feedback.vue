<script setup lang="ts">
import type UserFeedModel from '@/models/UserFeedModel';
import { useMessagesStore } from '@/stores/MessagesStore';
import { onMounted, ref } from 'vue';
import { Modal } from 'bootstrap';
import { useUserStore } from '@/stores/UserStore';

const { user } = useUserStore();
const { sendUserFeed, sendAdminFeed, status } = useMessagesStore();
const processing = ref<boolean>(false);

const modal = ref<HTMLElement>();
let modalInstance: Modal;

onMounted(() => {
    if (modal.value) {
        modalInstance = new Modal(modal.value);
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
    } catch (err) { }
    feedform.value.title = '';
    feedform.value.text = '';
    feedform.value.emailAddress = '';
    processing.value = false;
    modalInstance.show();
}


</script>

<template>
    <div class="background-color-view">
        <div class="row">
            <h1 class="display-5 text-center text-white margin-feed">Hibabejelentés</h1>
        </div>
        <div id="myModal" class="modal fade" tabindex="-1" role="dialog" ref="modal">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="alert text-center"
                            :class="{ 'alert-success': status.success, 'alert-danger': !status.success }"><i
                                class="bi me-3"
                                :class="{ 'bi-check-circle': status.success, 'bi-x-circle': !status.success }"></i>{{
                            status.resp }}</div>
                    </div>
                </div>
            </div>
        </div>
        <form @submit.prevent="onSend()">
            <div class="row">
                <input type="text" id="title" placeholder="Cím..." v-model="feedform.title"
                    class="form-control mx-auto w-50 mt-3">
            </div>
            <div v-if="user.role == 2" class="row">
                <input type="text" id="title" placeholder="Email..." v-model="feedform.emailAddress"
                    class="form-control mx-auto w-50 mt-2">
            </div>
            <div class="row">
                <textarea id="subject" placeholder="Tárgy..." rows="12" v-model="feedform.text"
                    class="form-control mt-2 mx-auto w-50"></textarea>
            </div>
            <div class="row">
                <button type="submit" class="btn btn-warning width-feedback mx-auto mt-4">Elküldés
                    <span v-if="processing" class="spinner-border spinner-border-sm"></span>
                </button>
            </div>
        </form>
    </div>
</template>

<style scoped>
.margin-feed {
    margin-top: 10vh;
}

.width-feedback {
    width: 250px;
}
</style>