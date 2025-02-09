<script setup lang="ts">
import type RegisterModel from '@/models/RegisterModel';
import { useUserStore } from '@/stores/UserStore';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const { register } = useUserStore();
const router = useRouter();

const registerform = ref<RegisterModel>({
    username: '',
    emailAddress: '',
    password: '',
});

function onRegister() {
    if (registerform.value.password === registerform.value.secondPassword) {
        register(registerform.value)
        .then(() => {router.push('/main-page')})
    }
}


</script>

<template>
    <div class="position-rel">
        <div class="container z-1 transform align-items-center glass-card width p-2 px-3">
            <div class="row">
                <div class="col-12  mb-2">
                    <h1 class="text-center display-4 text-light">Regisztráció</h1>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-12 mx-auto">
                    <form @submit.prevent="onRegister">
                        <div class="form-floating mb-3">
                            <input type="text" class="form-control" id="name" placeholder="Név" v-model="registerform.username">
                            <label for="name">Név</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="email" class="form-control" id="email" placeholder="E-mail" v-model="registerform.emailAddress">
                            <label for="email">E-mail</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="password" class="form-control" id="password" placeholder="Jelszó" v-model="registerform.password">
                            <label for="password">Jelszó</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="password" class="form-control" id="confirm_password" placeholder="Jelszó újra" v-model="registerform.secondPassword">
                            <label for="confirm_password">Jelszó újra</label>
                        </div>

                        <div class="mb-3">
                            <button type="submit" class="btn btn-info w-100 py-2">Regisztráció</button>
                        </div>

                    </form>
                </div>
            </div>
        </div>
    </div>

</template>

<style scoped></style>