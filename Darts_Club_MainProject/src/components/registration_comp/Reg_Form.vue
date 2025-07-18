<script setup lang="ts">
import type RegisterModel from '@/models/RegisterModel';
import { useUserStore } from '@/stores/UserStore';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

const { uploadimage, registerUser, status } = useUserStore();
const router = useRouter();
const processing = ref<boolean>(false);

const profileImage = ref<File | null>(null);

onMounted(() => {
    status.message = '';
});

const registerform = ref<RegisterModel>({
    username: '',
    emailAddress: '',
    password: '',
    secondPassword: ''
});

const handleFileChange = (event: Event) => {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput?.files?.length) {
        profileImage.value = fileInput.files[0];
    }
};



async function onRegister() {
    status.message = '';
    processing.value = true;
    
    await new Promise(resolve => setTimeout(resolve, 100));
    
    if (registerform.value.password !== registerform.value.secondPassword) {
        status.message = "A két jelszó nem egyezik meg!";
        processing.value = false;
        return;
    }
    
    try {
        await registerUser(registerform.value);
        const accessToken = JSON.parse(sessionStorage.getItem('user') || '{}')?.accessToken;
      
        if (profileImage.value) {
            await uploadimage(profileImage.value, accessToken);
        }

        router.push('/main-page');
    } catch (err) {
        processing.value = false;
    }
}


</script>

<template>
    <div class="position-rel">
        <div class="container mt-4 z-1 transform align-items-center glass-card opacity width-form p-2 px-3">
            <div class="row">
                <div class="col-12 mb-2">
                    <h1 class="text-center display-4 text-light">Regisztráció</h1>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-12 mx-auto">
                    <form @submit.prevent="onRegister">
                        <div class="form-floating mb-3">
                            <input type="text" class="form-control" id="name" placeholder="Név"
                                v-model="registerform.username" autocomplete="off" data-cy="username_input">
                            <label for="name">Felhasználónév</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="email" class="form-control" id="email" placeholder="E-mail"
                                v-model="registerform.emailAddress" autocomplete="off" data-cy="email_input">
                            <label for="email">E-mail</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="password" class="form-control" id="password" placeholder="Jelszó"
                                v-model="registerform.password" autocomplete="off" data-cy="password_input">
                            <label for="password">Jelszó</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="password" class="form-control" id="confirm_password" placeholder="Jelszó újra"
                                v-model="registerform.secondPassword" autocomplete="off" data-cy="SecondPassword_input">
                            <label for="confirm_password">Jelszó újra</label>
                        </div>

                        <div class="mt-2">
                            <input class="form-control" type="file" id="formFile" @change="handleFileChange" data-cy="image_upload">
                            <label for="formFile" class="form-label text-white">Profilkép feltöltése (Nem
                                kötelező!)</label>
                        </div>

                        <div class="my-2">
                            <button type="submit" class="btn btn-info w-100 py-2" :disabled="processing" data-cy="registration_btn">Regisztráció
                                <span v-if="processing" class="spinner-border spinner-border-sm" data-cy="loading_spinner"></span>
                            </button>
                        </div>
                    </form>
                    <div v-if="status.message" class="alert alert-danger text-center py-1" data-cy="error_message">{{ status.message }}</div>
                </div>
            </div>
        </div>
    </div>

</template>

<style scoped></style>