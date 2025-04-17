
<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter();

const goToCreate = () => {
    router.push('/competition-create');
};

const goToExisting = () => {
    router.push('/competition-selection');
};
</script>

<template>
    <div class="row justify-content-center align-items-center">
        <div class="col-md-6 col-12 d-flex justify-content-center align-items-center">
            <button class="admin-nav-btn btn" @click="goToCreate">
                <i class="bi bi-plus-circle me-2"></i> Verseny létrehozása
            </button>
        </div>
        <div class="col-md-6 col-12 d-flex justify-content-center align-items-center">
            <button class="admin-nav-btn btn" @click="goToExisting">
                <i class="bi bi-list-ul me-2"></i> Létező versenyek
            </button>
        </div>
    </div>
</template>

<style scoped>
.admin-nav-btn {
    width: 80%;
    padding: 1rem 1.2rem;
    font-size: 1.05rem;
    font-weight: 600;
    background-color: rgb(184, 134, 11);
    color: white;
    border: none;
    border-radius: 8px;
    transition: all 0.3s ease-in-out;
    box-shadow: 0 0 5px rgba(184, 134, 11, 0.5);
    margin-bottom: 15px;
    position: relative;
}

.admin-nav-btn::before {
    content: "";
    position: absolute;
    top: -2px;
    left: -2px;
    width: calc(100% + 4px);
    height: calc(100% + 4px);
    background: linear-gradient(45deg, #000000, rgb(199, 145, 20), #000000, rgb(199, 145, 20));
    background-size: 400%;
    filter: blur(8px);
    animation: glowing 4s linear infinite;
    border-radius: 10px;
    opacity: 0;
    z-index: -1;
}

@keyframes glowing {
    0% {
        background-position: 100% 50%;
    }

    100% {
        background-position: 0% 50%;
    }
}

.admin-nav-btn:hover::before {
    opacity: 1;
}

.admin-nav-btn:hover {
    transform: scale(1.05);
    background-color: rgb(199, 145, 20);
    box-shadow: 0 0 12px rgba(184, 134, 11, 0.8);
    color: #fff;
}

.row {
    height: 90vh;
}

@media (max-width: 767px) {
    .admin-nav-btn {
        margin-top: 130px;
        width: 80%;
        margin-bottom: 0px;
    }
    .row {
        height: 50vh;
    }
}
</style>
