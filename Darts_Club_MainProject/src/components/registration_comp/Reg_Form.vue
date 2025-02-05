<script setup lang="ts">

</script>

<template>
    <div class="position-rel">
        <div class="container z-1 transform align-items-center glass-card width py-1">
            <div class="row">
                <div class="col-12  mb-2">
                    <h1 class="text-center display-4 text-light">Regisztráció</h1>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-12 mx-auto">
                    <form>
                        <div class="form-floating mb-3">
                            <input type="text" class="form-control" id="name" placeholder="Név">
                            <label for="name">Név</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="email" class="form-control" id="email" placeholder="E-mail">
                            <label for="email">E-mail</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="password" class="form-control" id="password" placeholder="Jelszó">
                            <label for="password">Jelszó</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="password" class="form-control" id="confirm_password" placeholder="Jelszó újra">
                            <label for="confirm_password">Jelszó újra</label>
                        </div>

                        <div class="mb-3">
                            <button type="submit" class="btn btn-info w-100 py-2">Regisztráció</button>
                        </div>

                    </form>
                </div>
            </div>
        </div>
    </div>

</template>

<style scoped></style>