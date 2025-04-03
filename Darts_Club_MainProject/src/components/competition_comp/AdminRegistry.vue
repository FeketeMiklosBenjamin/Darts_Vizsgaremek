<script setup lang="ts">
</script>

<template>
    <div class="container-fluid">
        <div class="row main-div">
            <div class="col-md-6 col-12 mt-5">
                <div class="col-12 col-xl-7 offset-xl-3 col-lg-8 offset-lg-2 mt-md-5 mt-0">
                    <div class="glass-card width-form">
                        <form>
                            <div class="input-group mb-3">
                                <span class="input-group-text" id="competition-name">Verseny neve:</span>
                                <input type="text" class="form-control" placeholder="California Groupama"
                                    aria-label="competition-name" aria-describedby="competition-name">
                            </div>
                            <div class="mb-3 text-white fst-italic">
                                <label for="SetCount" class="form-label">Setek száma: </label>
                                <input type="range" class="form-range custom-range" min="0" max="12" value="0"
                                    id="SetCount">
                            </div>
                            <div class="mb-3 text-white fst-italic">
                                <label for="LegCount" class="form-label">Legek száma: </label>
                                <input type="range" class="form-range custom-range" min="3" max="20" value="3"
                                    id="LegCount">
                            </div>
                            <div class="row justify-content-center">
                                <label class="text-white text-center fs-5 fst-italic mb-2">Kezdőpontok száma:</label>
                                <div class="col-auto text-white">
                                    <div class="form-check">
                                        <input class="form-check-input" type="radio" name="startingPoint" id="301">
                                        <label class="form-check-label" for="301">301</label>
                                    </div>
                                </div>
                                <div class="col-auto text-white">
                                    <div class="form-check">
                                        <input class="form-check-input" type="radio" name="startingPoint" id="501">
                                        <label class="form-check-label" for="501">501</label>
                                    </div>
                                </div>
                                <div class="col-auto text-white">
                                    <div class="form-check">
                                        <input class="form-check-input" type="radio" name="startingPoint" id="701">
                                        <label class="form-check-label" for="701">701</label>
                                    </div>
                                </div>
                            </div>
                            <div class="input-group mt-4">
                                <span class="input-group-text" id="competition-psw">Verseny jelszava:</span>
                                <input type="password" class="form-control" placeholder="lajos"
                                    aria-label="competition-psw" aria-describedby="competition-psw">
                            </div>
                            <div class="input-group mt-4">
                                <span class="input-group-text" id="competition-psw">Jelszó újra:</span>
                                <input type="password" class="form-control" placeholder="lajos"
                                    aria-label="competition-psw" aria-describedby="competition-psw">
                            </div>
                            <label class="text-white text-center mt-3 fst-italic fs-5"
                                for="competition-maxPlayer">Szint:</label>
                            <div class="d-flex justify-content-center align-items-center">
                                <select class="form-select form-select-xl w-100 text-start text-center"
                                    id="competition-maxPlayer">
                                    <option value="Amatőr">Amatőr</option>
                                    <option value="Haladó">Haladó</option>
                                    <option value="Profi">Professzionális</option>
                                    <option value="Bajnok">Bajnok</option>
                                </select>
                            </div>
                            <button type="submit" class="btn btn-warning w-100 mt-5">Elküldés</button>
                        </form>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-12 mt-5">
                <div class="col-12 col-xl-7 offset-xl-3 col-lg-8 offset-lg-2 mt-md-5 mt-0">
                    <div class="glass-card width-form">
                        <form>
                            <div class="mt-3 text-white fst-italic fs-5">
                                <label for="start_competition" class="form-label">Jelentkezés kezdete:</label>
                                <input type="datetime-local" class="form-control" id="start_competition">
                            </div>
                            <div class="my-3 text-white fst-italic fs-5">
                                <label for="end_competition" class="form-label">Jelentkezés vége:</label>
                                <input type="datetime-local" class="form-control" id="end_competition">
                            </div>
                            <label class="text-white fst-italic fs-5" for="competition-maxPlayer">Hány fő:</label>
                            <div class="d-flex justify-content-center align-items-center">
                                <select class="form-select form-select-xl w-100 text-start" id="competition-maxPlayer">
                                    <option value="4">4</option>
                                    <option value="8">8</option>
                                    <option value="16">16</option>
                                </select>
                            </div>
                            <div class="mt-3">
                                <label for="background_file" class="form-label text-white fst-italic fs-5">Verseny háttérképe:</label>
                                <input class="form-control" type="file" id="background_file">
                            </div>
                            <div class="mt-3 text-white fst-italic fs-5">
                                <label for="matches_date" class="form-label">Versenyek időpontjai:</label>
                                <input type="datetime-local" class="form-control" id="matches_date">
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.width-form {
    width: 100%;
    min-height: 60vh;
    background-color: black;
    padding: 20px;
    border-radius: 10px;
    overflow-y: auto;
}

.glass-card {
    padding: 20px;
    background: rgba(0, 0, 0, 0.65);
    border: 5px solid gray;
}

.form-check {
    margin-top: 2%;
}

.custom-range::-webkit-slider-thumb {
    background: rgb(194, 46, 46);
}

.custom-range::-moz-range-thumb {
    background: rgb(194, 46, 46);
}

.custom-range::-ms-thumb {
    background: rgb(194, 46, 46);
}

.form-label {
    text-shadow: 4px 4px 5px rgba(0, 0, 0, 0.5);
}
</style>