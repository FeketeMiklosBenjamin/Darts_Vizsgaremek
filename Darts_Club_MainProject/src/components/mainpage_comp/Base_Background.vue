<script setup lang="ts">
import { useUserStore } from "@/stores/UserStore";

const { user } = useUserStore();

</script>

<template>
    <div class="background-color-view">
        <div class="main-div">
            <div class="row">
                <div class="col-10 col-lg-8 mt-5 ms-sm-5 ms-4">
                    <router-link :to="`/competition`" class="no-underline">
                        <div class="box-competition row">
                            <div class="col-12 col-md-6 col-xl-4">
                                <img src="../../assets/images/feature-world.jpg" alt="világ" class="box-image mb-5" />
                            </div>
                            <div class="col-12 col-md-6 text-white text-container">
                                <h3 class="display-6 box-title">Versenyek megtekintése</h3>
                                <p class="fst-italic box-description">
                                    Jelentkezés versenyekre, eredmények nyomon követése
                                </p>
                            </div>
                        </div>
                    </router-link>
                </div>
            </div>
            <div class="row mt-5">
                <div class="ms-auto col-10 col-lg-8 me-sm-5 me-4">
                    <router-link :to="`/leaderboard`" class="no-underline">
                        <div class="box-leaderboard row">
                            <div class="col-12 col-md-6 text-container text-white order-2 order-md-1">
                                <h3 class="display-6 box-title">Ranglista</h3>  
                                <p class="fst-italic box-description">
                                    Itt tekintheti meg globális szinten elért eredményeket
                                </p>
                            </div>
                            <div class="col-8 col-sm-6 col-md-3 col-lg-3 col-xxl-2 order-1 order-md-2 offset-md-0 offset-sm-3 offset-2">
                                <img src="../../assets/images/phone_img.jpg" alt="telefon" class="box-image px-4" />
                            </div>
                        </div>
                    </router-link>
                </div>
            </div>
            <div class="row">
                <div class="col-10 col-lg-8 mt-5 ms-sm-5 ms-4">
                    <router-link :to="`/search-profile`" class="no-underline">
                        <div class="box-search row">
                            <div class="col-12 offset-md-0 offset-sm-3 col-sm-6 col-lg-4">
                                <img src="../../assets/images/laptop_img.jpg" alt="telefon" class="box-image" />
                            </div>
                            <div class="col-12 col-md-6 text-white text-container">
                                <h3 class="display-6 box-title">Profil Keresés</h3>
                                <p class="fst-italic box-description">Más játékos adatainak megtekintése</p>
                            </div>
                        </div>
                    </router-link>
                </div>
            </div>
            <div class="row mt-5">
                <div class="ms-auto col-10 col-lg-8 me-sm-5 me-4">
                    <router-link :to="`/statistic/${user.id}`" class="no-underline">
                        <div class="box-statistic row">
                            <div class="col-12 col-md-6 text-container text-dark order-2 order-md-1">
                                <h3 class="display-6 box-title">Statisztika</h3>
                                <p class="fst-italic box-description">
                                    Egyéni adatok, elért eredmények megtekintése
                                </p>
                            </div>
                            <div class="col-12 col-sm-8 col-md-6 col-lg-6 col-xl-4 order-1 order-md-2 offset-0 offset-sm-2 offset-md-0">
                                <img src="../../assets/images/statistic_img.jpg" alt="statisztika"
                                    class="box-image" />
                            </div>
                        </div>
                    </router-link>
                </div>
            </div>
            <div class="row">
                <div class="col-10 col-lg-8 mt-5 ms-sm-5 ms-4">
                    <router-link :to="`/feedback`" class="no-underline">
                        <div class="box-feedback row">
                            <div class="row">
                                <div class="col-12 offset-md-0 offset-sm-2 col-sm-8 col-md-6 col-lg-6 col-xl-4">
                                    <img src="../../assets/images/feedback_img.jpg" alt="feedback" class="box-image" />
                                </div>
                                <div class="col-12 col-md-6 text-dark text-container">
                                    <h3 class="display-6 box-title">Visszajelzés</h3>
                                    <p class="fst-italic box-description">Adminoknak probléma, hiba jelentése</p>
                                </div>
                            </div>
                        </div>
                    </router-link>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.box-competition {
    min-height: 250px;
    border: 2px solid aqua;
    border-radius: 30px;
    background-color: #070602;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
}

.box-image {
    width: 100%;
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
    min-height: 280px;
    border: 2px solid red;
    border-radius: 30px;
    background-color: #3c2247;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
}

.text-container {
    flex-grow: 1;
    display: flex;
    flex-direction: column;
    justify-content: center;
    text-align: center;
}

.box-search {
    min-height: 280px;
    border: 4px solid #0d4e2a;
    border-radius: 30px;
    background-color: #5abfc7;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
}

.box-statistic {
    min-height: 280px;
    border: 4px solid #e2ad00;
    border-radius: 30px;
    background-color: #f4f4f4;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
}

.box-feedback {
    min-height: 280px;
    border: 4px solid #7f5d96;
    border-radius: 30px;
    background-color: #fdd49d;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
}
</style>