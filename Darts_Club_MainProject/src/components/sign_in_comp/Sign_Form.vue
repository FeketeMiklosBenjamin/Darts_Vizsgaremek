<script setup lang="ts">
import type LoginModel from '@/models/LoginModel';
import { useUserStore } from '@/stores/UserStore';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

const { login, status } = useUserStore();
const router = useRouter();
const processing = ref<boolean>(false);


onMounted(() => {
    status.message = '';
});

const loginform = ref<LoginModel>({
    emailAddress: '',
    password: ''
});

function onLogin(){
    processing.value = true;
    login(loginform.value)
        .then(() => {
            router.push('/main-page');
        })
        .catch((err) => {
            processing.value = false;
        })
}

</script>

<template>
    <div class="position-rel">
        <div class="container z-1 transform align-items-center glass-card width-form p-4">
            <div class="row">
                <div class="col-12 mb-2">
                    <h1 class="text-center display-5 text-light mx-auto">Bejelentkezés</h1>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-12 mx-auto">
                    <form @submit.prevent="onLogin">
                        <div class="form-floating mb-3">
                            <input type="email" class="form-control" id="email" placeholder="E-mail" v-model="loginform.emailAddress" autocomplete="off" data-cy="email_input">
                            <label for="email">E-mail</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="password" class="form-control" id="password" placeholder="Jelszó" v-model="loginform.password" autocomplete="off" data-cy="password_input">
                            <label for="password">Jelszó</label>
                        </div>
                        <div class="mb-3">
                            <button type="submit" class="btn btn-warning w-100 py-2" :disabled="processing" data-cy="sign-in_btn">Bejelentkezés
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