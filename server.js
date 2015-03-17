/**
 * Created by toby on 2/22/15.
 */
var io = require('socket.io')({
    transports: ['websocket']
});
var Enum = require('enum');
var playerState = new Enum('idle','jammed','fired','killed');
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
        console.log("recieved a challenge to code " + challenge.code);
        var gameCode = challenge.code;
        if (games.hasOwnProperty(gameCode)) {
            console.log("code is valid");
            if (games[gameCode].opponent === null) {
                io.to(games[gameCode].host).emit('challengePosted',{code:challenge.challengerGameCode});
                games[gameCode].opponent = challenge.challengerId;
            }
            else {
                socket.emit('challengedIsBusy');
            }
        }
        else{
            console.log("Code is invalid");
            socket.emit('invalidCode');
        }
    });
    //Need to also set the users opponent field to match
    socket.on('rejectChallenge', function(data){
        io.to(games[data.code].host).emit("challengeRejected");
        games[data.code].opponent =null;
    });
    socket.on('acceptChallenge', function(data){
        socket.join(data.code);
        socket.to(games[data.code].host).emit("challengeAccepted");
        io.sockets.in(games[data.code]).emit('gameBegin');
    });
});
