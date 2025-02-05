<script setup lang="ts">

</script>

<template>
    <div class="position-rel">
        <div class="container z-1 transform align-items-center glass-card width p-4">
            <div class="row">
                <div class="col-12 mb-2">
                    <h1 class="text-start display-4 text-light">Bejelentkezés</h1>
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-12 mx-auto">
                    <form>
                        <div class="form-floating mb-3">
                            <input type="email" class="form-control" id="email" placeholder="E-mail">
                            <label for="email">E-mail</label>
                        </div>

                        <div class="form-floating mb-3">
                            <input type="password" class="form-control" id="password" placeholder="Jelszó">
                            <label for="password">Jelszó</label>
                        </div>
                        <div class="mb-3">
                            <button type="submit" class="btn btn-warning w-100 py-2">Bejelentkezés</button>
                        </div>

                    </form>
                </div>
            </div>
        </div>
    </div>

</template>

<style scoped></style>