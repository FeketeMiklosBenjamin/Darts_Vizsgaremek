<script setup lang="ts">

</script>

<template>
    <div class="z-1 position-rel">
        <div class="col-12 col-sm-10 col-lg-6 col-md-8 mx-auto px-0 glass-card-intro text-center text-light">
            <div class="row">
                <p class="display-6 mt-2">Üdvözöljük weboldalunkon!</p>
            </div>
            <div class="row">
                <p>Ez egy Darts tematikájú oldal. Lényege, hogy megismerkedjen oldalunkkal és tetszen
                    Önnek!<br>Elsősorban kérem tegye meg a következőket:</p>
            </div>
            <div class="row ms-5">
                <ul class="ms-3 ms-md-5">
                    <div class="me-5 text-start my-3">
                        <li>Ha van már fiókja akkor jelentkezzen be és nézze meg rég nem látott profilját!</li>
                        <li>Ha nincsen fiókja, regisztráljon most további tartalmakért!</li>
                    </div>
                </ul>
            </div>
            <div class="row mb-2 mx-0">
                <div class="row mb-2 mx-0 px-4">
                    <div class="col-12 col-md-6 col-lg-4 col-xl-3 offset-xl-3 col-xxl-3 offset-xxl-3 offset-lg-2 mb-2">
                        <router-link :to="`/registration`">
                            <button class="btn btn-info opacity-75 w-100">Regisztráció</button>
                        </router-link>
                    </div>
                    <div class="col-12 col-md-6 col-lg-4 col-xl-3 col-xxl-3">
                        <router-link :to="`/sign-in`">
                            <button class="btn btn-dark w-100">Bejelentkezés</button>
                        </router-link>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.glass-card-intro {
    background: rgba(40, 38, 38, 0.636);
    border-radius: 16px;
    box-shadow: 0 4px 30px rgba(0, 0, 0, 0.1);
    backdrop-filter: blur(10px);
    -webkit-backdrop-filter: blur(10px);
    border: 1px solid rgba(40, 38, 38, 1);
    margin-top: 9vw;
    overflow-y: auto;
    overflow-x: hidden;
    max-height: 80vh;
}
.glass-card-intro {
    scrollbar-width: none;
    -ms-overflow-style: none;
}

.glass-card-intro::-webkit-scrollbar {
    display: none;
}

.display-6 {
    font-weight: 500;
}

</style>
