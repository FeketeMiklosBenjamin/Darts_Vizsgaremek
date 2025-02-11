import type RegisterModel from "@/models/RegisterModel";
import axios from './BackendService'
import type LoginModel from "@/models/LoginModel";
import type UserModel from "@/models/UserModel";


const token = sessionStorage.getItem('accessToken');

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
    logoutUser(token: string){
        return axios.post('/logout','',{
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
        .then((res)=>{
            return res.data
        })
        .catch(()=>{
            return Promise.reject()
        })
    }
}