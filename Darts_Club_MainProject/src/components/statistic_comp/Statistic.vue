<script setup lang="ts">
import { useUserStore } from '@/stores/UserStore';
import { storeToRefs } from 'pinia';
import { computed, onBeforeMount, ref, watch } from 'vue';
import { onBeforeRouteUpdate, useRoute } from 'vue-router';
import Admin_UserRegistry from '../registration_comp/Admin_UserRegistry.vue';

const { status, stats, user } = storeToRefs(useUserStore());

const route = useRoute();

const { getStatById } = useUserStore();

const isAdmin = computed(() => user.value.role === 2 && route.params.userId === user.value.id);

onBeforeMount(async () => {
  if (isAdmin.value) {
    return;
  }
  await fetchStats(route.params.userId as string);
});

onBeforeRouteUpdate(async (to, from, next) => {
  if (user.value.role === 2 && to.params.userId === user.value.id) {
    return next();
  }
  await fetchStats(to.params.userId as string);
  next();
});

const fetchStats = async (userId: string) => {
  try {
    await getStatById(userId);
  } catch (err) { }
};

const BorderColors = {
  Amateur: 'success-border',
  Advanced: 'warning-border',
  Professional: 'danger-border',
  Champion: 'purple-border'
};

const TextColors = {
  Amateur: 'success-text',
  Advanced: 'warning-text',
  Professional: 'danger-text',
  Champion: 'purple-text'
};

const BgColors = {
  Amateur: 'success-bg',
  Advanced: 'warning-bg',
  Professional: 'danger-bg',
  Champion: 'purple-bg'
};

const levelPoints = {
  Amateur: 1499,
  Advanced: 4499,
  Professional: 8999,
};

let playerLevel = '';

watch(stats, (newStats) => {
  if (newStats) {
    switch (newStats.level) {
      case "Amateur":
        playerLevel = "Amatőr";
        break;
      case "Advanced":
        playerLevel = "Haladó";
        break;
      case "Professional":
        playerLevel = "Professzionális";
        break;
      case "Champion":
        playerLevel = "Bajnok";
        break;
    }
  }
});

const borderColor = computed(() => `${BorderColors[stats.value.level as keyof typeof BorderColors]}`);
const textColor = computed(() => `${TextColors[stats.value.level as keyof typeof TextColors]}`);
const progressBarColor = computed(() => `${BgColors[stats.value.level as keyof typeof BgColors]}`);

const maxPoints = computed(() => (stats.value.dartsPoints <= 9000) ? (levelPoints[stats.value.level as keyof typeof levelPoints]) : stats.value.dartsPoints);
const progressWidth = computed(() => `${(stats.value.dartsPoints <= 9000) ? ((stats.value.dartsPoints / maxPoints.value) * 100) : 100 }%`);

</script>


