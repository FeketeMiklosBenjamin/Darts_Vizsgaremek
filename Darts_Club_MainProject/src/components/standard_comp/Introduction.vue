<script setup lang="ts">

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
                    <router-link :to="`/registration`">
                        <button class="btn btn-info opacity-75">Regisztráció</button>
                    </router-link>
                </div>
                <div class="col-2">
                    <router-link :to="`/sign-in`">
                        <button class="btn btn-dark">Bejelentkezés</button>
                    </router-link>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
</style>