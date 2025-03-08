<script setup lang="ts">
</script>

<template>
    <div class="background_color-feed">
        <div class="row">
            <h1 class="display-5 text-center text-white margin-feed">Hibabejelentés</h1>
        </div>
        <form>
            <div class="row">
                <input type="text" id="title" placeholder="Cím: pl. Felhasználó bejelentése!" class="form-control mx-auto w-50 mt-3">
            </div>
            <div class="row">
                <textarea id="subject" placeholder="Itt probléma van!" rows="12" class="form-control mt-2 mx-auto w-50"></textarea>
            </div>
            <div class="row">
                <button type="submit" class="btn btn-warning width-feedback mx-auto mt-4">Elküldés</button>
            </div>
        </form>
    </div>
</template>

<style scoped></style>