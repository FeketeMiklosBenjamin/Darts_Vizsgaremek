<script setup lang="ts">
import AdminRegistryComp from '@/components/competition_comp/AdminRegistry.vue';
import UserCompetition from '@/components/competition_comp/UserCompetition.vue';
import Background from '@/components/standard_comp/Background.vue';
import Navbar from '@/components/standard_comp/Navbar.vue';
import { useUserStore } from '@/stores/UserStore';

const { user } = useUserStore();

</script>

<template>
    <Navbar />
    <Background image="background-competition-img" />
    <AdminRegistryComp v-if="user.role == 2"/>
    <UserCompetition v-else/>
</template>

<style scoped>
</style>