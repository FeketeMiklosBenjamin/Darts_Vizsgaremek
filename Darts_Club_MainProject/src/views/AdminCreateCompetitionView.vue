<script setup lang="ts">
import AdminRegistry from '@/components/competition_comp/AdminRegistry.vue';
import Background from '@/components/standard_comp/Background.vue';
import Navbar from '@/components/standard_comp/Navbar.vue';
import { useUserStore } from '@/stores/UserStore';

const { user } = useUserStore();

</script>

<template>
    <Navbar />
    <Background image="background-competition-img" />
    <AdminRegistry/>
</template>

<style scoped>
</style>