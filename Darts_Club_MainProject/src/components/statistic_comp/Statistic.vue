<script setup lang="ts">
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';

const { status, user } = storeToRefs(useUserStore());

</script>

<template>
  <div class="background-color-view">
    <div class="row">
      <div class="col-4 mt-5">
        <div class="bg-secondary rounded-circle border border-5 statistic-img mx-auto" :class="{
          'border-success': user.level == 'Amateur',
          'border-warning': user.level == 'Advanced',
          'border-danger': user.level == 'Professional'
        }">
          <img :src=user.profilePictureUrl class="statistic-profileImg d-block" alt="Nincs">
        </div>
        <h1 class="display-6 text-white margin-statname text-center">{{ user.username }}</h1>
        <div class="col-5">
          <router-link :to="`/modify`">
            <button class="btn btn-warning px-1 marginst-btn">
              Módosítás
            </button>
          </router-link>
        </div>
      </div>
      <div class="col-7">
        <div class="statistic-box">
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szett:</p>
            <p class="display-1">5</p>
          </div>
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szettek nyerve:</p>
            <p class="display-1">1</p>
          </div>
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szett:</p>
            <p class="display-1">5</p>
          </div>
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szettek nyerve:</p>
            <p class="display-1">1</p>
          </div>
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szett:</p>
            <p class="display-1">5</p>
          </div>
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szettek nyerve:</p>
            <p class="display-1">1</p>
          </div>
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szett:</p>
            <p class="display-1">5</p>
          </div>
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szettek nyerve:</p>
            <p class="display-1">1</p>
          </div>
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szett:</p>
            <p class="display-1">5</p>
          </div>
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szettek nyerve:</p>
            <p class="display-1">1</p>
          </div>
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szett:</p>
            <p class="display-1">5</p>
          </div>
          <div class="statistic-section">
            <p class="display-2 text-secondary">Szettek nyerve:</p>
            <p class="display-1">1</p>
          </div>
          <div class="statistic-line-vertical"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
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

.statistic-box {
    margin: 6vh 1vw;
    width: 60vw;
    min-height: 63vh;
    background: rgba(0, 0, 0, 0.4);
    border: 1px solid black;
    border-radius: 2vh;
    position: relative;
    display: grid;
    grid-template-columns: 1fr 1fr;
    grid-template-rows: repeat(6, 1fr);
    grid-auto-rows: auto;
    gap: 10px;
}

.statistic-line-horizontal {
    width: 100%;
    height: 0.06vh;
    background-color: black;
    grid-column: span 2;
}

.statistic-line-vertical {
    position: absolute;
    left: 50%;
    top: 0;
    width: 0.06vw;
    height: 100%;
    background-color: black;
}

.statistic-section {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    color: white;
    font-weight: bold;
}

.statistic-section>.display-2 {
    margin-bottom: 0;
    font-size: 3vh;
}

.statistic-section>.display-1 {
    margin-bottom: 0;
    font-style: italic;
    font-size: 4vh;
}
</style>