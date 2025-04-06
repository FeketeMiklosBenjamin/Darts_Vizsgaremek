
<script setup lang="ts">
import { useUserStore } from "@/stores/UserStore";

const { user } = useUserStore();

</script>

<template>
    <div class="background-main-img">
        <div class="main-div">
            <div class="row">
                <div class="col-10 col-lg-8 mt-5 ms-sm-5 ms-4">
                    <router-link :to="`/competition`" class="no-underline">
                        <div class="box-left row">
                            <div class="col-12 col-md-6 col-xl-4 d-flex justify-content-center">
                                <img src="../../assets/images/earth.png" alt="világ" class="box-image"/>
                            </div>
                            <div class="col-12 col-md-6 text-light text-container">
                                <div v-if="user.role != 2">
                                    <h3 class="display-6 box-title">Versenyek Megtekintése</h3>
                                    <p class="fst-italic box-description">
                                        Jelentkezés versenyekre, eredmények nyomon követése
                                    </p>
                                </div>
                                <div v-else>
                                    <h3 class="display-6 box-title">Versenyek Létrehozása</h3>
                                    <p class="fst-italic box-description">
                                        Új versenyek beállítása, létrehozása 
                                    </p>
                                </div>
                            </div>
                        </div>
                    </router-link>
                </div>
            </div>
            <div class="row mt-5">
                <div class="ms-auto col-10 col-lg-8 me-sm-5 me-4">
                    <router-link :to="`/leaderboard`" class="no-underline">
                        <div class="box-right row">
                            <div class="col-12 col-md-6 text-container text-dark order-2 order-md-1">
                                <h3 class="display-6 box-title">Ranglista</h3>  
                                <p class="fst-italic box-description">
                                    Itt tekintheti meg globális szinten elért eredményeket
                                </p>
                            </div>
                            <div class="col-10 col-sm-8 col-md-4 col-lg-4 order-1 order-md-2 offset-md-0 offset-sm-2 offset-1 d-flex justify-content-center align-center">
                                <img src="../../assets/images/leaderboard-icon.png" alt="telefon" class="box-image" />
                            </div>
                        </div>
                    </router-link>
                </div>
            </div>
            <div class="row">
                <div class="col-10 col-lg-8 mt-5 ms-sm-5 ms-4">
                    <router-link :to="`/search-profile`" class="no-underline">
                        <div class="box-left row">
                            <div class="col-12 offset-md-0 offset-sm-3 col-sm-6 col-lg-4 d-flex justify-content-center align-center">
                                <img src="../../assets/images/search-icon.png" alt="telefon" class="box-image"/>
                            </div>
                            <div class="col-12 col-md-6 mx-auto text-light text-container">
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
                        <div class="box-right row">
                            <div class="col-12 col-md-6 text-container text-dark order-2 order-md-1">
                                <div v-if="user.role != 2">
                                    <h3 class="display-6 box-title">Statisztika</h3>
                                    <p class="fst-italic box-description">
                                        Egyéni adatok, elért eredmények megtekintése
                                    </p>
                                </div>
                                <div v-else>
                                    <h3 class="display-6 box-title">Admin Regisztrácó</h3>
                                    <p class="fst-italic box-description">
                                        Új admin profil létrehozása 
                                    </p>
                                </div>
                            </div>
                            <div class="col-12 col-sm-8 col-md-6 col-lg-4 order-1 order-md-2 offset-0 offset-sm-2 offset-md-0 d-flex justify-content-center align-center">
                                <img src="../../assets/images/stat-icon.png" alt="statisztika"
                                    class="box-image" style="height: 150px;"/>
                            </div>
                        </div>
                    </router-link>
                </div>
            </div>
            <div class="row">
                <div class="col-10 col-lg-8 mt-5 ms-sm-5 ms-4">
                    <router-link :to="`/feedback`" class="no-underline">
                        <div class="box-left row">
                            <div class="row">
                                <div class="col-12 offset-md-0 offset-sm-2 col-sm-8 col-md-6 col-lg-6 col-xl-4 d-flex justify-content-center align-center">
                                    <img src="../../assets/images/feedback-icon.png" alt="feedback" class="box-image" style="height: 150px;"/>
                                </div>
                                <div class="col-12 col-md-6 text-light text-container">
                                    <div v-if="user.role != 2">
                                        <h3 class="display-6 box-title">Hibabejelentés</h3>
                                        <p class="fst-italic box-description">Adminoknak probléma, hiba jelentése</p>
                                    </div>
                                    <div v-else>
                                        <h3 class="display-6 box-title">Visszajelzés</h3>
                                        <p class="fst-italic box-description">Felhasználóknak probléma, hiba megoldása</p>
                                    </div>
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
.box-left {
    min-height: 250px;
    border: 4px solid rgb(143, 46, 46);
    border-radius: 30px;
    background-color: black;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
}

.box-right {
    min-height: 250px;
    border: 4px solid rgb(46, 143, 46);
    border-radius: 30px;
    background-color: white;
    position: relative;
    display: flex;
    align-items: center;
    padding: 20px;
}

.box-image {
    height: 200px;
}

.box-title {
    font-size: 40px;
    margin-bottom: 10px;
}

.box-description {
    font-size: 18px;
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





