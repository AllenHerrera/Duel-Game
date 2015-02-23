/**
 * Created by toby on 2/22/15.
 */

var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);


var games = {};


http.listen(3000, function(){
    console.log('listening on *:3000');
});
io.on('connection', function(socket){
    console.log('a user connected');
    var gameCode = '----';
    socket.on('disconnect', function(){
        console.log('- user disconnected');
        if(games.hasOwnProperty(gameCode) &&  games[gameCode].hostID === socket.id) {
            console.log('- deleted ' + games[gameCode]);
            // emit a disconnect to all other connected clients in the room
            io.sockets.in(gameCode).emit('hostDisconnect');
            delete games[gameCode];
        }
    });
    socket.on('requestCode', function() {
        var code;
        do {
            var charSet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
            var randomString = '';
            for (var i = 0; i < 4; i++) {
                var randomPoz = Math.floor(Math.random() * charSet.length);
                randomString += charSet.substring(randomPoz,randomPoz+1);
            }
            code = randomString;
        } while(games.hasOwnProperty(code));
        socket.emit('roomCodeCreated', {userCode:code,userId:socket.id});
        gameCode = code;
        socket.join(code);
        games[gameCode].host = socket.id;
        games[gameCode].opponent = null;
    });
    socket.on('challenge', function(challenge) {
        var gameCode = challenge.code;
        if (games.hasOwnProperty(gameCode)) {
            if (games[gameCode].opponent === null) {
                socket.to(games[gameCode].host).emit('challengePosted');
                games[gameCode].opponent = challenge.challengerId;
            }
            else {
                socket.to(challenge.challengerId).emit('challengedIsBusy');
            }
        }
    });
    socket.on('rejectChallenge', function(){
        socket.to(games[gameCode].opponent).emit("challengeRejected");
        games[gameCode].opponent =null;
    });
    socket.on('acceptChallenge', function(){
        io.sockets[games[gameCode].opponent].join(gameCode);
        socket.to(games[gameCode].opponent).emit("challengeAccepted");
        io.sockets.in(games[gameCode]).emit('gameBegin');
    });
});
