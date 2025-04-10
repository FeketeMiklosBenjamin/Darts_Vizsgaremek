<script setup lang="ts">
import { ref, nextTick } from 'vue';
import type CompetitionModel from '@/models/CompetitionModel';
import { useAnnouncedTmStore } from '@/stores/AnnouncedTmStore';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';

const { UserApplication } = useAnnouncedTmStore();
const { user } = storeToRefs(useUserStore());

const props = defineProps<{
  currentComp: CompetitionModel | null;
  visible: boolean;
}>();

const emit = defineEmits(['close', 'applied']);

const isFormVisible = ref(true);

const Applicate = async (compId: string) => {
  try {
    await UserApplication(user.value.accessToken, compId)
  } catch (error) {
    
  }
  isFormVisible.value = false;

  emit('applied');
  emit('close');

  await nextTick();
  setTimeout(() => {
    isFormVisible.value = true;
  }, 500);
};
</script>

<template>
  <div v-if="visible" class="modal fade show d-block" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
      <div class="modal-content bg-white">
        <template v-if="isFormVisible">
          <div class="modal-header d-flex justify-content-center">
            <h5 class="modal-title text-center">
              Szeretne jelentkezni a versenyre?<br />
              Nem visszafordítható!
            </h5>
          </div>
          <div class="modal-body d-flex justify-content-center gap-2">
            <button class="btn btn-success" @click="Applicate(props.currentComp?.id || '')">Jelentkezés</button>
            <button class="btn btn-danger" @click="emit('close')">Mégse</button>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>

<style scoped>
.modal.fade.show {
  transition: opacity 0.3s ease-in-out;
}
</style>
