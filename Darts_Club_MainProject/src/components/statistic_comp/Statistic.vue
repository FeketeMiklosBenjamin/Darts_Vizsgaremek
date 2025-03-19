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
          <div class="">

          </div>
          <div class="statistic-line-vertical statistic-tline-margin-hor">
          </div>
          <div class="statistic-line-horizontal statistic-line-margin-ver">
          </div>
          <div class="statistic-line-vertical statistic-bline-margin-hor text-center">
            <!-- <div class="display-5 text-secondary">Sets:</div>
            <div class="display-6 text-white">1535435</div> -->
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped></style>