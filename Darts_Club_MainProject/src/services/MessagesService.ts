import type UserFeedModel from '@/models/UserFeedModel';
import { Messages_Endpoint } from './BackendService';

export default {
    getMessages(accesstoken: string) {
        return Messages_Endpoint.get('/', {
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
    userFeed(accesstoken: string, data: UserFeedModel) {
        return Messages_Endpoint.post('/send/user', data, {
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
    adminFeed(accesstoken: string, data: UserFeedModel) {
        return Messages_Endpoint.post('/send/admin', data, {
            headers: {
                Authorization: `Bearer ${accesstoken}`,
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
    deleteMessage(id: string, accesstoken: string) {
        return Messages_Endpoint.delete(`/${id}`, {
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