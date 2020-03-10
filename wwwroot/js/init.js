var connectionConsole = new signalR.HubConnectionBuilder().withUrl("/signalr/console").build();
var connectionControl = new signalR.HubConnectionBuilder().withUrl("/signalr/control").build();

connectionConsole.start().then(function () {
    connectionConsole.invoke("IsRunning").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

connectionControl.start().then(function () {

}).catch(function (err) {
    return console.error(err.toString());
});
