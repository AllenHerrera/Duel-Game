/**
 * Created by toby on 2/22/15.
 */
var io = require('socket.io')({
    transports: ['websocket']
});

io.attach(3000);

var games = {};
io.on('connection', function(socket){
    console.log('a user connected');
    var gameCode = '----';
    socket.on('disconnect', function(){
        console.log('- user disconnected');
        if(games.hasOwnProperty(gameCode) &&  games[gameCode].host === socket.id) {
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
        socket.emit('gameCodeCreated', {gameCode:code,userId:socket.id});
        gameCode = code;
        socket.join(code);
        games[gameCode] = {};
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
        else{
            socket.to(challenge.challengerId).emit('invalidCode');
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
