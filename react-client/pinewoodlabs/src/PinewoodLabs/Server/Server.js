/**
 * Not a React class. Just a regular ES6 class.
 */
require('../Models/Racer')

class Server {
    // still going to follow some React patterns
    // for consistency.
    constructor() {
        this.io = require('socket.io')();
    }

    start() {
        this.io.on('connection', (socket) => {
            console.log("New Connection!");
        
            this.updateCar(socket);
            this.startRace(socket);
            this.endRace(socket);
            this.sendStats(socket);
        });
        
        const port = 8000;
        this.io.listen(port);
        console.log('Listening on port ', port);
    }

    updateCar(socket) {
        socket.on('updateCar', (data) => {
            console.log('updateCar:');
            console.log(data);
            this.io.sockets.emit('updateCar', data);
        });
    }

    startRace(socket) {
        socket.on('startRace', (data) => {
            console.log('startRace');
            this.io.sockets.emit('startRace', data);
        });
    }

    endRace(socket) {
        socket.on('endRace', (data) => {
            console.log('endRace');
            this.io.sockets.emit('endRace', data);
        });
    }

    sendStats(socket) {
        socket.on('sendStats', (data) => {
            console.log('sendStats');
            console.log(data);
            this.io.sockets.emit('sendStats', data);
        });
    }

}

const server = new Server();
server.start();