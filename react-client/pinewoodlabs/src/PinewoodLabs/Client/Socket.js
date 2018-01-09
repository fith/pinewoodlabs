/**
 * Not a React class. Just a regular ES6 class.
 * Still going to follow some React patterns
 * for consistency.
 */
const io = require('socket.io-client');

class Socket {
    constructor(props) {
        this.socket = io('https://socket.pinewoodlabs.xyz'); // Should be in a config file.
        this.socket.on('connection', (socket) => {
            this.token = socket.handshake.query.token;
            this.clientid = socket.clientid;
        });
    }

    emit(message, data) {
        this.socket.emit(message, data);
    }

    on(message, fn) {
        this.socket.on(message, fn);
    }
}

export default Socket;