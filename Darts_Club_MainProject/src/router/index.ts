import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import RegistrationView from '@/views/RegistrationView.vue'
import LoginView from '@/views/LoginView.vue'
import MainView from '@/views/MainView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: HomeView,
    },
    { 
      path: '/registration', 
      component: RegistrationView
    },
    { 
      path: '/sign-in', 
      component: LoginView
    },
    { 
      path: '/main-page', 
      component: MainView
    }
  ],
})

export default router
