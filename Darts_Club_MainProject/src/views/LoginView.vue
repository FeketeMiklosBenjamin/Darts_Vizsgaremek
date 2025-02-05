<script setup lang="ts">

</script>

<template>
    <div class="container">
        <div class="row">
            <div class="col-12">
                <h1 class="text-center display-4">Bejelentkezés</h1>
            </div>
        </div>

        <div class="col-12 col-md-4 mx-auto mt-5">
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
                <button type="submit" class="btn btn-primary w-100 py-2">Bejelentkezés</button>
                </div>
            </form>
        </div>
  </div>
</template>

<style scoped>
</style>