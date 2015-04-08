/**
 * Created by toby on 2/22/15.
 * MICROSOFT REFERENCE SOURCE LICENSE (MS-RSL)

 This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.

 1. Definitions

 The terms "reproduce," "reproduction" and "distribution" have the same meaning here as under U.S. copyright law.

 "You" means the licensee of the software.

 "Your company" means the company you worked for when you downloaded the software.

 "Reference use" means use of the software within your company as a reference, in read only form, for the sole purposes of debugging your products, maintaining your products, or enhancing the interoperability of your products with the software, and specifically excludes the right to distribute the software outside of your company.

 "Licensed patents" means any Licensor patent claims which read directly on the software as distributed by the Licensor under this license.

 2. Grant of Rights

 (A) Copyright Grant- Subject to the terms of this license, the Licensor grants you a non-transferable, non-exclusive, worldwide, royalty-free copyright license to reproduce the software for reference use.

 (B) Patent Grant- Subject to the terms of this license, the Licensor grants you a non-transferable, non-exclusive, worldwide, royalty-free patent license under licensed patents for reference use.

 3. Limitations

 (A) No Trademark License- This license does not grant you any rights to use the Licensor's name, logo, or trademarks.

 (B) If you begin patent litigation against the Licensor over patents that you think may apply to the software (including a cross-claim or counterclaim in a lawsuit), your license to the software ends automatically.

 (C) The software is licensed "as-is." You bear the risk of using it. The Licensor gives no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the Licensor excludes the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
 */
var io = require('socket.io')({
    transports: ['websocket']
});
//var Enum = require('enum');
io.attach(3001);

//var playerState = new Enum(['idle','jammed','fired','killed']);
//var gameState = new Enum(['inactive', 'active','over']);
//Will implement enums in the future. Currently using ints
//player state ints(0:idle,1:fired,2:jammed,3:dead)
//game state ints(0:inactive,1:active,:2,over)

var games = {};
var players = {};
var channels = {};
var matchmaking = [];//queue

function playGame(data) {
    //Choose random  time in future to enable draw
    console.log('beginning game loop');
    if (channels[data.channel] !== undefined)
        channels[data.channel].gameState = 1;
    var delay = Math.random() * 20000;
    var gameLoop = function() {
        console.log('draw loop iteration beginning');
        if (channels[data.channel] === undefined) {
            console.log('game has been deleted. Ending loop');
            if (endDraw !== undefined)
                clearTimeout(endDraw);
            return;
        }
        if (channels[data.channel].gameState == 2) {
            console.log('game has ended, ending loop');
            if (endDraw !== undefined)
                clearTimeout(endDraw);
            return;
        }
        if (channels[data.channel].gameState === 1) {
            delay = Math.random() * 20000;
            var proc = Math.random().toFixed(2);
            if (proc > .75) {
                io.to(data.channel).emit('draw');
                channels[data.channel].drawActive = true;
                console.log('draw state entered');
            } else {
                io.to(data.channel).emit('distraction', { value: proc });
                channels[data.channel].drawActive = false;
                console.log('draw state entered');
            }
            var endDraw = setTimeout(function() {
                io.to(data.channel).emit('endDraw');
                if (channels[data.channel] !== undefined) {
                    channels[data.channel].drawActive = false;
                    console.log('draw state ended');
                }
            }, 3000);
            loop = setTimeout(gameLoop, Math.max(delay, 5000));
        }
    };
    var loop = setTimeout(gameLoop, Math.max(delay, 8000));
    var gameTest = function() {
        if (channels[data.channel] === undefined || channels[data.channel].gameState !== 1) {
            console.log('Game is over, ending loop');
            clearTimeout(loop);
        } else testLoop = setTimeout(gameTest, 500);
    };
    var testLoop = setTimeout(gameTest, 500);
}
function removeMatch(currentGame){
    for (var i = 0; i < matchmaking.length; i++) {
        if (matchmaking[i] === currentGame)
            matchmaking.splice(i, 1);
        console.log('deleted matchmaking match');
        console.log(matchmaking);
        break;
    }
}

