<script setup lang="ts">
import Navbar from '@/components/standard_comp/Navbar.vue';
import Statistic from '@/components/statistic_comp/Statistic.vue';
import { useUserStore } from '@/stores/UserStore';

const { user } = useUserStore();
</script>

<template>
    <Navbar />
    <Statistic/>
</template>

<style scoped>
</style>