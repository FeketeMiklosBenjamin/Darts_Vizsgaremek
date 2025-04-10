<script setup lang="ts">
import type ModifyModel from '@/models/ModifyModel';
import { useUserStore } from '@/stores/UserStore';
import { computed, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

const { uploadimage, modify, status } = useUserStore();
const router = useRouter();
const processing = ref<boolean>(false);

const profileImage = ref<File | null>(null);

const isFormModified = computed(() =>
    modifyform.value.username ||
    modifyform.value.emailAddress ||
    modifyform.value.oldPassword
);

onMounted(async () => {
    status.message = '';
});

const modifyform = ref<ModifyModel>({
    username: '',
    emailAddress: '',
    newPassword: '',
    newSecondPassword: '',
    oldPassword: ''
});

const handleFileChange = (event: Event) => {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput?.files?.length) {
        profileImage.value = fileInput.files[0];
    }
};

let modificationSuccess = false;

async function onModify() {
    console.log("ok");
    status.message = '';
    processing.value = true;
    modificationSuccess = false;

    if (!isFormModified.value && !profileImage.value) {
        status.message = "Kötelező legalább egy mezőt módosítani!";
        processing.value = false;
        return;
    }

    if (modifyform.value.oldPassword == "") {
        status.message = "A régi jelszót kötelező megadni!";
        processing.value = false;
        return;
    }

    if (modifyform.value.newPassword !== modifyform.value.newSecondPassword) {
        status.message = "A két jelszó nem egyezik meg!";
        processing.value = false;
        return;
    }

    const accessToken = JSON.parse(sessionStorage.getItem('user') || '{}')?.accessToken;
    console.log(accessToken);

    try {
        if (isFormModified.value) {
            await modify(modifyform.value, accessToken);
        }
        if (profileImage.value) {
            await uploadimage(profileImage.value, accessToken);
        }
        modificationSuccess = true;
        setTimeout(() => {
            router.push('/main-page');
        }, 3000);
    } catch (err) {
        if (status.message == "") {
            status.message = "Hiba történt a módosítás során!";
        }
    }
    processing.value = false;
}


</script>

<template>
    <div class="background-color-view">
        <div id="myModal" class="modal fade" tabindex="-1" role="dialog" ref="modal">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="alert alert-success text-center"><i class="bi bi-check-circle me-3"></i>
                            {{ status.message }}</div>
                    </div>
                </div>
            </div>
        </div>
        <div class="position-rel main-div" style="max-height: 100vh;">
            <div class="container z-1 transform align-items-center glass-card opacity width-form p-2 px-3 mt-5">
                <div class="row">
                    <div class="col-12 mb-2">
                        <h1 class="text-center display-4 text-light">Módosítás</h1>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12 col-md-12 mx-auto mb-3">
                        <form @submit.prevent="onModify">
                            <div class="form-floating mb-3">
                                <input type="text" class="form-control" id="name" placeholder="Név"
                                    v-model="modifyform.username" autocomplete="off">
                                <label for="name">Felhasználónév</label>
                            </div>

                            <div class="form-floating mb-3">
                                <input type="email" class="form-control" id="email" placeholder="E-mail"
                                    v-model="modifyform.emailAddress" autocomplete="off">
                                <label for="email">E-mail</label>
                            </div>

                            <div class="form-floating mb-3">
                                <input type="password" class="form-control" id="new_password" placeholder="Új jelszó"
                                    v-model="modifyform.newPassword" autocomplete="off">
                                <label for="new_password">Új jelszó</label>
                            </div>

                            <div class="form-floating mb-3">
                                <input type="password" class="form-control" id="confirm_password"
                                    placeholder="Jelszó újra" v-model="modifyform.newSecondPassword" autocomplete="off">
                                <label for="confirm_password">Új jelszó újra</label>
                            </div>

                            <div class="form-floating mb-3">
                                <input type="password" class="form-control" id="password" placeholder="Jelszó"
                                    v-model="modifyform.oldPassword" autocomplete="off">
                                <label for="password">Régi jelszó</label>
                            </div>

                            <div class="mt-2">
                                <input class="form-control" type="file" id="formFile" @change="handleFileChange">
                                <label for="formFile" class="form-label text-white">Profilkép módosítása (Nem
                                    kötelező!)</label>
                            </div>

                            <div class="my-2">
                                <button type="submit" class="btn btn-success w-100 py-2" :disabled="processing">Módosít
                                    <span v-if="processing" class="spinner-border spinner-border-sm"></span>
                                </button>
                            </div>
                        </form>
                        <div v-if="status.message" class="alert text-center py-1"
                            :class="modificationSuccess ? 'alert-success' : 'alert-danger'">{{ status.message }}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped></style>