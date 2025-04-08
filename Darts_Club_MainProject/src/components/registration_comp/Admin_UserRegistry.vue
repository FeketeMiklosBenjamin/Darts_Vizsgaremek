<script setup lang="ts">
import type ModifyModel from '@/models/ModifyModel';
import type RegisterModel from '@/models/RegisterModel';
import { useUserStore } from '@/stores/UserStore';
import { computed, onMounted, ref, watch } from 'vue';
import { useRouter } from 'vue-router';

const { uploadimage, registerAdmin, modify, status } = useUserStore();
const router = useRouter();
const processing = ref<boolean>(false);

const profileImage = ref<File | null>(null);

onMounted(() => {
    status.message = '';
});

const isFormModified = computed(() =>
    form.value.username ||
    form.value.emailAddress ||
    form.value.newPassword
);

let form = ref<ModifyModel>({
    username: '',
    emailAddress: '',
    newPassword: '',
    newSecondPassword: '',
    oldPassword: ''
});

const handleFileChange = (event: Event) => {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput?.files?.length) {
        profileImage.value = fileInput.files[0];
    }
};


const RegisterSuccess = ref(false);
const isModifyPage = ref(false);

watch(isModifyPage, (newValue, oldValue) => {
    form.value = {
        username: '',
        emailAddress: '',
        newPassword: '',
        newSecondPassword: '',
        oldPassword: ''
    };
    status.message = '';
    profileImage.value = null;
});

async function onRegister() {
    status.message = '';
    processing.value = true;
    RegisterSuccess.value = false;

    await new Promise(resolve => setTimeout(resolve, 100));

    if (form.value.newSecondPassword !== form.value.oldPassword) {
        status.message = "A két jelszó nem egyezik meg!";
        processing.value = false;
        return;
    }

    try {
        const registerform: RegisterModel = {
            username: form.value.username,
            emailAddress: form.value.emailAddress,
            password: form.value.newSecondPassword,
            secondPassword: form.value.oldPassword
        };
        await registerAdmin(registerform)
        RegisterSuccess.value = true;
        const accessToken = JSON.parse(sessionStorage.getItem('user') || '{}')?.accessToken;

        if (profileImage.value) {
            await uploadimage(profileImage.value, accessToken);
        }

        setTimeout(() => {
            router.push('/main-page');
        }, 3000);
    } catch (err) {
        processing.value = false;
    }
}

async function onModify() {
    console.log("ok");
    status.message = '';
    processing.value = true;
    RegisterSuccess.value = false;

    if (!isFormModified.value && !profileImage.value) {
        status.message = "Kötelező legalább egy mezőt módosítani!";
        processing.value = false;
        return;
    }

    if (form.value.oldPassword == "") {
        status.message = "A régi jelszót kötelező megadni!";
        processing.value = false;
        return;
    }

    if (form.value.newPassword !== form.value.newSecondPassword) {
        status.message = "A két jelszó nem egyezik meg!";
        processing.value = false;
        return;
    }

    const accessToken = JSON.parse(sessionStorage.getItem('user') || '{}')?.accessToken;
    console.log(accessToken);

    try {
        if (isFormModified.value) {
            await modify(form.value, accessToken);
        }
        if (profileImage.value) {
            await uploadimage(profileImage.value, accessToken);
        }
        RegisterSuccess.value = true;
        setTimeout(() => {
            router.push('/main-page');
        }, 3000);
    } catch (err) {
        if (status.message == "") {
            status.message = "Hiba történt a módosítás során!";
        }
    }
    processing.value = false;
}


</script>

<template>
    <div id="myModal" class="modal fade" tabindex="-1" role="dialog" ref="modal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="alert alert-success text-center"><i class="bi bi-check-circle me-3"></i>
                        {{ status.message }}</div>
                </div>
            </div>
        </div>
    </div>
    <div class="background-color-view position-rel">
        <div class="container z-1 transform-with-navbar glass-card opacity width-form p-2 px-3 main-div md-5">
            <div class="row">
                <div class="col-12 mb-2">
                    <div class="form-check form-switch justify-content-center d-flex gap-3">
                        <input class="form-check-input fs-5" type="checkbox" id="flexSwitchCheckDefault"
                            v-model="isModifyPage" :disabled="processing"/>
                        <label class="form-check-label fst-italic text-light fs-5" for="flexSwitchCheckDefault">
                            {{ (isModifyPage == false ? "Módosítás" : "Regisztrálás") }}
                        </label>
                    </div>
                    <h1 class="text-center display-5 text-light">{{ (isModifyPage == false ? "Regisztrálás" :
                        "Módosítás") }}</h1>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-md-12 mx-auto mb-3">
                    <div class="form-floating mb-3">
                        <input type="text" class="form-control" id="name" placeholder="Név" v-model="form.username"
                            autocomplete="off">
                        <label for="name">Felhasználónév</label>
                    </div>

                    <div class="form-floating mb-3">
                        <input type="email" class="form-control" id="email" placeholder="E-mail"
                            v-model="form.emailAddress" autocomplete="off">
                        <label for="email">E-mail</label>
                    </div>

                    <div class="form-floating mb-3" v-if="isModifyPage">
                        <input type="password" class="form-control" id="password" placeholder="Jelszó"
                            v-model="form.newPassword" autocomplete="off">
                        <label for="password">Új jelszó</label>
                    </div>

                    <div class="form-floating mb-3">
                        <input type="password" class="form-control" id="confirm_password" placeholder="Jelszó újra"
                            v-model="form.newSecondPassword" autocomplete="off">
                        <label for="confirm_password_modify">{{ isModifyPage ? "Új jelszó újra" : "Jelszó"
                        }}</label>
                    </div>

                    <div class="form-floating mb-3">
                        <input type="password" class="form-control" id="old_password" placeholder="Jelszó újra"
                            v-model="form.oldPassword" autocomplete="off">
                        <label for="confirm_password_register">{{ isModifyPage ? "Régi jelszó" : "Jelszó újra"
                        }}</label>
                    </div>

                    <div class="mt-2">
                        <input class="form-control" type="file" id="formFile" @change="handleFileChange">
                        <label for="formFile" class="form-label text-white">Profilkép feltöltése (Nem
                            kötelező!)</label>
                    </div>

                    <div class="my-2">
                        <button type="button" class="btn btn-darkred text-white w-100 py-2"
                            @click="isModifyPage ? onModify() : onRegister()" :disabled="processing">{{ isModifyPage ?
                                "Módosítás" : "Admin Regisztráció" }}
                            <span v-if="processing" class="spinner-border spinner-border-sm"></span>
                        </button>
                    </div>
                    <div v-if="status.message" class="alert text-center py-1"
                        :class="RegisterSuccess ? 'alert-success' : 'alert-danger'">{{ status.message }}
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>

<style scoped>
.form-check-input:focus {
    box-shadow: 0 0 0 0.25rem rgba(194, 46, 46, 0.25);
    border-color: rgb(194, 46, 46);
}

.btn-primary:focus {
    box-shadow: 0 0 0 0.25rem rgba(194, 46, 46, 0.5);
    border-color: rgb(194, 46, 46);
}

.form-check-input:checked {
    background-color: rgb(194, 46, 46);
    border-color: rgb(194, 46, 46);
}
</style>