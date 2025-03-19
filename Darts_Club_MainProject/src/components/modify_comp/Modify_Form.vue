<script setup lang="ts">
import type ModifyModel from '@/models/ModifyModel';
import type RegisterModel from '@/models/RegisterModel';
import { useUserStore } from '@/stores/UserStore';
import { computed, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

const { uploadimage, modify, status } = useUserStore();
const router = useRouter();
const processing = ref<boolean>(false);
let IsModifiedData: boolean = true;

const profileImage = ref<File | null>(null);

const isFormModified = computed(() => 
    modifyform.value.username || 
    modifyform.value.emailAddress || 
    modifyform.value.newPassword
);

onMounted(() => {
    status.message = '';
});

const modifyform = ref<ModifyModel>({
    username: '',
    emailAddress: '',
    password: '',
    secondPassword: '',
    newPassword: ''
});

const handleFileChange = (event: Event) => {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput?.files?.length) {
        profileImage.value = fileInput.files[0];
    }
};

async function onModify() {
    status.message = '';
    processing.value = true;

    await new Promise(resolve => setTimeout(resolve, 100));

    if (modifyform.value.password !== modifyform.value.secondPassword) {
        status.message = "A két jelszó nem egyezik meg!";
        processing.value = false;
        return;
    }

    if (!isFormModified.value && !profileImage.value) {
        status.message = "Kötelező legalább egy mezőt módosítani!";
        processing.value = false;
        return;
    }

    const accessToken = JSON.parse(sessionStorage.getItem('user') || '{}')?.accessToken;
    try {
        if (isFormModified.value) {
            await modify(modifyform.value, accessToken);
            router.push('/statistic');
        }
        if (profileImage.value) {
            await uploadimage(profileImage.value, accessToken);
        }
    } catch (err) {
        status.message = "Hiba történt a módosítás során!";
    }
    processing.value = false;
}


</script>

<template>
        <div class="position-rel">
            <div class="container z-1 transform align-items-center glass-card opacity width p-2 px-3 mt-4">
                <div class="row">
                    <div class="col-12 mb-2">
                        <h1 class="text-center display-4 text-light">Módosítás</h1>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12 col-md-12 mx-auto">
                        <form @submit.prevent="onModify">
                            <div class="form-floating mb-3">
                                <input type="text" class="form-control" id="name" placeholder="Név"
                                    v-model="modifyform.username" autocomplete="off">
                                <label for="name">Név</label>
                            </div>

                            <div class="form-floating mb-3">
                                <input type="email" class="form-control" id="email" placeholder="E-mail"
                                    v-model="modifyform.emailAddress" autocomplete="off">
                                <label for="email">E-mail</label>
                            </div>

                            <div class="form-floating mb-3">
                                <input type="password" class="form-control" id="password" placeholder="Jelszó"
                                    v-model="modifyform.password" autocomplete="off">
                                <label for="password">Jelszó</label>
                            </div>

                            <div class="form-floating mb-3">
                                <input type="password" class="form-control" id="confirm_password"
                                    placeholder="Jelszó újra" v-model="modifyform.secondPassword" autocomplete="off">
                                <label for="confirm_password">Jelszó újra</label>
                            </div>

                            <div class="form-floating mb-3">
                                <input type="password" class="form-control" id="new_password" placeholder="Új jelszó"
                                    v-model="modifyform.newPassword" autocomplete="off">
                                <label for="new_password">Új jelszó</label>
                            </div>

                            <div class="mt-2">
                                <input class="form-control" type="file" id="formFile" @change="handleFileChange">
                                <label for="formFile" class="form-label text-white">Profilkép módosítása (Nem
                                    kötelező!)</label>
                            </div>

                            <div class="my-2">
                                <button type="submit" class="btn btn-green w-100 py-2">Módosít
                                    <span v-if="processing" class="spinner-border spinner-border-sm"></span>
                                </button>
                            </div>
                        </form>
                        <div v-if="status.message" class="alert alert-danger text-center py-1">{{ status.message }}
                        </div>
                    </div>
                </div>
            </div>
        </div>
</template>

<style scoped></style>