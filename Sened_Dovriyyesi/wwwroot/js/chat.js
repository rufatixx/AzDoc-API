"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

//connection.on("ReceiveMessage", function (user, message) {
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    var encodedMsg = user + " says " + msg;
//    var li = document.createElement("li");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesList").appendChild(li);
//});



connection.on("ReciveList", function (list) {
    alert(list);
});





connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var username = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", username, message).catch(function (err) {
        return console.error(err.toString());
    });
//    event.preventDefault();
//});



document.getElementById("sendButton").addEventListener("click", function (event) {
    var id = document.getElementById("userInput").value;
    
    connection.invoke("getDocs", id).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});