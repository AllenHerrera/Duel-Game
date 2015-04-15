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
var moment = require('moment');
var MongoClient = require('mongodb').MongoClient;
moment().format();
var io = require('socket.io')({
    transports: ['websocket']
});
io.attach(3001);
MongoClient.connect("mongodb://localhost:27017/duelLeaderBoard", function (err, db) {
    if (!err) {
        console.log("Connected to database.");
    }
//player state ints(0:idle,1:fired,2:jammed,3:dead, 4:pending)
//game state ints(0:inactive,1:active,:2,over)
    console.log('server started');
//Server level variables
    var pingtime;
    var games = {};
    var players = {};
    var channels = {};
    var matchmaking = [];
//ping all players every few seconds
    setInterval(function () {
        io.emit('ping');
        pingtime = moment().valueOf();
    }, 3000);
    io.on('connection', function (socket) {
            console.log('a user connected');
            //INITIALIZE SOCKET VARIABLES
            var playerCode = '----';
            var suggestAiTimeout = null;
            var matchmakingTimeout = null;
            //FUNCTIONS
            function beginGame(game)//After delay indicate to clients that they should begin game
            {
                setTimeout(function () {
                    if (game !== undefined) {
                        console.log('beginning game ');
                        console.log(games[game.player1.code]);
                        console.log(games[game.player2.code]);
                        console.log('-----------------------');
                        var playerStatus = {player1: game.player1.code};
                        io.to(game.channel).emit('beginGame', playerStatus);
                        playGame({channel: game.channel});
                    }
                }, 3000);
            }

            function playGame(data)//Starts and runs game loop which spawns distractions/draw events at random intervals
            {
                //Choose random  time in future to enable draw
                console.log('beginning game loop');
                if (channels[data.channel] !== undefined) {
                    channels[data.channel].drawActive = false;
                    channels[data.channel].gameState = 1;
                }
                var delay = Math.random() * 12000;
                var gameLoop = function () {
                    console.log('draw loop iteration beginning');
                    if (channels[data.channel] === undefined) {
                        console.log('game has been deleted. Ending loop');
                        if (endDraw !== undefined)
                            clearTimeout(endDraw);
                        return;
                    }
                    if (channels[data.channel].gameState === 2) {
                        console.log('game has ended, ending loop');
                        if (endDraw !== undefined)
                            clearTimeout(endDraw);
                        return;
                    }
                    if (channels[data.channel].gameState === 1) {
                        delay = Math.random() * 15000;
                        var proc = Math.random().toFixed(2);
                        if (proc > .66) {
                            io.to(data.channel).emit('draw');
                            channels[data.channel].drawActive = true;
                            console.log('draw state entered');
                        } else {
                            io.to(data.channel).emit('distraction', {value: proc});
                            channels[data.channel].drawActive = false;
                            console.log('distraction state entered');
                        }
                        var endDraw = setTimeout(function () {
                            io.to(data.channel).emit('endDraw');
                            if (channels[data.channel] !== undefined) {
                                channels[data.channel].drawActive = false;
                                console.log('draw state ended');
                            }
                        }, 3000);
                        loop = setTimeout(gameLoop, Math.max(delay, 5000));
                    }
                };
                var loop = setTimeout(gameLoop, Math.max(delay, 5000));
                var gameTest = function () {
                    if (channels[data.channel] === undefined || channels[data.channel].gameState !== 1) {
                        //ping both players
                        console.log('Game is over, ending loop');
                        clearTimeout(loop);
                    } else testLoop = setTimeout(gameTest, 500);
                };
                var testLoop = setTimeout(gameTest, 500);
            }

            function removeMatch(currentGame)//Removes a player from matchmaking after they cancel or find a match
            {
                for (var i = 0; i < matchmaking.length; i++) {
                    if (matchmaking[i] === currentGame)
                        matchmaking.splice(i, 1);
                    console.log('deleted matchmaking match');
                    console.log(matchmaking);
                    break;
                }
            }

            function getCurrentState()//Return state of game and each player
            {
                //return states of players and game as JSON object
                if (games[playerCode].player2state === undefined)
                    games[playerCode].player2state = 0;
                return {
                    player1state: games[playerCode].player1state,
                    player2state: games[playerCode].player2state,
                    gameState: games[playerCode].gameState
                }
            }

            function clearTimeouts()//Clear timeouts that drive prompts for matchmaking ai and timeout
            {
                if (matchmakingTimeout !== null) {
                    clearTimeout(matchmakingTimeout);
                    matchmakingTimeout = null;
                }
                if (suggestAiTimeout !== null) {
                    clearTimeout(suggestAiTimeout);
                    matchmakingTimeout = null;
                }
            }

            function evaluateDeltas(delta, game)//In case of 2 player inputs within input window evaluate who won by comparing deltas in response time
            {
                if (delta < game.winnerDelta) {
                    switch (game.winner) {
                        case 1:
                            games[playerCode].gameState = 2;
                            games[playerCode].player1state = 1;
                            games[playerCode].player2state = 3;
                            break;
                        case 2:
                            games[playerCode].gameState = 2;
                            games[playerCode].player1state = 3;
                            games[playerCode].player2state = 1;
                            break;
                    }
                    io.to(games[playerCode].channel).emit('gameUpdate', getCurrentState());
                    updateStreaks(game);
                    removeGame(game);
                }
            }

            function removeGame(game)//Remove games when no longer needed
            {
                if (game.player1.currentGame !== null || (game.player2 !== null && game.player2.currentGame !== null)) {
                    console.log("match made game has ended. Should be deleting game");
                    io.to(games[playerCode].channel).emit('disconnectFromRoom', {channel: games[playerCode].channel});
                    players[playerCode].currentGame = null;
                    if (games[playerCode].player !== null)
                        players[games[playerCode].player2.code].currentGame = null;
                    delete games[playerCode];
                }
            }

            function updateStreaks(game)//update streaks after game result
            {
                if (game.player1.streak >= 0 && game.player1state === 3) {
                    game.player1.streak = 0;
                }
                else {
                    game.player1.streak++;
                    console.log(game.player1.streak);
                    updateRecord(game.player1);
                }
                if (game.player2 !== null) {
                    if (game.player2.streak >= 0 && game.player2state === 3) {
                        game.player2.streak = 0;
                    }
                    else {
                        game.player2.streak++;
                        updateRecord(game.player2);
                    }
                }
            }

            function initializeRecord(player)//After player gets to minimum streak create a db record and save id
            {
                var document = {
                    name: player.name,
                    streak: player.streak
                };
                db.collection('players').insert(document, function (err, records) {
                    if (err) throw err;
                    player.dbId = records.ops[0]._id;
                });
            }

            function updateRecord(player)//Update player record to reflect increase in streak
            {
                var record;
                db.collection('players').findOne({_id: player.dbId}, function (err, document) {
                    if (err) throw err;
                    record = document;
                    console.log(record.streak);
                    if (record.streak < player.streak) {
                        db.collection('players').update(
                            {_id: player.dbId},
                            {
                                $set: {
                                    name: player.name,
                                    streak: player.streak
                                }
                            }
                        ), function(err, count, success){
                            if(err)throw err;
                            console.log(success);
                        }
                    }
                });
            }

            //SOCKET LISTENERS
            //Startup
            socket.on('requestPlayerCode', function ()//Assign player a unique code
            {
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
                players[playerCode].ping = 0;
                players[playerCode].streak = 0;
                players[playerCode].name = '----';
                players[playerCode].dbId = '---';
                socket.emit('playerCodeCreated', {code: playerCode});
                console.log('player code assigned: ' + code);
            });
            //Challenging
            socket.on('challenge', function (data)//Issue a challenge to a player code
            {
                var code = data.code;
                var message = {};
                if (players.hasOwnProperty(data.code) && data.code !== playerCode) {
                    console.log("code is valid");
                    if (players[data.code].isBusy === false || (games[data.challengerId] !== undefined && games[data.challengerId] === games[playerCode])) {
                        if (games[data.challengerId] !== undefined && games[data.challengerId] === games[playerCode]) {
                            console.log('players are already in game. Disconnecting them from previous session');
                            io.to(games[data.challengerId].channel).emit('disconnectFromRoom', {channel: games[data.challengerId].channel});
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
                            player2State: 0,
                            inputWindow: true,
                            winner: 0,
                            winnerDelta: 0

                        };
                        socket.join(game.channel);
                        games[playerCode] = game;
                        channels[channelCode] = game;
                    }
                    else {
                        message.message = 'Player with code ' + data.code + ' is currently in a duel.';
                        socket.emit('connectionError', message);
                    }
                }
                else {
                    console.log("Code is invalid");
                    message.message = 'There is no player with code ' + data.code + ' to challenge.';
                    socket.emit('connectionError', message);
                }
            });
            socket.on('cancelChallenge', function (data)//Cancel a challenge to a player code/quit matchmaking
            {
                //Delete game object and allow challenges for both players
                socket.leave(games[playerCode].channel);
                delete games[playerCode];
                players[playerCode].isBusy = false;
                //If direct challenge (e.g. not matchmaking) then cancel challenge
                if (data.code !== undefined && players[data.code] !== undefined) {
                    players[data.code].isBusy = false;
                    io.to(players[data.code].id).emit("challengeCanceled");
                }
                //if in matchmaking then delete match
                else {
                    if (players[playerCode].currentGame !== null) {
                        removeMatch(players[playerCode].currentGame);
                        players[playerCode].currentGame = null;
                    }
                }
            });
            socket.on('rejectChallenge', function (data)//Reject challenge and tell challenging player they are rejected
            {
                //Delete game object and allow challenges for both players
                delete games[data.challengerId];
                players[data.challengerId].isBusy = false;
                players[playerCode].isBusy = false;
                var message = {};
                message.message = 'Your challenge was rejected by ' + playerCode + '.';
                io.to(players[data.challengerId].id).emit('connectionError', message);

            });
            socket.on('acceptChallenge', function (data)//Accept a direct challenge and begin game
            {
                socket.join(games[data.challengerId].channel);
                games[playerCode] = games[data.challengerId];
                games[playerCode].player2 = players[playerCode];
                socket.to(players[data.challengerId].id).emit("challengeAccepted");
                beginGame(games[playerCode]);
            });
            //Matchmaking
            socket.on('findMatch', function (data)//Join a game in matchmaking queue or add a game to queue
            {
                if (players[playerCode].name !== data.name) {
                    players[playerCode].name = data.name;
                    initializeRecord(players[playerCode]);
                }
                clearTimeouts();
                //If there are users looking for match join their game
                if (matchmaking.length > 0) {
                    var game = matchmaking.shift();
                    socket.join(game.channel);
                    games[playerCode] = game;
                    games[playerCode].player2 = players[playerCode];
                    beginGame(game);
                }
                else {
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
                        player2State: 0,
                        winner: 0,
                        winnerDelta: 0,
                        inputWindow: false
                    };
                    socket.join(game.channel);
                    games[playerCode] = game;
                    channels[channelCode] = game;
                    matchmaking.push(game);
                    players[playerCode].currentGame = game;
                    suggestAiTimeout = setTimeout(function () {
                        if (games[playerCode] !== undefined && games[playerCode].player2 === null)
                            socket.emit('suggestAIMatch');
                    }, 20000);
                    matchmakingTimeout = setTimeout(function () {
                        if (games[playerCode] !== undefined && games[playerCode].player2 === null) {
                            var message = {};
                            message.message = 'Matchmaking timed out. Try again or challenge a friend.';
                            socket.emit('connectionError', message);
                            if (players[playerCode] !== undefined && players[playerCode].currentGame !== null)
                                removeMatch(players[playerCode].currentGame);
                            if (players[playerCode] !== undefined)
                                players[playerCode].currentGame = null;
                        }
                    }, 60000);
                }
            });
            socket.on('challengeAI', function ()//Accept match against AI, remove from matchmaking and begin AI match
            {
                if (players[playerCode] !== undefined && players[playerCode].currentGame !== null)
                    removeMatch(players[playerCode].currentGame);
                clearTimeouts();
                players[playerCode].currentGame = null;
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
                players[playerCode].isBusy = true;
                setTimeout(function () {
                        if (games[playerCode] !== undefined) {
                            var playerStatus = {player1: games[playerCode].player1.code};
                            io.to(games[playerCode].channel).emit('beginGame', playerStatus);
                            playGame({channel: games[playerCode].channel});
                        }
                    }
                    , 3000);
            });
            //Input Handling
            socket.on('processAIInput', function ()//Recieve input from AI, determine if jam or won and update gamestate on server and clients
            {
                if (!games[playerCode].drawActive) {
                    if (games[playerCode].player2state === 2)
                        return;
                    console.log("should jam ai");
                    games[playerCode].player2state = 2;
                    io.to(games[playerCode].channel).emit('gameUpdate', getCurrentState());
                    var jamTimer = setTimeout(function () {
                        if (games[playerCode] !== undefined && games[playerCode].gameState === 1) {
                            games[playerCode].player2state = 0;
                            io.to(games[playerCode].channel).emit('gameUpdate', getCurrentState());
                        }
                    }, 5000);
                }
                else {
                    if (games[playerCode].player2state === 0) {
                        games[playerCode].gameState = 2;
                        games[playerCode].player1state = 3;
                        games[playerCode].player2state = 1;
                        io.to(games[playerCode].channel).emit('gameUpdate', getCurrentState());
                        updateStreaks(game);
                        if (jamTimer !== undefined)
                            clearTimeout(jamTimer);
                    }
                    removeGame(games[playerCode]);
                }
            });
            socket.on('processInput', function (data)//Recieve input from player, determine if jam or won and update gamestate on server and clients
            {
                console.log(games[playerCode]);
                if (games[playerCode] !== undefined) {
                    var isPlayer1 = (games[playerCode].player1.id === socket.id);
                    var jamTimer = null;
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
                        jamTimer = setTimeout(function () {
                            if (games[playerCode] !== undefined) {
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
                            }
                        }, 5000)
                    }
                    else {
                        //Need to not allow double input;
                        if (isPlayer1 && games[playerCode].player1State === 4) {
                            return;
                        }
                        if (!isPlayer1 && games[playerCode].player2State === 4) {
                            return;
                        }
                        //if there is already a flagged winner then evaluate deltas
                        if (games[playerCode].winner > 0 && games[playerCode].inputWindow) {
                            evaluateDeltas(data.delta, games[playerCode]);
                            return;
                        }
                        games[playerCode].winnerDelta = data.delta;
                        games[playerCode].inputWindow = true;
                        console.log('Input window is open');
                        //Set window determined by ping to give other player chance to respond based on latency
                        var delay = 10;
                        if (games[playerCode].player2 !== null)
                            delay = Math.max(games[playerCode].player1.ping, games[playerCode].player2.ping) + 10;
                        setTimeout(function () {
                            if (games[playerCode] !== undefined) {
                                console.log(delay + ' has elapsed and now input window is closed');
                                if (games[playerCode].winner === 1) {
                                    games[playerCode].gameState = 2;
                                    games[playerCode].player1state = 1;
                                    games[playerCode].player2state = 3;
                                }
                                else {
                                    games[playerCode].gameState = 2;
                                    games[playerCode].player1state = 3;
                                    games[playerCode].player2state = 1;
                                }
                                games[playerCode].inputWindow = false;
                                io.to(games[playerCode].channel).emit('gameUpdate', getCurrentState());
                                updateStreaks(games[playerCode]);
                                removeGame(games[playerCode]);
                            }
                        }, delay);
                        if (isPlayer1 && games[playerCode].player1state === 0) {
                            games[playerCode].winner = 1;
                            games[playerCode].player1state = 4;
                        }
                        if (!isPlayer1 && games[playerCode].player2state !== 2) {
                            games[playerCode].winner = 2;
                            games[playerCode].player2state = 4;
                        }
                        if (jamTimer !== undefined)
                            clearTimeout(jamTimer);
                    }
                }
            });
            //Disconnect/Cleanup
            socket.on('disconnect', function ()//When a socket disconnects delete player and game and tell other connected clients in same game to disconnect
            {
                if (players.hasOwnProperty(playerCode)) {
                    console.log('- deleted ' + players[playerCode]);
                    if (players[playerCode] !== undefined && players[playerCode].currentGame !== null) {
                        removeMatch(players[playerCode].currentGame);
                        players[playerCode].currentGame = null;
                    }
                    delete players[playerCode];
                }
                if (games.hasOwnProperty(playerCode)) {
                    console.log('- deleted ' + games[playerCode]);
                    var data = games[playerCode].channel;
                    delete channels[data];
                    //emit a disconnect to all other connected clients in the room
                    io.to(games[playerCode].channel).emit('playerDisconnected', {channel: data});
                }
            });
            socket.on('disconnectFromRoom', function (data)//Disconnect from channel of game that has ended
            {
                //last player in game deletes game
                console.log(playerCode + ' has disconnected from channel : ' + data.channel);
                socket.leave(data.channel);
                players[playerCode].isBusy = false;
            });
            socket.on('clientPing', function () {
                if (players[playerCode] !== undefined) {
                    players[playerCode].ping = moment().valueOf() - pingtime;
                    socket.emit('pingResult', {ping: players[playerCode].ping});
                }
            });
            socket.on('requestLeaderboard', function(){
                console.log('requested leaderboard!');
                var options = {
                    "limit": 25,
                    "sort": ['streak','desc']
                };
                db.collection('players').find({}, options).toArray(function(err, docs){
                    if(err) throw err;
                    console.log(docs);
                    socket.emit('sendLeaderboard', {leaderboard:docs});
                });
            });
        }
    );
});
