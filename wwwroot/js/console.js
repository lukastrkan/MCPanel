var doc = document.getElementById("textbox");

connectionConsole.on("ReceiveConsole", function (text) {
    doc.innerHTML += text + "<br>";
    let obj = document.getElementById("textbox");
    obj.scrollTop = obj.scrollHeight;
});

connectionConsole.on("IsRunning", function (xd) {
    if (xd == true) {        
        document.getElementById("execute").classList.remove("disabled");
        document.getElementById("execute").disabled = false;
    }
    else {
        document.getElementById("execute").classList.add("disabled");
        document.getElementById("execute").disabled = true;
    }
});

function send() {    
    connectionControl.invoke("ExecuteCommand", $("#command").val()).catch(function (err) {
        return console.error(err.toString());
    });
    $("#command").val("");
    event.preventDefault();
}




