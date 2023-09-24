import http from 'k6/http'
import { sleep } from 'k6'

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,

    stages: [
        { duration: '2m', target: 400 },
        { duration: '3h56m', target: 400 },
        { duration: '2m', target: 0 },
    ],
};


const API_BASE_URL = 'http://localhost:8085';
const headers = {
    Authorization: `Bearer ${__ENV.JWT_TOKEN}`,
    'Content-Type': 'application/json',
};

export default () => {
    http.batch([
        ['GET', `${API_BASE_URL}/buyers/${__ENV.BUYER_ID}`, null, {headers: headers}],
        ['GET', `${API_BASE_URL}/buyers/${__ENV.BUYER_ID}/orders`, null, {headers: headers}],
        ['GET', `${API_BASE_URL}/buyers/${__ENV.BUYER_ID}/orders/${__ENV.ORDER_ID}`, null, {headers: headers}]
    ]);
    sleep(1);
};