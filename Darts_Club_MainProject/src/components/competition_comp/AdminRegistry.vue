<script setup lang="ts">
</script>

<template>
    <div class="row">
        <div class="col-md-6">
            <div class="glass-card transform width-form">
                <form>

                    <div class="input-group mb-3">
                        <span class="input-group-text" id="competition-name">Verseny neve:</span>
                        <input type="text" class="form-control" placeholder="California Groupama"
                        aria-label="competition-name" aria-describedby="competition-name">
                    </div>
                    <div class="mb-3 text-white fst-italic">
                        <label for="SetCount" class="form-label">Setek száma: </label>
                        <input type="range" class="form-range custom-range" min="0" max="12" value="0" id="SetCount">
                    </div>
                    <div class="mb-3 text-white fst-italic">
                        <label for="LegCount" class="form-label">Legek száma: </label>
                        <input type="range" class="form-range custom-range" min="3" max="20" value="3" id="LegCount">
                    </div>
                    <div class="row mx-auto justify-content-center">
                        <label class="text-white text-center fs-5 fst-italic me-3">Kezdőpontok száma:</label>
                        <div class="form-check text-white">
                            <input class="form-check-input" type="radio" name="startingPoint" id="301">
                            <label class="form-check-label" for="301">
                                301
                            </label>
                        </div>
                        <div class="form-check text-white">
                            <input class="form-check-input" type="radio" name="startingPoint" id="501">
                            <label class="form-check-label" for="501">
                                501
                            </label>
                        </div>
                        <div class="form-check text-white">
                            <input class="form-check-input" type="radio" name="startingPoint" id="701">
                            <label class="form-check-label" for="701">
                                701
                            </label>
                        </div>
                    </div>
                    <div class="input-group mt-5">
                        <span class="input-group-text" id="competition-name">Verseny jelszava:</span>
                        <input type="password" class="form-control" placeholder="lajos" aria-label="competition-psw"
                        aria-describedby="competition-psw">
                    </div>
                    <label class="text-white mt-3 fst-italic fs-5" for="competition-maxPlayer">Hány fő:</label>
                    <div class="d-flex justify-content-center align-items-center">
                        <select class="form-select form-select-xl w-auto text-start" id="competition-maxPlayer">
                            <option value="4">4</option>
                            <option value="8">8</option>
                            <option value="16">16</option>
                        </select>
                    </div>
                    <div class="mt-3 text-white fst-italic fs-5">
                        <label for="start_competition" class="form-label">Verseny kezdete:</label>
                        <input type="datetime-local" class="form-control" id="start_competition">
                    </div>
                    <div class="my-3 text-white fst-italic fs-5">
                        <label for="end_competition" class="form-label">Verseny vége:</label>
                        <input type="datetime-local" class="form-control" id="end_competition">
                    </div>
                    <div class="my-3 text-white fst-italic fs-5">
                        <label for="end_competition" class="form-label">Verseny dátumai:</label>
                        <input type="datetime-local" class="form-control" id="end_competition">
                    </div>
                    <button type="submit" class="btn btn-warning">Elküldés</button>
                </form>
                </div>
        </div>
        <div class="col-md-6">
            <p>Szia</p>
        </div>
    </div>
</template>

<style scoped>
.form-check {
    margin-top: 2%;
    width: 10%;
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

.width-form {
    top: 52%;
    left: 25%;
    height: 85%;
    width: 40%;
    background-color: black;
}

.glass-card {
    padding: 20px;
    background: rgba(0, 0, 0, 0.65);
    border: 5px solid gray;
}
</style>