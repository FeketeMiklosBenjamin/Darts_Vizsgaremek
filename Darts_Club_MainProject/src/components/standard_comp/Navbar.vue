<script setup lang="ts">
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { onBeforeMount } from 'vue';
import { useRouter } from 'vue-router';

const {status, user} = storeToRefs(useUserStore());
const { logout } = useUserStore();
const router = useRouter();

function onLogout() {
    logout()
        .then(() => {
                status.value._id = '';
                router.push('/')}
            )
        .catch((err) => {
            status.value._id = '';
                router.push('/');
        })
}

</script>


<template>
    <div class="shadow-lg">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark py-2">
            <div class="container">
                <a class="navbar-brand"><em class="display-6 title">Dart's Club</em></a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ms-auto py-2"> 
                        <li class="nav-item">
                            <div v-if="status._id">
                                <a class="nav-link">{{ user.username }}</a>
                            </div>
                            <div v-else>
                                <router-link :to="`/sign-in`" class="no-underline">
                                    <a class="nav-link" href="#">Bejelentkezés</a>
                                </router-link>
                            </div>
                        </li> 
                        <div v-if="status._id" class="ms-1 m-auto px-1">
                            <li class="nav-item bg-secondary rounded-circle px-1 border border-danger text-white">
                                <i class="bi bi-person"></i>
                            </li>                
                        </div>
                        <div v-else class="ms-1 m-auto px-1">
                            <li class="nav-item bg-white rounded-circle px-1 border border-warning">
                                <i class="bi bi-person"></i>
                            </li>                
                        </div>
                        <div v-if="status._id" class="my-auto ms-3">
                            <li class="nav-item text-secondary my-auto">
                                <i class="bi bi-gear-fill"></i>
                            </li>                            
                        </div>
                        <div v-else class="my-auto ms-3">
                            <li class="nav-item text-white my-auto">
                                <i class="bi bi-gear-fill"></i>
                            </li>                                         
                        </div>
                        <div v-if="status._id" class="my-auto ms-4">
                            <li class="nav-item my-auto">
                                <a href="#" @click="onLogout" class="text-secondary"><i class="bi bi-box-arrow-right"></i></a>
                            </li>                            
                        </div>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
</template>


<style  scoped></style>