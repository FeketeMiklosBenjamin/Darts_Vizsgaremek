import type UserFeedModel from "@/models/UserFeedModel";
import MessagesService from "@/services/MessagesService";
import { defineStore } from "pinia";


export const useMessagesStore = defineStore('messagesStore', {
    state: () => ({
        status: {
            resp: '',
            success: false
        }
    }),
    actions: {
        sendUserFeed(accesstoken: string, data: UserFeedModel) {
            return MessagesService.userFeed(accesstoken, data)
                .then(() => {
                    this.status.success = true
                    this.status.resp = 'Sikeresen elküldte az üzenetet!'
                })
                .catch((err) => {
                    this.status.success = false;
                    this.status.resp = err.data.message;
                    return Promise.reject(err);
                })
        }
    }
})