import type LoginModel from "@/models/LoginModel"
import type RegisterModel from "@/models/RegisterModel"
import type UserModel from "@/models/UserModel"
import UserService from "@/services/UserService"
import { defineStore } from "pinia"

export const useUserStore = defineStore('userStore', {
    state: () => ({
        status: {
            message: '',
            _id: '',
        },
        user: <UserModel> {}
    }),
    actions: {
        getOneUser(id: string) {
            return UserService.getUser(id)
                .then((res) => {
                    this.user = {  
                        id: res.data.id,
                        username: res.data.username,
                        password: '',
                        emailAddress: res.data.emailAddress,
                        role: 1,
                        registerDate: res.data.register_date,
                        refreshToken: '',
                        refreshTokenExpiry: '',
                        lastLoginDate: res.data.last_login_date
                    };
                    sessionStorage.setItem('user', JSON.stringify(this.user));
                    return res.data;
                })
                .catch((err) => {
                    this.user = defaultUser;
                    return Promise.reject()
                })
        },
        register(data: RegisterModel) {
            return UserService.registerUser(data) 
                .then((res) => {
                    this.user =  { ...res.data.data},
                    this.status._id = res.data.id;
                })
                .catch((err) => {
                    this.user = defaultUser;
                    this.status.message = err.data.message;
                    return Promise.reject()
                })
        },
        login(data: LoginModel) {
            return UserService.loginUser(data)
                .then((res) => {
                    sessionStorage.setItem('accessToken', res.data.accessToken)
                    this.status._id = res.data.id;
                })
                .catch((err) => {
                    this.user = defaultUser;
                    this.status.message = err.data.message;
                    return Promise.reject()
                })
        },
        logout(){
            return UserService.logoutUser(this.user.accessToken!)
            .then(()=>{
                this.user = defaultUser;
                sessionStorage.removeItem('accessToken');
                sessionStorage.removeItem('user')
            })
            .catch(()=>{
                this.user = defaultUser;
                sessionStorage.removeItem('accessToken');
                sessionStorage.removeItem('user');
            })
        }
    }
})

const defaultUser = { id: '',
    username: '',
    password: '',
    emailAddress: '',
    role: 0,
    registerDate: '',
    refreshToken: '',
    refreshTokenExpiry: '',
    lastLoginDate: ''};