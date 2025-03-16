import type UserFeedModel from '@/models/UserFeedModel';
import { Messages_Endpoint } from './BackendService';

export default {
    userFeed(accesstoken: string, data: UserFeedModel) {
        return Messages_Endpoint.post('/send/user', data, {
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
    }
}