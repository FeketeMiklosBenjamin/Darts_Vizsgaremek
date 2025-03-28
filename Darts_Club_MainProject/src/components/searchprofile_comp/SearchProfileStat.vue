<script setup lang="ts">
import type AllUsersModel from '@/models/AllUsersModel';
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { computed, onBeforeMount, ref } from 'vue';

const { alluser, user } = storeToRefs(useUserStore());
const { getAllUser } = useUserStore();
const searchUsers = ref<AllUsersModel[]>([])
const searchFor = ref('');

onBeforeMount(async () => {
    await getAllUser();
    searchUsers.value = alluser.value.filter(x => x.id !== user.value.id);
});

const filteredUsers = computed(() => {
    const searchTerm = searchFor.value.trim().toLowerCase();

    if (!searchTerm) {
        return searchUsers.value;
    }

    return searchUsers.value.filter(x => x.username.toLowerCase().includes(searchTerm));
});

</script>

<template>
    <div class="z-1 position-rel">
        <div class="row justify-content-center my-5">
            <div class="col-md-6">
                <div class="input-group">
                    <span class="input-group-text">
                        <i class="bi bi-search"></i>
                    </span>
                    <input type="text" class="form-control" placeholder="Felhasználónév..." v-model="searchFor">
                </div>
            </div>
            <div class="row justify-content-center my-4">
                <div class="col-md-8">
                    <table class="table text-center">
                        <tbody>
                            <tr v-for="users in filteredUsers" :key="users.id">
                                <td>
                                    <div class="rounded-circle border mx-auto border-3" :class="{
                                        'border-success': users.level == 'Amateur',
                                        'border-warning': users.level == 'Advanced',
                                        'border-danger': users.level == 'Professional'
                                    }">
                                    <img :src="users.profilePictureUrl" class="profileImg border-0 mx-auto d-block" alt="Nincs">
                                    </div>
                                </td>
                                <td>{{ users.username }}</td>
                                <td>{{ users.emailAddress }}</td>
                                <td>{{ users.dartsPoints }}</td>                        
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>

.input-group .form-control {
    padding: 0.75vw;
}

.table {
    background-color: transparent;
    border-collapse: separate;
    border-spacing: 0 10px;
}

table tr {
    background-color: rgba(255, 255, 255, 0.685)
}

.table td,
.table th {
    background-color: transparent;
    border-top: 3px solid black;
    border-bottom: 3px solid black;
}

.table td {
    padding-bottom: 10px;
    vertical-align: middle
}

table td div {
    list-style: none;
    width: 2.25vw
}
</style>