<template>
  <div class="background-color-view main-div">
    <Admin_UserRegistry v-if="isAdmin"/>
    <div v-else class="row py-5">
      <div class="col-12 col-xl-5">
        <div class="col-12 left-side my-0 my-sm-3 my-xl-4">
          <img :src="stats.profilePictureUrl" :class="`statistic-profileImg ${borderColor}`" alt="Nincs" />
        </div>
        <div class="col-6 offset-3">
          <h1 class="display-6 text-white margin-statname text-center mb-3 mb-sm-5 mt-2">
            {{ stats.username }}
          </h1>
        </div>
        <div class="col-10 offset-1">
          <div v-if="status._id == stats.userId">
            <router-link :to="`/modify`">
              <button class="btn btn-warning col-6 offset-3 py-2 modify-btn">Módosítás</button>
            </router-link>
          </div>
        </div>
      </div>
      <div class="col-12 col-xl-7 my-5">
        <div class="col-10 offset-1 left-side">
          <table class="table table-dark table-bordered stat-table border-5">
            <tbody>
              <tr>
                <td colspan="2" class="med">
                  <span class="display-6">Mérkőzések száma:</span><br />{{ stats.matches }}
                </td>
                <td colspan="2" class="med">
                  <span class="display-6">Győzelmek:</span><br />{{ stats.matchesWon }}
                </td>
              </tr>
              <tr>
                <td colspan="4" class="med">
                  <span class="display-6">Megnyert versenyek száma:</span>
                  {{ stats.tournamentsWon }} db
                </td>
              </tr>
              <tr>
                <td colspan="2"><span class="display-5">Setek száma:</span> {{ stats.sets }} db</td>
                <td colspan="2">
                  <span class="display-5">Megnyert Setek:</span> {{ stats.setsWon }} db
                </td>
              </tr>
              <tr>
                <td colspan="2"><span class="display-5">Legek száma:</span> {{ stats.legs }} db</td>
                <td colspan="2">
                  <span class="display-5">Megnyert Legek:</span> {{ stats.legsWon }} db
                </td>
              </tr>
              <tr>
                <td colspan="2"><span class="display-5">Átlag:</span> {{ stats.averages }}</td>
                <td colspan="2">
                  <span class="display-5">Kiszállózási %-ék:</span> {{ stats.checkoutPercentage }}%
                </td>
              </tr>
              <tr>
                <td><span class="display-5">180-asok száma:</span><br />{{ stats.max180s }} db</td>
                <td colspan="2">
                  <span class="display-5">Legmagasabb kiszálló:</span><br />{{
                    stats.highestCheckout
                  }}
                </td>
                <td>
                  <span class="display-5">Kilenc nyilasok száma:</span><br />{{
                    stats.nineDarter
                  }}
                  db
                </td>
              </tr>
              <tr>
                <td colspan="2" rowspan="2" class="med">
                  <span class="display-6">Regisztrálás dátuma:</span> {{ stats.registerDate }}
                </td>
                <td colspan="2" class="med">
                  <span class="display-6">Utoljára bejelentkezve:</span><br />{{
                    stats.lastLoginDate
                  }}
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="progress-container col-10 offset-1 mt-3 mt-md-5 mb-5">
          <div class="progress-label progress-label-left">
            Szint: <span :class="textColor">{{ playerLevel }}</span>
          </div>
          <div class="progress" role="progressbar" aria-label="prog">
            <div class="progress-bar" :class="progressBarColor" :style="{ width: progressWidth }"></div>
          </div>
          <div class="progress-label progress-label-right">
            {{ stats.dartsPoints }}{{(stats.dartsPoints >= 9000 ? '' : '/')}}{{ (maxPoints < 9000 ? maxPoints : '') }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.left-side {
  display: flex;
  justify-content: center;
  align-items: center;
}

.progress-container {
  position: relative;
}

.progress-label-left,
.progress-label-right {
  position: absolute;
  font-size: 1.5vw;
  color: rgb(219, 217, 217);
  text-shadow: 2px 2px #000000;
  font-style: italic;
  bottom: 5vh;
}

.progress-label-left {
  left: 0;
  text-align: left;
}

.progress-label-right {
  right: 0;
  text-align: right;
}

.progress {
  height: 5vh;
  border: 1px solid black;
}

.progress-bar {
  height: 100%;
  box-shadow: 5px 0 10px rgba(0, 0, 0, 0.5);
}

.statistic-profileImg {
  width: 50%;
  height: auto;
  aspect-ratio: 1;
  border-radius: 100%;
}

.margin-statname {
  font-size: 3vw;
  font-style: italic;
  font-weight: 350;
  text-shadow: 3px 3px #000000;
}

.modify-btn {
  font-size: 1vw;
}

.stat-table {
  background-color: rgba(0, 0, 0, 0.5);
  border-radius: 20px;
  border: 2.25px solid black;
  overflow-y: auto;
  max-height: 47vh;
  max-width: 50vw;
  display: block;
}

.stat-table td {
  text-align: center;
  font-size: 1.2vw;
  border: 1.5px solid black;
  width: 20vw;
}

.stat-table .med {
  font-size: 1.2vw;
  font-weight: bold;
  padding: 2vw 0vw;
  vertical-align: middle;
}

td .display-6 {
  font-size: 1.5vw;
}

td .display-5 {
  font-size: 1.2vw;
}

@media (max-width: 1199.98px) {
  .stat-table {
    background-color: rgba(0, 0, 0, 0.5);
    border-radius: 20px;
    border: 2.25px solid black;
    overflow-y: auto;
    max-height: 100vh;
    max-width: 100vw;
    display: block;
  }

  .progress-label-left,
  .progress-label-right {
    font-size: 2.8vw;
  }

  td .display-6 {
    font-size: 3vw;
  }

  td .display-5 {
    font-size: 2.6vw;
  }

  .stat-table td {
    font-size: 2.4vw;
    width: 150vw;
  }

  .stat-table .med {
    font-size: 2.4vw;
  }

  .modify-btn {
    font-size: 2vw;
  }

  .margin-statname {
    font-size: 6vw;
    font-style: italic;
    font-weight: 350;
    text-shadow: 3px 3px #000000;
  }
}

@media (max-width: 500px) {
  .modify-btn {
    font-size: 4vw;
  }
}
</style>