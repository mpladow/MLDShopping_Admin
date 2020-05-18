// DOM ELEMENTS
$(document).ready(function () {
    let sendButton = document.querySelector("#sendButton");
    let userInput = document.querySelector("#userInput");
    let messageInput = document.querySelector("#messageInput");
    let userName =  
    // EVENT LISTENERS
    sendButton.addEventListener("click", (event) => {
        sendMessage();
        event.preventDefault();
    })
    // FUNCTIONS
    //Disable send button until connection is established
    var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/chatHub").build();
    sendButton.disabled = true;

    connection.on("ReceiveMessage", (user, message) => {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var encodedMsg = user + " says " + msg;
        var li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);
    });

    connection.start().then(() => {
        sendButton.disabled = false;
    }).catch((error) => {
        return console.error(error.toString());
    })
    function sendMessage() {
        let user = userInput.value;
        let message = messageInput.value;
        connection.invoke("SendMessage", user, message).catch((err) => {
            return console.error(err.toString());
        })
    }
})
