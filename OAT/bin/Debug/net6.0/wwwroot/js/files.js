window.onload = () => {
    const fileWindow = document.getElementById("file-error");
    const close = document.getElementById("file-error-close");
    close.addEventListener("click", () => {
        fileWindow.classList.remove("active");
    });
}

LoadSessionsFileByCorpus();
LoadPracticeFileByCorpus();
function UploadScheduleChanges(files_id) {
    var corpus = $('#corpus').find(":selected").val();
    var url = `/api/schedule/changes/${corpus}/upload`;
    var formData = new FormData();
    formData.append("file", GetFile(files_id));
    var button = document.getElementById("files-change-btn");
    button.textContent = "ЖДИТЕ! Около 10-30 секунд";
    POSTWithjqXHR(url, formData, 
        (response) => {
            MessageController("message-success");
            button.textContent = "Обновить";
        }, 
        (jqXHR) => {
            console.log(`Your error code: ${jqXHR.status} ${url}`);
            switch (jqXHR.status) {
                case 401: 
                    MessageController("message-fail-auth");
                    window.location = "https://www.oat.ru/api/logout";
                    break;
                case 406 || 403: 
                    MessageController("message-fail-perms");
                    break;
                case 400:
                    console.log(jqXHR.responseText);
                    var window = document.getElementById("file-error");
                    window.classList.add("active");
                    var text = document.getElementById("error-text");
                    text.innerText = jqXHR.responseText;
                    break;
                default:
                    MessageController("message-fail");
                    break;
            }
            button.textContent = "Обновить";

        });
}


function UploadSessionsChanges(files_id) {
    var filename = document.getElementById("filename-display").value;
    var corpus = $('#corpus-sessions').find(":selected").val();
    var url = `/api/sessions/${corpus}/upload`;
    var formData = new FormData();

    formData.append("filename", filename);
    formData.append("file",  GetFile(files_id));

    POST(url, formData,
        (response) => {
            MessageController("message-success-sessions");
            LoadSessionsFileByCorpus();
        }, 
        (code) => {
            console.log(`Your error code: ${code} ${url}`);
            switch(code) {
                case 401 || 405: 
                    MessageController("message-fail-auth");
                    window.location = "https://www.oat.ru/api/logout";
                    break;
                case 406 || 403: 
                    MessageController("message-fail-perms");
                    break;
                case 400:
                    MessageController("message-fail-extension");
                    break;
                default:
                    MessageController("message-fail");
                    break;
            }
        });

}

function LoadSessionsFileByCorpus(){
    var building = document.getElementById("corpus-sessions").value;
    var url = `/api/sessions/${building}/files`
    GET(url, 
        (response) => {
            var table = document.getElementById("session-tbody");
            table.innerHTML = '';
            const files = JSON.parse(response);;
            Array.from(files).forEach((filename, index) => {
                var row = table.insertRow(0);
                var cell_Filename = row.insertCell(0);
                var cell_controll = row.insertCell(1);
                cell_Filename.innerHTML = filename;
                cell_controll.innerHTML = `<button onclick="RemoveSessionFileByCorpus('${filename}'); return false" class="panel-table-button">удалить</button>`
            });
        },
        (code) => {
            console.log(code);
        });
}

function RemoveSessionFileByCorpus(filename){
    var building = document.getElementById("corpus-sessions").value;
    var url = `/api/sessions/${building}/delete`

    var formData = new FormData();
    formData.append("filename", filename);
    DELETE(url, formData, 
        (response) => {
            MessageController("message-success-delete-session");
            LoadPracticeFileByCorpus();
        }, 
        (code) => {
            console.log(`Your error code: ${code} ${url}`);
            switch(code) {
                case 401: 
                    MessageController("message-fail-auth");
                    window.location = "/api/logout";
                    break;
                case 406: 
                    MessageController("message-fail-perms");
                    break;
                default:
                    MessageController("message-fail");
                    break;
            }
        })
}


function UploadPracticeChanges(files_id) {
    var filename = document.getElementById("filename-display-practice").value;
    var corpus = $('#corpus-practice').find(":selected").val();
    var url = `/api/practice/${corpus}/upload`;
    var formData = new FormData();

    formData.append("filename", filename);
    formData.append("file",  GetFile(files_id));

    POST(url, formData,
        (response) => {
            MessageController("message-success-sessions");
            LoadPracticeFileByCorpus();
        }, 
        (code) => {
            console.log(`Your error code: ${code} ${url}`);
            switch(code) {
                case 401 || 405: 
                    MessageController("message-fail-auth");
                    window.location = "https://www.oat.ru/api/logout";
                    break;
                case 406 || 403: 
                    MessageController("message-fail-perms");
                    break;
                case 400:
                    MessageController("message-fail-extension");
                    break;
                default:
                    MessageController("message-fail");
                    break;
            }
        });

} 

function LoadPracticeFileByCorpus(){
    var building = document.getElementById("corpus-practice").value;
    var url = `/api/practice/${building}/files`
    GET(url, 
        (response) => {
            var table = document.getElementById("practice-tbody");
            table.innerHTML = '';
            const files = JSON.parse(response);;
            Array.from(files).forEach((filename, index) => {
                var row = table.insertRow(0);
                var cell_Filename = row.insertCell(0);
                var cell_controll = row.insertCell(1);
                cell_Filename.innerHTML = filename;
                cell_controll.innerHTML = `<button onclick="RemovePracticeFileByCorpus('${filename}'); return false" class="panel-table-button">удалить</button>`
            });
        },
        (code) => {
            console.log(code);
        });
}

function RemovePracticeFileByCorpus(filename){
    var building = document.getElementById("corpus-practice").value;
    var url = `/api/practice/${building}/delete`

    var formData = new FormData();
    formData.append("filename", filename);
    DELETE(url, formData, 
        (response) => {
            MessageController("message-success-delete-session");
            LoadPracticeFileByCorpus();
        }, 
        (code) => {
            console.log(`Your error code: ${code} ${url}`);
            switch(code) {
                case 401: 
                    MessageController("message-fail-auth");
                    window.location = "/api/logout";
                    break;
                case 406: 
                    MessageController("message-fail-perms");
                    break;
                default:
                    MessageController("message-fail");
                    break;
            }
        })
}