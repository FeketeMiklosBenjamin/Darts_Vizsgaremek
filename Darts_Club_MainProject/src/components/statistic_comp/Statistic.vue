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

<style scoped></style>