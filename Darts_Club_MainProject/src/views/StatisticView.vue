<script setup lang="ts">
import Admin_UserRegistry from '@/components/registration_comp/Admin_UserRegistry.vue';
import Navbar from '@/components/standard_comp/Navbar.vue';
import Statistic from '@/components/statistic_comp/Statistic.vue';
import { useUserStore } from '@/stores/UserStore';

const { user } = useUserStore();
</script>

<template>
    <Navbar />
    <Admin_UserRegistry v-if="user.role == 2"/>
    <Statistic v-else/>
</template>

<style scoped>
</style>