import axios from 'axios';

const User_Endpoint = axios.create({
    baseURL: import.meta.env.VITE_USER_BACKEND_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

const Messages_Endpoint = axios.create({
    baseURL: import.meta.env.VITE_MESSAGES_BACKEND_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

export { User_Endpoint, Messages_Endpoint };