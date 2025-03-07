<script setup lang="ts">
import { onMounted, onUnmounted } from "vue";

onMounted(() => {
  document.body.style.overflowY = "auto";
});

onUnmounted(() => {
  document.body.style.overflowY = "hidden";
});

</script>

<template>
    <div class="background_color">
        <div class="row">
            <div class="col-10 mt-5 ms-5">
                <router-link :to="`/competition`" class="no-underline">
                    <div class="box-competition">
                        <div class="col-2">
                            <img src="../../assets/images/feature-world.jpg" alt="világ" class="box-image">
                        </div>
                        <div class="col-6 text-white tops">
                            <h3 class="display-6 ms-5 box-title">Versenyek megtekintése</h3>
                            <p class="fst-italic ms-4 box-description">Jelentkezés versenyekre, eredmények nyomon  követése </p>
                        </div>
                    </div>
                </router-link>
            </div>
        </div>
        <div class="row mt-5">
            <div class="offset-3 col-10">
                <router-link :to="`/leaderboard`" class="no-underline">
                    <div class="box-leaderboard">
                        <div class="col-2 text-container text-white tops">
                            <h3 class="display-6 boxnd-title">Ranglista</h3>
                            <p class="fst-italic ms-4 boxnd-description">Itt tekintheti meg globális szinten elért eredményeket</p>
                        </div>
                        <div class="col-2">
                            <img src="../../assets/images/phone_img.jpg" alt="telefon" class="boxnd-image">
                        </div>
                    </div>
                </router-link>
            </div>
        </div>
        <div class="row">
            <div class="col-10 mt-5 ms-5">
                <router-link :to="`/search-profile`" class="no-underline">
                    <div class="box-search">
                        <div class="col-2">
                            <img src="../../assets/images/laptop_img.jpg" alt="telefon" class="box-image">
                        </div>
                        <div class="col-8 text-white tops">
                            <h3 class="display-6 ms-5 box-title">Profil Keresés</h3>
                            <p class="fst-italic ms-4 box-description">Más játékos adatainak megtekintése</p>
                        </div>
                    </div>
                </router-link>
            </div>
        </div>
        <div class="row mt-5">
            <div class="offset-3 col-10">
                <router-link :to="`/statistic`" class="no-underline">
                    <div class="box-statistic">
                        <div class="col-2 text-container text-dark tops">
                            <h3 class="display-6 boxnd-title">Statisztika</h3>
                            <p class="fst-italic ms-4 boxnd-description">Egyéni adatok, elért eredmények megtekintése</p>
                        </div>
                        <div class="col-2">
                            <img src="../../assets/images/statistic_img.jpg" alt="statisztika" class="box-statistic-image">
                        </div>
                    </div>
                </router-link>
            </div>
        </div>
        <div class="row">
            <div class="col-10 mt-5 ms-5">
                <router-link :to="`/feedback`" class="no-underline">
                    <div class="box-feedback">
                        <div class="col-2">
                            <img src="../../assets/images/feedback_img.jpg" alt="feedback" class="box-image">
                        </div>
                        <div class="col-8 text-dark tops">
                            <h3 class="display-6 ms-5 box-title">Visszajelzés</h3>
                            <p class="fst-italic ms-4 box-description">Adminoknak probléma, hiba jelentése</p>
                        </div>
                    </div>
                </router-link>
            </div>
        </div>
    </div>
</template>

<style scoped></style>