import type LoginModel from "@/models/LoginModel"
import type RegisterModel from "@/models/RegisterModel"
import type UserModel from "@/models/UserModel"
import UserService from "@/services/UserService"
import { defineStore } from "pinia"

export const useUserStore = defineStore('userStore', {
    state: () => ({
        status: {
            message: '',
            _id: JSON.parse(sessionStorage.getItem('user') || '{}')?.id || '',
            isLoggedIn: !!sessionStorage.getItem('user')
        },
        user: JSON.parse(sessionStorage.getItem('user') || '{}') as UserModel || <UserModel>{}
    }),
    actions: {
        register(data: RegisterModel) {
            return UserService.registerUser(data) 
                .then((res) => {
                    this.user = {
                        id: res.data.id,
                        username: res.data.username,
                        password: '',
                        emailAddress: res.data.emailAddress,
                        role: res.data.role,
                        registerDate: '',
                        refreshToken: res.data.refreshToken,
                        accessToken: res.data.accessToken,
                        profilePictureUrl: res.data.profilePictureUrl
                    };
                    sessionStorage.setItem('user', JSON.stringify(this.user));                   
                    this.status._id = res.data.id;
                    this.status.isLoggedIn = true;
                })
                .catch((err) => {
                    this.user = deleteUser();
                    this.status.message = err.data.message;
                    return Promise.reject()
                })
        },
        login(data: LoginModel) {
            return UserService.loginUser(data)
                .then((res) => {                  
                    this.user = {
                        id: res.data.id,
                        username: res.data.username,
                        password: '',
                        emailAddress: res.data.emailAddress,
                        role: res.data.role,
                        registerDate: '',
                        refreshToken: res.data.refreshToken,
                        accessToken: res.data.accessToken,
                        profilePictureUrl: res.data.profilePictureUrl
                    };
                    sessionStorage.setItem('user', JSON.stringify(this.user));
                    this.status._id = res.data.id;
                    this.status.isLoggedIn = true;
                })
                .catch((err) => {
                    this.user = deleteUser();
                    this.status.message = err.data.message;
                    return Promise.reject()
                })
        },
        logout(){
            return UserService.logoutUser(this.user.accessToken!, this.user.refreshToken!)
            .then(()=>{                
                this.user = deleteUser();
                this.status._id = '';
                this.status.message = '';
                this.status.isLoggedIn = false
                sessionStorage.removeItem('user')
            })
            .catch(()=>{
                this.user = deleteUser();
                this.status._id = '';
                this.status.message = '';
                this.status.isLoggedIn = false;
                sessionStorage.removeItem('user');
            })
        },
        uploadimage(image: File, accesstoken: string) {
            return UserService.uploadImage(image, accesstoken)
                .then((res) => {
                    this.user.profilePictureUrl = res.profilePictureUrl;
                    sessionStorage.setItem('user', JSON.stringify(this.user));
                })
                .catch((err) => {
                    return Promise.reject(err);
                });
        }
    }
})

function deleteUser() {
   const defaultUser = { id: '',
        username: '',
        password: '',
        emailAddress: '',
        role: 0,
        registerDate: '',
        refreshToken: '',
        refreshTokenExpiry: '',
        lastLoginDate: ''};
    
    return defaultUser;
}