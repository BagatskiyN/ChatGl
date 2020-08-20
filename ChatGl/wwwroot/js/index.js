//async function send() {
//    const url = '/?handler=Ajax';
//    const text = document.getElementById("Text").value;
//    let formData = new FormData();

//    formData.append("text", text);

//    let xsrf_token = document.getElementsByName("__RequestVerificationToken")[0].value;

//    try {
//        const response = await fetch(url, {
//            method: 'POST',
//            body: formData,
//            credentials: 'include',
//            headers: { "XSRF-TOKEN": xsrf_token }
//        });
//        if (response.status === 200) {
//            const json = await response.json();
//            add_message(text, json.userName);
//        }
//        else {
//            const text = await response.text();
//            alert(text);
//        }
//    } catch (error) {
//        console.error('Ошибка:', error);
//    }
//}

//function add_message(text, userName) {
//    var html = `<div style="font-size:larger">${text}</div>
//      <div style="font-size: smaller">${userName} --- Now</div>`;
//    var item = document.createElement("div");
//    item.innerHTML = html;
//    document.getElementById("bottom").before(item);
//}
// соединение
const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });

// вызов серверного метода
document.getElementById("sendButton").addEventListener("click", (e) => {
    const textEl = document.getElementById("Text");
    if (textEl.value =="") {
        return console.error("Пустое поле");
    }
    else {
   connection.invoke("SendToAll", textEl.value)
        .catch(function (err) {
            return console.error(err.toString());
        });
    }
 
    textEl.value = "";
});

// прием ответа от сервера
connection.on("ReceiveMessage", function (data) {
    // to prevent markup
    const text = data.text.replace(/&/g, "&").replace(/</g, "<").replace(/>/g, ">");

    const divEl = document.createElement('div');
    divEl.innerHTML =
        `<div style="font-size:larger">${text}</div>` +
        `<div style="font-size: smaller">${data.sign} --- ${data.when}</div>`;
    document.getElementById("bottom").before(divEl);
});