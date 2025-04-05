<script setup lang="ts">
import type AllUsersModel from '@/models/AllUsersModel';
import router from '@/router';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { computed, onBeforeMount, ref } from 'vue';

const { alluser, user } = storeToRefs(useUserStore());
const { getLBUser } = useUserStore();
const searchUsers = ref<AllUsersModel[]>([])
const searchFor = ref('');

onBeforeMount(async () => {
    await getLBUser();
    searchUsers.value = alluser.value.filter(x => x.id !== user.value.id).sort((a, b) => a.username.localeCompare(b.username));
});

const filteredUsers = computed(() => {
    const searchTerm = searchFor.value.trim().toLowerCase();

    if (!searchTerm) {
        return searchUsers.value;
    }

    return searchUsers.value.filter(x => x.username.toLowerCase().includes(searchTerm)).sort((a, b) => a.username.localeCompare(b.username));
});

const NavigateToStatistic = (userId: string) => {
    router.push(`/statistic/${userId}`)
}

</script>

<template>
    <div class="z-1 position-rel">
        <div class="row justify-content-center my-5">
            <div class="col-lg-5 col-md-6 col-sm-8 col-10">
                <div class="input-group">
                    <span class="input-group-text">
                        <i class="bi bi-search"></i>
                    </span>
                    <input type="text" class="form-control" placeholder="Felhasználónév..." v-model="searchFor">
                </div>
            </div>
            <div class="row justify-content-center my-4">
                <div class="col-md-10 col-lg-10 table-responsive">
                    <div class="main-div" style="max-height: 70vh;">
                        <table class="table text-center" v-if="filteredUsers.length > 0">
                            <tbody>
                                <tr v-for="users in filteredUsers" :key="users.id"
                                    @click="NavigateToStatistic(users.id)">
                                    <td>
                                        <div class="rounded-circle mx-auto border-3"
                                            style="width: 36px; height: 36px;" :class="{
                                                'success-border': users.level == 'Amateur',
                                                'warning-border': users.level == 'Advanced',
                                                'danger-border': users.level == 'Professional',
                                                'purple-border': users.level == 'Champion',
                                            }">
                                            <img :src="users.profilePictureUrl"
                                                class="profileImg border-0 mx-auto d-block" alt="Nincs">
                                        </div>
                                    </td>
                                    <td>{{ users.username }}</td>
                                    <td>{{ users.emailAddress }}</td>
                                    <td>{{ users.dartsPoints }}</td>
                                </tr>
                            </tbody>
                        </table>
                        <div v-else class="alert alert-warning text-center mx-auto w-50">
                            <i class="bi bi-exclamation-circle mx-2 d-inline"></i>
                            <div class="d-inline">Nem létezik ez a felhasználó!</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.input-group .form-control {
    padding: 0.75vw;
}

.input-group .form-control:focus {
    outline: none;
    box-shadow: none;
    border-color: #aaa;
}

.table {
    background-color: transparent;
    border-collapse: separate;
    border-spacing: 0 10px;
}

table tr {
    background-color: rgba(255, 255, 255, 0.585);
    cursor: pointer;
    border: 3px solid black;
}

.table tr:hover {
    background-color: rgba(255, 255, 255, 0.910);
}

.table td {
    background-color: transparent;
}

.table td {
    padding-bottom: 10px;
    vertical-align: middle;
}


table td div {
    list-style: none;
    width: 2.25vw
}
</style>