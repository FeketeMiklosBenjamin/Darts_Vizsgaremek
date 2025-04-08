<script setup lang="ts">
import AnnouncedTmService from '@/services/AnnouncedTmService';
import { useAnnouncedTmStore } from '@/stores/AnnouncedTmStore';
import { useUserStore } from '@/stores/UserStore';
import { defineEmits, onMounted } from 'vue';

const { status } = useAnnouncedTmStore();

const props = defineProps<{
  message: string;
  success: boolean;
}>();

const emit = defineEmits(['close']);

onMounted(() => {
  setTimeout(() => {
    emit('close');
  }, 3000);
});
</script>

<template>
  <div class="alert text-center mt-3" :class="success ? 'alert-success' : 'alert-danger'">
    <i class="bi me-2" :class="success ? 'bi-check-circle' : 'bi-x-circle'"></i>
    {{ status.resp }}
  </div>
</template>

<style scoped>
.alert {
  transition: opacity 0.3s ease-in-out;
}
</style>
