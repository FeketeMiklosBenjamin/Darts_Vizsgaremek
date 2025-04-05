<script setup lang="ts">
import type AllUsersModel from '@/models/AllUsersModel';
import router from '@/router';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { computed, onBeforeMount, ref } from 'vue';

const { alluser } = storeToRefs(useUserStore());
const { getAllUser } = useUserStore();

const leaderBoardUsers = ref<AllUsersModel[]>([]);
const selectedLevel = ref<string>('Amateur');

onBeforeMount(async () => {
    await getAllUser();
    leaderBoardUsers.value = [...alluser.value];
});

const filteredUsers = computed(() => {
    return leaderBoardUsers.value
        .filter(user => user.level === selectedLevel.value)
        .sort((a, b) => Number(b.dartsPoints) - Number(a.dartsPoints));
});

const getRankClass = (index: number) => {
    if (index === 0) return 'gold-rank';
    if (index === 1) return 'silver-rank';
    if (index === 2) return 'bronze-rank';
    return '';
};


const NavigateToStatistic = (userId: string) => {
    router.push(`/statistic/${userId}`)
}

</script>

<template>
    <div class="z-1 position-rel">
        <div class="row justify-content-center my-5">
            <div class="col-md-5 col-sm-8 col-10">
                <select class="form-control text-center" v-model="selectedLevel">
                    <option value="Amateur">Amatőr</option>
                    <option value="Advanced">Haladó</option>
                    <option value="Professional">Professzionális</option>
                    <option value="Champion">Bajnok</option>
                </select>
            </div>
            <div class="row justify-content-center my-4">
                <div class="col-md-10 col-lg-8 table-responsive">
                    <div class="main-div" style="max-height: 70vh;">
                        <table class="table text-center" v-if="filteredUsers.length > 0">
                            <tbody>
                                <tr v-for="(users, index) in filteredUsers" :key="users.id"
                                    @click="NavigateToStatistic(users.id)">
                                    <td :class="getRankClass(index)">{{ index + 1 }}.</td>
                                    <td>
                                        <div class="rounded-circle mx-auto border-3" style="width: 36px; height: 36px;"
                                            :class="{
                                                'success-border': users.level == 'Amateur',
                                                'warning-border': users.level == 'Advanced',
                                                'danger-border': users.level == 'Professional',
                                                'purple-border': users.level == 'Champion',
                                            }">
                                            <img :src="users.profilePictureUrl"
                                                class="profileImg border-0 mx-auto d-block" alt="Nincs">
                                        </div>
                                    </td>
                                    <td class="text-white">{{ users.username }}</td>
                                    <td class="text-white">{{ users.emailAddress }}</td>
                                    <td class="text-white">{{ users.dartsPoints }}</td>
                                </tr>
                            </tbody>
                        </table>
                        <div v-else class="alert alert-warning text-center mx-auto w-50">
                            <i class="bi bi-exclamation-circle mx-2 d-inline"></i>
                            <div class="d-inline">Nincsen ilyen szintű felhasználó!</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.gold-rank {
    color: #FFD70D;
    font-style: italic;
    font-size: 1.2em;
}

.silver-rank {
    color: #C0C0C0;
    font-style: italic;
    font-size: 1.1em;
}

.bronze-rank {
    color: #cd7f32;
    font-style: italic;
    font-size: 1.1em;
}

.form-control {
    padding: 0.75vw;
    font-weight: 600;
    font-size: 2.5vh;
    background-color: rgba(212, 210, 210, 0.601);
}

.form-control:focus {
    outline: none;
    box-shadow: none;
    color: none;
}

.table {
    background-color: transparent;
    border-collapse: separate;
    border-spacing: 0 10px;
}

table tr {
    cursor: pointer;
}

.table tr:hover {
    background-color: rgba(255, 255, 255, 0.910);
}

.table td {
    background-color: rgba(86, 83, 83, 0.84);
    border-top: 3px solid black;
    border-bottom: 3px solid black;
    padding-bottom: 10px;
    vertical-align: middle;
}


table td div {
    list-style: none;
    width: 2.25vw
}
</style>