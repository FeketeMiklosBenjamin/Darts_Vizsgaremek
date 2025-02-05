<script setup lang="ts">
import { computed } from 'vue';

const controlls = computed(()=>{
  return [
        {
            title: 'Regisztráció',
            to: '/registration',
        },
        {
            title: 'Bejelentkezés',
            to: '/sign-in',
        },
    ]   
});
</script>

<template>
    <div class="z-1 position-rel">
        <div class="w-50 m-auto my-5 px-0 glass-card-intro text-center transform tops">
            <div class="row">
                <p class="display-6 mt-2">Üdvözöljük weboldalunkon!</p>
            </div>
            <div class="row">
                <p>Ez egy Darts tematikájú oldal. Lényege, hogy megismerkedjen oldalunkkal és tetszen Önnek!<br>Elsősorban kérem tegye meg a következőket:</p>
            </div>
            <div class="row ms-5">
                <ul class="ms-5">
                    <div class="me-5 text-start my-3">
                        <li>Ha van már fiókja akkor jelentkezzen be és nézze meg rég nem látott profilját!</li>
                        <li>Ha nincsen fiókja, regisztráljon most további tartalmakért!</li>
                    </div>
                </ul>
            </div>
            <div class="row mb-2 mx-0">
                <div class="col-2 offset-4">
                    <router-link :to="controlls[0].to">
                        <button class="btn btn-info">Regisztráció</button>
                    </router-link>
                </div>
                <div class="col-2">
                    <router-link :to="controlls[1].to">
                        <button class="btn btn-dark">Bejelentkezés</button>
                    </router-link>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
</style>