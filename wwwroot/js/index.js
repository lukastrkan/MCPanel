
//Server is running
connectionConsole.on("IsRunning", function (xd) {
    if (xd == true) {
        document.getElementById("start").classList.add("disabled");
        document.getElementById("start").disabled = true;
        document.getElementById("stop").classList.remove("disabled");
        document.getElementById("stop").disabled = false;
        document.getElementById("restart").classList.remove("disabled");
        document.getElementById("restart").disabled = false;
    }
    else {
        document.getElementById("start").classList.remove("disabled");
        document.getElementById("start").disabled = false;
        document.getElementById("stop").classList.add("disabled");
        document.getElementById("stop").disabled = true;
        document.getElementById("restart").classList.add("disabled");
        document.getElementById("restart").disabled = true;
    }
});


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