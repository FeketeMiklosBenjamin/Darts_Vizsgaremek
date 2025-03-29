<script setup lang="ts">
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { computed, onBeforeMount } from 'vue';

const { stats } = storeToRefs(useUserStore());

const { getYourStat } = useUserStore();

onBeforeMount(() => {
  getYourStat();
});

const levelColors = {
  Amateur: 'success',
  Advanced: 'warning',
  Professional: 'danger'
};

const levelPoints = {
  Amateur: 1500,
  Advanced: 4500,
  Professional: 9000
};


const ownLevelColor = computed(() => `text-${levelColors[stats.value.level as keyof typeof levelColors]}`);
const progressBarColor = computed(() => `bg-${levelColors[stats.value.level as keyof typeof levelColors]}`);

const maxPoints = computed(() => levelPoints[stats.value.level as keyof typeof levelPoints]);
const progressWidth = computed(() => `${(stats.value.dartsPoints / maxPoints.value) * 100}%`);


</script>

<template>
  <div class="background-color-view">
    <div class="row">
      <div class="col-4 mt-5">
        <div class="bg-secondary rounded-circle border border-5 statistic-img mx-auto" :class="`border-${levelColors[stats.level as keyof typeof levelColors]}`">
          <img :src=stats.profilePictureUrl class="statistic-profileImg d-block" alt="Nincs">
        </div>
        <h1 class="display-6 text-white margin-statname text-center">{{ stats.username }}</h1>
        <div class="col-5">
          <router-link :to="`/modify`">
            <button class="btn btn-warning px-1 marginst-btn">
              Módosítás
            </button>
          </router-link>
        </div>
      </div>
      <div class="col-7">
        <table class="table table-dark table-bordered stat-table">
          <tbody>
            <tr>
              <td colspan="2" class="med"><span class="display-6">Mérkőzések száma:</span><br>{{ stats.matches }}</td>
              <td colspan="2" class="med"><span class="display-6">Győzelmek:</span><br>{{ stats.matchesWon }}</td>
            </tr>
            <tr>
              <td colspan="4" class="med"><span class="display-6">Megnyert versenyek száma:</span> {{
                stats.tournamentsWon }} db</td>
            </tr>
            <tr>
              <td colspan="2"><span class="display-5">Setek száma:</span> {{ stats.sets }} db</td>
              <td colspan="2"><span class="display-5">Megnyert Setek:</span> {{ stats.setsWon }} db</td>
            </tr>
            <tr>
              <td colspan="2"><span class="display-5">Legek száma:</span> {{ stats.legs }} db</td>
              <td colspan="2"><span class="display-5">Megnyert Legek:</span> {{ stats.legsWon }} db</td>
            </tr>
            <tr>
              <td colspan="2"><span class="display-5">Átlag:</span> {{ stats.averages }}</td>
              <td colspan="2"><span class="display-5">Kiszállózási %-ék:</span> {{ stats.checkoutPercentage }}%</td>
            </tr>
            <tr>
              <td><span class="display-5">180-asok száma:</span><br>{{ stats.max180s }} db</td>
              <td colspan="2"><span class="display-5">Legmagasabb kiszálló:</span><br>{{ stats.highestCheckout }}</td>
              <td><span class="display-5">Kilenc nyilasok száma:</span><br>{{ stats.nineDarter }} db</td>
            </tr>
            <tr>
              <td colspan="2" rowspan="2" class="med"><span class="display-6">Regisztrálás dátuma:</span> {{
                stats.registerDate }}</td>
              <td colspan="2" class="med"><span class="display-6">Utoljára bejelentkezve:</span>{{ stats.lastLoginDate
              }}</td>
            </tr>
          </tbody>
        </table>
        <div class="progress-container">
          <div class="progress-label progress-label-left">Szint: <span :class="ownLevelColor">{{ stats.level }}</span></div>
          <div class="progress" role="progressbar" aria-label="prog">
            <div class="progress-bar" :class="progressBarColor" :style="{ width: progressWidth }"></div>
          </div>
          <div class="progress-label progress-label-right">{{ stats.dartsPoints }}/{{ maxPoints }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.progress-container {
  position: relative;
}

.progress-label-left,
.progress-label-right {
  position: absolute;
  font-size: 1.5vw;
  color: rgb(219, 217, 217);
  text-shadow: 2px 2px #000000;
  font-style: italic;
  bottom: 5vh;
}

.progress-label-left {
  left: 0;
  text-align: left;
}

.progress-label-right {
  right: 0;
  text-align: right;
}


.progress {
  height: 5vh;
  border: 1px solid black;
}

.progress-bar {
  height: 100%;
  box-shadow: 5px 0 10px rgba(0, 0, 0, 0.5);
}


.statistic-img {
  margin: 10vh 8vw 2vh 8vw;
  width: 19.5vw;
  height: 40vh;
}

.statistic-profileImg {
  width: 19vw;
  height: 38.75vh;
  border-radius: 100%;
}

.margin-statname {
  margin: 2vh 6vw 2vh 6vw;
  font-size: 40px;
  font-style: italic;
  font-weight: 350;
  text-shadow: 3px 3px #000000;
}

.marginst-btn {
  margin: 2vh 11vw;
  width: 10vw;
}

.stat-table {
  background-color: rgba(0, 0, 0, 0.5);
  margin: 10vh 5vw 10vh 5vw;
  border-radius: 20px;
  border: 2.25px solid black;
  overflow-y: auto;
  max-height: 47vh;
  max-width: 50vw;
  display: block;
}

.stat-table td {
  text-align: center;
  font-size: 1.2vw;
  border: 1.5px solid black;
}

.stat-table .med {
  font-size: 1.2vw;
  font-weight: bold;
  padding: 2vw 0vw;
  vertical-align: middle;
}

td .display-6 {
  font-size: 1.5vw;
}

td .display-5 {
  font-size: 1.2vw;
}
</style>