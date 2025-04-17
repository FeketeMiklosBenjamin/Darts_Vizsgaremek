<script setup lang="ts">
import { defineEmits, ref, onBeforeMount } from 'vue';
import type AllUsersModel from '@/models/AllUsersModel';
import { useUserStore } from '@/stores/UserStore';

const { banThisUser, status, alluser } = useUserStore();

onBeforeMount(() => {
    status.message = '';
    updatedUser = alluser.find(u => u.id === props.currentUser.id);
})

const messageSuccess = ref(false);

let updatedUser: AllUsersModel | undefined = undefined;

const banTime = ref<{ bannedDuration: number | null }>(
    { bannedDuration: null }
);

const props = defineProps<{
    currentUser: AllUsersModel;
    visible: boolean;
}>();

const emit = defineEmits(['close']);

const banUser = async () => {
    if (banTime.value.bannedDuration != null) {
        await banThisUser(props.currentUser.id, banTime.value.bannedDuration);
        messageSuccess.value = true;
        if (updatedUser) {
            const milliseconds = banTime.value.bannedDuration * 24 * 60 * 60 * 1000;
            updatedUser.bannedUntil = new Date(Date.now() + milliseconds).toLocaleString(undefined, {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                hour: '2-digit',
                minute: '2-digit'
            });
        }
    } else {
        status.message = 'Nem adott meg adatot!';
    }
}

const unBunUser = async () => {
    banTime.value.bannedDuration = 0;
    await banThisUser(props.currentUser.id, banTime.value.bannedDuration);
    messageSuccess.value = true;

    if (updatedUser) {
        updatedUser.bannedUntil = '';
    }
}

</script>

<template>
    <div class="modal fade show d-block" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content bg-white">
                <div class="modal-header">
                    <h5 class="modal-title">Felhasználó kitiltása</h5>
                    <button type="button" class="btn-close" @click="$emit('close')"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-2">
                        <strong>Felhasználó:</strong> {{ currentUser.username }} {{ updatedUser?.bannedUntil ?
                            `(${updatedUser.bannedUntil})` : '' }}
                    </div>
                    <label>Ban ideje (napban megadva):</label>
                    <div class="input-group">
                        <span class="input-group-text">
                            <i class="ms-1 bi bi-ban"></i>
                        </span>
                        <input type="number" min="1" minlength="1" class="form-control" placeholder=""
                            v-model="banTime.bannedDuration">
                    </div>
                </div>
                <div class="modal-footer d-flex justify-content-center">
                    <div class="col-12 d-flex justify-content-center gap-3">
                        <button type="button" class="btn btn-secondary" v-if="props.currentUser.bannedUntil != ''"
                            @click="unBunUser()">Feloldás</button>
                        <button type="button" class="btn btn-danger text-white" @click="banUser()">Mentés</button>
                    </div>
                    <div v-if="status.message" class="alert text-center py-1"
                        :class="messageSuccess ? 'alert-success' : 'alert-danger'">{{ status.message }}</div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped></style>
