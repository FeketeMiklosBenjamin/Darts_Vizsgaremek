import type RegisterModel from "@/models/RegisterModel";
import axios from './BackendService'
import type LoginModel from "@/models/LoginModel";


export default {
    registerUser(data: RegisterModel) {
        return axios.post('/register', data)
            .then((res) => {
                return res;
            })
            .catch((err) => {
                return Promise.reject(err.response)
            })
    },
    loginUser(data: LoginModel) {
        return axios.post('/login', data)
            .then((res) => {
                return res;
            })
            .catch((err) => {
                return Promise.reject(err.response)
            })
    },
    logoutUser(accesstoken: string, refresh: string) {
        return axios.post('/logout', {
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
    uploadImage(image: File, accesstoken: string) {
        const formData = new FormData();
        formData.append('file', image);

        return axios.post('/picture/upload', formData, {
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
    }
}