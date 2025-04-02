<script setup lang="ts">
import AdminRegistryComp from '@/components/competition_comp/AdminRegistry.vue';
import Competition_Background from '@/components/competition_comp/Competition_Background.vue';
import UserCompetition from '@/components/competition_comp/UserCompetition.vue';
import Navbar from '@/components/standard_comp/Navbar.vue';
import { useUserStore } from '@/stores/UserStore';

const { user } = useUserStore();

</script>

<template>
    <Navbar />
    <Competition_Background />
    <AdminRegistryComp v-if="user.role == 2"/>
    <UserCompetition v-else/>
</template>

<style scoped>
</style>