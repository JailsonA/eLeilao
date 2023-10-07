"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("subMessage").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var msg = document.getElementById("sms").appendChild(li);
    msg.textContent = `says ${message}`;
});

connection.start().then(function () {
    document.getElementById("subMessage").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("subMessage").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});