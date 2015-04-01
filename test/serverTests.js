var io = require('socket.io-client')
, assert = require('assert')
, expect = require('expect.js')
, should = require('should');

describe('Suite of unit tests', function () {
    this.timeout(5000);
    var socketURL = 'http://localhost:3001';
    var options = {
        transports: ['websocket'],
        'force new connection': true
    };
    describe('Testing Code Request', function () {
        it('Code Request', function (done) {
            var client = io.connect(socketURL, options);
            client.on('connect', function () {
                client.emit('requestPlayerCode');
                client.on('playerCodeCreated', function (data) {
                    var length = data.code.length
                    length.should.equal(4);
                    client.disconnect();
                    done();
                });
            });
        });
    });
    describe('Testing invalid Code', function () {
        it('Invalid code', function (done) {
            var client = io.connect(socketURL, options);
            var clientCode;
            client.on('connect', function () {
                client.emit('requestPlayerCode');
                client.on('playerCodeCreated', function (data) {                  
                    clientCode = data.code;
                    client.emit('challenge', { code: 'AAAAA', challengerId: clientCode });
                    client.on('connectionError', function (data) {
                        client.disconnect();
                        done();
                    });
                });
            });
        });
    });
    describe('Testing valid Code', function () {
        it('Valid code', function (done) {
            var client1 = io.connect(socketURL, options);
            var client2 = io.connect(socketURL, options);
            var client1Code;
            var client2Code;
            client1.on('connect', function () {
                client1.emit('requestPlayerCode');
                client1.on('playerCodeCreated', function (data) {
                    client1Code = data.code;
                });
                client2.on('connect', function () {
                    client2.emit('requestPlayerCode');
                    client2.on('playerCodeCreated', function (data) {
                        client2Code = data.code;
                        client1.emit('challenge', { code: client2Code, challengerId: client1Code });
                        client2.on('challengePosted', function (data) {
                            var id = data.id;
                            id.should.equal(client1Code);
                            client1.disconnect();
                            client2.disconnect();
                            done();
                        });
                    });
                });
            });
        });
    });
    describe('Testing Challenge Acceptance', function () {
        it('Challenge Accept', function (done) {
            var client1 = io.connect(socketURL, options);
            var client2 = io.connect(socketURL, options);
            var client1Code;
            var client2Code;
            client1.on('connect', function () {
                client1.emit('requestPlayerCode');
                client1.on('playerCodeCreated', function (data) {
                    client1Code = data.code;
                });
                client2.on('connect', function () {
                    client2.emit('requestPlayerCode');
                    client2.on('playerCodeCreated', function (data) {
                        client2Code = data.code;
                        client1.emit('challenge', { code: client2Code, challengerId: client1Code });
                        client2.on('challengePosted', function (data) {
                            client2.emit('acceptChallenge', { challengerId: data.id });
                            client1.on('challengeAccepted', function () {
                                client1.disconnect();
                                client2.disconnect();
                                done();
                            });
                        });
                    });
                });
            });
        });
    });
    describe('Testing Player Disconnect in Game', function () {
        it('Player Disconnect', function (done) {
            var client1 = io.connect(socketURL, options);
            var client2 = io.connect(socketURL, options);
            var client1Code;
            var client2Code;
            client1.on('connect', function () {
                client1.emit('requestPlayerCode');
                client1.on('playerCodeCreated', function (data) {
                    client1Code = data.code;
                });
                client2.on('connect', function () {
                    client2.emit('requestPlayerCode');
                    client2.on('playerCodeCreated', function (data) {
                        client2Code = data.code;
                        client1.emit('challenge', { code: client2Code, challengerId: client1Code });
                        client2.on('challengePosted', function (data) {
                            client2.emit('acceptChallenge', { challengerId: data.id });
                            client1.on('challengeAccepted', function () {
                                client1.disconnect();
                                client2.on('playerDisconnected', function () {
                                    client2.disconnect();
                                    done();
                                });
                            });
                        });
                    });
                });
            });
        });
    });
    describe('Testing Game State Update', function () {
        it('Game Update', function (done) {
            var client1 = io.connect(socketURL, options);
            var client2 = io.connect(socketURL, options);
            var client1Code;
            var client2Code;
            client1.on('connect', function () {
                client1.emit('requestPlayerCode');
                client1.on('playerCodeCreated', function (data) {
                    client1Code = data.code;
                });
                client2.on('connect', function () {
                    client2.emit('requestPlayerCode');
                    client2.on('playerCodeCreated', function (data) {
                        client2Code = data.code;
                        client1.emit('challenge', { code: client2Code, challengerId: client1Code });
                        client2.on('challengePosted', function (data) {
                            client2.emit('acceptChallenge', { challengerId: data.id });
                            client1.on('challengeAccepted', function () {
                                client1.on('beginGame', function () {
                                    client1.emit('processInput');
                                    client1.on('gameUpdate', function (data) {
                                        client1.disconnect();
                                        client2.disconnect();
                                        done();
                                    });
                                });                                
                            });
                        });
                    });
                });
            });
        });
    });
});