import type RegisterModel from "@/models/RegisterModel";
import { User_Endpoint, RefreshTk_Endpoint } from './BackendService';
import type LoginModel from "@/models/LoginModel";
import type ModifyModel from "@/models/ModifyModel";


export default {
    registerUser(data: RegisterModel) {
        return User_Endpoint.post('/register', data)
            .then((res) => {
                return res
            })
            .catch((err) => {
                return Promise.reject(err.response)
            })
    },
    registerAdmin(data: RegisterModel, accessToken: string) {
        return User_Endpoint.post('/register/admin', data, {
            headers: {
                Authorization: `Bearer ${accessToken}`,
                'Content-Type': 'application/json'
            }
        })
            .then((res) => {
                return res
            })
            .catch((err) => {
                return Promise.reject(err.response)
            })
    },
    loginUser(data: LoginModel) {
        return User_Endpoint.post('/login', data)
            .then((res) => {
                return res
            })
            .catch((err) => {
                return Promise.reject(err.response)
            })
    },
    logoutUser(accesstoken: string, refresh: string) {
        return User_Endpoint.post('/logout', {
            refreshToken: refresh
        }, {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'application/json'
            }
        })
            .then((res) => {
                return res.data
            })
            .catch(() => {
                return Promise.reject()
            })
    },
    modifyUser(data: ModifyModel, accesstoken: string) {      
        return User_Endpoint.put('/', data, { 
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'application/json'
            }
        })
        .then((res) => {
            return res.data;
        })
        .catch((err) => {
            return Promise.reject(err.response);
        });
    },
    uploadImage(image: File, accesstoken: string) {
        const formData = new FormData();
        formData.append('file', image);

        return User_Endpoint.post('/picture/upload', formData, {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'multipart/form-data', 
            },
        })
            .then((res) => {
                return res.data;
            })
            .catch((err) => {
                return Promise.reject(err.response);
            });
    },
    getStat(id: string, accesstoken: string) {
        return User_Endpoint.get(`/tournamentstat/${id}`, {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'application/json'
            }
        })
            .then((res) => {
                return res;
            })
            .catch((err) => {
                return Promise.reject(err.response);
            })
    },
    getAll(accesstoken: string) {
        return User_Endpoint.get(`/all`, {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'application/json'
            }
        })
            .then((res) => {
                return res;
            })
            .catch((err) => {
                return Promise.reject(err.response);
            })
    },
    getLeaderBoard(accesstoken: string) {
        return User_Endpoint.get(`/leaderboard`, {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'application/json'
            }
        })
            .then((res) => {
                return res;
            })
            .catch((err) => {
                return Promise.reject(err.response);
            })
    },
    refreshToken(id:string, accesstoken: string, refresh: string) {
        return RefreshTk_Endpoint.post(`/${id}`, {
            refreshToken: refresh
        }, {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
                'Content-Type': 'application/json'
            }
        })
            .then((res) => {
                return res;
            })
            .catch((err) => {
                return Promise.reject(err.response);
            })
    }
}