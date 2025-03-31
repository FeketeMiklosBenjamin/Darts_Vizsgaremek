import type UserFeedModel from "@/models/UserFeedModel";
import MessagesService from "@/services/MessagesService";
import { defineStore, storeToRefs } from "pinia";
import { useUserStore } from "./UserStore";
import type AdminEmailModel from "@/models/AdminEmailModel";

export const useMessagesStore = defineStore('messagesStore', {
    state: () => ({
        status: {
            resp: '',
            success: false
        },
        forUserEmails: [] as UserFeedModel[],
        forAdminEmails: [] as AdminEmailModel[]
    }),
    actions: {
        getYourMessages(accesstoken: string) {
            return MessagesService.getMessages(accesstoken)
                .then((res) => {
                    const userStore = useUserStore();
                    const { user } = storeToRefs(userStore);
                    if (user.value.role != 2) {
                        this.forUserEmails = res.data.map((email: UserFeedModel) => ({
                            id: email.id,
                            title: email.title,
                            text: email.text,
                            sendDate: email.sendDate
                        }));
                    } else {
                        this.forAdminEmails = res.data.map((email: AdminEmailModel) => ({
                            id: email.id,
                            title: email.title,
                            emailAddress: email.emailAddress,
                            text: email.text,
                            sendDate: email.sendDate
                        }))
                    }
                })
                .catch((err) => {
                    return Promise.reject(err);
                })
        },
        sendUserFeed(accesstoken: string, data: UserFeedModel) {
            return MessagesService.userFeed(accesstoken, data)
                .then(() => {
                    this.status.success = true;
                    this.status.resp = 'Sikeresen elküldte az üzenetet!';
                })
                .catch((err) => {
                    this.status.success = false;
                    this.status.resp = err.data.message;
                    return Promise.reject(err);
                })
        },
        sendAdminFeed(accesstoken: string, data: UserFeedModel) {
            return MessagesService.adminFeed(accesstoken, data)
                .then(() => {
                    this.status.success = true;
                    this.status.resp = 'Sikeresen elküldte az üzenetet!';
                })
                .catch((err) => {
                    this.status.success = false;
                    this.status.resp = err.data.message;
                    return Promise.reject(err);
                })
        }
    }
})