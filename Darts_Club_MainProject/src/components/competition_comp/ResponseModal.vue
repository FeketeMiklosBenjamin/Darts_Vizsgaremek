<script setup lang="ts">
import { useAnnouncedTmStore } from '@/stores/AnnouncedTmStore';
import { Modal } from 'bootstrap';
import { onMounted, ref } from 'vue';

const { status } = useAnnouncedTmStore();

const modal = ref<HTMLElement>();
let modalInstance: Modal;

const props = defineProps<{
  message: string;
  success: boolean;
}>();

const emit = defineEmits(['close']);

onMounted(() => {
  if (modal.value) {
    modalInstance = new Modal(modal.value);
    modalInstance.show();

    setTimeout(() => {
      modalInstance.hide();
      emit('close');
    }, 3000);
  }
});
</script>

<template>
  <div id="myModal" class="modal fade" role="dialog" ref="modal">
    <div class="modal-dialog" role="document">
      <div class="modal-content">
        <div class="modal-body">
          <div class="alert text-center mt-3" :class="props.success ? 'alert-success' : 'alert-danger'"><i class="bi me-2"
            :class="props.success ? 'bi-check-circle' : 'bi-x-circle'"></i>
            {{ props.message }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
</style>