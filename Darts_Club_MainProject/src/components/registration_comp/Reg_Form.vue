<script setup lang="ts">
import type RegisterModel from '@/models/RegisterModel';
import { useUserStore } from '@/stores/UserStore';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

const { uploadimage, register, status } = useUserStore();
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
        await register(registerform.value);
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
        <div class="container z-1 transform align-items-center glass-card opacity width p-2 px-3">
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
                                v-model="registerform.username" autocomplete="off">
                            <label for="name">Név</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="email" class="form-control" id="email" placeholder="E-mail"
                                v-model="registerform.emailAddress" autocomplete="off">
                            <label for="email">E-mail</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="password" class="form-control" id="password" placeholder="Jelszó"
                                v-model="registerform.password" autocomplete="off">
                            <label for="password">Jelszó</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="password" class="form-control" id="confirm_password" placeholder="Jelszó újra"
                                v-model="registerform.secondPassword" autocomplete="off">
                            <label for="confirm_password">Jelszó újra</label>
                        </div>

                        <div class="mt-2">
                            <input class="form-control" type="file" id="formFile" @change="handleFileChange">
                            <label for="formFile" class="form-label text-white">Profilkép feltöltése (Nem
                                kötelező!)</label>
                        </div>

                        <div class="my-2">
                            <button type="submit" class="btn btn-info w-100 py-2">Regisztráció
                                <span v-if="processing" class="spinner-border spinner-border-sm"></span>
                            </button>
                        </div>
                    </form>
                    <div v-if="status.message" class="alert alert-danger text-center py-1">{{ status.message }}</div>
                </div>
            </div>
        </div>
    </div>

</template>

<style scoped></style>