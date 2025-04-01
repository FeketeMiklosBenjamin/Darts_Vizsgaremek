import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import RegistrationView from '@/views/RegistrationView.vue'
import LoginView from '@/views/LoginView.vue'
import MainView from '@/views/MainView.vue'
import { useUserStore } from '@/stores/UserStore'
import NotFound from '@/views/NotFound.vue'
import CompetitionView from '@/views/CompetitionView.vue'
import LeaderBoardView from '@/views/LeaderBoardView.vue'
import SearchProfileView from '@/views/SearchProfileView.vue'
import StatisticView from '@/views/StatisticView.vue'
import FeedBackView from '@/views/FeedBackView.vue'
import Modifyview from '@/views/Modifyview.vue'
import SeeYourMessageView from '@/views/SeeYourMessageView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: HomeView,
      meta: { requiresGuest: true}
    },
    { 
      path: '/registration', 
      component: RegistrationView,
      meta: { requiresGuest: true}
    },
    { 
      path: '/sign-in', 
      component: LoginView,
      meta: { requiresGuest: true}
    },
    { 
      path: '/main-page', 
      component: MainView,
      meta: { requiresAuth: true}
    },
    { 
      path: '/competition', 
      component: CompetitionView,
      meta: { requiresAuth: true}
    },
    { 
      path: '/leaderboard', 
      component: LeaderBoardView,
      meta: { requiresAuth: true}
    },
    { 
      path: '/search-profile', 
      component: SearchProfileView,
      meta: { requiresAuth: true}
    },
    { 
      path: '/statistic/:userId', 
      component: StatisticView,
      meta: { requiresAuth: true}
    },
    { 
      path: '/feedback', 
      component: FeedBackView,
      meta: { requiresAuth: true}
    },
    { 
      path: '/modify', 
      component: Modifyview,
      meta: { requiresAuth: true}
    },
    { 
      path: '/messages', 
      component: SeeYourMessageView,
      meta: { requiresAuth: true}
    },
    {
      path: "/:pathMatch(.*)*",
      component: NotFound
    }
  ],
  scrollBehavior() {
    return { top: 0, behavior: 'instant' };
  }
})


router.beforeEach((to, from, next) => {
  const userStore = useUserStore();

  if (to.matched.some(record => record.meta.requiresGuest) && userStore.status.isLoggedIn) {
    return next('/main-page');
  } 
  if (to.matched.some(record => record.meta.requiresAuth) && !userStore.status.isLoggedIn) {
    return next('/sign-in');
  }
  return next();
});

export default router
