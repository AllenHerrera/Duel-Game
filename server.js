/**
 * Created by toby on 2/22/15.
 */
var io = require('socket.io')({
    transports: ['websocket']
});
var Enum = require('enum');
io.attach(3000);

var playerState = new Enum('idle','jammed','fired','killed');
var games = {};
var players = {};
var channels={};

console.log('server started');
io.on('connection', function(socket){
    console.log('a user connected');
    var playerCode = '----';

    socket.on('disconnect', function(){
        console.log('- user disconnected');
        if(players.hasOwnProperty(playerCode)){
            console.log('- deleted ' + players[playerCode]);
            delete players[playerCode];
        }
        if(games.hasOwnProperty(playerCode)){
            console.log('- deleted ' + games[playerCode].channel);
            // emit a disconnect to all other connected clients in the room
            io.sockets.in(games[playerCode].channel).emit('playerDisconnected');
            delete games[playerCode];
        }
    });
    socket.on('requestPlayerCode', function() {
        var code;
        do {
            var charSet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
            var randomString = '';
            for (var i = 0; i < 4; i++) {
                var randomPoz = Math.floor(Math.random() * charSet.length);
                randomString += charSet.substring(randomPoz,randomPoz+1);
            }
            code = randomString;
        } while(players.hasOwnProperty(code));
        playerCode = code;
        players[playerCode]={};
        players[playerCode].id=socket.id;
        players[playerCode].isBusy = false;
        socket.emit('playerCodeCreated', {code:playerCode})
    });
    socket.on('challenge', function(data) {
        console.log("received a challenge to code " + data.code);
        var code = data.code;
        if (players.hasOwnProperty(data.code)) {
            console.log("code is valid");
            if (players[data.code].isBusy === false) {
                console.log('posting challenge!');
                io.to(players[data.code].id).emit('challengePosted',{id:data.challengerId});
                //Set both players as currently busy until challenge is accepted or declined
                players[playerCode].isBusy = true;
                players[data.code].isBusy=true;
                //Create a new game and add it to the games list
                //generate unique channel code
                do
                {
                    var channelCode = Math.random().toString(36).slice(2).substring(0,4).toUpperCase();
                } while(channels.hasOwnProperty(channelCode));
                var game=
                {
                    channel:channelCode,
                    player1:players[playerCode],
                    player2:null
                    //player1State:state,
                    //player2State:state,
                };
                //challenger is added to game channel
                //socket.join(game.channel);
		socket.join('test');
		console.log(socket.rooms);
                games[playerCode] = game;
                channels[channelCode]=game;                
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
    //Recieve challenger's id
    socket.on('rejectChallenge', function(data){
        //Delete game object and allow challenges for both players
        delete games[data.challengerId];
        players[data.challengerId].isBusy = false;
        players[playerCode].isBusy = false;
        io.to(players[data.challengerId].id).emit("challengeRejected");
    });
    //recieve challenge id
    socket.on('acceptChallenge', function(data){
	socket.join('test');
        //socket.join(games[data.challengerId].channel);
        games[playerCode] = games[data.challengerId];
        games[playerCode].player2=players[playerCode];
	console.log(socket.id);
	console.log(socket.rooms);
        socket.to(games[playerCode].player1.id).emit("challengeAccepted");
	console.log("Game should be beginning");
	console.log(games[playerCode]);
	socket.to(games[playerCode].player1.id).emit("gameBegin");
	socket.to(games[playerCode].player2.id).emit("gameBegin");       
	// io.sockets.in(games[playerCode].channel).emit('gameBegin');
    });
});
