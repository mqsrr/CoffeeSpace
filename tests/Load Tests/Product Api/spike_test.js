import http from 'k6/http'
import { sleep } from 'k6'

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,

    stages: [
        { duration: '10s', target: 100 },
        { duration: '1m', target: 100 },
        { duration: '10s', target: 1400 },
        { duration: '3m', target: 1400 },
        { duration: '10s', target: 100 },
        { duration: '3m', target: 100 },
        { duration: '10s', target: 0 },
    ],
};

const API_BASE_URL = 'http://localhost:8085';
const headers = {
    Authorization: `Bearer ${__ENV.JWT_TOKEN}`,
    'Content-Type': 'application/json',
};

export default () => {
    http.batch([
        ['GET', `${API_BASE_URL}/products`, null, {headers: headers}],
        ['GET', `${API_BASE_URL}/products/${__ENV.PRODUCT_ID}`, null,  {headers: headers}]
    ]);
    sleep(1);
};
