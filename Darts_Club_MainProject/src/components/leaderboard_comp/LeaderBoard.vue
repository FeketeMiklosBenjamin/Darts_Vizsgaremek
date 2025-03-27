<script setup lang="ts">
import type LeaderBoardModel from '@/models/LeaderBoardModel';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { computed, onBeforeMount, ref } from 'vue';

const { leaderboard, user } = storeToRefs(useUserStore());
const { getLeaderBoard } = useUserStore();
const leaderboardUsers = ref<LeaderBoardModel[]>([])
const searchFor = ref('');

onBeforeMount(() => {
    getLeaderBoard()
        .then(() => {
            leaderboard.value = leaderboard.value.filter(x => x.id !== user.value.id)
            leaderboardUsers.value = leaderboard.value
        })
})

const filteredUsers = computed(() => {
    const searchTerm = searchFor.value.trim();

    if (!searchTerm) {
        return leaderboardUsers.value = [...leaderboard.value];
    }
   return leaderboardUsers.value = leaderboard.value.filter(x => x.username.toLowerCase().includes(searchTerm.toLowerCase()))
})

</script>

<template>
    <div class="z-1 position-rel">
        <div class="row justify-content-center mt-5">
            <div class="col-md-6">
                <div class="input-group">
                    <span class="input-group-text">
                        <i class="bi bi-search"></i>
                    </span>
                    <input type="text" class="form-control" placeholder="Felhasználónév..." v-model="searchFor">
                </div>
            </div>
            <div class="row justify-content-center mt-2">
                <div class="col-md-8">
                    <table class="table text-center">
                        <tbody>
                            <tr v-for="users in filteredUsers" :key="users.id">
                                <td>
                                    <div class="rounded-circle border mx-auto border-3" :class="{
                                        'border-success': user.level == 'Amateur',
                                        'border-warning': user.level == 'Advanced',
                                        'border-danger': user.level == 'Professional'
                                    }">
                                    <img :src="users.profilePictureUrl" class="profileImg border-0 mx-auto d-block" alt="Nincs">
                                    </div>
                                </td>
                                <td>{{ users.username }}</td>
                                <td>{{ users.emailAddress }}</td>
                                <td>{{ users.registerDate }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped></style>