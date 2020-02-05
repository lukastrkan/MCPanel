var connectionConsole = new signalR.HubConnectionBuilder().withUrl("/signalr/console").build();
var doc = document.getElementById("textbox");


connectionConsole.on("ReceiveConsole", function (text) {
    doc.innerHTML += text + "<br>";
    let obj = document.getElementById("textbox");
    obj.scrollTop = obj.scrollHeight;
});


connectionConsole.on("IsRunning", function (xd) {
    console.log(xd);
});

connectionConsole.start().then(function () { 
    connectionConsole.invoke("IsRunning").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

var connectionControl = new signalR.HubConnectionBuilder().withUrl("/signalr/control").build();

connectionControl.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});


function send() {    
    connectionControl.invoke("ExecuteCommand", $("#command").val()).catch(function (err) {
        return console.error(err.toString());
    });
    $("#command").val("");
    event.preventDefault();
}


function start() {
    connectionControl.invoke("Start").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}

function stop() {
    connectionControl.invoke("Stop").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}

function restart() {
    connectionControl.invoke("Restart").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}