console.log('server started');
io.on('connection', function (socket) {
    function getCurrentState() {
        //return states of players and game as JSON object
        if (games[playerCode].player2state === undefined)
            games[playerCode].player2state = 0;
        return {
            player1state: games[playerCode].player1state,
            player2state: games[playerCode].player2state,
            gameState: games[playerCode].gameState
        }
    }

    console.log('a user connected');
    var playerCode = '----';
    var currentGame = null;
    socket.on('disconnect', function () {
        console.log('- user disconnected');
        if (players.hasOwnProperty(playerCode)) {
            console.log('- deleted ' + players[playerCode]);
            delete players[playerCode];
            console.log(players);
            if(players[playerCode] !== undefined && players[playerCode].currentGame!==null)
                removeMatch(players[playerCode].currentGame);
            players[playerCode].currentGame=null;
        }
        if (games.hasOwnProperty(playerCode)) {
            console.log('- deleted ' + games[playerCode]);
            var data = games[playerCode].channel;
            delete channels[data];
            //emit a disconnect to all other connected clients in the room
            io.to(games[playerCode].channel).emit('playerDisconnected', {channel: data});
        }
    });
    socket.on('requestPlayerCode', function () {
        var code;
        do {
            var charSet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
            var randomString = '';
            for (var i = 0; i < 4; i++) {
                var randomPoz = Math.floor(Math.random() * charSet.length);
                randomString += charSet.substring(randomPoz, randomPoz + 1);
            }
            code = randomString;
        } while (players.hasOwnProperty(code));
        playerCode = code;
        players[playerCode] = {};
        players[playerCode].id = socket.id;
        players[playerCode].isBusy = false;
        players[playerCode].code = playerCode;
        players[playerCode].currentGame = null;
        socket.emit('playerCodeCreated', {code: playerCode})
    });

    socket.on('findMatch', function () {
        //look through players already looking for matches and join one, otherwise create lobby and wait for someone to join. Timeout after a while
        if(matchmaking.length > 0) {
            var game = matchmaking.shift();
            socket.join(game.channel);
            games[playerCode] = game;
            games[playerCode].player2 = players[playerCode];
            setTimeout(function () {
                    if (games[playerCode] !== undefined) {
                        console.log('all games are:');
                        console.log(games);
                        console.log('game is beginning. Game information :');
                        console.log(games[playerCode]);
                        var playerStatus = {player1:games[playerCode].player1.code};
                        io.to(games[playerCode].channel).emit('beginGame', playerStatus);
                        console.log("current game state is: ");
                        console.log(getCurrentState());
                        playGame({channel: games[playerCode].channel});
                    }
                }
                , 3000);
        }
        else{
            players[playerCode].isBusy = true;
            //Create a new game and add it to the games list
            //generate unique channel code
            do
            {
                var channelCode = Math.random().toString(36).slice(2).substring(0, 4).toUpperCase();
            } while (channels.hasOwnProperty(channelCode));
            var game =
            {
                channel: channelCode,
                gameState: 0,
                drawActive: false,
                player1: players[playerCode],
                player2: null,
                player1state: 0,
                player2State: 0
            };
            socket.join(game.channel);
            games[playerCode] = game;
            channels[channelCode] = game;
            matchmaking.push(game);
            players[playerCode].currentGame = game;
            setTimeout(function() {
                if(games[playerCode]!==undefined && games[playerCode].player2 === null) {
                    var message = {};
                    message.message = 'Matchmaking timed out. Try again or challenge a friend.';
                    socket.emit('connectionError', message);
                    if(players[playerCode].currentGame!==null)
                        removeMatch(players[playerCode].currentGame);
                    players[playerCode].currentGame=null;
                }
            },60000);
        }
    });
    socket.on('challenge', function (data) {
        var code = data.code;
        var message={};
        if (players.hasOwnProperty(data.code) && data.code !== playerCode) {
            console.log("code is valid");
            if (players[data.code].isBusy === false || (games[data.challengerId]!== undefined && games[data.challengerId] === games[playerCode])) {
                if(games[data.challengerId]!== undefined && games[data.challengerId] === games[playerCode]){
                    console.log('players are already in game. Disconnecting them from previous session');
                    io.to(games[data.challengerId].channel).emit('disconnectFromRoom', {channel:games[data.challengerId].channel});
                    delete games[data.code];
                }
                io.to(players[data.code].id).emit('challengePosted', {id: data.challengerId});
                //Set both players as currently busy until challenge is accepted or declined
                players[playerCode].isBusy = true;
                players[data.code].isBusy = true;
                //Create a new game and add it to the games list
                //generate unique channel code
                do
                {
                    var channelCode = Math.random().toString(36).slice(2).substring(0, 4).toUpperCase();
                } while (channels.hasOwnProperty(channelCode));
                var game =
                {
                    channel: channelCode,
                    gameState: 0,
                    drawActive: false,
                    player1: players[playerCode],
                    player2: null,
                    player1state: 0,
                    player2State: 0
                };
                socket.join(game.channel);
                games[playerCode] = game;
                channels[channelCode] = game;
            }
            else {
                message.message='Player with code ' + data.code +' is currently in a duel.';
                socket.emit('connectionError',message);
            }
        }
        else {
            console.log("Code is invalid");
            message.message='There is no player with code ' + data.code +' to challenge.';
            socket.emit('connectionError',message);
        }
    });
    socket.on('playerDisconnected', function (data) {
        //last player in game deletes game
        console.log(data);
        socket.leave(data.channel);
        players[playerCode].isBusy = false;
    });
    socket.on('cancelChallenge', function (data) {
        //Delete game object and allow challenges for both players
        socket.leave(games[playerCode].channel);
        delete games[playerCode];
        players[playerCode].isBusy = false;
        //If direct challenge (e.g. not matchmaking) then cancel challenge
        if(data.code !== undefined && players[data.code]!==undefined) {
            players[data.code].isBusy = false;
            io.to(players[data.code].id).emit("challengeCanceled");
        }
        //if in matchmaking then delete match
        else{
            if(players[playerCode].currentGame!==null)
                removeMatch(players[playerCode].currentGame);
            players[playerCode].currentGame=null;
        }
    });
    socket.on('rejectChallenge', function (data) {
        //Delete game object and allow challenges for both players
        delete games[data.challengerId];
        players[data.challengerId].isBusy = false;
        players[playerCode].isBusy = false;
        var message={};
        message.message='Your challenge was rejected by ' + playerCode +'.';
        io.to(players[data.challengerId].id).emit('connectionError',message);

    });
    socket.on('acceptChallenge', function (data) {
        socket.join(games[data.challengerId].channel);
        games[playerCode] = games[data.challengerId];
        if (games[playerCode].player2 === null)
            games[playerCode].player2 = players[playerCode];
        socket.to(players[data.challengerId].id).emit("challengeAccepted");
        setTimeout(function () {
                if (games[playerCode] !== undefined) {
                    console.log('all games are:');
                    console.log(games);
                    console.log('game is beginning. Game information :');
                    console.log(games[playerCode]);
                    var playerStatus = {player1:games[playerCode].player1.code};
                    io.to(games[playerCode].channel).emit('beginGame', playerStatus);
                    console.log("current game state is: ");
                    console.log(getCurrentState());
                    playGame({channel: games[playerCode].channel});
                }
            }
            , 3000);
    });
    socket.on('processInput', function () {
        if (games[playerCode] !== undefined && games[playerCode].gameState === 1) {
            var isPlayer1 = (games[playerCode].player1.id === socket.id);
            //handle gun jams
            if (!games[playerCode].drawActive) {
                //set player state
                if (isPlayer1) {
                    if (games[playerCode].player1state === 2)
                        return;
                    games[playerCode].player1state = 2;
                }
                else {
                    if (games[playerCode].player2state === 2)
                        return;
                    games[playerCode].player2state = 2;
                }
                //set update states to be sent to clients
                //send current states to game
                io.to(games[playerCode].channel).emit('gameUpdate', getCurrentState());
                console.log(playerCode + 'is jammed');
                setTimeout(function () {
                    if (games[playerCode].gameState === 1) {
                        if (isPlayer1) {
                            games[playerCode].player1state = 0;
                        }
                        else {
                            games[playerCode].player2state = 0;
                        }
                        io.to(games[playerCode].channel).emit('gameUpdate', getCurrentState());
                        console.log(playerCode + 'is no longer jammed');
                    }
                }, 5000)
            }
            else {
                if (isPlayer1 && games[playerCode].player1state !== 2) {
                    games[playerCode].gameState = 2;
                    console.log('player 1 wins!');
                    games[playerCode].player1state = 1;
                    games[playerCode].player2state = 3;
                    io.to(games[playerCode].channel).emit('gameUpdate', getCurrentState());

                }
                if (!isPlayer1 && games[playerCode].player2state !== 2) {
                    games[playerCode].gameState = 2;
                    console.log('player 2 wins!');
                    games[playerCode].player1state = 3;
                    games[playerCode].player2state = 1;
                    io.to(games[playerCode].channel).emit('gameUpdate', getCurrentState());
                }
                //Delete game and unsubscribe players to channel if match was matchmakingmatch
                console.log(players[playerCode].currentGame);
                console.log(players[games[playerCode].player2.code].currentGame);
                if(players[playerCode].currentGame!==null || players[games[playerCode].player2.code].currentGame!==null){
                    console.log("match made game has ended. SHould be deleting game");
                    io.to(games[data.challengerId].channel).emit('disconnectFromRoom', {channel:games[data.challengerId].channel});
                    console.log(games);
                    players[playerCode].currentGame = null;
                    players[games[playerCode].player2.code].currentGame =null;
                    delete games[playerCode];
                }
            }
        }
    });
    socket.on('reset', function () {
        if (games.hasOwnProperty(playerCode)) {
            console.log('resetting game ' + games[playerCode]);
            var data = games[playerCode].channel;
            socket.leave(data);
            players[playerCode].isBusy = false;
            delete channels[data];
            var message={};
            message.message='Your opponent has disconnected.';
            io.sockets.in(games[playerCode].channel).emit('connectionError',message);
            //emit a disconnect to all other connected clients in the room
            io.sockets.in(games[playerCode].channel).emit('playerDisconnected', {channel: data});
        }
    });
});
