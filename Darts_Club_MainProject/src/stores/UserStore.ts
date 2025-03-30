import type AllUsersModel from "@/models/AllUsersModel"
import type LoginModel from "@/models/LoginModel"
import type ModifyModel from "@/models/ModifyModel"
import type RegisterModel from "@/models/RegisterModel"
import type UserModel from "@/models/UserModel"
import type UserStatModel from "@/models/UserStatModel"
import router from "@/router"
import UserService from "@/services/UserService"
import { defineStore } from "pinia"

export const useUserStore = defineStore('userStore', {
    state: () => ({
        status: {
            message: '',
            _id: JSON.parse(sessionStorage.getItem('user') || '{}')?.id || '',
            isLoggedIn: !!sessionStorage.getItem('user')
        },
        user: JSON.parse(sessionStorage.getItem('user') || '{}') as UserModel || <UserModel>{},
        stats: <UserStatModel>{},
        alluser: <AllUsersModel[]>{}
    }),
    actions: {
        register(data: RegisterModel) {
            return UserService.registerUser(data)
                .then((res) => {
                    this.user = SetUser(res);
                    sessionStorage.setItem('user', JSON.stringify(this.user));
                    this.status._id = res.data.id;
                    this.status.isLoggedIn = true;
                })
                .catch((err) => {
                    this.status.message = err.data.message;
                    return Promise.reject(err)
                })
        },
        modify(data: ModifyModel, accessToken: string) {
            return UserService.modifyUser(data, accessToken)
                .then((res) => {
                    if (data.username != '') {
                        this.user.username = data.username;
                    } if (data.emailAddress != '') {
                        this.user.emailAddress = data.emailAddress;
                    }
                    sessionStorage.setItem('user', JSON.stringify(this.user));

                })
                .catch((err) => {
                    this.status.message = err.data.message;
                    return Promise.reject(err)
                })
        },
        login(data: LoginModel) {
            return UserService.loginUser(data)
                .then((res) => {
                    this.user = SetUser(res);
                    sessionStorage.setItem('user', JSON.stringify(this.user));
                    this.status._id = res.data.id;
                    this.status.isLoggedIn = true;
                })
                .catch((err) => {
                    this.status.message = err.data.message;
                    return Promise.reject(err)
                })
        },
        logout() {
            return UserService.logoutUser(this.user.accessToken!, this.user.refreshToken!)
                .then(() => {
                    this.user = defaultUser;
                    this.status._id = '';
                    this.status.message = '';
                    this.status.isLoggedIn = false;
                    this.alluser = [];
                    this.stats = defaultStats;
                    sessionStorage.removeItem('user');
                })
                .catch(() => {
                    this.user = defaultUser;
                    this.status._id = '';
                    this.status.message = '';
                    this.status.isLoggedIn = false;
                    this.alluser = [];
                    this.stats = defaultStats;
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
        },
        getStatById(id: string) {
            return UserService.getStat(id, this.user.accessToken!)
                .then((res) => {
                    this.stats = {
                        userId: res.data.userId,
                        matches: res.data.matches,
                        matchesWon: res.data.matchesWon,
                        sets: res.data.sets,
                        setsWon: res.data.setsWon,
                        legs: res.data.legs,
                        legsWon: res.data.legsWon,
                        tournamentsWon: res.data.tournamentsWon,
                        dartsPoints: res.data.dartsPoints,
                        level: res.data.level,
                        averages: res.data.averages,
                        max180s: res.data.max180s,
                        checkoutPercentage: res.data.checkoutPercentage,
                        highestCheckout: res.data.highestCheckout,
                        nineDarter: res.data.nineDarter,
                        username: res.data.username,
                        profilePictureUrl: res.data.profilePictureUrl,
                        registerDate: res.data.registerDate,
                        lastLoginDate: res.data.lastLoginDate
                    }
                })
                .catch((err) => {
                    return Promise.reject(err);
                });
        },
        getAllUser() {
            return UserService.getAll(this.user.accessToken)
                .then((res) => {
                    this.alluser = res.data.map((user: AllUsersModel) => ({
                        id: user.id,
                        username: user.username,
                        emailAddress: user.emailAddress,
                        profilePictureUrl: user.profilePictureUrl,
                        level: user.level,
                        dartsPoints: user.dartsPoints
                    }));
                })
                .catch((err) => {
                    return Promise.reject(err);
                })
        },
        getLBUser() {
            return UserService.getLeaderBoard(this.user.accessToken)
                .then((res) => {
                    this.alluser = res.data.map((user: AllUsersModel) => ({
                        id: user.id,
                        username: user.username,
                        emailAddress: user.emailAddress,
                        profilePictureUrl: user.profilePictureUrl,
                        level: user.level,
                        dartsPoints: user.dartsPoints
                    }));
                })
                .catch((err) => {
                    return Promise.reject(err);
                })
        },
        refreshTk() {
            return UserService.refreshToken(this.user.id!, this.user.accessToken!, this.user.refreshToken!)
                .then((res) => {
                    this.user.accessToken = res.data.accessToken;
                    sessionStorage.setItem('user', JSON.stringify(this.user));
                })
                .catch((err) => {
                    // this.logout(); // Folytatni a hibás refresh-tokent!
                    return Promise.reject(err);
                })
        }
    }
})

function SetUser(res: any) {
    const incomingUser: UserModel = {
        id: res.data.id,
        username: res.data.username,
        emailAddress: res.data.emailAddress,
        role: res.data.role,
        refreshToken: res.data.refreshToken,
        accessToken: res.data.accessToken,
        profilePictureUrl: res.data.profilePictureUrl,
        level: res.data.level
    }

    return incomingUser;
}

const defaultUser: UserModel = {
    id: '',
    username: '',
    emailAddress: '',
    role: 0,
    refreshToken: '',
    accessToken: '',
    profilePictureUrl: '',
    level: ''
};

const defaultStats: UserStatModel = {
    userId: '',
    matches: 0,
    matchesWon: 0, 
    sets: 0,
    setsWon: 0,
    legs: 0,
    legsWon: 0,
    tournamentsWon: 0,
    dartsPoints: 0,
    level: '',
    averages: 0,
    max180s: 0,
    checkoutPercentage: 0,
    highestCheckout: 0,
    nineDarter: 0,
    username: '',
    profilePictureUrl: '',
    registerDate: '',
    lastLoginDate: ''
}
