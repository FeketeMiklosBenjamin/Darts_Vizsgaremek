<script setup lang="ts">
import { useUserStore } from "@/stores/UserStore";
import { onMounted, onUnmounted } from "vue";

const { user } = useUserStore();

onMounted(() => {
  document.body.style.overflowY = "auto";
});

onUnmounted(() => {
  document.body.style.overflowY = "hidden";
});

</script>

<template>
    <div class="background-color-view">
        <div class="row">
            <div class="col-10 mt-5 ms-5">
                <router-link :to="`/competition`" class="no-underline">
                    <div class="box-competition">
                        <div class="col-2">
                            <img src="../../assets/images/feature-world.jpg" alt="világ" class="box-image">
                        </div>
                        <div class="col-6 text-white tops">
                            <h3 class="display-6 ms-5 box-title">Versenyek</h3>
                            <p v-if="user.role == 2" class="fst-italic ms-4 box-description">Versenyek létrehozása megadott dátummal</p>
                            <p v-else class="fst-italic ms-4 box-description">Jelentkezés versenyekre, eredmények nyomon  követése</p>
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
                <router-link :to="`/statistic/${user.id}`" class="no-underline">
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

<style scoped>
.box-competition {
    width: 85%;
    height: 250px;
    border: 2px solid aqua;
    border-radius: 30px;
    background-color: #070602;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
    gap: 300px;
}

.box-image {
    margin-top: 25px;
    margin-left: 50px;
    height: 200px;
}

.box-title {
    font-size: 40px;
    margin-bottom: 10px;
}

.box-description {
    font-size: 18px;
}

.box-leaderboard {
    width: 85%;
    height: 250px;
    border: 2px solid red;
    border-radius: 30px;
    background-color: #3C2247;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
    gap: 50px;
}

.boxnd-image {
    height: 200px;
}

.text-container {
    flex-grow: 1;
    display: flex;
    flex-direction: column;
    justify-content: center;
    text-align: left;
    padding-left: 100px;
}

.boxnd-title {
    font-size: 40px;
    margin-bottom: 10px;
    margin-left: 150px;
}

.boxnd-description {
    font-size: 18px;
}

.box-search {
    width: 85%;
    height: 250px;
    border: 4px solid #0D4E2A;
    border-radius: 30px;
    background-color: #5ABFC7;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
    gap: 300px;
}


.box-statistic {
    width: 85%;
    height: 250px;
    border: 4px solid #E2AD00;
    border-radius: 30px;
    background-color: #F4F4F4;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
}

.box-statistic-image {
    height: 200px;
    margin-left: -200px;
}

.box-feedback {
    width: 85%;
    height: 250px;
    border: 4px solid #7F5D96;
    border-radius: 30px;
    background-color: #FDD49D;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
    gap: 300px;
}

</style>