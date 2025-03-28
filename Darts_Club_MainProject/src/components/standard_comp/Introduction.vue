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
.glass-card-intro {
    background: rgba(40, 38, 38, 0.336);
    border-radius: 16px;
    box-shadow: 0 4px 30px rgba(0, 0, 0, 0.1);
    backdrop-filter: blur(10px);
    -webkit-backdrop-filter: blur(10px);
    border: 1px solid rgba(40, 38, 38, 1);
}

.tops {
    top: 30%;
}
</style>